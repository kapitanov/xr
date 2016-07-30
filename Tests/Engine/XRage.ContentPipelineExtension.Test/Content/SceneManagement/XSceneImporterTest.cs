using AISTek.XRage.Content.SceneManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml.Linq;

namespace AISTek.XRage.Content.SceneManagement
{
    [TestClass]
    public class XSceneImporterTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ImportTest()
        {
            try
            {
                var importer = new XSceneImporter();
                var processor = new XSceneProcessor();
                var writer = new XSceneWriter();

                var filename = @"d:\dev\X-Rage\XRage.Sample\XRage.SampleContent\levels\xtest.xlevel";
                var context = new ContentImporterContextMock();

                var xd = importer.Import(filename, context);
                var scene = processor.Process(xd, null);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
