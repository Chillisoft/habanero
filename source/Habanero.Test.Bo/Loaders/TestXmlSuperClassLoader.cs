//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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
        public void SetupTest()
        {
            Initialise();
            ClassDef.ClassDefs.Clear();

            ClassDef.ClassDefs.Add(
                new XmlClassDefsLoader(SuperClassClassDefXml, new DtdLoader(), GetDefClassFactory()).LoadClassDefs());
        }

        protected string SuperClassClassDefXml = 
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

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidInheritanceType()
        {
            itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""class"" assembly=""ass"" orMapping=""invalid"" />");
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

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSingleTableInheritanceException()
        {
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""SingleTableInheritance"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSingleTableInheritanceWithIDException()
        {
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""SingleTableInheritance"" id="""" />");
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

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestConcreteTableInheritanceWithIDException()
        {
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""ConcreteTableInheritance"" id=""prop"" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestConcreteTableInheritanceWithDiscriminatorException()
        {
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""ConcreteTableInheritance"" discriminator=""abc"" />");
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
    }
}