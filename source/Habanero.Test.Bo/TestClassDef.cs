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

using System;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.BO;
using Habanero.Test;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestClassDef.
    /// </summary>
    [TestFixture]
    public class TestClassDef
    {
        private ClassDef itsClassDef;

        [Test]
        public void TestCreateBusinessObject()
        {
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            itsClassDef =
                loader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            Assert.AreSame(typeof (MyBO), bo.GetType());
            bo.SetPropertyValue("TestProp", "TestValue");
            Assert.AreEqual("TestValue", bo.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestLoadClassDefs()
        {
            XmlClassDefsLoader loader =
                new XmlClassDefsLoader(
                    GetTestClassDefinition(""),
                     new DtdLoader());
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(loader);
            Assert.AreEqual(2, ClassDef.ClassDefs.Count);
        }

    	private string GetTestClassDefinition(string suffix)
    	{
    		string classDefString = String.Format(
                @"
					<classes>
						<class name=""TestClass{0}"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClass{0}ID"" />
                            <primaryKey>
                                <prop name=""TestClass{0}ID""/>
                            </primaryKey>
						</class>
						<class name=""TestRelatedClass{0}"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClass{0}ID"" />
                            <primaryKey>
                                <prop name=""TestRelatedClass{0}ID""/>
                            </primaryKey>
						</class>
					</classes>
			", suffix);
    		return classDefString;
    	}

		[Test]
		public void TestLoadRepeatedClassDefs()
		{
			XmlClassDefsLoader loader;
            loader = new XmlClassDefsLoader(GetTestClassDefinition(""), new DtdLoader());
    		ClassDef.ClassDefs.Clear();
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(2, ClassDef.ClassDefs.Count);
			//Now load the same again.
            loader = new XmlClassDefsLoader(GetTestClassDefinition(""), new DtdLoader());
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(2, ClassDef.ClassDefs.Count);
		}

		[Test]
		public void TestLoadMultipleClassDefs()
		{
			XmlClassDefsLoader loader;
            loader = new XmlClassDefsLoader(GetTestClassDefinition(""), new DtdLoader());
			ClassDef.ClassDefs.Clear();
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(2, ClassDef.ClassDefs.Count);
			// Now load some more classes
            loader = new XmlClassDefsLoader(GetTestClassDefinition("Other"), new DtdLoader());
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(4, ClassDef.ClassDefs.Count);
		}

        [Test]
        public void TestImmediateChildren()
        {
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            ClassDef parentClassDef = loader.LoadClass(
                @"<class name=""Parent"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef childClassDef = loader.LoadClass(
                @"<class name=""Child"" assembly=""Habanero.Test"">
					<superClass class=""Parent"" assembly=""Habanero.Test"" orMapping=""SingleTableInheritance"" discriminator=""blah"" />
                    <property  name=""Prop1"" />
				</class>
			");
            ClassDef grandchildClassDef = loader.LoadClass(
                @"<class name=""Grandchild"" assembly=""Habanero.Test"">
					<superClass class=""Child"" assembly=""Habanero.Test"" orMapping=""SingleTableInheritance"" discriminator=""blah"" />
                    <property  name=""Prop2"" />
				</class>
			");
            ClassDef.ClassDefs.Add(parentClassDef);
            ClassDef.ClassDefs.Add(childClassDef);
            ClassDef.ClassDefs.Add(grandchildClassDef);

            ClassDefCol children = parentClassDef.ImmediateChildren;
            Assert.AreEqual(1, children.Count);
            Assert.IsTrue(children.Contains(childClassDef));
        }

        [Test]
        public void TestPropDefColAddCollection()
        {
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);

            PropDefCol col1 = new PropDefCol();
            col1.Add(propDef1);
            col1.Add(propDef2);
            Assert.AreEqual(2, col1.Count);

            PropDefCol col2 = new PropDefCol();
            col2.Add(col1);
            Assert.AreEqual(2, col2.Count);
            Assert.IsTrue(col2.Contains("prop1"));
            Assert.IsTrue(col2.Contains("prop2"));
        }

        [Test]
        public void TestPropDefColIncludingInheritance()
        {
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            ClassDef parentClassDef = loader.LoadClass(
                @"<class name=""Parent"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef childClassDef = loader.LoadClass(
                @"<class name=""Child"" assembly=""Habanero.Test"">
					<superClass class=""Parent"" assembly=""Habanero.Test"" orMapping=""SingleTableInheritance"" discriminator=""blah"" />
                    <property  name=""Prop1"" />
				</class>
			");
            ClassDef grandchildClassDef = loader.LoadClass(
                @"<class name=""Grandchild"" assembly=""Habanero.Test"">
					<superClass class=""Child"" assembly=""Habanero.Test"" orMapping=""SingleTableInheritance"" discriminator=""blah"" />
                    <property  name=""Prop2"" />
				</class>
			");
            ClassDef.ClassDefs.Add(parentClassDef);
            ClassDef.ClassDefs.Add(childClassDef);
            ClassDef.ClassDefs.Add(grandchildClassDef);

            Assert.AreEqual(1, parentClassDef.PropDefColIncludingInheritance.Count);
            Assert.AreEqual(2, childClassDef.PropDefColIncludingInheritance.Count);
            Assert.AreEqual(3, grandchildClassDef.PropDefColIncludingInheritance.Count);

            Assert.AreEqual(1, parentClassDef.PropDefcol.Count);
            Assert.AreEqual(1, childClassDef.PropDefcol.Count);
            Assert.AreEqual(1, grandchildClassDef.PropDefcol.Count);
        }
    }
}