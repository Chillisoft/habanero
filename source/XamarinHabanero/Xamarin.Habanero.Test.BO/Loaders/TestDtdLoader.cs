#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
using System.IO;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.Loaders;
using Habanero.Util;
using NSubstitute;
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
            catch (Exception) {
                Console.Out.WriteLine("Problem removing test dtd files.");
            }
        }

        [Test]
        public void TestSimpleDtd()
        {
            var textFileLoader = Substitute.For<ITextFileLoader>();

            textFileLoader.LoadTextFile("class.dtd").Returns(info => new StringReader(dtd1));

            var loader = new DtdLoader(textFileLoader, "");

            String dtdFileContents = loader.LoadDtd("class");
            Assert.AreEqual(dtd1 + Environment.NewLine, dtdFileContents);
        }
        
        [Test]
        public void TestIncludeDtd()
        {
            var textFileLoader = Substitute.For<ITextFileLoader>();

            textFileLoader.LoadTextFile("property.dtd").Returns(info => new StringReader(dtd2));
            textFileLoader.LoadTextFile("class.dtd").Returns(info => new StringReader(dtd1));

            var loader = new DtdLoader(textFileLoader, "");

            String dtdFileContents = loader.LoadDtd("property");
            Assert.AreEqual(dtd2and1 + Environment.NewLine, dtdFileContents);
        }

        [Test]
        public void TestIncludeDtdTwice()
        {
            var textFileLoader = Substitute.For<ITextFileLoader>();
            var loader = new DtdLoader(textFileLoader, "");

            textFileLoader.LoadTextFile("key.dtd").Returns(info => new StringReader(dtd3));
            textFileLoader.LoadTextFile("class.dtd").Returns(info => new StringReader(dtd1));
            textFileLoader.LoadTextFile("property.dtd").Returns(info => new StringReader(dtd2));

            String dtdFileContents = loader.LoadDtd("key");
            Assert.AreEqual(dtd3processed + Environment.NewLine, dtdFileContents);
        }

        [Test]
        public void TestLoadFromResource()
        {
            DtdLoader loader = new DtdLoader();
            string dtd = loader.LoadDtd("class");
            Assert.AreNotEqual(0, dtd.Length);
            Assert.AreNotEqual("#include", dtd.Substring(0, 8));
        }

        [Test]
        public void TestDtdNodeInvalidException()
        {
            //---------------Set up test pack-------------------
            DtdLoader loader = new DtdLoader();
            //---------------Execute Test ----------------------
            try
            {
                string dtd = loader.LoadDtd("notexists");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An invalid node 'notexists' was encountered when loading the class definitions", ex.Message);
            }
        }

        [Test]
        public void TestDtdNotFoundException()
        {
            //---------------Set up test pack-------------------
            DtdLoader loader = new DtdLoader(new TextFileLoader(), "somepath");
            //---------------Execute Test ----------------------
            try
            {
                string dtd = loader.LoadDtd("notexists");
                Assert.Fail("Expected to throw an FileNotFoundException");
            }
                //---------------Test Result -----------------------
            catch (FileNotFoundException ex)
            {
                StringAssert.Contains("The Document Type Definition (DTD) for the XML element 'notexists' was not found in the path", ex.Message);
            }
        }

        [Test]
        public void TestDtdNotFoundExceptionWithEmptyPath()
        {
            //---------------Set up test pack-------------------
            DtdLoader loader = new DtdLoader(new TextFileLoader(), "");
            //---------------Execute Test ----------------------
            try
            {
                string dtd = loader.LoadDtd("notexists");
                Assert.Fail("Expected to throw an FileNotFoundException");
            }
                //---------------Test Result -----------------------
            catch (FileNotFoundException ex)
            {
                StringAssert.Contains("The Document Type Definition (DTD) for the XML element 'notexists' was not found in the application's output/execution directory", ex.Message);
            }
        }

        [Test]
        public void TestDtdNotFoundExceptionWithIncludesList()
        {
            //---------------Set up test pack-------------------
            DtdLoader loader = new DtdLoader(new TextFileLoader(), "somepath");
            //---------------Execute Test ----------------------
            try
            {
                string dtd = loader.LoadDtd("somefile", new ArrayList());
                Assert.Fail("Expected to throw an FileNotFoundException");
            }
                //---------------Test Result -----------------------
            catch (FileNotFoundException ex)
            {
                StringAssert.Contains("The Document Type Definition (DTD) file, 'somefile', was not found", ex.Message);
            }
        }

        [Test]
        public void TestMethod()
        {
            //---------------Set up test pack-------------------
            DtdLoader loader = new DtdLoader();
            string dtd = loader.LoadDtd("ui");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            //---------------Tear Down -------------------------          
        }
    }
}
