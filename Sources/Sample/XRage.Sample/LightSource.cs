using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Sample
{
    public class LightSource : IRenderChunkProvider
    {
        public LightSource(XGame game, Color diffuseColor, Color specularColor)
        {
            Game = game;
            DiffuseColor = diffuseColor;
            SpecularColor = specularColor;
            Intensity = 1.0f;
            Type = LightType.Directional;
        }

        public XGame Game { get; private set; }

        public Vector3 Position { get; set; }

        public Vector3 Direction { get; set; }

        public LightType Type { get; set; }

        public Color DiffuseColor { get; set; }

        public Color SpecularColor { get; set; }

        public float Intensity { get; set; }

        public float Radius { get; set; }

        public FalloffType Falloff { get; set; }

        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            var chunk = Game.Graphics.RenderChunkManager.AllocateLightChunk();

            chunk.Position = Position;
            chunk.Direction = Direction;
            chunk.Type = Type;
            chunk.DiffuseColor = DiffuseColor.ToVector3();
            chunk.SpecularColor = SpecularColor.ToVector3();
            chunk.Intensity = Intensity;
            chunk.Radius = Radius;
            chunk.Falloff = Falloff;
        }
    }
}
