using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;
using AISTek.XRage.Content.SceneManagement;
using System.Diagnostics;

namespace AISTek.XRage.SceneManagement
{
    public class XScene : Scene
    {
        public XScene(XGame game, string scenePath, IEntityFactory entityFactory)
            : base(game, "<x_scene>")
        {
            this.scenePath = scenePath;
            World = new XWorld(Game);
            EntityFactory = entityFactory;
        }

        public XScene(XGame game, string scenePath)
            : this(game, scenePath, new DefaultEntityFactory(game))
        { }

        public XWorld World { get; private set; }

        protected IEntityFactory EntityFactory { get; private set; }

        public override void LoadContent()
        {
            var compiledScene = Game.Content.Load<CompiledXScene>(scenePath);
            base.Name = compiledScene.Properties["scene_name"];
            var progress = new LoadingProgress(this);

            World.LoadContent(compiledScene.World, progress);

            var itemIndex = 0;
            foreach (var xEntity in compiledScene.Entities)
            {
                progress.Update(Stage.LoadingEntities, itemIndex++, compiledScene.Entities.Count);

                var entity = EntityFactory.CreateEntity(xEntity);
                if (entity == null)
                {
                    Trace.TraceWarning("Unable to create entity {0}. Entity type is unknown.", xEntity.ClassName);
                }
                else
                {
                    AddEntity(entity);
                }
            }
            
            itemIndex = 0;
            foreach (var entity in Entities.Values)
            {
                progress.Update(Stage.InitializingEntities, itemIndex++, compiledScene.Entities.Count);
                
                entity.Initialize();
                entity.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.Input.GetAction("menu.switchMenu").IsOn)
            {
                Game.SwitchToMenu();
                return;
            }

            foreach (var entity in Entities.Values)
            {
                entity.Update(gameTime);
            }
        }

        public override void QueryForChunks(ref RenderPassDescriptor pass)
        {
            World.QueryForChunks(ref pass);

            foreach (var entity in Entities.Values)
            {
                entity.QueryForChunks(ref pass);
            }
        }

        public override void UnloadContent()
        {
            World.UnloadContent();
            base.UnloadContent();
        }

        public override void Activate()
        {
            base.Activate();
            Game.Input.IsMouseLocked = true;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            Game.Input.IsMouseLocked = false;
        }

        private string scenePath;
    }
}
