using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Configuration;

namespace AISTek.XRage.Graphics
{
    public class GraphicsSystem : XInterface
    {
        public GraphicsSystem(XGame game)
            : base(game, InterfaceType.Graphics)
        {
            Variables = new VariableManager(game);
            RenderChunkManager = new RenderChunkManager(game);
            RenderChunkProviders = new RenderChunkProviderManager(game);
            RenderingStatistics = new RenderingStatistics(game);
        }

        public void Initialize(GraphicsSettings settings)
        {
            GraphicsSettings = settings;
            InitializeGraphicsDevice();
            CreateRenderer();
        }
        
        public void InitializeGraphicsDevice()
        {
            // Here we do the following:
            // 1) Create GraphicsDevice with requested properties
            Game.GraphicsDeviceManager.PreferMultiSampling = true;
            Game.GraphicsDeviceManager.IsFullScreen = GraphicsSettings.IsFullScreen;
            Game.GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsSettings.BackBufferWidth;
            Game.GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsSettings.BackBufferHeight;
            Game.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = GraphicsSettings.SynchronizeWithVerticalRetrace;
            Game.GraphicsDeviceManager.ApplyChanges();

            // 2) Create SpriteBatch for 2D rendering
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // 3) Initialize RenderChunkManager
            RenderChunkManager.ClearChunks();            
            RenderChunkManager.PreallocateChunks(20, 1);
        }
        
        public void CreateRenderer()
        {
            // Here we do the following:
            // 1) Create instance of Renderer
            Renderer = RendererFactory.CreateRenderer(Game, GraphicsSettings);
            
            // 2) Initialize renderer
            Renderer.Initialize();
        }
        
        public void DrawFrame(ICamera camera, GameTime gameTime)
        {
            RenderingStatistics.Clear();
            camera.AssignCamera(this);
            MatrixOperations.Perform(Variables);
            Renderer.DrawFrame(camera, gameTime);
            RenderChunkManager.ClearChunks();
        }

        public void SaveSettings()
        {
            Renderer.SaveSettings();
        }

        internal IRendererFactory RendererFactory { get; set; }

        public GraphicsDevice GraphicsDevice { get { return Game.GraphicsDeviceManager.GraphicsDevice; } }

        public SpriteBatch SpriteBatch { get; private set; }

        public GraphicsSettings GraphicsSettings { get; private set; }

        public VariableManager Variables { get; private set; }

        public RenderChunkManager RenderChunkManager { get; private set; }

        public RenderChunkProviderManager RenderChunkProviders { get; private set; }

        public RenderingStatistics RenderingStatistics { get;private set; }

        public Renderer Renderer { get; private set; }
    }
}
