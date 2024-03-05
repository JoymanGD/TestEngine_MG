#include "Common.fx"

#define POINT_CONSTANT 1
#define POINT_LINEAR 0.09
#define POINT_QUADRATIC 0.032

float4x4 WorldMatrix;
float4x4 ViewProjectionMatrix;
float4x4 LightViewProjection;
float3 CameraPosition;
float3 Diffuse;
float Ambient;
float Specular;

StructuredBuffer<LightData> Lights;
int NumLights;

// Texture2D MainTexture : register(t1);
// SamplerState MainTextureSampler : register(s1)
// {
//     Texture = <MainTexture>;
// };

Texture2D<float> ShadowMap : register(t2);
SamplerState ShadowMapSampler : register(s2);

struct VertexShaderInput
{
    float4 Position : POSITION;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

struct PixelShaderInput
{
    float4 WorldPosition : POSITION;
    float3 Normal : NORMAL0;
    float4 ProjectedPosition : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
};

PixelShaderInput VertexShaderFunction(VertexShaderInput input)
{
    PixelShaderInput output = (PixelShaderInput)0;
    output.WorldPosition = mul(input.Position, WorldMatrix);
    output.ProjectedPosition = mul(output.WorldPosition, ViewProjectionMatrix);
    output.TexCoord = input.TexCoord;
    
    //we need to truncate 4x4 matrix to 3x3 matrix since normal vectors mustn't be affected by translations (which are actually last row and last column of the 4x4 matrix)
    output.Normal = normalize(mul(input.Normal, (float3x3)WorldMatrix));

    return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : SV_Target
{
    //we define color as float3 since we dont want the a component to be affected by calculations
    float3 allLightsColor = float3(0,0,0);
    
    // float3 textureColor = MainTexture.Sample(MainTextureSampler, input.TexCoord);
    float3 textureColor = 1;
    
    for(int i = 0; i < NumLights; i++)
    {
        float atten = 1;
        float ShadowContribution = 1;
        
        float3 lightDirection;
    
        LightData light = Lights[i];
    
        if(light.Type == 0) //Directional
        {
            lightDirection = -light.Direction;
        }
        else if(light.Type == 1) //Point
        {
            lightDirection = light.Position - input.WorldPosition.xyz;
            
            float dist = length(lightDirection);
    
            atten = 1 / ( 1 + POINT_CONSTANT + POINT_LINEAR * dist + POINT_QUADRATIC * pow(dist, 2));
        }
    
        lightDirection = normalize(lightDirection);

        float NDotL = saturate(dot(lightDirection, input.Normal));
        
        //diffuse
        float intensity = NDotL * light.Intensity;
        float3 diffuse = intensity * light.Color * atten * Diffuse;
    
        //specular
        float3 viewDir = normalize(CameraPosition - (float3)input.WorldPosition);
        float3 reflection = reflect(-lightDirection, input.Normal);
        float specularIntensity = pow(saturate(dot(viewDir, reflection)), 32);
        float3 specular = light.Color * specularIntensity * Specular * atten;
    
        //ambient
        float3 ambient = light.Color * Ambient;
    
        //shadow
        if(i == 0)
        {
            // float4 LightPosProjected = mul(input.WorldPosition, light.ViewProjection);
            float4 LightPosProjected = mul(input.WorldPosition, LightViewProjection);
            float2 ShadowTexCoord = saturate(mad(0.5f, float2(LightPosProjected.x, -LightPosProjected.y), 0.5f));
            float CurrentDepth = LightPosProjected.z - 0.0001f;

            float SampledDepth = ShadowMap.Sample(ShadowMapSampler, ShadowTexCoord);
            if(SampledDepth < CurrentDepth)
            {
                ShadowContribution = .1f;
            }
        }
    
        allLightsColor += ((diffuse + specular) * ShadowContribution + ambient) * textureColor;
    }

    return float4(allLightsColor, 1);
}

technique Tech0
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}