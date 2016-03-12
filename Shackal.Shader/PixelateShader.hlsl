float PixelatedWidth : register(C0);
float PixelatedHeight : register(C1);

Texture2D input : register(S0);

SamplerState samplerState
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

float4 main(float2 uv : TEXCOORD) : SV_TARGET
{
	float2 pixelatedImageSize = { PixelatedWidth, PixelatedHeight };

	float2 uvPixelated = floor(uv * pixelatedImageSize) / pixelatedImageSize;
	
	float4 color = input.Sample(samplerState, uvPixelated);

	return color;
}


