//------- K variables --------
float Ka;
float Kd;
float Ks;
float A;

//------- Light variables --------

// Point Lights:
float3 xLight1Pos;
float4 xLight1Color;
float xLight1Intensity = 1.0f;

// Camera:
float3 xCamPos;
float3 xCamUp;

// Ambient Light:
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.3f;

// Diffuse Light:
float3 DiffuseLightDirection = float3(1, 0, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0f;

//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float4x4 xWorldViewProjection;

//------- Shader I/O structures --------
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
	float4 Intensity : COLOR;
	float3 Normal : NORMAL0;
};

struct PixelToFrame
{
	float4 Color : COLOR0;
};

//------- Texture sampler --------
Texture2D xTexture;
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

//------- Lighting functions --------
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
	float4 Is = Ks * xLight1Intensity * pow(saturate(dot(N, H)), 2 * A);

	return Ia * AmbientColor + (Id + Is) * xLight1Color;
}

//------- Flat shading shader--------
VertexShaderOutput CommonVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, xWorld);
	float4 viewPosition = mul(worldPosition, xView);
	output.Position = mul(viewPosition, xProjection);
	output.PositionWorld = worldPosition;

	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 FlatPhongPixelShader(VertexShaderOutput input) : COLOR0
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

float4 FlatBlinnPixelShader(VertexShaderOutput input) : COLOR0
{
	input.TextureCoordinate.y--;
	float3 N = normalize(cross(ddy(input.PositionWorld.xyz), ddx(input.PositionWorld.xyz)));
	float3 L = normalize(xLight1Pos - (float3) input.PositionWorld);
	float3 V = normalize(xCamPos - (float3) input.PositionWorld);
	float3 H = normalize(L + V);

	float4 lightColor = BlinnLighting(N, L, H);
	lightColor.a = 1;

	float4 textureColor = xTexture.Sample(TextureSampler, input.TextureCoordinate);
	textureColor.a = 1;

	return saturate(textureColor * lightColor);
}

//------- Flat shading techniques --------
technique FlatPhong
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 CommonVertexShader();
		PixelShader = compile ps_3_0 FlatPhongPixelShader();
	}
}

technique FlatBlinn
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 CommonVertexShader();
		PixelShader = compile ps_3_0 FlatBlinnPixelShader();
	}
}

//------- Gouraud shading shaders --------
float4 GouraudPixelShader(VertexShaderOutput input) : COLOR0
{
	input.TextureCoordinate.y--;
	return input.Intensity * xTexture.Sample(TextureSampler, input.TextureCoordinate);
}

VertexShaderOutput GouraudPhongVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	//transform position to clip space
	input.Position = mul(input.Position, xWorld);
	output.Position = mul(input.Position, xView);
	output.Position = mul(output.Position, xProjection);

	//set texture coords
	output.TextureCoordinate = input.TextureCoordinate;

	//calculate lighting vectors
	float3 L = normalize(xLight1Pos - (float3) input.Position);
	float3 N = normalize(mul(input.Normal, (float3x3) xWorld));
	float3 V = normalize(xCamPos - (float3) input.Position);
	float3 R = (2 * dot(N, L)) * N - L;

	//calculate per vertex lighting intensity and interpolate it like a color
	output.Intensity = PhongLighting(N, L, V, R);

	return output;
}

VertexShaderOutput GouraudBlinnVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	//transform position to clip space
	input.Position = mul(input.Position, xWorld);
	output.Position = mul(input.Position, xView);
	output.Position = mul(output.Position, xProjection);

	//set texture coords
	output.TextureCoordinate = input.TextureCoordinate;

	//calculate lighting vectors
	float3 L = normalize(xLight1Pos - (float3) input.Position);
	float3 N = normalize(mul(input.Normal, (float3x3) xWorld));
	float3 V = normalize(xCamPos - (float3) input.Position);
	float3 R = reflect(xLight1Pos - N, N);
	float3 H = normalize(L + V);

	//calculate per vertex lighting intensity and interpolate it like a color
	output.Intensity = BlinnLighting(N, L, H);

	return output;
}

//------- Gouraud shading techniques --------
technique GouraudPhong
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_3_0, GouraudPhongVertexShader()));
		SetPixelShader(CompileShader(ps_3_0, GouraudPixelShader()));
	}
}

technique GouraudBlinn
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_3_0, GouraudBlinnVertexShader()));
		SetPixelShader(CompileShader(ps_3_0, GouraudPixelShader()));
	}
}

//------- Phong shading shaders --------
VertexShaderOutput PhongVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.PositionWorld = mul(input.Position, xWorld);
	output.Position = mul(output.PositionWorld, xView);
	output.Position = mul(output.Position, xProjection);

	output.TextureCoordinate = input.TextureCoordinate;
	output.Normal = normalize(mul(input.Normal, (float3x3)xWorld));

	return output;
}


float4 PhongPhongPixelShader(VertexShaderOutput input) : COLOR0
{
	input.TextureCoordinate.y--;
	float3 L = normalize(xLight1Pos - (float3) input.PositionWorld);
	float3 N = normalize(input.Normal);
	float3 V = normalize(xCamPos - (float3) input.PositionWorld);
	float3 R = reflect(L, N);

	return PhongLighting(N, L, V, R) * xTexture.Sample(TextureSampler, input.TextureCoordinate);
}

float4 PhongBlinnPixelShader(VertexShaderOutput input) : COLOR0
{
	input.TextureCoordinate.y--;
	float3 L = normalize(xLight1Pos - (float3) input.PositionWorld);
	float3 N = normalize(input.Normal);
	float3 V = normalize(xCamPos - (float3) input.PositionWorld);
	float3 H = normalize(L + V);

	return BlinnLighting(N, L, H) * xTexture.Sample(TextureSampler, input.TextureCoordinate);
}

//------- Phong shading techniques --------
technique PhongPhong
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 PhongVertexShader();
		PixelShader = compile ps_3_0 PhongPhongPixelShader();
	}
}

technique PhongBlinn
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 PhongVertexShader();
		PixelShader = compile ps_3_0 PhongBlinnPixelShader();
	}
}
