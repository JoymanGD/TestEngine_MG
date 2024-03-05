using System.Collections.Generic;
using Common.ECS.Systems;
using Common.Settings;
using DefaultEcs;
using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    //this struct should be absolutely same as one in shader code
    //data should be correctly aligned so the final size of the type is multiple of 8
    public struct LightData
    {
        public Matrix ViewProjection;
        
        public Vector3 Position;
        public float Radius;
        
        public Vector3 Direction;
        public float Intensity;
        
        public Vector3 Color;
        public uint Type;
    }
    
    public struct Light
    {
        public LightType Type;
        public Color Color;
        public float Radius;
        public float Intensity;
        public bool IsActive;

        public Light(LightType type, Color color, float intensity = 1, float radius = 1, bool isActive = true)
        {
            IsActive = isActive;
            Type = type;
            Color = color;
            Intensity = intensity;
            Radius = radius;
        }
        
        public static List<LightData> ConstructLightDataList(Entity[] lightsEntities)
        {
            var lightDatas = new List<LightData>();

            for (int i = 0; i < lightsEntities.Length; i++)
            {
                var lightEntity = lightsEntities[i];

                var lightComponent = lightEntity.Get<Light>();
                var transformComponent = lightEntity.Get<Transform>();

                if(lightComponent.IsActive)
                {
                    Matrix viewMatrix;
                    
                    Matrix projectionMatrix;
                    
                    if (lightComponent.Type == 0)
                    {
                        var screenSize = new Vector2(128, 128);
                        var directLightPos = transformComponent.Forward * -200;
                        var nearFarPlanes = new Vector2(1, directLightPos.Y * 2);
                        viewMatrix = Matrix.CreateLookAt(directLightPos, directLightPos + transformComponent.Forward, Vector3.Up);
                        projectionMatrix = Matrix.CreateOrthographic(screenSize.X, screenSize.Y, nearFarPlanes.X, nearFarPlanes.Y);
                    }
                    else
                    {
                        var screenSize = GameSettings.Instance.ScreenSize;
                        var nearFarPlanes = new Vector2(1f, 1000);
                        viewMatrix = Matrix.CreateLookAt(transformComponent.Position, transformComponent.Position + transformComponent.Forward, Vector3.Up);
                        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, screenSize.X/screenSize.Y, nearFarPlanes.X, nearFarPlanes.Y);
                    }
                    
                    lightDatas.Add(new LightData
                    {
                        Type = (uint)lightComponent.Type,
                        Position = transformComponent.Position,
                        Direction = transformComponent.Forward,
                        Color = lightComponent.Color.ToVector3(),
                        Radius = lightComponent.Radius,
                        Intensity = lightComponent.Intensity,
                        ViewProjection = viewMatrix * projectionMatrix
                    });
                }
            }

            return lightDatas;
        }
    }

    public enum LightType
    {
        Directional = 0,
        Point = 1
    }
}