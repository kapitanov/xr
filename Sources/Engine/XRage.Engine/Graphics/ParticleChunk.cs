using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    public class ParticleChunk : IRenderChunk
    {
        public ParticleChunk()
        {
            ParticlePositions = new List<Vector3>();
        }

        public List<Vector3> ParticlePositions { get; private set; }

        /// <summary>
        /// The world transformation matrix of the geometry chunk.
        /// </summary>
        public Matrix WorldTransform { get; set; }

        /// <summary>
        /// The material assigned to the geometry chunk.
        /// </summary>
        public Material Material { get; set; }

        public float Opacity { get; set; }

        public void Recycle()
        {
            ParticlePositions.Clear();
            Material = null;
            WorldTransform = Matrix.Identity;
        }
    }
}
