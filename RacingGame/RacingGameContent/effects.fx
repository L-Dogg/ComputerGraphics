//------- K - variables --------
float Ka;
float Kd;
float Ks;
float A;

//------- Light - variables --------

// Point Lights:
float3 xLight1Pos;
float4 xLight1Color;
float xLight1Intensity = 1.0f;

// Ambient Light:
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.7f;

// Diffuse Light:
float3 DiffuseLightDirection = float3(1, 0, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0f;

//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float4x4 xWorldViewProjection;
float3 xCamPos;
float3 xCamUp;

//------- Shader I/O Structures --------
struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 PositionWorld : TEXCOORD0;
	float2 TextureCoordinate : TEXCOORD1;
};

Texture2D xTexture;
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Shading+Lighting functions --------

float4 PhongLighting(float3 N, float3 L, float3 V, float3 R)
{
	float4 Ia = Ka;
	float4 Id = Kd * DiffuseIntensity * saturate(dot(N, L));
	float4 Is = Ks * xLight1Intensity * pow(saturate(dot(R, V)), A);

	return Ia * AmbientColor + (Id + Is) * xLight1Color;
}

float4 BlinnLighting(float3 N, float3 L, float3 H)
{
	float4 Ia = Ka;
	float4 Id = Kd * DiffuseIntensity * saturate(dot(N, L));
	float4 Is = Ks * xLight1Intensity * pow(saturate(dot(N, H)), 2*A);
	
	return Ia * AmbientColor + (Id + Is) * xLight1Color;
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, xWorld);
	float4 viewPosition = mul(worldPosition, xView);
	output.Position = mul(viewPosition, xProjection);
	output.PositionWorld = worldPosition;

	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 Flat_Phong_PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	input.TextureCoordinate.y--;
	float3 N = normalize(cross(ddy(input.PositionWorld.xyz), ddx(input.PositionWorld.xyz)));
	float3 L = normalize(xLight1Pos - (float3) input.PositionWorld);
	float3 V = normalize(xCamPos - (float3) input.PositionWorld);
	float3 R = reflect(L, N);

	float4 lightColor = PhongLighting(N, L, V, R);
	lightColor.a = 1;

	float4 textureColor = xTexture.Sample(TextureSampler, input.TextureCoordinate);
	textureColor.a = 1;

	return saturate(textureColor * lightColor);
}

float4 Flat_Blinn_PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	input.TextureCoordinate.y--;
	float3 N = normalize(cross(ddy(input.PositionWorld.xyz), ddx(input.PositionWorld.xyz)));
	float3 L = normalize(xLight1Pos - (float3) input.PositionWorld);
	float3 V = normalize(xCamPos - L);
	float3 H = normalize(L + V);

	float4 lightColor = BlinnLighting(N, L, H);
	lightColor.a = 1;

	float4 textureColor = xTexture.Sample(TextureSampler, input.TextureCoordinate);
	textureColor.a = 1;

	return saturate(textureColor * lightColor);
}

technique FlatPhong
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 Flat_Phong_PixelShaderFunction();
	}
}

technique FlatBlinn
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 Flat_Blinn_PixelShaderFunction();
	}
}