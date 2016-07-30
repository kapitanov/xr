using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.SceneManagement
{
    public abstract class Scene : XComponent
    {
        protected Scene(XGame game, string name)
            : base(game)
        {
            Name = name;
            IsActive = false;
            Entities = new Dictionary<long, BaseEntity>();
            SceneRoot = new SceneRootEntity(game);
        }

        #region Public properites

        public bool IsActive { get; private set; }

        public IDictionary<long, BaseEntity> Entities { get; private set; }

        public BaseEntity SceneRoot { get; private set; }

        public string Name { get; protected set; }

        #endregion
        
        #region Scene content management

        public virtual void Activate()
        {
            IsActive = true;
        }

        public virtual void Deactivate()
        {
            IsActive = false;
        }

        public void AddEntity(BaseEntity entity)
        {
            Entities.Add(entity.UniqueId, entity);
            entity.Scene = this;
        }

        public void AddEntities(params BaseEntity[] entities)
        {
            foreach (var entity in entities)
            {
                AddEntity(entity);
            }
        }

        public void RemoveEntity(BaseEntity entity)
        {
            entity.Shutdown();

            Entities.Remove(entity.UniqueId);
            entity.Scene = null;
        }

        public BaseEntity GetEntityByName(string name)
        {
            return Entities.Values.FirstOrDefault(entity => entity.Name == name);
        }

        #endregion

        #region Engine event handling

        public virtual void Initialize()
        {
            foreach (var entity in Entities.Values)
            {
                entity.Initialize();
            }
        }

        public virtual void LoadContent()
        {
            var counter = 0;
            foreach (var entity in Entities.Values)
            {
                Game.SceneManager.UpdateLoadingState(
                    new LoadingState(
                        Name, 
                        string.Format("Loading scene objects ({0})...", entity.Name), 
                        (int)(counter * 100.0f / Entities.Count)));
                entity.LoadContent();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var entity in Entities.Values)
            {
                entity.Update(gameTime);
            }
        }

        public virtual void QueryForChunks(ref RenderPassDescriptor pass)
        {
            foreach (var entity in Entities.Values)
            {
                entity.QueryForChunks(ref pass);
            }
        }

        public virtual void UnloadContent()
        {
            foreach (var entity in Entities.Values)
            {
                entity.UnloadContent();
            }
        }

        public virtual void Shutdown()
        {
            foreach (var entity in Entities.Values)
            {
                entity.Shutdown();
            }
        }

        #endregion
    }
}
