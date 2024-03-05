using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using System;
using System.Linq;
using Common.Settings;
using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Systems
{
    [With(typeof(Controller))]
    [With(typeof(Player))]
    
    public partial class DebugSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        private bool lightsOn = true;
        private int SelectedLightIndex = -1;
        private const float RotationSpeed = .01f;
        private bool ShowShadowMap = false;
        
        public DebugSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0)
        {
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Controller controller)
        {
            if(controller.WasPressed("Debug_LightsSwitch"))
            {
                if(lightsOn)
                {
                    lightsOn = false;
                }
                else
                {
                    lightsOn = true;
                }

                var lights = World.Get<Components.Light>();

                foreach(ref Components.Light item in lights)
                {
                    item.IsActive = lightsOn;
                }
            }
            
            if(controller.IsHolding("Debug_LightSelection"))
            {
                var lights = World.Get<Components.Light>();

                int lightIndex = -1;

                if(controller.WasPressed("Debug_SelectLight_1"))
                {
                    lightIndex = 0;
                }
                else if(controller.WasPressed("Debug_SelectLight_2"))
                {
                    lightIndex = 1;
                }
                else if(controller.WasPressed("Debug_SelectLight_3"))
                {
                    lightIndex = 2;
                }
                else if(controller.WasPressed("Debug_SelectLight_4"))
                {
                    lightIndex = 3;
                }
                else if(controller.WasPressed("Debug_SelectLight_5"))
                {
                    lightIndex = 4;
                }
                else if(controller.WasPressed("Debug_SelectLight_6"))
                {
                    lightIndex = 5;
                }

                if(lightIndex != -1)
                {
                    if(lightIndex < lights.Length)
                    {
                        SelectedLightIndex = lightIndex;
                        Console.WriteLine($"Light selected: {lightIndex}");
                    }
                    else
                    {
                        Console.WriteLine($"Invalid light index to select: {lightIndex}");
                    }
                }
            }

            if(SelectedLightIndex >= 0)
            {
                if(
                    (controller.IsHolding("Debug_ArrowLeft") ||
                    controller.IsHolding("Debug_ArrowRight") ||
                    controller.IsHolding("Debug_ArrowUp") ||
                    controller.IsHolding("Debug_ArrowDown")))
                {
                    var lightsEntities = World.GetEntities().With<Components.Light>().With<Transform>().AsEnumerable().ToArray();

                    var inputVector = controller.GetInputVector("Debug_ArrowLeft", "Debug_ArrowRight", "Debug_ArrowUp", "Debug_ArrowDown");

                    if(controller.IsHolding("Debug_Shift"))
                    {
                        lightsEntities[SelectedLightIndex].Get<Transform>().Rotation *= Quaternion.CreateFromYawPitchRoll(inputVector.X * RotationSpeed, inputVector.Y * RotationSpeed, 0);
                        Console.WriteLine($"Light_{SelectedLightIndex} rotation changed to: {lightsEntities[SelectedLightIndex].Get<Transform>().Rotation}");
                    }
                    else if(controller.IsHolding("Debug_Control"))
                    {
                        lightsEntities[SelectedLightIndex].Get<Transform>().Position += new Vector3(inputVector.X, inputVector.Y, 0);
                        Console.WriteLine($"Light_{SelectedLightIndex} position changed to: {lightsEntities[SelectedLightIndex].Get<Transform>().Position}");
                    }
                    else if(controller.IsHolding("Debug_Alt"))
                    {
                        var lastColor = lightsEntities[SelectedLightIndex].Get<Components.Light>().Color.ToVector3();
                        var newColor = new Color(new Vector4(lastColor.X + inputVector.X/100, lastColor.Y + inputVector.Y/100, 0f, 0f));
                        lightsEntities[SelectedLightIndex].Get<Components.Light>().Color = newColor;
                        Console.WriteLine($"Light_{SelectedLightIndex} color changed to: {lightsEntities[SelectedLightIndex].Get<Components.Light>().Color}");
                    }
                }
            
                if(controller.WasPressed("Debug_ShadowMap"))
                {
                    ProfilingSystem.ShowShadowMap = !ProfilingSystem.ShowShadowMap;
                }
            }
        }
    }
}