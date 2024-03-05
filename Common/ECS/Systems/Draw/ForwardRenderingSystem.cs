using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.Settings;

namespace Common.ECS.Systems
{
    [With(typeof(Material))]
    [With(typeof(ModelRenderer))]
    [With(typeof(Transform))]
    public partial class ForwardRenderingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        private List<LightData> LightDatas = new ();
        
        public ForwardRenderingSystem(World world, IParallelRunner runner) : base(world, CreateEntityContainer, null, 0)
        {
            Runner = runner;
        }

        [Update]
        private void Update(ref Transform transform, ref ModelRenderer modelRenderer, ref Material material)
        {
            var cameraEntities = World.GetEntities().With<Camera>().With<Transform>().AsEnumerable().ToArray();
            var lightsEntities = World.GetEntities().With<Light>().With<Transform>().AsEnumerable().ToArray();
            var graphicsDevice = GameSettings.Instance.GraphicsDevice;

            LightDatas = Light.ConstructLightDataList(lightsEntities);
            
            var lightsAmount = LightDatas.Count;

            //Rendering
            foreach(var camera in cameraEntities)
            {
                var cameraComponent = camera.Get<Camera>();
                var cameraTransfromComponent = camera.Get<Transform>();

                var viewProjection = cameraComponent.ViewMatrix * cameraComponent.ProjectionMatrix;

                foreach (ModelMesh mesh in modelRenderer.Model.Meshes)
                {
                    var WorldMatrix = mesh.ParentBone.Transform * transform.WorldMatrix;

                    foreach (Effect effect in mesh.Effects)
                    {
                        MonogameEffectFunctions.SetParameterSafe(effect, "WorldMatrix", WorldMatrix);
                        MonogameEffectFunctions.SetParameterSafe(effect, "ViewProjectionMatrix", viewProjection);
                        MonogameEffectFunctions.SetParameterSafe(effect, "LightViewProjection", LightDatas[0].ViewProjection);
                        MonogameEffectFunctions.SetParameterSafe(effect, "CameraPosition", cameraTransfromComponent.Position);
                        MonogameEffectFunctions.SetParameterSafe(effect, "Diffuse", material.Diffuse.ToVector3());
                        MonogameEffectFunctions.SetParameterSafe(effect, "Ambient", material.Ambient);
                        MonogameEffectFunctions.SetParameterSafe(effect, "Specular", material.Specular);

                        MonogameEffectFunctions.SetParameterSafe(effect, "MainTexture", material.Texture);

                        MonogameEffectFunctions.SetParameterSafe(effect, "ShadowMap", ShadowMapGenerationSystem.ShadowMap);
                        //set lights amount
                        MonogameEffectFunctions.SetParameterSafe(effect, "NumLights", lightsAmount);

                        //set lights buffer
                        var lightsBuffer = new StructuredBuffer(graphicsDevice, typeof(LightData), lightsAmount, BufferUsage.None, ShaderAccess.Read);
                        lightsBuffer.SetData(LightDatas.ToArray());
                        MonogameEffectFunctions.SetParameterSafe(effect, "Lights", lightsBuffer);

                        effect.Techniques[0].Passes[0].Apply();
                    }

                    mesh.Draw();
                }
            }
        }
    }
}