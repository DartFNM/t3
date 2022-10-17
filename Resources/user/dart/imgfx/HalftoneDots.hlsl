//>>> _common parameters
float4x4 objectToWorldMatrix;
float4x4 worldToCameraMatrix;
float4x4 projMatrix;
Texture2D txDiffuse;
float2 RenderTargetSize;
//<<< _common parameters

//>>> _parameters
Texture2D Image;
float DotsSize;
float Contrast;
float Angle;
float4 Color0;
float4 Color1;
float Bright;
//<<< _parameters
 
//>>> setup
SamplerState samLinear
{
    Filter = MIN_MAG_LINEAR_MIP_POINT;
    //Filter = MIN_MAG_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};

SamplerState samNearest
{
    Filter = MIN_MAG_LINEAR_MIP_POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};
//<<< setup

//>>> declarations
struct VS_IN
{
    float4 pos : POSITION;
    float2 texCoord : TEXCOORD;
};

struct PS_IN
{
    float4 pos : SV_POSITION;
    float2 texCoord: TEXCOORD0;	    
    float3 worldTViewPos: TEXCOORD1;
    float3 worldTViewDir: TEXCOORD2;
};

static float2 iResolution;

//<<< declarations

//>>> _GS

//<<< _GS

//>>> _VS 
PS_IN VS( VS_IN input )
{
    PS_IN output = (PS_IN)0;

    output.pos = mul(input.pos, worldToCameraMatrix);
    output.pos = mul(output.pos, projMatrix);
    output.texCoord = input.texCoord;

    return output;
}
//<<< _VS

//>>> PS


float roundNearest(float f, float size)
{
    return round(f / size) * size;
}

// slope = f'(x) at x = 0;
float sigmoid(float f, float slope) {
    return tanh(slope * f);
}

static float iTime;

void mainImage( out float4 fragColor, in float2 fragCoord )
{
    // Normalized pixel coordinates (from 0 to 1)
    float2 uv = fragCoord/iResolution.xy;
    float ratio = iResolution.x / iResolution.y;

    float scale = (uv.x + 1.) / 2. + (uv.y + 1.) / 3.;

    // Time varying pixel color
    // Rotate 22.5 degrees
    float2x2 t;
    float c = cos(Angle), s = sin(Angle);
    t[0] = float2(c, s); // cos, sin
    t[1] = float2(-s, c); // -sin, cos
    //float scale = length(uv.xy - float2(-1, 1)) + 1.;
    //float scale = 1.;
    //t *= scale;
    float2x2 tInv = transpose(t);
    float2 pIso = mul(tInv, fragCoord.xy);
    
    float size = DotsSize;
    float stopX = roundNearest(pIso.x, size);
    float stopY = roundNearest(pIso.y, size);
    
    float2 nearestIso = float2(stopX, stopY);
    
    float4 tcol = Image.Sample(samLinear, mul(t, nearestIso) / iResolution.xy);
    float real = 1.0 - clamp( 0., 1., length(tcol.rgb) * Contrast + Bright );
    //float real = length(tex2D(_MainTex, uv.xy).rgb) / sqrt(3.);
    
    float3 hue = Color1;
    
    float3 col;
    
    float light = 1.;
    float dark = real;
    
    float d = abs(distance( mul(t, nearestIso), mul(t, pIso) ));
    float pivot = d - (1. - real) * size / 1.0;
    float alpha = sigmoid(pivot, 2.); // sigmoid for AA
    
    float lum = alpha * light + (1. - alpha) * dark;
    //lum = real;
    //col = lum * hue;
    col = lerp( Color1, Color0, lum);

    // Output to screen
    fragColor = float4(col, 1.0);
}




    float4 PS(PS_IN input) : SV_Target
    {
        iResolution = RenderTargetSize;  
        
        //Image.GetDimensions(iResolution.x, iResolution.y);
        float2 fragCoord = input.pos.xy;
        float4 fragColor;
        mainImage( fragColor, fragCoord );
        return fragColor;
    }
    
//<<< PS

//>>> _technique
technique10 Render
{
    pass P0
    {
        SetGeometryShader( 0 );
        SetVertexShader( CompileShader( vs_4_0, VS() ) );
        SetPixelShader( CompileShader( ps_4_0, PS() ) );
    }
}
//<<< _technique
