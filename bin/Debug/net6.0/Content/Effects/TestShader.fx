#define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1

#define MAX_DIRECTIONAL_LIGHTS 6

float4x4 WorldMatrix;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;
float4x4 WorldInverseTransposeMatrix;

//..............................LIGHTING...........................//
int ActiveDirectionalLightsCount;

float4 DirectionalLightsAmbientColors[MAX_DIRECTIONAL_LIGHTS];
float DirectionalLightsAmbientIntensities[MAX_DIRECTIONAL_LIGHTS];
float4 DirectionalLightsDiffuseColors[MAX_DIRECTIONAL_LIGHTS];
float DirectionalLightsDiffuseIntensities[MAX_DIRECTIONAL_LIGHTS];
float3 DirectionalLightsDirections[MAX_DIRECTIONAL_LIGHTS];
// float3 DiffuseLightDirection;
//..............................LIGHTING...........................//


struct VertexShaderInput
{
    float4 Position : SV_POSITION;
    float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, WorldMatrix);
    float4 viewPosition = mul(worldPosition, ViewMatrix);
    output.Position = mul(viewPosition, ProjectionMatrix);

    float4 normal = mul(input.Normal, WorldInverseTransposeMatrix);
    
    float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);

    [loop]
    for(int i = 0; i < ActiveDirectionalLightsCount; i++){
        float4 ambientColor = DirectionalLightsAmbientColors[i];
        float ambientIntensity = DirectionalLightsAmbientIntensities[i];
        float4 diffuseColor = DirectionalLightsDiffuseColors[i];
        float diffuseIntensity = DirectionalLightsDiffuseIntensities[i];
        float3 lightDirection = DirectionalLightsDirections[i];

        float finalAmbientIntensity = dot(normal.rgb, lightDirection) * ambientIntensity;
        float finalDiffuseIntensity = dot(normal.rgb, lightDirection) * diffuseIntensity;

        color += saturate(diffuseColor * finalDiffuseIntensity * ambientColor * finalAmbientIntensity);
    }
    
    output.Color = color;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_TARGET
{
    return saturate(input.Color);
}

technique Ambient
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}