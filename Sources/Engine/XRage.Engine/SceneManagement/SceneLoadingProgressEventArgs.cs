using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.SceneManagement
{
    public class SceneLoadingProgressEventArgs : EventArgs
    {
        public SceneLoadingProgressEventArgs(LoadingState loadingState)
        {
            LoadingState = loadingState;
        }

        public LoadingState LoadingState { get; private set; }
    }
}
