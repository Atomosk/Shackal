float PixelatedWidth : register(C0);
float PixelatedHeight : register(C1);

Texture2D input : register(S0);

SamplerState samplerState
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

static float2 PixelatedImageSize = { PixelatedWidth, PixelatedHeight };
static float2 PixelSize = 1.0 / PixelatedImageSize;

float4 main(float2 uv : TEXCOORD) : SV_TARGET
{
	float2 uvPixelated = floor(uv * PixelatedImageSize) * PixelSize;
	
	float4 color = input.Sample(samplerState, uvPixelated);

	return color;
}


