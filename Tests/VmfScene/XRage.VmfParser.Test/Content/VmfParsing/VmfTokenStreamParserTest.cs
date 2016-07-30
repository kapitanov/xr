using AISTek.XRage.Content.VmfParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AISTek.XRage.Content.VmfParsing
{
	[TestClass]
	public class VmfTokenStreamParserTest
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
		public void ReadTokensTest()
		{
			var target = new VmfTokenStreamParser();
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
			var expected = new VmfToken[]
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


			var actual = target.ReadTokens(vmfContent).ToArray();

			Assert.AreEqual(expected.Length, actual.Length);

			for (var i = 0; i < expected.Length; i++)
			{
				var expectedToken = expected[i];
				var actualToken = actual[i];

				Assert.IsTrue(expectedToken.Equals(actualToken), "Expected {0}, but got {1}", expectedToken, actualToken);
			}
		}

		[TestMethod]
		public void ReadTokens2Test()
		{
			var target = new VmfTokenStreamParser();
			var vmfContent = @"
state
{
	""id"" ""0""
}
world
{
	""id"" ""1""
	""comment"" ""Decompiled by VMEX v0.98e from C:\Users\Bers\Documents\VMaps\r&d\bg.bsp""
	solid
	{
		""id"" ""2""
		side
		{
			""plane"" ""(-512 -2064 256) (-512 -2064 0) (-512 -896 0)""
			""material"" ""TOOLS/TOOLSNODRAW""
		}
	}
}";
			var expected = new VmfToken[]
			{
				VmfToken.ClassName("state"),
				VmfToken.ClassOpening(),

				VmfToken.PropertyName("id"),
				VmfToken.PropertyValue("0"),

				VmfToken.ClassClosing(),

				VmfToken.ClassName("world"),
				VmfToken.ClassOpening(),
				
				VmfToken.PropertyName("id"),
				VmfToken.PropertyValue("1"),

				VmfToken.PropertyName("comment"),
				VmfToken.PropertyValue(@"Decompiled by VMEX v0.98e from C:\Users\Bers\Documents\VMaps\r&d\bg.bsp"),

				VmfToken.ClassName("solid"),
				VmfToken.ClassOpening(),

				VmfToken.PropertyName("id"),
				VmfToken.PropertyValue("2"),

				VmfToken.ClassName("side"),
				VmfToken.ClassOpening(),
	
				VmfToken.PropertyName("plane"),
				VmfToken.PropertyValue("(-512 -2064 256) (-512 -2064 0) (-512 -896 0)"),

				VmfToken.PropertyName("material"),
				VmfToken.PropertyValue(@"TOOLS/TOOLSNODRAW"),

				VmfToken.ClassClosing(),
				VmfToken.ClassClosing(),
				VmfToken.ClassClosing()
			};


			var actual = target.ReadTokens(vmfContent).ToArray();

			Assert.AreEqual(expected.Length, actual.Length);

			for (var i = 0; i < expected.Length; i++)
			{
				var expectedToken = expected[i];
				var actualToken = actual[i];

				Assert.IsTrue(expectedToken.Equals(actualToken), "Expected {0}, but got {1}", expectedToken, actualToken);
			}
		}
	}
}
