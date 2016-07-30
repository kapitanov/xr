using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;
using AISTek.XRage.Messaging;

namespace AISTek.XRage.Components
{
    public abstract class BaseComponent
    {
        protected BaseComponent(BaseEntity parent)
        {
            ParentEntity = parent;
            Initialize();
        }

        #region Public properties

        public bool IsActive { get; private set; }

        public BaseEntity ParentEntity { get; private set; }

        protected XGame Game { get { return ParentEntity.Game; } }

        #endregion

        #region Component state management

        public virtual void Initialize()
        {
            ParentEntity.AddComponent(this);
        }

        protected void ActivateComponent()
        {
            if (!IsActive)
            {
                IsActive = true;
                ParentEntity.ActivateComponent(this);
            }
        }

        protected void DeactivateComponent()
        {
            if (IsActive)
            {
                IsActive = false;
                ParentEntity.DeactivateComponent(this);
            }
        }

        public virtual void Shutdown()
        { }

        #endregion

        #region Engine event handing

        public abstract bool ExecuteMessage(IMessage message);

        public virtual void LoadContent()
        { }

        public virtual void UnloadContent()
        { }

        public virtual void Update(GameTime gameTime)
        { }

        public virtual void QueryForChunks(ref RenderPassDescriptor desc)
        { }

        #endregion
    }
}
