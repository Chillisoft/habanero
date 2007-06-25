using System;
using System.IO;
using Habanero.Base;
using Habanero.Bo.Loaders;
using Habanero.Util;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestDtdLoader.
    /// </summary>
    [TestFixture]
    public class TestDtdLoader
    {
        private string dtd1 = "TestDtd";
        private string dtd2 = @"
#include classdef.dtd
TestDtd2";
        private string dtd2and1 = @"
TestDtd
TestDtd2";
        private string dtd3 = @"
#include classdef.dtd
#include propertydef.dtd
TestDtd3";
        private string dtd3processed = @"
TestDtd

TestDtd2
TestDtd3";

        [TestFixtureSetUp]
        public void SetupFixture() {
            if (!File.Exists("propertydef.dtd"))
            {
                File.Create("propertydef.dtd");
            }
            if (!File.Exists("classdef.dtd"))
            {
                File.Create("classdef.dtd");
            }
            if (!File.Exists("keydef.dtd"))
            {
                File.Create("keydef.dtd");
            }
        }

        [TestFixtureTearDown]
        public void TearDownFixture() {
            if (File.Exists("propertydef.dtd"))
            {
                File.Delete("propertydef.dtd");
            }
            if (File.Exists("classdef.dtd"))
            {
                File.Delete("classdef.dtd");
            }
            if (File.Exists("keydef.dtd"))
            {
                File.Delete("keydef.dtd");
            }
        }

        [Test]
        public void TestSimpleDtd()
        {
            Mock mockControl = new DynamicMock(typeof (ITextFileLoader));
            ITextFileLoader textFileLoader = (ITextFileLoader) mockControl.MockInstance;

            DtdLoader loader = new DtdLoader(textFileLoader, "");

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] {"classdef.dtd"});
            String dtdFileContents = loader.LoadDtd("classdef");
            Assert.AreEqual(dtd1 + Environment.NewLine, dtdFileContents);
            mockControl.Verify();
        }


        [Test]
        public void TestIncludeDtd()
        {

            Mock mockControl = new DynamicMock(typeof (ITextFileLoader));
            ITextFileLoader textFileLoader = (ITextFileLoader) mockControl.MockInstance;

            DtdLoader loader = new DtdLoader(textFileLoader, "");

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd2), new object[] {"propertydef.dtd"});
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] {"classdef.dtd"});

            String dtdFileContents = loader.LoadDtd("propertydef");
            Assert.AreEqual(dtd2and1 + Environment.NewLine, dtdFileContents);
        }

        [Test]
        public void TestIncludeDtdTwice()
        {
            Mock mockControl = new DynamicMock(typeof (ITextFileLoader));
            ITextFileLoader textFileLoader = (ITextFileLoader) mockControl.MockInstance;

            DtdLoader loader = new DtdLoader(textFileLoader, "");

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd3), new object[] {"keydef.dtd"});
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] { "classdef.dtd" });
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd2), new object[] { "propertydef.dtd" });
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] { "classdef.dtd" });

            String dtdFileContents = loader.LoadDtd("keydef");
            Assert.AreEqual(dtd3processed + Environment.NewLine, dtdFileContents);
        }

        [Test]
        public void TestLoadFromResource() {
            DtdLoader loader = new DtdLoader();
            string dtd = loader.LoadDtd("classDef");
            Assert.AreNotEqual(0, dtd.Length);
            Assert.AreNotEqual("#include", dtd.Substring(0, 8));
        }
        
    }
}