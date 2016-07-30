using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Descriptor for one rendering pass of a graphics frame.
    /// </summary>
    public struct RenderPassDescriptor
    {
        /// <summary>
        /// The <see cref="BaseEntity"/> defining the view to render.
        /// </summary>
        public ICamera RenderCamera;

        /// <summary>
        /// Describes what kind of rendering pass this is.
        /// </summary>
        public RenderPassType Type;

        /// <summary>
        /// LOD that this render pass prefers.
        /// </summary>
        public LevelOfDetail RequestedLod;

        public static RenderPassDescriptor SolidGeometry(ICamera renderCamera)
        {
            return new RenderPassDescriptor { RenderCamera = renderCamera, Type = RenderPassType.SolidGeometry };
        }
        
        public static RenderPassDescriptor ShadowMap(ICamera renderCamera)
        {
            return new RenderPassDescriptor { RenderCamera = renderCamera, Type = RenderPassType.ShadowCasters };
        }

        public static RenderPassDescriptor Lighting(ICamera renderCamera)
        {
            return new RenderPassDescriptor { RenderCamera = renderCamera, Type = RenderPassType.Lighting };
        }
    }
}
