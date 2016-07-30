using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    public sealed class SpotLightShader : Shader<DeferredRenderer>
    {
        public SpotLightShader(DeferredRenderer renderer)
            : base(renderer)
        {
            Initialize();
        }

        public override void Initialize()
        {
            LoadShader("shaders/deferred_renderer/spot_light");

            matInvertViewProjection = GetParameterBySemantic("INV_VIEW_PROJECTION");
            vLightPosition = GetParameterBySemantic("LIGHT_POSITION");
            vLightDirection = GetParameterBySemantic("LIGHT_DIRECTION");
            fLightOuterConeAngle = GetParameterBySemantic("LIGHT_OUTER_CONE_ANGLE");
            fLightInnerConeAngle = GetParameterBySemantic("LIGHT_INNER_CONE_ANGLE");
            vCameraPosition = GetParameterBySemantic("CAMERA_POSITION");
            fLightRadius = GetParameterBySemantic("LIGHT_RADIUS");
            iFalloffType = GetParameterBySemantic("LIGHT_FALLOFF");
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
            vLightPosition.SetValue(light.Position);
            vLightDirection.SetValue(light.Direction);
            fLightOuterConeAngle.SetValue(light.OuterConeAngle);
            fLightInnerConeAngle.SetValue(light.InnerConeAngle);
            vCameraPosition.SetValue(Renderer.Camera.Position);
            fLightRadius.SetValue(light.Radius);
            iFalloffType.SetValue((int)light.Falloff);
            fLightIntensity.SetValue(light.Intensity);
            vHalfPixel.SetValue(Renderer.HalfPixel);
            cDiffuseColor.SetValue(light.DiffuseColor);
            cSpecularColor.SetValue(light.SpecularColor);
            texColorMap.SetValue(Renderer.GBuffer.DiffuseBuffer);
            texNormalMap.SetValue(Renderer.GBuffer.NormalBuffer);
            texDepthMap.SetValue(Renderer.GBuffer.DepthBuffer);
            texShadowMap.SetValue(shadowOcclusionMap);

            UseTechnique("RenderSpotLightWithShadowOcclusion");

            return Effect;
        }

        public Effect Bind(LightRenderChunk light)
        {
            matInvertViewProjection.SetValue(Renderer.CameraInViewProjection);
            vLightPosition.SetValue(light.Position);
            vLightDirection.SetValue(light.Direction);
            fLightOuterConeAngle.SetValue(light.OuterConeAngle);
            fLightInnerConeAngle.SetValue(light.InnerConeAngle);
            vCameraPosition.SetValue(Renderer.Camera.Position);
            fLightRadius.SetValue(light.Radius);
            iFalloffType.SetValue((int)light.Falloff);
            fLightIntensity.SetValue(light.Intensity);
            vHalfPixel.SetValue(Renderer.HalfPixel);
            cDiffuseColor.SetValue(light.DiffuseColor);
            cSpecularColor.SetValue(light.SpecularColor);
            texColorMap.SetValue(Renderer.GBuffer.DiffuseBuffer);
            texNormalMap.SetValue(Renderer.GBuffer.NormalBuffer);
            texDepthMap.SetValue(Renderer.GBuffer.DepthBuffer);

            UseTechnique("RenderSpotLight");

            return Effect;
        }

        private EffectParameter matInvertViewProjection;
        private EffectParameter vLightPosition;
        private EffectParameter vLightDirection;
        private EffectParameter fLightOuterConeAngle;
        private EffectParameter fLightInnerConeAngle;
        private EffectParameter vCameraPosition;
        private EffectParameter fLightRadius;
        private EffectParameter iFalloffType;
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
