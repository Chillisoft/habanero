using System;
using System.IO;
using Habanero.Base;
using Habanero.BO.Loaders;
using Habanero.Util;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestDtdLoader.
    /// </summary>
    [TestFixture]
    public class TestDtdLoader
    {
        private string dtd1 = "TestDtd";
        private string dtd2 = @"
#include class.dtd
TestDtd2";
        private string dtd2and1 = @"
TestDtd
TestDtd2";
        private string dtd3 = @"
#include class.dtd
#include property.dtd
TestDtd3";
        private string dtd3processed = @"
TestDtd

TestDtd2
TestDtd3";

        [TestFixtureSetUp]
        public void SetupFixture() {
            
                if (!File.Exists("property.dtd")) {
                    File.Create("property.dtd");
                }
                if (!File.Exists("class.dtd")) {
                    File.Create("class.dtd");
                }
                if (!File.Exists("key.dtd")) {
                    File.Create("key.dtd");
                }

        }

        [TestFixtureTearDown]
        public void TearDownFixture() {
            try
            {
                if (File.Exists("property.dtd"))
                {
                    File.Delete("property.dtd");
                }
                if (File.Exists("class.dtd"))
                {
                    File.Delete("class.dtd");
                }
                if (File.Exists("key.dtd"))
                {
                    File.Delete("key.dtd");
                }
            }
            catch (Exception ex) {
                Console.Out.WriteLine("Problem removing test dtd files.");
            }
        }

        [Test]
        public void TestSimpleDtd()
        {
            Mock mockControl = new DynamicMock(typeof (ITextFileLoader));
            ITextFileLoader textFileLoader = (ITextFileLoader) mockControl.MockInstance;

            DtdLoader loader = new DtdLoader(textFileLoader, "");

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] {"class.dtd"});
            String dtdFileContents = loader.LoadDtd("class");
            Assert.AreEqual(dtd1 + Environment.NewLine, dtdFileContents);
            mockControl.Verify();
        }


        [Test]
        public void TestIncludeDtd()
        {

            Mock mockControl = new DynamicMock(typeof (ITextFileLoader));
            ITextFileLoader textFileLoader = (ITextFileLoader) mockControl.MockInstance;

            DtdLoader loader = new DtdLoader(textFileLoader, "");

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd2), new object[] {"property.dtd"});
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] {"class.dtd"});

            String dtdFileContents = loader.LoadDtd("property");
            Assert.AreEqual(dtd2and1 + Environment.NewLine, dtdFileContents);
        }

        [Test]
        public void TestIncludeDtdTwice()
        {
            Mock mockControl = new DynamicMock(typeof (ITextFileLoader));
            ITextFileLoader textFileLoader = (ITextFileLoader) mockControl.MockInstance;

            DtdLoader loader = new DtdLoader(textFileLoader, "");

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd3), new object[] {"key.dtd"});
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] { "class.dtd" });
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd2), new object[] { "property.dtd" });
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] { "class.dtd" });

            String dtdFileContents = loader.LoadDtd("key");
            Assert.AreEqual(dtd3processed + Environment.NewLine, dtdFileContents);
        }

        [Test]
        public void TestLoadFromResource() {
            DtdLoader loader = new DtdLoader();
            string dtd = loader.LoadDtd("class");
            Assert.AreNotEqual(0, dtd.Length);
            Assert.AreNotEqual("#include", dtd.Substring(0, 8));
        }
        
    }
}
