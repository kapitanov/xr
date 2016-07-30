using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics.Contracts;

namespace AISTek.XRage.Graphics
{
    public abstract class Shader<TRenderer> : IDisposable
            where TRenderer : Renderer
    {
        protected Shader(TRenderer renderer)
        {
            Renderer = renderer;
        }

        public virtual void Initialize()
        { }
        
        protected void LoadShader(string path)
        {
            Effect = Game.Content.Load<Effect>(path);
        }

        protected EffectParameter GetParameterBySemantic(string semantic)
        {
            return Effect.Parameters.GetParameterBySemantic(semantic);
        }

        protected EffectParameter GetParameterByName(string name)
        {
            return Effect.Parameters[name];
        }

        protected void UseTechnique (string technique)
        {
            Effect.CurrentTechnique = Effect.Techniques[technique];
        }

        protected XGame Game { get { return Renderer.Game; } }

        protected TRenderer Renderer { get; private set; }

        protected Effect Effect { get; private set; }

        public virtual void Dispose()
        {
            Effect.Dispose();
        }
    }
}
