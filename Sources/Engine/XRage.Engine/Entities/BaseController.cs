using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Entities
{
    public abstract class BaseController : BaseEntity
    {
        protected BaseController(XGame game, string name, string controlledEntityName, BaseEntity parentEntity)
            : base(game, name, parentEntity)
        {
            this.controlledEntityName = controlledEntityName;
        }

        protected BaseController(XGame game, string name, string controlledEntityName)
            : base(game, name, null)
        {
            this.controlledEntityName = controlledEntityName;
        }

        public BaseEntity ControlledEntity { get; private set; }

        public override void LoadContent()
        {
            ControlledEntity = Scene.GetEntityByName(controlledEntityName);
            if (ControlledEntity == null)
            {
                Trace.TraceError(
                    "{0} failed to bind to the target entity \"{1}\". The specified entity doesn't exist.",
                    this.GetType(),
                    controlledEntityName);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (ControlledEntity != null)
            {
                UpdateControlledEntity(gameTime);
            }
        }

        protected abstract void UpdateControlledEntity(GameTime gameTime);

        private string controlledEntityName;
    }
}
