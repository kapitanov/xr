using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Content.SceneManagement
{
    public class CompiledXFace
    {
        public CompiledXFace(IEnumerable<Vector3> vertices, Vector3 uAxis, Vector3 vAxis, Vector2 uvScale)
        {
            Positions = vertices.Reverse().ToList();
            ComputeTexCoords(Vector3.Normalize(uAxis), Vector3.Normalize(vAxis), uvScale * 0.01f);
            ComputeNormal();
            ComputeBinormalTangent();
        }

        public IList<Vector3> Positions { get; private set; }

        public IList<Vector2> TexCoords { get; private set; }

        public Vector3 Normal { get; private set; }

        public Vector3 Binormal { get; private set; }

        public Vector3 Tangent { get; private set; }

        private void ComputeTexCoords(Vector3 u, Vector3 v, Vector2 uvScale)
        {
            var texCoords = from vertex in Positions
                            let proj = new Vector2(
                                Vector3.Dot(vertex, u),
                                Vector3.Dot(vertex, v))
                            select proj * uvScale;

            TexCoords = new List<Vector2>(texCoords);
        }

        private void ComputeNormal()
        {
            Normal = -Vector3.Normalize(new Plane(Positions[0], Positions[1], Positions[2]).Normal);
        }

        private void ComputeBinormalTangent()
        {
            Tangent = Vector3.Normalize(Positions[1] - Positions[0]);
            Binormal = Vector3.Normalize(-Vector3.Cross(Normal, Tangent));
        }
    }
}
