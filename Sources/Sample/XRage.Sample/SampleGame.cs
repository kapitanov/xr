using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AISTek.XRage.Graphics;
using AISTek.XRage.Interfaces;
using AISTek.XRage.Entities;
using AISTek.XRage.Composition;
using System.Diagnostics;
using AISTek.XRage.SceneManagement;
using AISTek.XRage.Sample.GameMenu;

namespace AISTek.XRage.Sample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SampleGame : XGame
    {
        public SampleGame()
        { }

        protected override IRendererFactory ProvideRendererFactory()
        {
            return new DeferredRendererFactory();
        }

        protected override void Initialize()
        {
            VisualComposer.AddScreen(SceneManager);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Graphics.RenderChunkProviders.RegisterRenderChunkProvider(SceneManager);

            cameraInformationScreen = new CameraInformationScreen(this);
            preloader = new PreloaderScreen(this) { IsEnabled = false };
            renderingStatisticsScreen = new RenderingStatisticsScreen(this);
            pausedScreen = new PausedScreen(this);
            gameMenu = new GameMenuScreen(this, "./conf/menu.xml", "mainMenu") { IsEnabled = true };

            VisualComposer.AddScreen(new FpsCounterScreen(this));
            VisualComposer.AddScreen(renderingStatisticsScreen);
            VisualComposer.AddScreen(cameraInformationScreen);
            VisualComposer.AddScreen(preloader);
            VisualComposer.AddScreen(pausedScreen);
            //VisualComposer.AddScreen(gameMenu);
           
            SceneManager.SceneLoading += (_, e) =>
            {
                preloader.IsEnabled = true;
                cameraInformationScreen.IsEnabled = false;
                renderingStatisticsScreen.IsEnabled = false;
                gameMenu.IsEnabled = false;
            };

            SceneManager.SceneLoaded += (_, e) =>
            {
                lock (restarts)
                {
                    restarts.Add(new RestartQuery());
                }
            };

            Activated += (_, e) =>
            {
                SceneManager.IsPaused = false;
                pausedScreen.IsEnabled = false;
            };

            Deactivated += (_, e) =>
            {
                SceneManager.IsPaused = true;
                pausedScreen.IsEnabled = true;
            };

            CreateScene();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        
        protected override void Update(GameTime gameTime)
        {
            gameMenu.IsEnabled = !SceneManager.IsPaused;

            base.Update(gameTime);



            lock (restarts)
            {
                if (restarts.Count > 0)
                {
                    restarts.RemoveAt(0);

                    preloader.IsEnabled = false;
                    SceneManager.StartScene(scene);
                    cameraInformationScreen.IsEnabled = true;
                    renderingStatisticsScreen.IsEnabled = true;
                }
            }

            var currentKeyState = Keyboard.GetState();
            lock (restarts)
            {
                if (restarts.Count == 0)
                {
                    if (currentKeyState.IsKeyDown(Keys.Space) &&
                        prevKeyState.IsKeyUp(Keys.Space))
                    {
                        CreateScene();
                    }

                    if (currentKeyState.IsKeyDown(Keys.Enter) &&
                        prevKeyState.IsKeyUp(Keys.Enter))
                    {
                        CreateVmfScene();
                    }
                }
            }

            if (currentKeyState.IsKeyDown(Keys.Insert))
                Graphics.Renderer.IsDebugVisualMode = true;

            if (currentKeyState.IsKeyDown(Keys.Delete))
                Graphics.Renderer.IsDebugVisualMode = false;


            prevKeyState = currentKeyState;
        }

        private void CreateScene()
        {
            if (SceneManager.ActiveScene != null)
            {
                var activeScene = SceneManager.ActiveScene;
                SceneManager.ActiveScene = null;
                SceneManager.UnloadScene(activeScene);
                scene = null;
                activeScene = null;

            }

            scene = new SampleScene(this, string.Format("scene_{0}", scenesCounter++));

            Interfaces.QueryInterface<CameraInterface>().ActiveCamera = new SampleControlledCamera(this)
            {
                Position = Vector3.Zero,
                FarClippingPlane = 100000.0f
            };

            SceneManager.LoadScene(scene);
        }

        private void CreateVmfScene()
        {
            if (SceneManager.ActiveScene != null)
            {
                var activeScene = SceneManager.ActiveScene;
                SceneManager.ActiveScene = null;
                SceneManager.UnloadScene(activeScene);
                scene = null;
                activeScene = null;

            }

            VmfScene.StaticGame = this;
            scene = null;

            // scene = Content.LoadSettings<VmfScene>("levels/temporary");
            //  scene = Content.LoadSettings<VmfScene>("levels/test");
            scene = new XScene(this, @"levels\test_scene\scene");
          //  scene = new XScene(this, @"levels\gulscene\scene");


            Interfaces.QueryInterface<CameraInterface>().ActiveCamera = new SampleControlledCamera(this)
            {
                Position = Vector3.Zero,
                FarClippingPlane = 100000.0f
            };

            SceneManager.LoadScene(scene);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void SwitchToMenu()
        {
            gameMenu.IsEnabled = true;
            SceneManager.IsPaused = true;
        }

        public override void SwitchToScene()
        {
            gameMenu.IsEnabled = false;
            SceneManager.IsPaused = false;
        }

        private Scene scene;
        private KeyboardState prevKeyState;
        private CameraInformationScreen cameraInformationScreen;
        private RenderingStatisticsScreen renderingStatisticsScreen;
        private PausedScreen pausedScreen;
        private PreloaderScreen preloader;
        private GameMenuScreen gameMenu;
        private int scenesCounter = 0;

        private List<RestartQuery> restarts = new List<RestartQuery>();
        private class RestartQuery { }
    }
}
