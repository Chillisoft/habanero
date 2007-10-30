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

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlSuperClassDescLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlSuperClassDescLoader
    {
        private XmlSuperClassLoader itsLoader;

        [SetUp]
        public void SetupTest()
        {
            itsLoader = new XmlSuperClassLoader();
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(
                new XmlClassDefsLoader(
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
					</classes>",
                    new DtdLoader()));
        }

        [Test]
        public void TestSimpleProperty()
        {
            SuperClassDef def =
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />");
            Assert.AreEqual(ORMapping.ClassTableInheritance, def.ORMapping);
            //ClassDef parentDef = ClassDef.ClassDefs[typeof(TestClass)];
            ClassDef parentDef = ClassDef.ClassDefs["Habanero.Test.BO.Loaders", "TestClass"];
            ClassDef superClassDef = def.SuperClassClassDef;
            Assert.AreSame(parentDef, superClassDef);
        }

        [Test]
        public void TestORMapping()
        {
            SuperClassDef def =
                itsLoader.LoadSuperClassDesc(
                    @"<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" orMapping=""SingleTableInheritance"" />");
            Assert.AreEqual(ORMapping.SingleTableInheritance, def.ORMapping);
        }
    }
}