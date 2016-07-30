using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using AISTek.XRage.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace AISTek.XRage.SceneManagement
{
    public class VmfSceneReader : ContentTypeReader<VmfScene>
    {
        public override bool CanDeserializeIntoExistingObject
        {
            get { return false; }
        }

        protected override VmfScene Read(ContentReader input, VmfScene existingInstance)
        {
            var scene = new VmfScene("vmf_scene");
            var brushesCount = input.ReadInt32();
            for (var i = 0; i < brushesCount; i++)
            {
                var materialPath = "materials/" + input.ReadString();

                // TODO: Temporary
                materialPath = "materials/temporary";

                var sidesCount = input.ReadInt32();
                var vertices = new List<VertexPositionNormalTexture>(sidesCount * 3);

                for (var j = 0; j < sidesCount; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        var positon = input.ReadVector3();
                        var texCoord = input.ReadVector2();
                        var normal = input.ReadVector3();

                        vertices.Add(new VertexPositionNormalTexture
                        {
                            Position = positon,
                            TextureCoordinate = texCoord,
                            Normal = normal
                        });
                    }
                }

                var brush = new VmfBrush(scene, vertices.ToArray(), materialPath);
                scene.Brushes.Add(brush);
                //    Thread.Sleep(75);
            }

            return scene;
        }
    }
}
