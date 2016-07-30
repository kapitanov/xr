using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.SceneManagement
{
    internal class LoadingProgress
    {
        public LoadingProgress(XScene scene)
        {
            this.scene = scene;
            ObjectName = string.Empty;
        }

        public Stage Stage { get; set; }

        public int ProgressValue { get; set; }

        public int MaxProgress { get; set; }

        public string ObjectName { get; set; }

        public void Update(int progressValue, int maxProgress)
        {
            Update(Stage, progressValue, maxProgress, ObjectName);
        }

        public void Update(Stage stage, int progressValue = 0, int maxProgress = 0)
        {
            Update(stage, progressValue, maxProgress, ObjectName);
        }

        public void Update(Stage stage, int progressValue, int maxProgress, string objectName)
        {
            Stage = stage;
            ProgressValue = progressValue;
            MaxProgress = maxProgress;
            ObjectName = objectName;

            Update();
        }

        public void Update()
        {
            var progress = (int)(ProgressValue * 100.0 / (MaxProgress * (StagesCount() - (int)Stage)));
            scene.Game.SceneManager.UpdateLoadingState(new LoadingState(
                scene.Name,
                string.Format(StageName(Stage), ObjectName, ProgressValue, MaxProgress),
                progress));
        }

        private static int StagesCount()
        {
            return (int)Stage.StagesCount;
        }

        private static string StageName(Stage stage)
        {
            switch (stage)
            {
                case Stage.LoadingBrushes:
                    return "Loading brushes ({1}/{2})...";

                case Stage.LoadingEntities:
                    return "Loading entity \"{0}\" ({1}/{2})...";

                case Stage.InitializingEntities:
                    return "Initializing entity \"{0}\" ({1}/{2})...";


                case Stage.LoadingTerrain:
                    return "Loading terrain...";

                default:
                    return "Loading <?>";
            }
        }

        private readonly XScene scene;
    }
}