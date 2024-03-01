#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float sinFunc;
float4 filterColor;
Texture2D Texture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

// This data comes from the sprite batch vertex shader
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

// Our pixel shader
float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 texColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);

	float4 color;

	if (texColor.a != 0)
	{
		color = float4(texColor.r + (texColor.r - filterColor.r) * sinFunc,
						texColor.g + (texColor.g - filterColor.g) * sinFunc,
						texColor.b + (texColor.b - filterColor.b) * sinFunc,
						texColor.a);
	}
	else
	{
		color = float4(texColor.r, texColor.g, texColor.b, texColor.a);
	}

	return color * filterColor;
}

// Compile our shader
technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
}