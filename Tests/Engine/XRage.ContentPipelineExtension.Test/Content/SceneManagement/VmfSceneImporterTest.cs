using AISTek.XRage.Content.SceneManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace AISTek.XRage.Content.SceneManagement
{
    [TestClass]
    public class VmfSceneImporterTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ImportTest()
        {
            try
            {
                var target = new VmfSceneImporter();
                var filename = @"d:\dev\X-Rage\XRage.Sample\XRage.SampleContent\levels\test.vmf";
                var context = new ContentImporterContextMock();
                var actual = target.Import(filename, context);
            }
            catch (Exception e)
            {
                
                throw;
            }
        }
    }
}
