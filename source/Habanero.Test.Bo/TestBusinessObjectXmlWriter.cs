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
            new Address();
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