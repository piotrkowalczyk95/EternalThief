#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

float r;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 texelColorFromLoadedImage = tex2D(SpriteTextureSampler, input.TextureCoordinates);

   float4 theColorWeGaveToSpriteBatchDrawAsaParameter = input.Color;

   float dx = input.TextureCoordinates.x - 0.5f;
   float dy = input.TextureCoordinates.y - 0.5f;
   if (dx * dx + dy * dy <= r) {
	   texelColorFromLoadedImage.a = 0;
   }
   return texelColorFromLoadedImage;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};