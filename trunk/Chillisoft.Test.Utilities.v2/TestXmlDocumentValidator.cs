using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Utilities
{
    /// <summary>
    /// Summary description for XmlDocumentValidator.
    /// </summary>
    [TestFixture]
    public class TestXmlDocumentValidator
    {
        private string itsXmlDocument = @"<test><oneprop name=""oneproptest"" /></test>";

        private string itsDTD =
            @"
<!ELEMENT test (oneprop)>
<!ELEMENT oneprop EMPTY>
<!ATTLIST oneprop 
	name NMTOKEN #REQUIRED
>
";

        [Test]
        public void TestValidateDocument()
        {
            XmlDocumentValidator validator = new XmlDocumentValidator();
            validator.ValidateDocument(itsXmlDocument, "test", itsDTD);
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestWithInvalidDocument()
        {
            XmlDocumentValidator validator = new XmlDocumentValidator();
            validator.ValidateDocument(itsXmlDocument.Replace("oneprop", "twoprop"), "test", itsDTD);
        }
    }
}