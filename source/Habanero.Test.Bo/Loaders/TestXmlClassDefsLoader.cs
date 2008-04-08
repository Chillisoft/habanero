//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using System.Collections;
using System.Xml;
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