sampler2D implicitInputSampler : register(S0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInputSampler, uv);
    color.rgb = dot(color.rgb, float3(0.33, 0.33, 0.33));
    return color;
}