//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test
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