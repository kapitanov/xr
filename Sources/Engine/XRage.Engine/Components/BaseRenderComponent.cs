using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using AISTek.XRage.Messaging;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Components
{
    public abstract class BaseRenderComponent : BaseComponent
    {
        protected BaseRenderComponent(BaseEntity entity, string materialPath)
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

        public bool CastsShadows { get; set; }

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
            if (!CheckVisibility(ref pass))
                return;

            if (pass.Type == RenderPassType.SolidGeometry &&
               Opacity == 1.0f)
            {
                ProvideGeometryChunk(ref pass);
            }

            if (pass.Type == RenderPassType.SemiTransparentGeometry &&
               Opacity < 1.0f)
            {
                ProvideGeometryChunk(ref pass);
            }

            if (pass.Type == RenderPassType.ShadowCasters &&
               CastsShadows)
            {
                ProvideGeometryChunk(ref pass);
            }

            if (pass.Type == RenderPassType.ShadowTargets &&
               !CastsShadows)
            {
                ProvideGeometryChunk(ref pass);
            }
        }

        public override bool ExecuteMessage(IMessage message)
        {
            return false;
        }

        protected abstract bool CheckVisibility(ref RenderPassDescriptor pass);

        protected abstract void ProvideGeometryChunk(ref RenderPassDescriptor pass);

        private string materialPath;
        private float opacity;
    }
}
