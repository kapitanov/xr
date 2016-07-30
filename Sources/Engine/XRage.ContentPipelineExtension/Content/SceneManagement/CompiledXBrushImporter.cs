using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.SceneManagement;
using System.Xml.Linq;

namespace AISTek.XRage.Content.SceneManagement
{
    internal class CompiledXBrushImporter
    {
        public CompiledXBrush Process(XElement brushData)
        {
            var materialPath = brushData.Attribute("material").Value;

            var useSmoothing = brushData.ReadBoolAttribute("useSmoothing", defaultValue: false);

            var faces = from faceData in brushData.Descendants("brush.faces")
                                                  .Descendants("face")
                        let vertices = from vertexData in faceData.Descendants("face.vertices")
                                                                  .Descendants("vertex")
                                       select vertexData.ReadVector3()
                        let uvData = faceData.Descendants("face.uv_data").First()
                        let uAxis = uvData.Descendants("u_axis").First().ReadVector3()
                        let vAxis = uvData.Descendants("v_axis").First().ReadVector3()
                        let uvScale = uvData.Descendants("uv_scale").First().ReadVector2()

                        select new CompiledXFace(vertices, uAxis, vAxis, uvScale);

            return new CompiledXBrush(materialPath, faces, useSmoothing);
        }
    }
}
