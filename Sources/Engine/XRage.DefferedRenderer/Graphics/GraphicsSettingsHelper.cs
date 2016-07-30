using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Configuration;

namespace AISTek.XRage.Graphics
{
    internal static class GraphicsSettingsHelper
    {
        public static GBufferFormat GetGBufferFormat(this GraphicsSettings settings)
        {
            var value = settings.RendererSettings.GetNode("gbuffer")["format"];
            switch (value)
            {
                case "32bit":
                    return GBufferFormat.Format32Bit;

                default:
                    throw new InvalidOperationException("Unknown GBuffer format.");
            }
        }

        public static int GetShadowMapSize(this GraphicsSettings settings)
        {
            var value = settings.RendererSettings.GetNode("shadows")["size"];
            int result;

            if (!int.TryParse(value, out result))
                throw new InvalidOperationException("Wrong shadow map size.");

            return result;
        }

        public static void SetGBufferFormat(this GraphicsSettings settings, GBufferFormat format)
        {
            string value;
            if (format == GBufferFormat.Format32Bit)
            {
                value = "32bit";
            }
            else
            {
                throw new InvalidOperationException("Unable to write GBuffer format");
            }
            
            settings.RendererSettings.GetNode("gbuffer")["format"] = value;
        }

        public static void SetShadowMapSize(this GraphicsSettings settings, int value)
        {
            settings.RendererSettings.GetNode("shadows")["size"]=value.ToString();
        }
    }
}
