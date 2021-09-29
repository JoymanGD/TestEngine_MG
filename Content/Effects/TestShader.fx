#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0

#define MAX_DIRECTIONAL_LIGHTS 6
#define MAX_POINT_LIGHTS 20

float4x4 WorldMatrix;
float3x3 WorldMatrix3x3;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;

//..............................LIGHTING...........................//
float3 AmbientColor = float3(.15, .15, .15);


int ActiveDirectionalLightsCount;

float4 DirectionalLightsAmbientColors[MAX_DIRECTIONAL_LIGHTS];
float DirectionalLightsAmbientIntensities[MAX_DIRECTIONAL_LIGHTS];
float4 DirectionalLightsDiffuseColors[MAX_DIRECTIONAL_LIGHTS];
float DirectionalLightsDiffuseIntensities[MAX_DIRECTIONAL_LIGHTS];
float3 DirectionalLightsDirections[MAX_DIRECTIONAL_LIGHTS];


int ActivePointLightsCount;

float4 PointLightsAmbientColors[MAX_POINT_LIGHTS];
float PointLightsAmbientIntensities[MAX_POINT_LIGHTS];
float4 PointLightsDiffuseColors[MAX_POINT_LIGHTS];
float PointLightsDiffuseIntensities[MAX_POINT_LIGHTS];
float3 PointLightsPositions[MAX_POINT_LIGHTS];
float PointLightsRadii[MAX_POINT_LIGHTS];
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
    float3 Normal : NORMAL0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, WorldMatrix);
    float4 viewPosition = mul(worldPosition, ViewMatrix);
    output.Position = mul(viewPosition, ProjectionMatrix);
    output.Normal = normalize(mul(input.Normal, WorldMatrix3x3));

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
    if(ActiveDirectionalLightsCount <= MAX_DIRECTIONAL_LIGHTS){
        for(int i = 0; i < ActiveDirectionalLightsCount; i++){
            float3 finalColor = float3(0,0,0);
            float3 diffuseColor = DirectionalLightsDiffuseColors[i];
            float3 ambientColor = DirectionalLightsAmbientColors[i];
            float3 lightDirection = normalize(DirectionalLightsDirections[i]);

            finalColor += AmbientColor;

            float normalIntensity = dot(-lightDirection, input.Normal);
            float3 diffuse = normalIntensity * ambientColor;
            finalColor += diffuse;

            input.Color += float4(finalColor * diffuseColor, 1);
        }
    }

    // if(ActivePointLightsCount <= MAX_POINT_LIGHTS){
    //     float4 color = float4(0.0f);

    //     float3 l = float3(0.0f);

    //     float atten = 0.0f;
    //     float n = normalize(input.Normal);
    //     float nDotL = 0.0f;

    //     for(int i = 0; i < ActivePointLightsCount; i++){
    //         float4 ambientColor = PointLightsAmbientColors[i];
    //         float ambientMultiplier = PointLightsAmbientIntensities[i];
    //         float4 diffuseColor = PointLightsDiffuseColors[i];
    //         float diffuseIntensity = PointLightsDiffuseIntensities[i];
    //         float3 lightPosition = PointLightsPositions[i];
    //         float lightRadius = PointLightsRadii[i];

    //         l = (lightPosition - input.Position) / lightRadius;
    //         atten = saturate(1.0f - dot(l, l));
    //         l = normalize(l);

    //         nDotL = saturate(dot(n, l));

    //         color += (diffuseColor * nDotL * atten);

    //         // float finalAmbientIntensity = (dot(normal, l) * ambientMultiplier) / distance;
    //         // float finalDiffuseIntensity = saturate(dot(normal, -l));

    //         // // input.Color *= diffuseColor * finalDiffuseIntensity * ambientColor * finalAmbientIntensity;
    //         // input.Color.rgb *= ambientColor + (diffuseColor * finalDiffuseIntensity);
    //     }

    //     input.Color += color;
    // }

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