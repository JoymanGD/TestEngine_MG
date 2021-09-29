using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common.Settings;
using Common.ECS.Components;
using Common.ECS.Systems;
using DefaultEcs.System;
using Common.ECS.SceneManagement;

namespace Common.Core.Scenes
{
    public class TestScreen : ECSScreen
    {
        public TestScreen(byBullet game) : base(game) { }

        public override void LoadContent()
        {
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
            var shader = Content.Load<Effect>("Effects/TestShader");

            var player = World.CreateEntity();
            player.Set(new Transform(Vector3.Zero));
            player.Set(new ModelRenderer(Content.Load<Model>("Models/Mage")));
            player.Set(shader.Clone());
            var playerBindings = new Bindings("Player");
            player.Set(playerBindings);
            player.Set(new Controller(playerBindings));
            player.Set(new Animation("Armature|Idle"));
            player.Set(new Player(0));
            
            var player2 = World.CreateEntity();
            player2.Set(new Transform(Vector3.Left * 4, Vector3.Left, Vector3.Up));
            player2.Set(new ModelRenderer(Content.Load<Model>("Models/byBulletMan_v_3_2")));
            player2.Set(shader.Clone());

            var camera = World.CreateEntity();
            var cameraTransform = new Transform(new Vector3(0, 0, 20), Vector3.Forward, Vector3.Up);
            camera.Set(cameraTransform);
            camera.Set(new Camera(graphics, cameraTransform.WorldMatrix));
            var cameraBindings = new Bindings("Camera");
            camera.Set(cameraBindings);
            camera.Set(new Controller(cameraBindings));

            var firstLight = World.CreateEntity();
            firstLight.Set(new Transform(new Vector3(0, 20, 0), Vector3.Down, Vector3.Backward));
            firstLight.Set(new Light(LightType.Directional, Color.Yellow, 2f));

            var secondLight = World.CreateEntity();
            secondLight.Set(new Transform(new Vector3(0, 0, 20), Vector3.Forward, Vector3.Up));
            secondLight.Set(new Light(LightType.Directional, Color.Violet, 2f));

            var thirdLight = World.CreateEntity();
            thirdLight.Set(new Transform(new Vector3(-30, 0, 0), Vector3.Right, Vector3.Up));
            thirdLight.Set(new Light(LightType.Directional, Color.Blue, 2f));

            // var fourthLight = World.CreateEntity();
            // fourthLight.Set(new Transform(new Vector3(10, 0, 0), Vector3.Zero, 20));
            // fourthLight.Set(new Light(LightType.Point, Color.Red, 2f));
        }

        public override ISystem<GameTime> InitializeUpdateSystems()
        {
            return new SequentialSystem<GameTime>(
                new InputSystem(World, MainRunner),
                new PlayerControllingSystem(World, MainRunner),
                new MovementSystem(World, MainRunner),
                new RotationSystem(World, MainRunner),
                new LookAtRegistrationSystem(World, MainRunner),
                // new CameraControllingSystem(World, MainRunner),
                new CameraWorldToViewSystem(World, MainRunner),
                new LightRegistrationSystem(World, MainRunner),
                new AnimationRegistrationSystem(World, MainRunner),
                new AnimationPlayingSystem(World, MainRunner),
                new LightingSystem(World, MainRunner),
                // new DebugSystem(World, MainRunner),
                new EffectsApplymentSystem(World, MainRunner)
            );
        }

        public override ISystem<GameTime> InitializeDrawSystems()
        {
            return new SequentialSystem<GameTime>(
                new EffectsDrawingSystem(World, MainRunner),
                new ModelRenderingSystem(World, MainRunner)
            );
        }
    }
}