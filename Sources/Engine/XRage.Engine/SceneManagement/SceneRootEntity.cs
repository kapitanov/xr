using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;

namespace AISTek.XRage.SceneManagement
{
    internal class SceneRootEntity : BaseEntity
    {
        public SceneRootEntity (XGame game)
            : base(game, "<scene_root>")
        { }
    }
}
