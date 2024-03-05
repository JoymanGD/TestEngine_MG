#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0

struct LightData
{
    float4x4 ViewProjection;
    
    float3 Position;
    float Radius;
    
    float3 Direction;
    float Intensity;
    
    float3 Color;
    uint Type;
};

float CalcShadowTermSoftPCF(Texture2D ShadowMap, SamplerState ShadowMapSampler, float fLightDepth, float ndotl, float2 vTexCoord, int iSqrtSamples)
{
    float fShadowTerm = 0.0f;
    
    float variableBias = clamp(0.0005 * tan(acos(ndotl)), 0.00001, 0.02f);

    //float variableBias = (-cos(ndotl) + 1)*0.02;
    //variableBias = DepthBias;

    float shadowMapSize = 512;

    float fRadius = iSqrtSamples - 1; //mad(iSqrtSamples, 0.5, -0.5);//(iSqrtSamples - 1.0f) / 2;

    for (float y = -fRadius; y <= fRadius; y++)
    {
        for (float x = -fRadius; x <= fRadius; x++)
        {
            float2 vOffset = 0;
            vOffset = float2(x, y);
            vOffset /= shadowMapSize;
            //vOffset *= 2;
            //vOffset /= variableBias*200;
            float2 vSamplePoint = vTexCoord + vOffset;
            float fDepth = ShadowMap.Sample(ShadowMapSampler, vSamplePoint).x;
            float fSample = (fLightDepth <= fDepth + variableBias);
            
            // Edge tap smoothing
            float xWeight = 1;
            float yWeight = 1;
            
            if (x == -fRadius)
                xWeight = 1 - frac(vTexCoord.x * shadowMapSize);
            else if (x == fRadius)
                xWeight = frac(vTexCoord.x * shadowMapSize);
                
            if (y == -fRadius)
                yWeight = 1 - frac(vTexCoord.y * shadowMapSize);
            else if (y == fRadius)
                yWeight = frac(vTexCoord.y * shadowMapSize);
                
            fShadowTerm += fSample * xWeight * yWeight;
        }
    }
    
    fShadowTerm /= (fRadius*fRadius*4);
    
    return fShadowTerm;
}