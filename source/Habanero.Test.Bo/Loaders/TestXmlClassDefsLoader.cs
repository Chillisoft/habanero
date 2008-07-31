//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using System;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlClassDefsLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlClassDefsLoader
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
        }

        [Test]
        public void TestLoadClassDefs()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList =
                loader.LoadClassDefs(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>
			");
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
        }

        [Test]
        public void TestLoadClassDefs_InheritedClassWithNoPrimaryKey()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList =
                loader.LoadClassDefs(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestClass2"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClass2ID"" />
                            <primaryKey>
                                <prop name=""TestClass2ID""/>
                            </primaryKey>
						</class>
						<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                            <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
						</class>
					</classes>
			");
            Assert.AreEqual(3, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass2"), "Class 'TestClass2' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClassInherited"), "Class 'TestClassInherited' should have been loaded.");
            ClassDef classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
            ClassDef classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
            Assert.IsNotNull(classDefInherited.SuperClassDef);
            Assert.IsNull(classDefInherited.PrimaryKeyDef);
        }

        [Test]
        public void TestLoadClassDefs_KeyDefinedWithInheritedProperties()
        {
            //-------------Setup Test Pack ------------------
            string xml = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" />
							<property  name=""TestClassName"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                            <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
                            <key>
                                <prop name=""TestClassName""/>
                            </key>
						</class>
					</classes>
			";
            //-------------Execute test ---------------------
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList = loader.LoadClassDefs(xml);
            //-------------Test Result ----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClassInherited"), "Class 'TestClassInherited' should have been loaded.");
            ClassDef classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
            IPropDef propDef = classDefTestClass.PropDefcol["TestClassName"];
            ClassDef classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
            Assert.IsNotNull(classDefInherited.SuperClassDef);
            Assert.AreEqual(1, classDefInherited.KeysCol.Count);
            KeyDef keyDef = classDefInherited.KeysCol.GetKeyDefAtIndex(0);
            IPropDef keyDefPropDef = keyDef["TestClassName"];
            Assert.AreSame(propDef, keyDefPropDef, "The key's property should have been resolved to be the property of the superclass by the loader.");
        }

        [Test]
        public void TestLoadClassDefs_KeyDefinedWithNonExistantProperty()
        {
            //-------------Setup Test Pack ------------------
            string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property  name=""TestClassID"" />
						<property  name=""TestClassName"" />
                        <primaryKey>
                            <prop name=""TestClassID""/>
                        </primaryKey>
					</class>
					<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                        <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
                        <key>
                            <prop name=""DoesNotExist""/>
                        </key>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            Exception exception = null;
            //-------------Execute test ---------------------
            try
            {
                ClassDefCol classDefList = loader.LoadClassDefs(xml);
            } catch(Exception ex)
            {
                exception = ex;
            }
            //-------------Test Result ----------------------
            Assert.IsNotNull(exception, "An error should have been thrown for this xml.");
            Assert.IsInstanceOfType(typeof(InvalidXmlDefinitionException), exception);
        }

        [Test, ExpectedException(typeof(XmlException))]
        public void TestNoRootNodeException()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList = loader.LoadClassDefs(@"<invalidRootNode>");
        }

        // Trying to catch the exception in line 209 of XmlClassDefsLoader
        //   but the exception gets caught earlier.
//        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
//        public void TestRelatedPropertyException()
//        {
//            XmlClassDefsLoader loader = new XmlClassDefsLoader();
//            ClassDefCol classDefList = loader.LoadClassDefs(@"
//					<classes>
//						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
//							<property name=""TestClassID"" />
//                            <primaryKey>
//                                <prop name=""TestClassID""/>
//                            </primaryKey>
//						</class>
//						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
//							<property name=""TestRelatedClassID"" />
//							<property name=""TestClassID"" />
//                            <primaryKey>
//                                <prop name=""TestRelatedClassID""/>
//                            </primaryKey>
//                            <relationship name=""TestClass"" type=""single"" relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"" >
//                                <relatedProperty name=""notexists"" relatedProperty=""notexists"" />
//                            </relationship>
//						</class>
//					</classes>
//			");
//        }


    }
}