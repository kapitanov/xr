using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Messaging;

namespace AISTek.XRage.Components
{
    public class ParticleEmitter : BaseComponent
    {
        public ParticleEmitter(BaseEntity entity, string materialPath)
            : base(entity)
        {
            this.materialPath = materialPath;
            Opacity = 1.0f;
        }

        public Material Material { get; protected set; }

        public float Opacity
        {
            get { return opacity; }
            set { opacity = MathHelper.Clamp(value, 0.0f, 1.0f); }
        }

        public PreferredRenderOrder RenderOrder { get; set; }
    
        protected Matrix WorldTransform { get; set; }

        public override void LoadContent()
        {
            Material = Game.Content.Load<Material>(materialPath);
        }

        public override void Update(GameTime gameTime)
        {
            WorldTransform = Matrix.CreateScale(ParentEntity.Scale) *
                ParentEntity.Rotation *
                Matrix.CreateTranslation(ParentEntity.Position);
        }

        public override void QueryForChunks(ref RenderPassDescriptor pass)
        {
        }

        public override bool ExecuteMessage(IMessage message)
        {
            return false;
        }

        private string materialPath;
        private float opacity;
    }
}
