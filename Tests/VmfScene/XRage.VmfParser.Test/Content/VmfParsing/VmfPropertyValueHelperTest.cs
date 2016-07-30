using AISTek.XRage.Content.VmfParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Xna.Framework;
using System.Globalization;

namespace AISTek.XRage.Content.VmfParsing
{
    [TestClass]
    public class VmfPropertyValueHelperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ReadVector3Test()
        {
            var expected = new Vector3(10.0f, -20.0f, 0.5f);
            var name = "vector";
            var node = new VmfClassNode("",
                new VmfPropertyNode(name,
                    string.Format("{0} {1} {2}", expected.X, expected.Y, expected.Z)));

            var actual = VmfPropertyValueHelper.ReadVector3(node, name);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ReadVertices
        ///</summary>
        [TestMethod()]
        public void ReadVerticesTest()
        {
            var expected = new[]
            {
                new Vector3(10.0f, -20.0f, 0.5f),
                new Vector3(-10.0f, 20.0f, 10.5f),
                new Vector3(-10.0f, 20.0f,- 0.5f)
            };
            var name = "plane";
            var node = new VmfClassNode("",
                new VmfPropertyNode(name,
                    string.Format(CultureInfo.InvariantCulture, "({0} {1} {2}) ({3} {4} {5}) ({6} {7} {8})",
                        expected[0].X, expected[0].Y, expected[0].Z,
                        expected[1].X, expected[1].Y, expected[1].Z,
                        expected[2].X, expected[2].Y, expected[2].Z)));

            var actual = VmfPropertyValueHelper.ReadVertices(node, name);

            Assert.AreEqual(3, actual.Length);

            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod()]
        public void ReadVerticesFromPresetValuesTest()
        {
            var expected = new[]
            {
                1024 * new Vector3(-1, 1, 1),
                1024 * new Vector3(-1, -1, 1),
                1024 * new Vector3(-1, -1, -1)
            };
            var name = "plane";
            var node = new VmfClassNode("",
                new VmfPropertyNode(name, "(-1024 1024 1024) (-1024 -1024 1024) (-1024 -1024 -1024)"));

            var actual = VmfPropertyValueHelper.ReadVertices(node, name);

            Assert.AreEqual(3, actual.Length);

            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }        
    }
}
