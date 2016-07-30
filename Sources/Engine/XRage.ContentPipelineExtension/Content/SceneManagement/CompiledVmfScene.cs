using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.SceneManagement
{
    public class CompiledVmfScene
    {
        public CompiledVmfScene(string name)
        {
            Name = name;
            StaticEntities = new List<CompiledVmfStaticEntity>();
            Brushes = new List<CompiledVmfBrush>();
        }

        public string Name { get; private set; }

        public IList<CompiledVmfStaticEntity> StaticEntities { get; internal set; }

        public IList<CompiledVmfBrush> Brushes { get; internal set; }

        internal int BrushesSkipped { get; set; }
    }
}
