Texture2D<float4> ImageA : register(t0);
sampler texSampler : register(s0);

cbuffer ParamConstants : register(b0)
{
    float2 Size;
    float2 Shift;
}

cbuffer TimeConstants : register(b1)
{
    float globalTime;
    float time;
    float runTime;
    float beatTime;
}

cbuffer Resolution : register(b2)
{
    float2 RenderTargetSize;
}

struct vsOutput
{
    float4 position : SV_POSITION;
    float2 texCoord : TEXCOORD;
};


float4 psMain(vsOutput input) : SV_TARGET
{
    float2 centTarget = 0.5 * 1.0 / (RenderTargetSize);
    float2 p = input.texCoord + centTarget;

    float2 tileFrag = (p + Shift) % (1.0 / Size) * Size;
    if (tileFrag.x < 0) tileFrag.x += 1.0 / Size.x;
    if (tileFrag.y < 0) tileFrag.y += 1.0 / Size.y;

    float2 tile = p - tileFrag / Size + centTarget;

    float4 c = ImageA.Sample(texSampler, tile);

    return c; 
}
