#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0
#define MAX_LIGHTS 20

float4x4 WorldMatrix;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;
float3 CameraPosition;
float3 Diffuse;
float Ambient;
float Specular;
int ActiveLights;
int LightTypes[MAX_LIGHTS];
float4 LightPositions[MAX_LIGHTS];
float3 LightDirections[MAX_LIGHTS];
float3 LightColors[MAX_LIGHTS];
float LightRadii[MAX_LIGHTS];
float LightIntensities[MAX_LIGHTS];

Texture2D MainTexture;
SamplerState MainTextureSampler
{
    Texture = <FirstTexture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

const float POINT_CONSTANT = 1;
const float POINT_LINEAR = 0.09;
const float POINT_QUADRATIC = 0.032;

struct LightData
{
    int LightType;
    float4 Position;
    float3 Direction;
    float3 Color;
    float Radius;
    float Intensity;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float3 Color : COLOR0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
    float4 WorldPosition : POSITION1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    float4 worldPosition = mul(input.Position, WorldMatrix);
    float4 viewPosition = mul(worldPosition, ViewMatrix);
    output.Position = mul(viewPosition, ProjectionMatrix);
    output.WorldPosition = worldPosition;
    output.TexCoord = input.TexCoord;
    //we need to truncate 4x4 matrix to 3x3 matrix since normal vectors mustn't be affected by translations (which are actually last row and last column of the 4x4 matrix)
    output.Normal = normalize(mul(input.Normal, (float3x3)WorldMatrix));

    return output;
}

LightData GetLightData(int index)
{
    LightData lightData;

    lightData.LightType = LightTypes[index];
    lightData.Position = LightPositions[index];
    lightData.Direction = LightDirections[index];
    lightData.Color = LightColors[index];
    lightData.Radius = LightRadii[index];
    lightData.Intensity = LightIntensities[index];

    return lightData;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
    //we define color as float3 since we dont want the a component to be affected by calculations
    float3 allLightsColor = float3(0,0,0);

    float3 textureColor = (float3)MainTexture.Sample(MainTextureSampler, input.TexCoord);
    bool emptyTexture = (bool)(textureColor == float3(0, 0, 0));
    if(emptyTexture)
    {
        textureColor += 1;
    }

    for(int i = 0; i < ActiveLights; i++)
    {
        float atten = 1;
        float3 lightDirection;
    
        LightData light = GetLightData(i);

        if(light.LightType == 0) //Directional
        {
            lightDirection = -light.Direction;
        }
        else if(light.LightType == 1) //Point
        {
            lightDirection = (float3)(light.Position - input.WorldPosition);
            
            float dist = length(lightDirection);

            atten = 1 / ( 1 + POINT_CONSTANT + POINT_LINEAR * dist + POINT_QUADRATIC * pow(dist, 2));
        }

        lightDirection = normalize(lightDirection);
        
        //diffuse
        float intensity = saturate(dot(lightDirection, input.Normal)) * light.Intensity;
        float3 diffuse = intensity * light.Color * atten * Diffuse;

        //specular
        float3 viewDir = normalize(CameraPosition - (float3)input.WorldPosition);
        float3 reflection = reflect(-lightDirection, input.Normal);
        float specularIntensity = pow(saturate(dot(viewDir, reflection)), 32);
        float3 specular = light.Color * specularIntensity * Specular;

        //ambient
        float3 ambient = light.Color * Ambient;

        allLightsColor += (diffuse + specular + ambient) * textureColor;
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