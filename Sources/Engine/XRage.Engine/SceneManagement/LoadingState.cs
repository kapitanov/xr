using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.SceneManagement
{
    public class LoadingState
    {
        public LoadingState(string scene, string stage, int progress)
        {
            Scene = scene;
            Stage = stage;
            Progress = progress;
        }

        public string Scene { get; private set; }

        public string Stage { get; private set; }

        public int Progress { get; private set; }
    }
}
