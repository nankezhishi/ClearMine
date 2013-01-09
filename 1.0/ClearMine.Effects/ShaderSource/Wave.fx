float strength : register(C0);
float length : register(C1);
float offset : register(C2);
sampler2D implicitInputSampler : register(S0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    uv.y = uv.y + sin(uv.x * length + offset) * strength;
   
    return tex2D(implicitInputSampler, uv);
}
