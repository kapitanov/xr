using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.Entities
{
    public abstract class BaseSky : BaseEntity
    {
        protected BaseSky(XGame game, string skyMaterialPath)
            : base(game, "sky")
        {
            this.skyMaterialPath = skyMaterialPath;
        }

        public Material Material { get; private set; }

        public override void LoadContent()
        {
            base.LoadContent();
            Material = Game.Content.Load<Material>(skyMaterialPath);
        }

        private string skyMaterialPath;
    }
}
