using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;
using AISTek.XRage.Configuration;
using AISTek.XRage.SceneManagement;
using AISTek.XRage.Messaging;
using AISTek.XRage.Composition;
using AISTek.XRage.Physics;
using AISTek.XRage.InputManagement;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics.Contracts;

namespace AISTek.XRage
{
    public abstract class XGame : Game
    {
        protected XGame()
            : base()
        {
            SettingsStorage = new ConfigurationStorage(this);
            MessagingPoll = new MessagingPoll(this);
            InterfaceManager = new InterfaceManager();

            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Graphics = new GraphicsSystem(this);
            SceneManager = new SceneManager(this);
            VisualComposer = new VisualComposer(this);
        }

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        public new GraphicsDevice GraphicsDevice { get { return Graphics.GraphicsDevice; } }

        public GraphicsSystem Graphics { get; private set; }

        public ConfigurationStorage SettingsStorage { get; private set; }

        public Settings Settings { get; protected set; }

        public SceneManager SceneManager { get; protected set; }

        public MessagingPoll MessagingPoll { get; protected set; }

        public VisualComposer VisualComposer { get; private set; }

        public PhysicsManager Physics { get; private set; }

        public InputManager Input { get; private set; }

        public IInterfaceManager Interfaces { get { return InterfaceManager; } }

        private InterfaceManager InterfaceManager { get; set; }

        protected abstract IRendererFactory ProvideRendererFactory();

        protected override void Initialize()
        {
            // load settings
            Settings = SettingsStorage.LoadSettings();
            Content.RootDirectory = Settings.ContentSettings.ContentRootPath;

            // initialize graphics
            Graphics.RendererFactory = ProvideRendererFactory();
            Graphics.Initialize(Settings.GraphicsSettings);

            // Initialize action
            Input = new InputManager(this);
            Input.LoadInputConfiguration(SettingsStorage);

            // Initialize physics
            var physicsManagerFactory = (IPhysicManagerFactory)Activator.CreateInstance(Type.GetType(Settings.PhysicsSettings.PhysicsManagerFactory));
            Physics = physicsManagerFactory.CreatePhysicsManager(this);

            IsFixedTimeStep = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            VisualComposer.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Update interfaces
            InterfaceManager.Update(gameTime);

            // Update visuals
            VisualComposer.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Firstly update inputs
            Input.Update(gameTime);

            // Then update visuals
            VisualComposer.Draw(gameTime);
        }

        protected override void UnloadContent()
        {   
            VisualComposer.UnloadContent();
            InterfaceManager.Shutdown();

            // Save settings
            // TODO: currently saving is disabled

            Graphics.SaveSettings();
            Input.SaveInputConfiguration(SettingsStorage);
            SettingsStorage.SaveSettings(Settings);
        }

        public virtual void SwitchToMenu()
        {}

        public virtual void SwitchToScene()
        { }
    }
}
