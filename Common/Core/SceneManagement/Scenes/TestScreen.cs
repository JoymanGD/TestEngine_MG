using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common.Settings;
using Common.ECS.Components;
using Common.ECS.Systems;
using DefaultEcs.System;
using Common.ECS.SceneManagement;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace Common.Core.Scenes
{
    public class TestScreen : ECSScreen
    {
        public TestScreen(byBullet game) : base(game) { }

        public override void LoadContent()
        {
            Game.IsMouseVisible = false;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        public override void SetStartEntities()
        {
            var graphics = GameSettings.Instance.Graphics;
            var effect = Content.Load<Effect>("Effects/TestShader");

            var worldEntity = World.CreateEntity();
            worldEntity.Set(new Main());
            worldEntity.Set(new Input(false));

            var player = World.CreateEntity();
            var playerTransform = new Transform(Vector3.Zero, 9);
            player.Set(playerTransform);
            player.Set(new ModelRenderer(Content.Load<Model>("Models/Mage")));
            player.Set(effect.Clone());
            player.Set(new Bindings("Player"));
            player.Set(new Controller());
            player.Set(new Player(0));

            var camera = World.CreateEntity();
            var cameraTransform = new Transform(new Vector3(0, 0, 20), Vector3.Forward);
            camera.Set(cameraTransform);
            camera.Set(new Camera(graphics, cameraTransform.WorldMatrix));
            camera.Set(new CameraZoom(20));
            camera.Set(new OrbitalCamera(playerTransform, -1, 1, 3, new Vector3(0, 2, 12)));
            camera.Set(new Bindings("Camera"));
            camera.Set(new Controller());

            var player2 = World.CreateEntity();
            var playerTransform2 = new Transform(new Vector3(-4, -9, 0), 9);
            player2.Set(playerTransform2);
            player2.Set(new ModelRenderer(Content.Load<Model>("Models/Flemer")));
            player2.Set(effect.Clone());

            var ground = World.CreateEntity();
            ground.Set(new Transform(new Vector3(0, 0, 0)));
            ground.Set(new ModelRenderer(Content.Load<Model>("Models/Ground")));
            ground.Set(effect.Clone());

            var firstLight = World.CreateEntity();
            var firstPosition = new Vector3(0, 20, 0);
            firstLight.Set(new Transform(firstPosition, Vector3.Down));
            firstLight.Set(new Light(LightType.Directional, Color.Yellow, 2f));

            var secondLight = World.CreateEntity();
            var secondPosition = new Vector3(0, 0, 20);
            secondLight.Set(new Transform(secondPosition, Vector3.Forward));
            secondLight.Set(new Light(LightType.Directional, Color.Violet, 2f));

            var thirdLight = World.CreateEntity();
            var thirdPosition = new Vector3(0, 5, 0);
            thirdLight.Set(new Transform(thirdPosition, Vector3.Forward, 5));
            thirdLight.Set(new Light(LightType.Point, Color.Yellow, 2f));

            var fourthLight = World.CreateEntity();
            var fourthPosition = new Vector3(10, 5, 0);
            fourthLight.Set(new Transform(fourthPosition, Vector3.Forward, .2f));
            fourthLight.Set(new Light(LightType.Point, Color.LightCoral, 2f));

            var fifthLight = World.CreateEntity();
            var fifthPosition = new Vector3(-10, 5, 0);
            fifthLight.Set(new Transform(fifthPosition));
            fifthLight.Set(new Light(LightType.Point, Color.Aqua, 1f, 6f));

            var profiler = World.CreateEntity();
            profiler.Set(new GameInfo());
            profiler.Set(Content.Load<SpriteFont>("Fonts/Default"));
        }

        public override ISystem<GameTime> InitializeUpdateSystems()
        {
            return new SequentialSystem<GameTime>(
                new InputSystem(World, MainRunner), //must be before BindingsReadingSystem, PlayerControllingSystem and OrbitalCameraSystem
                new ControllerRegistrationSystem(World, MainRunner),
                new BindingsReadingSystem(World, MainRunner),
                new PlayerControllingSystem(World, MainRunner),
                new RotationSystem(World, MainRunner),
                new MovementSystem(World, MainRunner),
                new LookAtSystem(World, MainRunner),
                new OrbitalCameraSystem(World, MainRunner), //must be after movement system
                new CameraWorldToViewSystem(World, MainRunner),
                new FollowingSystem(World, MainRunner),
                new LightRegistrationSystem(World, MainRunner),
                new LightingSystem(World, MainRunner),
                new DebugSystem(World, MainRunner),
                new EffectsApplymentSystem(World, MainRunner)
            );
        }

        public override ISystem<GameTime> InitializeDrawSystems()
        {
            return new SequentialSystem<GameTime>(
                new EffectsDrawingSystem(World, MainRunner),
                new ModelRenderingSystem(World, MainRunner),
                new ProfilingSystem(SpriteBatch, World, MainRunner)
            );
        }
    }
}