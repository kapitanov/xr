using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Sample.GameMenu
{
    public abstract class BaseMenuAction : XComponent
    {
        protected BaseMenuAction(XGame game, GameMenuItem menuItem)
            : base(game)
        {
            MenuItem = menuItem;
        }

        public GameMenuItem MenuItem { get; private set; }

        public GameMenuScreen Menu { get { return MenuItem.Menu; } }

        public abstract void Execute();
    }
}
