using System.Xml;
using Habanero.UI.Forms;
using NUnit.Framework;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.Util;
using Habanero.DB;
using Habanero;

namespace Habanero.Test
{
    /// <summary>
    /// TODO - Test:
    /// - Filepath constructor
    /// - ReadXmlValue
    /// - WriteXmlValue
    /// - WriteXmlDocToFile
    /// - WriteXmlDocToFile
    /// </summary>
    [TestFixture]
    public class TestXmlWrapper
    {
        [Test]
        public void TestWrapper()
        {
            XmlDocument doc = new XmlDocument();
            XmlWrapper wrapper = new XmlWrapper(doc);

            Assert.AreEqual(doc, wrapper.XmlDocument);
        }
    }
}
