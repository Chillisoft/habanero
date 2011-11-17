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
using System.IO;
using System.Text;
using System.Xml;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectXmlWriter
    {
        
        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            //new Address();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            var writer = new BusinessObjectXmlWriter();
            //---------------Test Result -----------------------
            
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Write()
        {
            //---------------Set up test pack-------------------
            var stream = new MemoryStream();
            var xmlWriter = CreateXmlWriter(stream);
            var writer = new BusinessObjectXmlWriter();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, stream.Length);
            //---------------Execute Test ----------------------
            writer.Write(xmlWriter, new[] { new Car()});
            //---------------Test Result -----------------------
            Assert.AreNotEqual(0, stream.Length);
        }


        private XmlWriter CreateXmlWriter(MemoryStream stream)
        {
            return XmlWriter.Create(stream,
                                    new XmlWriterSettings() {ConformanceLevel = ConformanceLevel.Auto});
        }
    }
}