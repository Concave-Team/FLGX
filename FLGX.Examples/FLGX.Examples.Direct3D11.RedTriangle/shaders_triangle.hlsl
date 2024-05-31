struct vs_in
{
    float3 position_local : POSITION;
    float3 normal_local : NORMAL;
    float2 texcoord_local : TEXCOORD0;
};

struct vs_out
{
    float4 position_clip : SV_POSITION;
};

cbuffer DataBuffer : register(b0)
{
    float3 draw_color;
    float padding;
};

vs_out vs_main(vs_in input)
{
    vs_out output = (vs_out) 0;
    output.position_clip = float4(input.position_local, 1.0);
    return output;
}

float4 ps_main(vs_out input) : SV_TARGET
{
    return float4(draw_color, 1.0);
}