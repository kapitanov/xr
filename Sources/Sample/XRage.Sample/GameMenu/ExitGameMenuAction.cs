using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Sample.GameMenu
{
    public class ExitGameMenuAction : BaseMenuAction
    {
        public ExitGameMenuAction(XGame game, GameMenuItem menuItem)
            : base(game, menuItem)
        { }

        public override void Execute()
        {
            Game.Exit();
        }
    }
}
