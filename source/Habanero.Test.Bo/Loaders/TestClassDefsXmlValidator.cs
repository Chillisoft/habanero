using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Habanero.Base.Exceptions;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    [TestFixture]
    public class TestClassDefsXmlValidator
    {

        [Test]
        public void TestValidateValidXml()
        {
            //---------------Set up test pack-------------------
            string xml =
                @"<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" />
							<property  name=""TestClassID"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>";
            ClassDefsXmlValidator validator = new ClassDefsXmlValidator();

            //---------------Execute Test ----------------------
            XmlValidationResult validationResult = validator.ValidateClassDefsXml(xml);

            //---------------Test Result -----------------------

            Assert.IsTrue(validationResult.IsValid);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestValidateInvalidXml()
        {
            //---------------Set up test pack-------------------
            string xml =
                @"<classes bob=""Asdf"">
						<bob />
					</classes>";
            ClassDefsXmlValidator validator = new ClassDefsXmlValidator();

            //---------------Execute Test ----------------------
            XmlValidationResult validationResult = validator.ValidateClassDefsXml(xml);

            //---------------Test Result -----------------------

            Assert.IsFalse(validationResult.IsValid);
            //---------------Tear Down -------------------------          
        }
    }

    

 
}
