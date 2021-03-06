﻿using AISTek.XRage.Content.SceneManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace AISTek.XRage
{
    
    
    /// <summary>
    ///This is a test class for XSceneProcessorTest and is intended
    ///to contain all XSceneProcessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class XSceneProcessorTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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


        /// <summary>
        ///A test for ReadEntity
        ///</summary>
        [TestMethod()]
        [DeploymentItem("XRage.ContentPipelineExtension.dll")]
        public void ReadEntityTest()
        {
            XElement root = null; // TODO: Initialize to an appropriate value
            CompiledEntity expected = null; // TODO: Initialize to an appropriate value
            CompiledEntity actual;
            actual = XSceneProcessor_Accessor.ReadEntity(root);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Process
        ///</summary>
        [TestMethod()]
        public void ProcessTest()
        {
            XSceneProcessor target = new XSceneProcessor(); // TODO: Initialize to an appropriate value
            XDocument input = null; // TODO: Initialize to an appropriate value
            ContentProcessorContext context = null; // TODO: Initialize to an appropriate value
            CompiledXScene expected = null; // TODO: Initialize to an appropriate value
            CompiledXScene actual;
            actual = target.Process(input, context);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for XSceneProcessor Constructor
        ///</summary>
        [TestMethod()]
        public void XSceneProcessorConstructorTest()
        {
            XSceneProcessor target = new XSceneProcessor();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
