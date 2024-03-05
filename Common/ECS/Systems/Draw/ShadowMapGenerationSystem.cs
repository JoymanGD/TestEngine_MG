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
    public class ShadowMapGenerationSystem : ISystem<GameTime>
    {
        public static RenderTarget2D ShadowMap;
        
        private Effect ShadowMapGenerationShader;
        private World World;
        private List<LightData> LightDatas = new ();
        
        public ShadowMapGenerationSystem(World world)
        {
            World = world;
            ShadowMapGenerationShader = GameSettings.Instance.ShadowMapGenerationShader;

            ShadowMap = new RenderTarget2D(GameSettings.Instance.GraphicsDevice, 2048, 2048, false, SurfaceFormat.Single, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents, false);
        }

        public void Update(GameTime state)
        {
            var lightsEntities = World.GetEntities().With<Light>().With<Transform>().AsEnumerable().ToArray();
            var modelEntities = World.GetEntities().With<ModelRenderer>().With<Transform>().AsEnumerable().ToArray();
            var graphicsDevice = GameSettings.Instance.GraphicsDevice;

            LightDatas = Light.ConstructLightDataList(lightsEntities);
            
            var lightsAmount = LightDatas.Count;

            graphicsDevice.SetRenderTarget(ShadowMap);
            
            //ShadowMapArray generation
            for (int i = 0; i < lightsAmount; i++)
            {
                if (i != 0)
                    continue;
                
                var lightData = LightDatas[i];

                foreach (var modelEntity in modelEntities)
                {
                    var modelRenderer = modelEntity.Get<ModelRenderer>();
                    var transform = modelEntity.Get<Transform>();
                    
                    foreach (ModelMesh mesh in modelRenderer.Model.Meshes) 
                    {
                        var WorldMatrix = mesh.ParentBone.Transform * transform.WorldMatrix;

                        foreach (var meshpart in mesh.MeshParts)
                        {
                            Matrix WorldViewProjection = WorldMatrix * lightData.ViewProjection;
                            MonogameEffectFunctions.SetParameterSafe(ShadowMapGenerationShader, "WorldViewProjection", WorldViewProjection);
                            MonogameEffectFunctions.SetParameterSafe(ShadowMapGenerationShader, "LightId", i);

                            ShadowMapGenerationShader.CurrentTechnique.Passes[0].Apply();
                        
                            graphicsDevice.SetVertexBuffer(meshpart.VertexBuffer);
                            graphicsDevice.Indices = meshpart.IndexBuffer;
                            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshpart.VertexOffset, meshpart.StartIndex, meshpart.PrimitiveCount);
                        }
                    }
                }
            }
            
            graphicsDevice.SetRenderTarget(null);
        }

        public void Dispose()
        {
            ShadowMap?.Dispose();
        }

        public bool IsEnabled { get; set; }
    }
}