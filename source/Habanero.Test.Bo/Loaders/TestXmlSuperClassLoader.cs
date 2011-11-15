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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Still to test:
    ///   - is ID required for ClassTableInheritance, and what are the implications?
    /// </summary>
    [TestFixture]
    public class TestXmlSuperClassLoader
    {
        private XmlSuperClassLoader itsLoader;

        [SetUp]
        public virtual void SetupTest()
        {
            Initialise();
            ClassDef.ClassDefs.Clear();

            ClassDef.ClassDefs.Add(
                new XmlClassDefsLoader(SuperClassClassDefXml, new DtdLoader(), GetDefClassFactory()).LoadClassDefs());
                        GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected const string SuperClassClassDefXml = 
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>";

        protected void Initialise()
        {
            itsLoader = new XmlSuperClassLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestSimpleProperty()
        {
            ISuperClassDef def =
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />");
            
            Assert.AreEqual(ORMapping.ClassTableInheritance, def.ORMapping);
            Assert.AreEqual("Habanero.Test.BO.Loaders", def.AssemblyName);
            Assert.AreEqual("TestClass", def.ClassName);
            Assert.IsNull(def.Discriminator);
        }

        [Test]
        public void TestInvalidInheritanceType()
        {
            try
            {
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""class"" assembly=""ass"" orMapping=""invalid"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The specified ORMapping type, 'invalid', is not a valid inheritance type.  The valid options are ClassTableInheritance (the default", ex.Message);
            }
        }

        [Test]
        public void TestSingleTableInheritance()
        {
            ISuperClassDef def = itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""SingleTableInheritance"" discriminator=""propname"" />");
            Assert.AreEqual(ORMapping.SingleTableInheritance, def.ORMapping);
            Assert.AreEqual("propname", def.Discriminator);
            Assert.IsNull(def.ID);
        }

        [Test]
        public void TestSingleTableInheritanceDiscriminatorWithSpaces()
        {
            ISuperClassDef def = itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""SingleTableInheritance"" discriminator=""prop name"" />");
            Assert.AreEqual("prop name", def.Discriminator);
        }

        [Test]
        public void TestSingleTableInheritanceException()
        {
            try
            {
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""SingleTableInheritance"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a superClass definition, a 'discriminator' attribute is missing where the SingleTableInheritance OR-mapping type has been specified", ex.Message);
            }
        }

        [Test]
        public void TestSingleTableInheritanceWithIDException()
        {
            try
            {
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""SingleTableInheritance"" id="""" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a superClass definition, a 'discriminator' attribute is missing where the SingleTableInheritance OR-mapping type has been specified", ex.Message);
            }
        }

        [Test]
        public void TestConcreteTableInheritance()
        {
            ISuperClassDef def =
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""ConcreteTableInheritance"" />");
            Assert.AreEqual(ORMapping.ConcreteTableInheritance, def.ORMapping);
            Assert.IsNull(def.ID);
            Assert.IsNull(def.Discriminator);
        }

        [Test]
        public void TestConcreteTableInheritanceWithIDException()
        {
            try
            {
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""ConcreteTableInheritance"" id=""prop"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a superClass definition, an 'id' attribute has been specified for an OR-mapping type other than ClassTableInheritance.", ex.Message);
            }
        }

        [Test]
        public void TestConcreteTableInheritanceWithDiscriminatorException()
        {
            try
            {
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""ConcreteTableInheritance"" discriminator=""abc"" />");
                    Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a superClass definition, a 'discriminator' attribute has been specified for OR-mapping type ConcreteTableInheritance", ex.Message);
            }
        }

        [Test]
        public void TestClassTableInheritanceWithEmptyID()
        {
            ISuperClassDef def =
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" id="""" />");
            Assert.AreEqual(ORMapping.ClassTableInheritance, def.ORMapping);
            Assert.IsTrue(string.IsNullOrEmpty(def.ID));
            Assert.IsNull(def.Discriminator);
        }

        [Test]
        public void TestClassTableInheritanceWithID()
        {
            ISuperClassDef def =
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" id=""propname"" />");
            Assert.AreEqual(ORMapping.ClassTableInheritance, def.ORMapping);
            Assert.AreEqual("propname", def.ID);
            Assert.IsNull(def.Discriminator);
        }

        [Test]
        public void TestClassTableInheritanceWithDiscriminatorIsValid()
        {
            ISuperClassDef def =
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" discriminator=""abc"" />");
            Assert.AreEqual(ORMapping.ClassTableInheritance, def.ORMapping);
            Assert.AreEqual("abc", def.Discriminator);
        }

        [Test]
        public void Test_TypeParameter()
        {
            //---------------Set up test pack-------------------
            const string superClassXml = @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" typeParameter=""TypeParam1"" />";
            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ISuperClassDef def = itsLoader.LoadSuperClassDesc(superClassXml);
            //---------------Test Result -----------------------
            Assert.AreEqual("TypeParam1", def.TypeParameter);
            //---------------Tear Down -------------------------          
        }
    }
}