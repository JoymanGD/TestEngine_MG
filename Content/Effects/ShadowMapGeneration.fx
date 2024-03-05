#include "Common.fx"

float4x4 WorldViewProjection;
uint LightId;

struct VertexShaderInput
{
    float4 Position : POSITION;
};

struct PixelShaderInput
{
    float4 Position : SV_POSITION;
    float Depth : TEXCOORD0;
};

PixelShaderInput VertexShaderFunction(VertexShaderInput Input)
{
    PixelShaderInput output;
    output.Position = mul(Input.Position, WorldViewProjection);
    output.Depth = output.Position.z;

    return output;
}

float PixelShaderFunction(PixelShaderInput input) : SV_Target
{
    return input.Depth;
}

technique Tech0
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}