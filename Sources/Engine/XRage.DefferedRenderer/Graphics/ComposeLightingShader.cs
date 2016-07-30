using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    public sealed class ComposeLightingShader : Shader<DeferredRenderer>
    {
        public ComposeLightingShader(DeferredRenderer renderer)
            : base(renderer)
        {
            Initialize();
        }

        public override void Initialize()
        {
            LoadShader("shaders/deferred_renderer/compose_light");

            vHalfPixel = GetParameterBySemantic("HALF_PIXEL_OFFSET");
            texColorMap = GetParameterBySemantic("GBUFFER_DIFFUSE_MAP");
            texLightingMap = GetParameterBySemantic("LIGHTING_MAP");
        }

        public Effect Bind()
        {
            vHalfPixel.SetValue(Renderer.HalfPixel);
            texColorMap.SetValue(Renderer.GBuffer.DiffuseBuffer);
            texLightingMap.SetValue(Renderer.LightBuffer);

            UseTechnique("ComposeLightingWithGBuffer");

            return Effect;
        }
        
        private EffectParameter vHalfPixel;
        private EffectParameter texColorMap;
        private EffectParameter texLightingMap;
    }
}
