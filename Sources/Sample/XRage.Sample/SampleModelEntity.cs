using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using AISTek.XRage.Components;

namespace AISTek.XRage.Sample
{
    public class SampleModelEntity : BaseEntity
    {
        public SampleModelEntity(XGame game, string modelPath, string materialPath)
            : base(game, "static_model:\"" + modelPath + "\"")
        {
            var renderComponent = new WorldModelRenderComponent(this, modelPath, materialPath);
            AddComponent(renderComponent);
            ActivateComponent(renderComponent);
        }
    }
}
