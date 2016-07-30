using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Composition;
using Microsoft.Xna.Framework;
using AISTek.XRage.InputManagement;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Sample.GameMenu
{
    public class GameMenuScreen : XComponent, ICompositionScreen
    {
        public GameMenuScreen(XGame game, string menuPath, string menuName)
            : base(game)
        {
            this.menuPath = menuPath;
            this.menuName = menuName;

            prevItemAction = Game.Input.GetAction("menu.prevItem");
            nextItemAction = Game.Input.GetAction("menu.nextItem");
            execItemAction = Game.Input.GetAction("menu.exec");

            ActionFactory = new MenuActionFactory();
        }

        public string Name { get { return "main_menu"; } }

        public bool IsEnabled { get; set; }

        public IList<GameMenuItem> Items { get; private set; }

        public GameMenuItem SelectedItem { get; private set; }

        public IMenuActionFactory ActionFactory { get; private set; }

        public SpriteFont MenuFont { get; private set; }

        public void Initialize(VisualComposer visualComposer)
        { }

        public void LoadContent()
        {
            MenuFont = Game.Content.Load<SpriteFont>("sprites/fonts/preloader");
            LoadMenu();
        }

        public void Update(GameTime gameTime)
        {
            if (prevItemAction.IsOn)
            {
                if (selectedItemIndex > 0)
                    selectedItemIndex--;
            }
            else  if (nextItemAction.IsOn)
            {
                if (selectedItemIndex < Items.Count - 1)
                    selectedItemIndex++;
            } 
            else if (execItemAction.IsOn)
            {
                SelectedItem.Action.Execute();
            }

            SelectedItem = Items[selectedItemIndex];
        }

        public void Draw(GameTime gameTime)
        {
            Game.Graphics.SpriteBatch.Begin();

            var itemIndex = 0;
            foreach (var item in Items)
            {
                item.IsSelected = item == SelectedItem;
                item.Draw(gameTime, itemIndex++);
            }

            Game.Graphics.SpriteBatch.End();
        }

        public void UnloadContent()
        { }

        private void LoadMenu()
        {
            var document = XDocument.Load(menuPath);
            Items = document.Descendants("menu").First()
                            .Descendants(menuName)
                            .Descendants("item")
                            .Select(node => new GameMenuItem(this, node))
                            .ToList();
        }

        private int selectedItemIndex;
        private string menuPath;
        private string menuName;

        private NamedAction prevItemAction;
        private NamedAction nextItemAction;
        private NamedAction execItemAction;
    }
}
