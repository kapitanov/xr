using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    public sealed class DirectionalLightShader : Shader<DeferredRenderer>
    {
        public DirectionalLightShader(DeferredRenderer renderer)
            : base(renderer)
        {
            Initialize();
        }

        public override void Initialize()
        {
            LoadShader("shaders/deferred_renderer/directional_light");

            matInvertViewProjection = GetParameterBySemantic("INV_VIEW_PROJECTION");
            vLightDirection = GetParameterBySemantic("LIGHT_DIRECTION");
            vCameraPosition = GetParameterBySemantic("CAMERA_POSITION");
            fLightIntensity = GetParameterBySemantic("LIGHT_INTENSITY");
            vHalfPixel = GetParameterBySemantic("HALF_PIXEL_OFFSET");
            cDiffuseColor = GetParameterBySemantic("LIGHT_DIFFUSE_COLOR");
            cSpecularColor = GetParameterBySemantic("LIGHT_SPECULAR_COLOR");
            texColorMap = GetParameterBySemantic("GBUFFER_DIFFUSE_MAP");
            texNormalMap = GetParameterBySemantic("GBUFFER_NORMAL_MAP");
            texDepthMap = GetParameterBySemantic("GBUFFER_DEPTH_MAP");
            texShadowMap = GetParameterBySemantic("SHADOW_OCCLUSION_MAP");
        }

        public Effect Bind(LightRenderChunk light, Texture2D shadowOcclusionMap)
        {
            matInvertViewProjection.SetValue(Renderer.CameraInViewProjection);
            vLightDirection.SetValue(light.Direction);
            vCameraPosition.SetValue(Renderer.Camera.Position);
            fLightIntensity.SetValue(light.Intensity);
            vHalfPixel.SetValue(Renderer.HalfPixel);
            cDiffuseColor.SetValue(light.DiffuseColor);
            cSpecularColor.SetValue(light.SpecularColor);
            texColorMap.SetValue(Renderer.GBuffer.DiffuseBuffer);
            texNormalMap.SetValue(Renderer.GBuffer.NormalBuffer);
            texDepthMap.SetValue(Renderer.GBuffer.DepthBuffer);
            texShadowMap.SetValue(shadowOcclusionMap);

            UseTechnique("RenderDirectionalLightWithShadowOcclusion");

            return Effect;
        }

        public Effect Bind(LightRenderChunk light)
        {
            matInvertViewProjection.SetValue(Renderer.CameraInViewProjection);
            vLightDirection.SetValue(light.Direction);
            vCameraPosition.SetValue(Renderer.Camera.Position);
            fLightIntensity.SetValue(light.Intensity);
            vHalfPixel.SetValue(Renderer.HalfPixel);
            cDiffuseColor.SetValue(light.DiffuseColor);
            cSpecularColor.SetValue(light.SpecularColor);
            texColorMap.SetValue(Renderer.GBuffer.DiffuseBuffer);
            texNormalMap.SetValue(Renderer.GBuffer.NormalBuffer);
            texDepthMap.SetValue(Renderer.GBuffer.DepthBuffer);

            UseTechnique("RenderDirectionalLight");

            return Effect;
        }
        
        private EffectParameter matInvertViewProjection;
        private EffectParameter vLightDirection;
        private EffectParameter vCameraPosition;
        private EffectParameter fLightIntensity;
        private EffectParameter vHalfPixel;
        private EffectParameter cDiffuseColor;
        private EffectParameter cSpecularColor;
        private EffectParameter texColorMap;
        private EffectParameter texNormalMap;
        private EffectParameter texDepthMap;
        private EffectParameter texShadowMap;
    }
}
