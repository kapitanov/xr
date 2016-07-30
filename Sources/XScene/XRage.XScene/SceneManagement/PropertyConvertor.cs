using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.SceneManagement
{
    public static class PropertyConvertor
    {
        public static string MaterialPath(string material)
        {
            return "materials/" + material;
        }

        public static string ModelPath(string model)
        {
            return "models/" + model;
        }

        public static string ScenePath(string scene)
        {
            return "levels/" + scene;
        }
    }
}
