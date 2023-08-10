#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0

#define MAX_DIRECTIONAL_LIGHTS 6
#define MAX_POINT_LIGHTS 20

float4x4 WorldMatrix;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;

//..............................LIGHTING...........................//
float4 AmbientColor = float4(.15, .15, .15, .15);


int ActiveDirectionalLightsCount;

float4 DirectionalLightsAmbientColors[MAX_DIRECTIONAL_LIGHTS];
float DirectionalLightsAmbientIntensities[MAX_DIRECTIONAL_LIGHTS];
float4 DirectionalLightsDiffuseColors[MAX_DIRECTIONAL_LIGHTS];
float DirectionalLightsDiffuseIntensities[MAX_DIRECTIONAL_LIGHTS];
float4 DirectionalLightsDirections[MAX_DIRECTIONAL_LIGHTS];


int ActivePointLightsCount;

float4 PointLightsDiffuseColors[MAX_POINT_LIGHTS];
float PointLightsIntensities[MAX_POINT_LIGHTS];
float4 PointLightsPositions[MAX_POINT_LIGHTS];
float PointLightsRadii[MAX_POINT_LIGHTS];
//..............................LIGHTING...........................//

//..............................SHADOW MAPPING...........................//
cbuffer MatrixBuffer
{
    matrix worldMatrix;
    matrix viewMatrix;
    matrix projectionMatrix;
    matrix lightViewMatrix;
    matrix lightProjectionMatrix;
};

cbuffer LightBuffer2
{
    float3 lightPosition;
    float padding;
};
//..............................SHADOW MAPPING...........................//

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float4 Normal : NORMAL0;
    float4 WorldPosition : TEXCOORD0;
    float4 lightViewPosition : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, WorldMatrix);
    float4 viewPosition = mul(worldPosition, ViewMatrix);
    output.Position = mul(viewPosition, ProjectionMatrix);
    output.WorldPosition = worldPosition;
    output.Normal = normalize(mul(input.Normal, WorldMatrix));

    // output.lightViewPosition = mul(input.position, WorldMatrix);
    // output.lightViewPosition = mul(output.lightViewPosition, WorldMatrix);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
    float4 allLightsColor = float4(0,0,0,0);
    if(ActiveDirectionalLightsCount <= MAX_DIRECTIONAL_LIGHTS){
        float4 directionalColor = float4(0,0,0,0);
        for(int i = 0; i < ActiveDirectionalLightsCount; i++){
            float4 diffuseColor = DirectionalLightsDiffuseColors[i];
            float4 ambientColor = DirectionalLightsAmbientColors[i];
            float4 lightDirection = normalize(DirectionalLightsDirections[i]);

            directionalColor += AmbientColor;

            float normalIntensity = dot(-lightDirection, input.Normal);
            float4 diffuse = normalIntensity * ambientColor;
            directionalColor += diffuse * diffuseColor;
        }
        allLightsColor += directionalColor;
    }

    if(ActivePointLightsCount <= MAX_POINT_LIGHTS){
        float4 pointColor = float4(0,0,0,0);
        for(int i = 0; i < ActivePointLightsCount; i++){
            // float4 ambientColor = PointLightsAmbientColors[i];
            float4 lightPosition = PointLightsPositions[i] - input.WorldPosition;
            float lightRadius = PointLightsRadii[i];
            float dist = length(lightPosition);
            float atten = (lightRadius / dist) + (PointLightsIntensities[i] / (dist * dist * dist));

            lightPosition /= dist; //normalize
            float intensity = dot(lightPosition, input.Normal);
            float4 diffuseColor = PointLightsDiffuseColors[i];
            float4 color = intensity * diffuseColor * atten;
            pointColor += saturate(color);
        }
        allLightsColor += pointColor;
    }

    input.Color = allLightsColor;

    return input.Color;
}

technique Ambient
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}