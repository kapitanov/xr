using AISTek.XRage.Content.VmfParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace AISTek.XRage.Content.VmfParsing
{
    [TestClass]
    public class VmfParserTest
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
        public void ReadVmfTest()
        {
            var target = new VmfParser();
            var vmfContent = @"
vgs
{
   vg
   {
      ""name"" ""null""
      ""color"" ""red""
      entity
      {
        ""id"" ""0""
        ""plane"" ""(-6400 -2304 192) (-6400 -2304 64) (-6400 -1664 64)""
      }
   }
}";

            var expected = new VmfDocument(
                new VmfClassNode("vgs",
                    new VmfClassNode("vg",
                        new VmfPropertyNode("name", "null"),
                        new VmfPropertyNode("color", "red"),
                        new VmfClassNode("entity",
                            new VmfPropertyNode("id", "0"),
                            new VmfPropertyNode("plane", "(-6400 -2304 192) (-6400 -2304 64) (-6400 -1664 64)")))));

            var actual = target.ReadVmf(vmfContent);

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [TestMethod]
        public void ReadRealVmfTest()
        {
            var vmf = File.ReadAllText(@"d:\dev\X-Rage\XRage.Sample\XRage.SampleContent\levels\bridge_d.vmf");
            var target = new VmfParser();
            var actual = target.ReadVmf(vmf);
        }
    }
}
