#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

float InputRandomValue;
float random (float2 st) {
    return frac(sin(dot(st.xy, float2(12.9898,78.233)))*InputRandomValue);
}

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
	float4 clr = input.Color;
	float rand = random(input.TextureCoordinates);
	float4 randomColor = float4(rand, rand, rand, 1);
	return tex2D(SpriteTextureSampler,input.TextureCoordinates) * (input.Color * ((randomColor * 0.5) + 0.5));
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};