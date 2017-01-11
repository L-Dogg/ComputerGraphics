struct VertexToPixel
{
    float4 Position   	: POSITION;    
    float4 Color		: COLOR0;
    float LightingFactor: TEXCOORD0;
    float2 TextureCoords: TEXCOORD1;
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};

struct MyVertexToPixel
{
	float4 Position     : POSITION;
	float2 TexCoords    : TEXCOORD0;
	float3 Normal        : TEXCOORD1;
	float3 Position3D    : TEXCOORD2;
};

struct VS_INPUT
{
	float4 p : POSITION;
	float2 t : TEXCOORD;
	float3 n : NORMAL;
};

//pixel shader inputs
struct PS_INPUT_PV
{
	float4 p : SV_POSITION;
	float2 t : TEXCOORD;
	float4 i : COLOR;
};

//------- K - variables --------
float Ka;
float Kd;
float Ks;
float A;

//------- Light - variables --------
float3 xLight1Pos;
float4 xLight1Color;
float ambientLight;

//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xLightDirection;
float xAmbient;
bool xEnableLighting;
float3 xCamPos;
float3 xCamUp;
float xPointSpriteSize;
float4x4 xWorldViewProjection;
float3 xLightPos;
float xLightPower;

//------- Texture Samplers --------

Texture2D xTexture;
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Technique: Colored --------

VertexToPixel ColoredVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float4 inColor: COLOR)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Color = inColor;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = dot(Normal, -xLightDirection);
    
	return Output;    
}

PixelToFrame ColoredPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
    
	Output.Color = PSIn.Color;
	Output.Color.rgb *= saturate(PSIn.LightingFactor) + xAmbient;

	return Output;
}

technique Colored
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 ColoredVS();
		PixelShader  = compile ps_2_0 ColoredPS();
	}
}

//------- Technique: Textured --------

VertexToPixel TexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	Output.TextureCoords = inTexCoords;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = dot(Normal, -xLightDirection);
    
	return Output;    
}

PixelToFrame TexturedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
	Output.Color.rgb *= saturate(PSIn.LightingFactor) + xAmbient;

	return Output;
}

technique Textured
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 TexturedVS();
		PixelShader  = compile ps_2_0 TexturedPS();
	}
}

//------- Technique: Simplest --------

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
	float3 lightDir = normalize(pos3D - lightPos);
	return dot(-lightDir, normal);
}

PixelToFrame OurFirstPixelShader(MyVertexToPixel PSIn)
{
	PSIn.TexCoords.y--;
	PixelToFrame Output = (PixelToFrame)0;

	float diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
	diffuseLightingFactor *= xLightPower;
	float4 baseColor = tex2D(TextureSampler, PSIn.TexCoords);
	Output.Color = baseColor*(diffuseLightingFactor + xAmbient);


	return Output;
}

MyVertexToPixel SimplestVertexShader(float4 inPos : POSITION0, float3 inNormal : NORMAL0, float2 inTexCoords : TEXCOORD0)
{
	MyVertexToPixel Output = (MyVertexToPixel)0;

	Output.Position = inPos;
	Output.Position = mul(inPos, xWorldViewProjection);
	Output.TexCoords = inTexCoords;
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);

	return Output;
}

technique Simplest
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 SimplestVertexShader();
		PixelShader = compile ps_2_0 OurFirstPixelShader();
	}
}

//------- Shading+Lightning functions --------

float4 FlatShading_PhongLightning(float4 LColor, float3 N, float3 L, float3 V, float3 R)
{
	float4 Ia = Ka * ambientLight;
	float4 Id = Kd * saturate(dot(N, L));
	float4 Is = Ks * pow(saturate(dot(R, V)), A);

	return Ia + (Id + Is) * LColor;
}

float4 FlatShading_BlinnLightning(float4 LColor, float3 N, float3 L, float3 H)
{
	float4 Ia = Ka * ambientLight;
	float4 Id = Kd * saturate(dot(N, L));
	float4 Is = Ks * pow(saturate(dot(N, H)), A);

	return Ia + (Id + Is) * LColor;
}

//------- Shading+Lighting functions --------

PixelToFrame PS_VERTEX_LIGHTING_PHONG(PS_INPUT_PV input)
{
	input.t.y--;
	PixelToFrame Output = (PixelToFrame)0;
	Output.Color = input.i * xTexture.Sample(TextureSampler, input.t);
	return Output;
}

PS_INPUT_PV VS_VERTEX_LIGHTING_PHONG(VS_INPUT input)
{
	PS_INPUT_PV output;

	//transform position to clip space
	input.p = mul(input.p, xWorld);
	output.p = mul(input.p, xView);
	output.p = mul(output.p, xProjection);

	//set texture coords
	output.t = input.t;

	//calculate lighting vectors
	float3 N = normalize(mul(input.n, (float3x3) xWorld));
	float3 V = normalize(xCamPos - (float3) input.p);
	//DONOT USE -light.dir since the reflection returns a ray from the surface	
	float3 R = reflect(xLight1Pos - N, N);

	//calculate per vertex lighting intensity and interpolate it like a color
	output.i = FlatShading_PhongLightning(xLight1Color, N, -xLight1Pos, V, R);

	return output;
}

technique RENDER_VL_PHONG
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_2_0, VS_VERTEX_LIGHTING_PHONG()));
		//SetGeometryShader(NULL);
		SetPixelShader(CompileShader(ps_2_0, PS_VERTEX_LIGHTING_PHONG()));
		//SetRasterizerState(rsSolid);
	}
}