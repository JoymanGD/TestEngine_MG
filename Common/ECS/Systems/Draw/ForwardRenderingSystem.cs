using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Common.Settings;

namespace Common.ECS.Systems
{
    [With(typeof(Material))]
    [With(typeof(ModelRenderer))]
    [With(typeof(Transform))]
    public partial class ForwardRenderingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        List<LightData> lightDatas;
        
        public ForwardRenderingSystem(World world, IParallelRunner runner) : base(world, CreateEntityContainer, null, 0)
        {
            Runner = runner;
            lightDatas = new List<LightData>();
        }

        [Update]
        private void Update(ref Transform transform, ref ModelRenderer modelRenderer, ref Material material)
        {
            var cameraEntities = World.GetEntities().With<Camera>().With<Transform>().AsEnumerable().ToArray();
            var lightsEntities = World.GetEntities().With<Light>().With<Transform>().AsEnumerable().ToArray();

            var lightDatas = GetLightDatas(lightsEntities);
            var lightsAmount = lightDatas.Count;

            foreach(var camera in cameraEntities)
            {
                var cameraComponent = camera.Get<Camera>();
                var cameraTransfromComponent = camera.Get<Transform>();

                foreach (ModelMesh mesh in modelRenderer.Model.Meshes)
                {
                    var world = mesh.ParentBone.Transform * transform.WorldMatrix;

                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.Parameters["WorldMatrix"].SetValue(world);
                        effect.Parameters["ViewMatrix"].SetValue(cameraComponent.ViewMatrix);
                        effect.Parameters["ProjectionMatrix"].SetValue(cameraComponent.ProjectionMatrix);
                        effect.Parameters["CameraPosition"].SetValue(cameraTransfromComponent.Position);
                        effect.Parameters["MainTexture"].SetValue(material.Texture);
                        effect.Parameters["Diffuse"].SetValue(material.Diffuse.ToVector3());
                        effect.Parameters["Ambient"].SetValue(material.Ambient);
                        effect.Parameters["Specular"].SetValue(material.Specular);

                        //set lights amount parameter
                        effect.Parameters["ActiveLights"].SetValue(lightsAmount);

                        //clear all lights data in shader
                        ClearShaderLightData(effect);

                        //set actual lights data
                        for (int i = 0; i < lightsAmount; i++)
                        {
                            var data = lightDatas[i];

                            effect.Parameters["LightTypes"].Elements[i].SetValue(data.LightType);
                            effect.Parameters["LightPositions"].Elements[i].SetValue(data.Position);
                            effect.Parameters["LightDirections"].Elements[i].SetValue(data.Direction);
                            effect.Parameters["LightColors"].Elements[i].SetValue(data.Color);
                            effect.Parameters["LightRadii"].Elements[i].SetValue(data.Radius);
                            effect.Parameters["LightIntensities"].Elements[i].SetValue(data.Intensity);
                        }

                        effect.Techniques[0].Passes[0].Apply();
                    }

                    mesh.Draw();
                }
            }
        }

        private void ClearShaderLightData(Effect effect)
        {
            effect.Parameters["LightTypes"].SetValue(new int[0]);
            effect.Parameters["LightPositions"].SetValue(new Vector4[0]);
            effect.Parameters["LightDirections"].SetValue(new Vector3[0]);
            effect.Parameters["LightColors"].SetValue(new Vector3[0]);
            effect.Parameters["LightRadii"].SetValue(new float[0]);
            effect.Parameters["LightIntensities"].SetValue(new float[0]);
        }

        private List<LightData> GetLightDatas(Entity[] lightsEntities)
        {
            lightDatas.Clear();

            for (int i = 0; i < lightsEntities.Length; i++)
            {
                var lightEntity = lightsEntities[i];

                var lightComponent = lightEntity.Get<Light>();
                var transformComponent = lightEntity.Get<Transform>();

                if(lightComponent.IsActive)
                {
                    lightDatas.Add(new LightData()
                    {
                        LightType = (int)lightComponent.Type,
                        Position = new Vector4(transformComponent.Position, 1),
                        Direction = transformComponent.Forward,
                        Color = lightComponent.Color.ToVector3(),
                        Radius = lightComponent.Radius,
                        Intensity = lightComponent.Intensity,
                    });
                }
            }

            return lightDatas;
        }
    }

    internal struct LightData
    {
        public int LightType;
        public Vector4 Position;
        public Vector3 Direction;
        public Vector3 Color;
        public float Radius;
        public float Intensity;
    }
}