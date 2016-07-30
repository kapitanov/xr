using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Components;

namespace AISTek.XRage.Entities
{
    public class StaticModelEntity : BaseEntity
    {
        public StaticModelEntity(XGame game, string name, string modelPath, string materialPath)
            : base(game, name)
        {
            var renderComponent = new WorldModelRenderComponent(this, modelPath, materialPath);
            AddComponent(renderComponent);
            ActivateComponent(renderComponent);
        }
    }
}
