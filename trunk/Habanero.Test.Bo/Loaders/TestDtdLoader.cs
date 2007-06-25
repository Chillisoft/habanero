using System;
using System.IO;
using Habanero.Base;
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

        [Test]
        public void TestSimpleDtd()
        {
            Mock mockControl = new DynamicMock(typeof (ITextFileLoader));
            ITextFileLoader textFileLoader = (ITextFileLoader) mockControl.MockInstance;

            DtdLoader loader = new DtdLoader(textFileLoader, "");

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] {"classdef.dtd"});
            String dtdFileContents = loader.LoadDtd("classdef.dtd");
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

            String dtdFileContents = loader.LoadDtd("propertydef.dtd");
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

            String dtdFileContents = loader.LoadDtd("keydef.dtd");
            Assert.AreEqual(dtd3processed + Environment.NewLine, dtdFileContents);
        }
    }
}