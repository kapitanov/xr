using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using AISTek.XRage.Graphics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using AISTek.XRage.Interfaces;
using AISTek.XRage.Composition;
using System.Threading;
using System.Diagnostics;

namespace AISTek.XRage.SceneManagement
{
    public class SceneManager : XComponent, IRenderChunkProvider, ICompositionScreen
    {
        public SceneManager(XGame game)
            : base(game)
        {
            Scenes = new List<Scene>();
            Game.Interfaces.AddInterface(new CameraInterface(game));
            IsPaused = false;
        }

        public event EventHandler SceneLoading;
        public event EventHandler SceneLoaded;
        public event EventHandler<SceneLoadingProgressEventArgs> SceneLoadingProgress;

        public bool IsPaused
        {
            get { return isPaused; }
            set
            {
                isPaused = value;
                if (ActiveScene != null)
                {
                    if (isPaused)
                    {
                        ActiveScene.Deactivate();
                    }
                    else
                    {
                        ActiveScene.Activate();
                    }
                }
            }
        }

        private bool isPaused = false;

        public IList<Scene> Scenes { get; private set; }

        public Scene ActiveScene
        {
            get { return activeScene; }
            set
            {
                if (activeScene != value)
                {
                    if (activeScene != null)
                    {
                        activeScene.Deactivate();
                    }

                    activeScene = value;
                    if (activeScene != null)
                    {
                        activeScene.Activate();
                    }
                }
            }
        }

        #region Scene updating

        public virtual void Update(GameTime gameTime)
        {
            if (IsPaused)
                return;

            if (ActiveScene != null &&
                !IsPaused)
            {
                ActiveScene.Update(gameTime);
            }

            // TODO: End the current physics frame

            // TODO: Begin the next physics frame
        }

        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            if (ActiveScene != null &&
                !IsPaused)
            {
                ActiveScene.QueryForChunks(ref pass);
            }
        }

        public void Shutdown()
        {
            foreach (var scene in Scenes)
            {
                scene.Shutdown();
            }
        }

        #endregion

        #region Interfaces and scenes management

        public void AddScene(Scene scene)
        {
            Scenes.Add(scene);
        }

        public void LoadScene(Scene scene)
        {
            AddScene(scene);
            ThreadPool.QueueUserWorkItem(_ =>
            {
                InvokeSceneLoading();

                scene.Initialize();
                scene.LoadContent();

                InvokeSceneLoaded();
            });
        }

        public void UnloadScene(Scene scene)
        {
            if (ActiveScene == scene)
                ActiveScene = null;

            Scenes.Remove(scene);
            scene.UnloadContent();
            scene.Shutdown();
        }

        #endregion

        #region Scene playback

        public void StartScene(Scene scene)
        {
            ActiveScene = scene;
        }

        public void StopScene(Scene scene)
        {
            scene.Deactivate();
        }

        #endregion

        private Scene activeScene;

        string ICompositionScreen.Name { get { return "scene_manager"; } }

        bool ICompositionScreen.IsEnabled { get { return (ActiveScene != null) && (!IsPaused); } }

        void ICompositionScreen.Initialize(VisualComposer visualComposer)
        { }

        void ICompositionScreen.LoadContent()
        { }

        void ICompositionScreen.Draw(GameTime gameTime)
        {
            Game.Graphics.DrawFrame(Game.Interfaces.QueryInterface<CameraInterface>().ActiveCamera, gameTime);
        }

        void ICompositionScreen.UnloadContent()
        {
            Shutdown();
        }

        public void UpdateLoadingState(LoadingState state)
        {
            InvokeSceneLoadingProgress(state);
        }

        private void InvokeSceneLoading()
        {
            var handler = SceneLoading;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void InvokeSceneLoaded()
        {
            var handler = SceneLoaded;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void InvokeSceneLoadingProgress(LoadingState state)
        {
            var handler = SceneLoadingProgress;
            if (handler != null)
            {
                handler(this, new SceneLoadingProgressEventArgs(state));
            }
        }
    }
}
