// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using Habanero.Base.Exceptions;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
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

        [Test]
        public void TestWithInvalidDocument()
        {
            //---------------Set up test pack-------------------
            XmlDocumentValidator validator = new XmlDocumentValidator();
            //---------------Execute Test ----------------------
            try
            {
                validator.ValidateDocument(itsXmlDocument.Replace("oneprop", "twoprop"), "test", itsDTD);
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The relationship node does not conform to the dtd.The 'twoprop' element is not declared", ex.Message);
            }
        }
    }
}