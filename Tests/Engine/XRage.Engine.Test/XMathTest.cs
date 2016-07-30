using AISTek.XRage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AISTek.XRage
{
    [TestClass]
    public class XMathTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void GetIntersectionPointTest()
        {
            var vertices = new Dictionary<string, Vector3>
            {
                {"A", 1024*new Vector3(-1,1,1)},
                {"B", 1024*new Vector3(1,1,1)},
                {"C", 1024*new Vector3(1,-1,1)},
                {"D", 1024*new Vector3(-1,-1,1)},
                {"E", 1024*new Vector3(-1,1,-1)},
                {"F", 1024*new Vector3(1,1,-1)},
                {"G", 1024*new Vector3(-1,-1,-1)},
                {"H", 1024*new Vector3(1,-1,-1)},
            };



            var planes = new Plane[] 
            {
                new Plane(vertices["A"],vertices["B"], vertices["D"]),
                new Plane(vertices["C"],vertices["G"], vertices["H"]),
                new Plane(vertices["D"],vertices["F"], vertices["H"]),
            };

            var expected = vertices["D"];
            var actual = XMath.GetIntersectionPoint(planes[0], planes[1], planes[2]);

            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual<Vector3>(expected, actual.Value);
        }

        [TestMethod]
        public void IsPowerOfTwoTest()
        {
            int value = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = XMath.IsPowerOfTwo(value);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod]
        public void OrderClockwiseTest()
        {
            var vertices = new[]
            {
                new Vector3(-1,1,1),
                new Vector3(-1,1,-1),
                new Vector3(1,1,-1),
                new Vector3(1,1,1)
            };

            var expected = new[]
            {
                new Vector3(-1,1,-1),
                new Vector3(1,1,-1),
                new Vector3(1,1,1),
                new Vector3(-1,1,1),
            };

            var actual = XMath.OrderClockwise(vertices, Vector3.UnitY);
            if (!expected.SequenceEqual(actual))
                Assert.Fail();
        }
    }
}
