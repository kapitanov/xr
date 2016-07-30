using AISTek.XRage.Content.VmfParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AISTek.XRage.Content.VmfParsing
{
    [TestClass]
    public class VmfTreeBuilderTest
    {
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        public void BuildTreeTest()
        {
            var target = new VmfTreeBuilder();

            var tokens = new VmfToken[]
            {
                VmfToken.ClassName("vgs"),
                VmfToken.ClassOpening(),
                VmfToken.ClassName("vg"),
                VmfToken.ClassOpening(),
                VmfToken.PropertyName("name"),
                VmfToken.PropertyValue("null"),
                VmfToken.PropertyName("color"),
                VmfToken.PropertyValue("red"),
                VmfToken.ClassName("entity"),
                VmfToken.ClassOpening(),
                VmfToken.PropertyName("id"),
                VmfToken.PropertyValue("0"),
                VmfToken.PropertyName("plane"),
                VmfToken.PropertyValue("(-6400 -2304 192) (-6400 -2304 64) (-6400 -1664 64)"),
                VmfToken.ClassClosing(),
                VmfToken.ClassClosing(),
                VmfToken.ClassClosing()
            };

            var expected = new VmfDocument(
                new VmfClassNode("vgs",
                    new VmfClassNode("vg",
                        new VmfPropertyNode("name", "null"),
                        new VmfPropertyNode("color", "red"),
                        new VmfClassNode("entity",
                            new VmfPropertyNode("id", "0"),
                            new VmfPropertyNode("plane", "(-6400 -2304 192) (-6400 -2304 64) (-6400 -1664 64)")))));

            var actual = target.BuildTree(tokens);

            Assert.IsTrue(expected.IsEqual(actual));
        }
    }
}
