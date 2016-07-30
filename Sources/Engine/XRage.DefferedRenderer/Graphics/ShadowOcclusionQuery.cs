using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    internal class ShadowOcclusionQuery : IPoolItem, IDisposable
    {
        public ShadowOcclusionQuery()
        { }

        public static ShadowOcclusionQuery Create(
            ICamera renderCamera,
            LightRenderChunk light,
            bool useSoftShadow)
        {
            var query = XObjectPool.Acquire<ShadowOcclusionQuery>();
            query.RenderCamera = renderCamera;
            query.Light = light;
            query.UseSoftShadows = useSoftShadow;

            return query;
        }

        public bool UseSoftShadows { get; private set; }

        public ICamera RenderCamera { get; private set; }

        public ICamera LightCamera { get; private set; }

        public LightRenderChunk Light { get; private set; }

        public Texture2D ShadowOcclusion { get; private set; }

        public void SetLightCamera(ICamera lightCamera)
        {
            LightCamera = lightCamera;
        }

        public void SetShadowOcclusion(Texture2D shadowOcclusion)
        {
            ShadowOcclusion = shadowOcclusion;
        }

        public void Dispose()
        {
            XObjectPool.Release(this);
        }

        void IPoolItem.Release()
        {
            RenderCamera = null;
            LightCamera = null;
            Light = null;
            ShadowOcclusion = null;
        }

        void IPoolItem.Acquire()
        { }

        bool IPoolItem.IsHandled { get; set; }
    }
}
