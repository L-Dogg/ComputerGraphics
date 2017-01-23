//------- K variables --------
float Ka;
float Kd;
float Ks;
float A;

//------- Light variables --------
float4x4 xLightPositions;
float4x4 xLightColors;
int xLightCount = 0;

float4 AmbientColor = float4(0.6f, 0.6f, 0.6f, 0.6f);
float   LightIntensity = 1.0f;

float4x4 xCarLightPositions;
float4x4 xCarLightColors;
int xCarLightCount = 4;
float xCarLightIntensity = 0.9f;
float4x4 xCarLightDirections;

float spotCosCutOff = 0.9;
float cos_outer_cone_angle = 0.8;

//------- Camera variables --------
float3 xCamPos;
float3 xCamUp;

//------- Matrices --------
float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;

//------- Texturing --------
bool xUseColors;
float4 xDiffuseColor;

//------- Shader I/O structures --------
struct VertexShaderInput
{
	float4 Position			 : POSITION0;
	float4 Normal			 : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position			 : POSITION0;
	float4 PositionWorld	 : TEXCOORD0;
	float2 TextureCoordinate : TEXCOORD1;
	float4 Intensity         : COLOR0;
	float3 Normal			 : NORMAL0;
};

//------- Spotlight structure --------

struct SpotLighting
{
	float4 DiffuseColor;
	float4 SpecularColor;
};

//------- Texture sampler --------
Texture2D xTexture;
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

//------- Lighting functions --------
float4 PhongLighting(float3 N, float3 L, float3 V, float3 R, float dist, float4 color, bool isCarLight)
{
	float intensity = isCarLight ? xCarLightIntensity : LightIntensity;

	float4 Id = Kd * intensity * 95 / (dist * dist) * saturate(dot(N, L));
	float4 Is = Ks * intensity * 95/ (dist * dist) * pow(saturate(dot(-R, V)), A);

	return (Id + Is) * color;
}

float4 BlinnLighting(float3 N, float3 L, float3 H, float dist, float4 color, bool isCarLight)
{
	float intensity = isCarLight ? xCarLightIntensity : LightIntensity;

	float4 Id = Kd * intensity * 95 / (dist * dist) * saturate(dot(N, L));
	float4 Is = Ks * intensity * 95 / (dist * dist) * pow(saturate(dot(N, H)), 2 * A);

	return (Id + Is) * color;
}

SpotLighting PhongSpotlight(VertexShaderOutput PSIn, float3 N, float3 V)
{
	SpotLighting Output = (SpotLighting)0;
	for (int j = 0; j < xCarLightCount; j++)
	{
		float3 L = xCarLightPositions[j] - (float3)PSIn.PositionWorld;
		float3 D = normalize(xCarLightDirections[j]);
		float cos_cur_angle = dot(normalize(-L), D);
		
		float spotEffect = pow(abs(cos_cur_angle), 20);
		float att = spotEffect / (0.5 * length(L) + 0.25 * length(L) * length(L));
		L = normalize(L);
		float cos_inner_cone_angle = spotCosCutOff;
		float cos_inner_minus_outer_angle = cos_inner_cone_angle - cos_outer_cone_angle;
		float spot = 0.0;
		spot = saturate((cos_cur_angle - cos_outer_cone_angle) / cos_inner_minus_outer_angle);
		Output.DiffuseColor += xDiffuseColor * xCarLightColors[j] * att * spot;
		float3 R = normalize(2 * dot(L, N) * N - L);

		float specular = pow(max(dot(R, V), 0.0), 0.9f);
		Output.SpecularColor += xCarLightColors[j] * Ks * specular * att * spot;		
	}

	Output.DiffuseColor = saturate(Output.DiffuseColor);
	Output.SpecularColor = saturate(Output.SpecularColor);
	return Output;
}

SpotLighting BlinnSpotlight(VertexShaderOutput PSIn, float3 N, float3 V)
{
	SpotLighting Output = (SpotLighting)0;
	for (int j = 0; j < xCarLightCount; j++)
	{
		float3 L = xCarLightPositions[j] - (float3)PSIn.PositionWorld;
		float3 D = normalize(xCarLightDirections[j]);
		float cos_cur_angle = dot(normalize(-L), D);
		
		float spotEffect = pow(abs(cos_cur_angle), 20);
		float att = spotEffect / (0.5 * length(L) +
			0.25 * length(L) * length(L));
		L = normalize(L);
		float cos_inner_cone_angle = spotCosCutOff;
		float cos_inner_minus_outer_angle = cos_inner_cone_angle - cos_outer_cone_angle;
		float spot = 0.0;
		spot = saturate((cos_cur_angle - cos_outer_cone_angle) / cos_inner_minus_outer_angle);
		Output.DiffuseColor += xDiffuseColor * xCarLightColors[0] * att * spot;
		float3 H = normalize(L + V);

		float specular = pow(max(dot(N, H), 0.0), 2 * 0.9f);
		Output.SpecularColor += xCarLightColors[0] * Ks * specular * att * spot;
		
	}

	Output.DiffuseColor = saturate(Output.DiffuseColor);
	Output.SpecularColor = saturate(Output.SpecularColor);
	return Output;
}

//------- Flat vertex shader --------
VertexShaderOutput FlatVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, xWorld);
	float4 viewPosition = mul(worldPosition, xView);
	output.Position = mul(viewPosition, xProjection);
	output.PositionWorld = worldPosition;

	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

//------- Flat shading pixel shaders --------
float4 FlatPhongPixelShader(VertexShaderOutput input) : COLOR0
{
	float3 N = normalize(cross(ddy(input.PositionWorld.xyz), ddx(input.PositionWorld.xyz)));
	float3 V = normalize(xCamPos - (float3) input.PositionWorld);

	SpotLighting spot = PhongSpotlight(input, N, V);
	
	float4 lightColor = Ka * (AmbientColor + spot.DiffuseColor);
	lightColor += spot.SpecularColor;
	for (int i = 0; i < xLightCount; i++)
	{
		float4 light = xLightPositions[i];
		float3 L = light - (float3) input.PositionWorld;
		float dist = length(L);
		L = normalize(L);
		float3 R = reflect(L, N);

		lightColor += PhongLighting(N, L, V, R, dist, xLightColors[i], false);
	}
	
	float4 textureColor = 0;
	if (xUseColors)
		textureColor = xDiffuseColor;
	else
		textureColor = xTexture.Sample(TextureSampler, input.TextureCoordinate);

	return saturate(textureColor * lightColor);
}

float4 FlatBlinnPixelShader(VertexShaderOutput input) : COLOR0
{
	float3 N = normalize(cross(ddy(input.PositionWorld.xyz), ddx(input.PositionWorld.xyz)));
	float3 V = normalize(xCamPos - (float3) input.PositionWorld);

	SpotLighting spot = BlinnSpotlight(input, N, V);

	float4 lightColor = Ka * (AmbientColor + spot.DiffuseColor);
	lightColor += spot.SpecularColor;
	for (int i = 0; i < xLightCount; i++)
	{
		float4 light = xLightPositions[i];
		float3 L = light - (float3) input.PositionWorld;
		float dist = length(L);
		L = normalize(L);
		float3 H = normalize(L + V);

		lightColor += BlinnLighting(N, L, H, dist, xLightColors[i], false);
	}
		
	float4 textureColor = 0;
	if (xUseColors)
		textureColor = xDiffuseColor;
	else
		textureColor = xTexture.Sample(TextureSampler, input.TextureCoordinate);
	
	return saturate(textureColor * lightColor);
}

//------- Flat shading techniques --------
technique FlatPhong
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 FlatVertexShader();
		PixelShader = compile ps_3_0 FlatPhongPixelShader();
	}
}

technique FlatBlinn
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 FlatVertexShader();
		PixelShader = compile ps_3_0 FlatBlinnPixelShader();
	}
}

//------- Gouraud vertex shaders --------
VertexShaderOutput GouraudPhongVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	input.Position = mul(input.Position, xWorld);
	output.Position = mul(input.Position, xView);
	output.Position = mul(output.Position, xProjection);

	output.TextureCoordinate = input.TextureCoordinate;

	float3 N = normalize(mul(input.Normal, (float3x3) xWorld));
	float3 V = normalize(xCamPos - (float3) input.Position);

	float4 intensity = Ka * AmbientColor;
	for (int i = 0; i < xLightCount; i++)
	{
		float4 light = xLightPositions[i];
		float3 L = light - (float3) input.Position;
		float dist = length(L);
		L = normalize(L);
		float3 R = reflect(L, N);

		intensity += PhongLighting(N, L, V, R, dist, xLightColors[i], false);
	}

	output.Intensity = intensity;

	return output;
}

VertexShaderOutput GouraudBlinnVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	input.Position = mul(input.Position, xWorld);
	output.Position = mul(input.Position, xView);
	output.Position = mul(output.Position, xProjection);

	output.TextureCoordinate = input.TextureCoordinate;

	float3 N = normalize(mul(input.Normal, (float3x3) xWorld));
	float3 V = normalize(xCamPos - (float3) input.Position);

	float4 intensity = Ka * AmbientColor;
	for (int i = 0; i < xLightCount; i++)
	{
		float4 light = xLightPositions[i];
		float3 L = light - (float3) input.Position;
		float dist = length(L);
		L = normalize(L);
		float3 H = normalize(L + V);

		intensity += BlinnLighting(N, L, H, dist, xLightColors[i], false);
	}

	output.Intensity = intensity;

	return output;
}

//------- Gouraud pixel shader --------
float4 GouraudPixelShader(VertexShaderOutput input) : COLOR0
{
	if (xUseColors)
		return saturate(input.Intensity * xDiffuseColor);
	else
		return saturate(input.Intensity * xTexture.Sample(TextureSampler, input.TextureCoordinate));
}

//------- Gouraud shading techniques --------
technique GouraudPhong
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 GouraudPhongVertexShader();
		PixelShader = compile ps_3_0 GouraudPixelShader();
	}
}

technique GouraudBlinn
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 GouraudBlinnVertexShader();
		PixelShader = compile ps_3_0 GouraudPixelShader();
	}
}

//------- Phong vertex shader --------
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

//------- Phong pixel shaders --------
float4 PhongPhongPixelShader(VertexShaderOutput input) : COLOR0
{
	float3 N = normalize(input.Normal);
	float3 V = normalize(xCamPos - (float3) input.PositionWorld);

	float4 intensity = Ka * AmbientColor;
	for (int i = 0; i < xLightCount; i++)
	{
		float4 light = xLightPositions[i];
		float3 L = light - (float3) input.PositionWorld;
		float dist = length(L);
		L = normalize(L);
		float3 R = reflect(L, N);

		intensity += PhongLighting(N, L, V, R, dist, xLightColors[i], false);
	}

	if (xUseColors)
		return saturate(intensity * xDiffuseColor);
	else
		return saturate(intensity * xTexture.Sample(TextureSampler, input.TextureCoordinate));
}

float4 PhongBlinnPixelShader(VertexShaderOutput input) : COLOR0
{
	float3 N = normalize(input.Normal);
	float3 V = normalize(xCamPos - (float3) input.PositionWorld);

	float4 intensity = Ka * AmbientColor;
	for (int i = 0; i < xLightCount; i++)
	{
		float4 light = xLightPositions[i];
		float3 L = light - (float3) input.PositionWorld;
		float dist = length(L);
		L = normalize(L);
		float3 H = normalize(L + V);

		intensity += BlinnLighting(N, L, H, dist, xLightColors[i], false);
	}

	if (xUseColors)
		return saturate(intensity * xDiffuseColor);
	else
		return saturate(intensity * xTexture.Sample(TextureSampler, input.TextureCoordinate));
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