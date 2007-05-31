using System;
using System.IO;
using Chillisoft.Generic.v2;
using NMock;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.Loaders.v2
{
    /// <summary>
    /// Summary description for TestDtdLoader.
    /// </summary>
    [TestFixture]
    public class TestDtdLoader
    {
        private string dtd1 = "TestDtd";
        private string dtd2 = @"
#include dtd1.dtd
TestDtd2";
        private string dtd2and1 = @"
TestDtd
TestDtd2";
        private string dtd3 = @"
#include dtd1.dtd
#include dtd2.dtd
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

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] {"dtd1.dtd"});
            String dtdFileContents = loader.LoadDtd("dtd1.dtd");
            Assert.AreEqual(dtd1 + Environment.NewLine, dtdFileContents);
            mockControl.Verify();
        }


        [Test]
        public void TestIncludeDtd()
        {
            Mock mockControl = new DynamicMock(typeof (ITextFileLoader));
            ITextFileLoader textFileLoader = (ITextFileLoader) mockControl.MockInstance;

            DtdLoader loader = new DtdLoader(textFileLoader, "");

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd2), new object[] {"dtd2.dtd"});
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] {"dtd1.dtd"});

            String dtdFileContents = loader.LoadDtd("dtd2.dtd");
            Assert.AreEqual(dtd2and1 + Environment.NewLine, dtdFileContents);
        }

        [Test]
        public void TestIncludeDtdTwice()
        {
            Mock mockControl = new DynamicMock(typeof (ITextFileLoader));
            ITextFileLoader textFileLoader = (ITextFileLoader) mockControl.MockInstance;

            DtdLoader loader = new DtdLoader(textFileLoader, "");

            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd3), new object[] {"dtd3.dtd"});
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] {"dtd1.dtd"});
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd2), new object[] {"dtd2.dtd"});
            mockControl.ExpectAndReturn("LoadTextFile", new StringReader(dtd1), new object[] {"dtd1.dtd"});

            String dtdFileContents = loader.LoadDtd("dtd3.dtd");
            Assert.AreEqual(dtd3processed + Environment.NewLine, dtdFileContents);
        }
    }
}