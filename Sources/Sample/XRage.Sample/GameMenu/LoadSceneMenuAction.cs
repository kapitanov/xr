using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.SceneManagement;

namespace AISTek.XRage.Sample.GameMenu
{
    public class LoadSceneMenuAction : BaseMenuAction
    {
        public LoadSceneMenuAction(XGame game, GameMenuItem menuItem, string xScenePath)
            : base(game, menuItem)
        {
            this.xScenePath = xScenePath;
        }

        public override void Execute()
        {
            var scene = new XScene(Game, xScenePath);
            Game.SceneManager.LoadScene(scene);
        }

        private string xScenePath;
    }
}
