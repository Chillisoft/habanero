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
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestClassDef
    {
        private IClassDef _classDef;

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            //new Address();
        }
        [Test]
        public void TestConstructors()
        {
            PropDef propDef = new PropDef("prop", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDefCol propDefCol = new PropDefCol();
            propDefCol.Add(propDef);
            PrimaryKeyDef primaryKeyDef = new PrimaryKeyDef();
            primaryKeyDef.Add(propDef);
            KeyDef keyDef = new KeyDef();
            keyDef.Add(propDef);
            KeyDefCol keyDefCol = new KeyDefCol();
            keyDefCol.Add(keyDef);
            RelPropDef relPropDef = new RelPropDef(propDef, "relProp");
            RelKeyDef relKeyDef = new RelKeyDef();
            relKeyDef.Add(relPropDef);
            //RelationshipDef relDef = new SingleRelationshipDef("rel", new BusinessObject().GetType(), relKeyDef, true);
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            //relDefCol.Add(relDef);
            UIDef uiDef = new UIDef("default", null, null);
            UIDefCol uiDefCol = new UIDefCol();
            uiDefCol.Add(uiDef);

            ClassDef classDef = new ClassDef("ass", "class", null, null, null, null, null);
            Assert.AreEqual("ass", classDef.AssemblyName);
            Assert.AreEqual("class", classDef.ClassName);
            Assert.AreEqual("class", classDef.TableName);
            Assert.IsNull(classDef.PrimaryKeyDef);
            Assert.IsNull(classDef.PropDefcol);
            Assert.IsNull(classDef.KeysCol);
            Assert.IsNull(classDef.RelationshipDefCol);
            Assert.AreEqual(0, classDef.UIDefCol.Count);

            classDef = new ClassDef("ass", "class", primaryKeyDef, propDefCol,
                                             keyDefCol, relDefCol, uiDefCol);
            Assert.AreEqual("ass", classDef.AssemblyName);
            Assert.AreEqual("class", classDef.ClassName);
            Assert.AreEqual("class", classDef.TableName);
            Assert.AreEqual(1, classDef.PrimaryKeyDef.Count);
            Assert.AreEqual(1, classDef.PropDefcol.Count);
            Assert.AreEqual(1, classDef.KeysCol.Count);
            Assert.AreEqual(0, classDef.RelationshipDefCol.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);

            classDef = new ClassDef(typeof(String), primaryKeyDef, "table", propDefCol,
                                             keyDefCol, relDefCol, uiDefCol);
            //Assert.AreEqual("db", classDef.);  ? database has no usage
            Assert.AreEqual(typeof(String), classDef.ClassType);
            Assert.AreEqual("table", classDef.TableName);
            Assert.AreEqual(1, classDef.PrimaryKeyDef.Count);
            Assert.AreEqual(1, classDef.PropDefcol.Count);
            Assert.AreEqual(1, classDef.KeysCol.Count);
            Assert.AreEqual(0, classDef.RelationshipDefCol.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);

            classDef = new ClassDef(typeof(String), primaryKeyDef, "table", propDefCol,
                                             keyDefCol, relDefCol);
            //Assert.AreEqual("db", classDef.);  ? database has no usage
            Assert.AreEqual(typeof(String), classDef.ClassType);
            Assert.AreEqual("table", classDef.TableName);
            Assert.AreEqual(1, classDef.PrimaryKeyDef.Count);
            Assert.AreEqual(1, classDef.PropDefcol.Count);
            Assert.AreEqual(1, classDef.KeysCol.Count);
            Assert.AreEqual(0, classDef.RelationshipDefCol.Count);
            Assert.AreEqual(0, classDef.UIDefCol.Count);

            classDef = new ClassDef(typeof(String), primaryKeyDef, propDefCol,
                                             keyDefCol, relDefCol, uiDefCol);
            //Assert.AreEqual("db", classDef.);  ? database has no usage
            Assert.AreEqual(typeof(String), classDef.ClassType);
            Assert.AreEqual("String", classDef.TableName);
            Assert.AreEqual(1, classDef.PrimaryKeyDef.Count);
            Assert.AreEqual(1, classDef.PropDefcol.Count);
            Assert.AreEqual(1, classDef.KeysCol.Count);
            Assert.AreEqual(0, classDef.RelationshipDefCol.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);

            classDef = new ClassDef(typeof(String), primaryKeyDef, "table", propDefCol,
                                             keyDefCol, relDefCol, uiDefCol);
            //Assert.AreEqual("db", classDef.);  ? database has no usage
            Assert.AreEqual(typeof(String), classDef.ClassType);
            Assert.AreEqual("table", classDef.TableName);
            Assert.AreEqual(1, classDef.PrimaryKeyDef.Count);
            Assert.AreEqual(1, classDef.PropDefcol.Count);
            Assert.AreEqual(1, classDef.KeysCol.Count);
            Assert.AreEqual(0, classDef.RelationshipDefCol.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
        }

        [Test]
        public void TestCreateBusinessObject()
        {
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            _classDef =
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
            ClassDef.ClassDefs.Add(_classDef);
            IBusinessObject bo = _classDef.CreateNewBusinessObject();
            Assert.AreSame(typeof(MyBO), bo.GetType());
            bo.SetPropertyValue("TestProp", "TestValue");
            Assert.AreEqual("TestValue", bo.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestCreateBusinessObject_WithParametrizedClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef basicClassDef =
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
            IClassDef parametrizedClassDef =
                loader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"" typeParameter=""Special"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" />
					<property  name=""TestPropSpecial"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(basicClassDef);
            ClassDef.ClassDefs.Add(parametrizedClassDef);

            //---------------Assert PreConditions---------------    
            Assert.AreEqual(2, ClassDef.ClassDefs.Count);

            //---------------Execute Test ----------------------
            IBusinessObject bo = parametrizedClassDef.CreateNewBusinessObject();

            //---------------Test Result -----------------------
            Assert.AreSame(typeof(MyBO), bo.GetType());
            Assert.AreSame(parametrizedClassDef, bo.ClassDef);
            Assert.IsTrue(bo.Props.Contains("TestPropSpecial"));
        }

        [Test]
        public void TestCreateBusinessObject_SetsDefaults()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            _classDef =
                loader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" default=""defaultValue"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(_classDef);
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            IBusinessObject bo = _classDef.CreateNewBusinessObject();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(MyBO), bo);
            Assert.AreEqual("defaultValue", bo.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestLoadClassDefs()
        {
            //---------------Set up test pack-------------------
            XmlClassDefsLoader loader = new XmlClassDefsLoader(
                    GetTestClassDefinition(""),
                    new DtdLoader(), new DefClassFactory());
            //---------------Execute Test ----------------------
            ClassDefCol classDefs = loader.LoadClassDefs();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, classDefs.Count);
        }

        private static string GetTestClassDefinition(string suffix)
        {
            string classDefString = String.Format(
                @"
					<classes>
						<class name=""TestClass{0}"" assembly=""Habanero.Test.BO.Loaders"" >
							<property name=""TestClass{0}ID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClass{0}ID""/>
                            </primaryKey>
						</class>
						<class name=""TestRelatedClass{0}"" assembly=""Habanero.Test.BO.Loaders"" >
							<property name=""TestRelatedClass{0}ID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestRelatedClass{0}ID""/>
                            </primaryKey>
						</class>
					</classes>
			", suffix);
            return classDefString;
        }

        [Test]
        public void TestImmediateChildren()
        {
             //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, out grandchildClassDef);

             //---------------Execute Test ----------------------
            ClassDefCol children = ((ClassDef)parentClassDef).ImmediateChildren;
             //---------------Test Result -----------------------
            Assert.AreEqual(1, children.Count);
            Assert.IsTrue(children.Contains(childClassDef));
        }

        [Test]
        public void TestAllChildren()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, out grandchildClassDef);

            //---------------Execute Test ----------------------
            ClassDefCol children = ((ClassDef)parentClassDef).AllChildren;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, children.Count);
            Assert.IsTrue(children.Contains(childClassDef));
            Assert.IsTrue(children.Contains(grandchildClassDef));
            //---------------Tear Down -------------------------

        }

        [Test]
        public void TestPropDefColAddCollection()
        {
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);

            PropDefCol col1 = new PropDefCol {propDef1, propDef2};
            Assert.AreEqual(2, col1.Count);

            PropDefCol col2 = new PropDefCol {col1};
            Assert.AreEqual(2, col2.Count);
            Assert.IsTrue(col2.Contains("prop1"));
            Assert.IsTrue(col2.Contains("prop2"));
        }

        [Test]
        public void TestPropDefColIncludingInheritance()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, out grandchildClassDef);

            Assert.AreEqual(2, parentClassDef.PropDefColIncludingInheritance.Count);
            Assert.AreEqual(4, childClassDef.PropDefColIncludingInheritance.Count);
            Assert.AreEqual(6, grandchildClassDef.PropDefColIncludingInheritance.Count);

            Assert.AreEqual(2, parentClassDef.PropDefcol.Count);
            Assert.AreEqual(2, childClassDef.PropDefcol.Count);
            Assert.AreEqual(2, grandchildClassDef.PropDefcol.Count);
        }

        [Test]
        public void Test_KeyDefColIncludingInheritance_ShouldLoadAllKeyDefsInHierarchy()
        {
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, out grandchildClassDef);

            Assert.AreEqual(1, parentClassDef.KeyDefColIncludingInheritance.Count);
            Assert.AreEqual(2, childClassDef.KeyDefColIncludingInheritance.Count);
            Assert.AreEqual(3, grandchildClassDef.KeyDefColIncludingInheritance.Count);

            Assert.AreEqual(1, parentClassDef.KeysCol.Count);
            Assert.AreEqual(1, childClassDef.KeysCol.Count);
            Assert.AreEqual(1, grandchildClassDef.KeysCol.Count);
        }


        private static void LoadInheritedClassdefStructure(out IClassDef parentClassDef, out IClassDef childClassDef, out IClassDef grandchildClassDef)
        {
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, out grandchildClassDef, ORMapping.SingleTableInheritance);
        }

        private static void LoadInheritedClassdefStructure(out IClassDef parentClassDef, out IClassDef childClassDef, 
            out IClassDef grandchildClassDef, ORMapping orMappingType)
        {
            string inheritanceType = orMappingType.ToString();
            string discriminator = "";
            if (orMappingType == ORMapping.SingleTableInheritance)
            {
                discriminator = @"discriminator=""blah""";
            }
            XmlClassLoader loader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            parentClassDef = loader.LoadClass(
                @"<class name=""Parent"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
                    <property name=""FakeUCProp"" type=""string"" />
                    <key name=""UC_Fake"">
                      <prop name=""FakeUCProp"" />
                    </key>
                    <primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>

				</class>
			");
            childClassDef = loader.LoadClass(String.Format(
                @"<class name=""Child"" assembly=""Habanero.Test"">
					<superClass class=""Parent"" assembly=""Habanero.Test"" orMapping=""{0}"" {1} />
                    <property  name=""Prop1"" />
                    <property name=""FakeUCPropChild"" type=""string"" />
                    <key name=""UC_FakeChild"">
                      <prop name=""FakeUCPropChild"" />
                    </key>
				</class>
			", inheritanceType, discriminator));
            grandchildClassDef = loader.LoadClass(String.Format(
                @"<class name=""Grandchild"" assembly=""Habanero.Test"">
					<superClass class=""Child"" assembly=""Habanero.Test"" orMapping=""{0}"" {1} />
                    <property name=""Prop2"" />
                    <property name=""FakeUCPropGrandchild"" type=""string"" />
                    <key name=""UC_FakeGrandchild"">
                      <prop name=""FakeUCPropGrandchild"" />
                    </key>
				</class>
			", inheritanceType, discriminator));
            ClassDef.ClassDefs.Add(parentClassDef);
            ClassDef.ClassDefs.Add(childClassDef);
            ClassDef.ClassDefs.Add(grandchildClassDef);
        }

        // Trying to get a MissingMethodException here, not sure what to do
        //[Test, ExpectedException(typeof(MissingMethodException))]
        //public void TestNoParameterlessConstructorException()
        //{
        //    ClassDef classDef = new ClassDef(typeof(TempBO), null, null, null, null, null);
        //    TempBO temp = (TempBO)classDef.CreateNewBusinessObject();
        //}

        [Test]
        public void TestHasAutoIncrementingField()
        {
            PropDef propDef = new PropDef("prop", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDefCol propDefCol = new PropDefCol();
            propDefCol.Add(propDef);
            ClassDef classDef = new ClassDef(typeof(String), null, propDefCol, null, null);
            Assert.IsFalse(classDef.HasAutoIncrementingField);

            PropDef propDef2 = new PropDef("prop2", "ass", "class", PropReadWriteRule.ReadWrite, "field", null, false, true);
            propDefCol.Add(propDef2);
            Assert.IsTrue(classDef.HasAutoIncrementingField);
        }

        [Test]
        public void TestGetNullLookupList()
        {
            ClassDef parentClassDef = new ClassDef(typeof(String), null, new PropDefCol(), null, null);
            Assert.AreEqual(typeof(NullLookupList), parentClassDef.GetLookupList("wrongprop").GetType());

            ClassDef childClassDef = new ClassDef(typeof(String), null, new PropDefCol(), null, null);
            childClassDef.SuperClassDef = new SuperClassDef(parentClassDef, ORMapping.ClassTableInheritance);
            Assert.AreEqual(typeof(NullLookupList), childClassDef.GetLookupList("wrongprop").GetType());
        }

        [Test]
        public void TestGetRelationship()
        {
            ClassDef parentClassDef = new ClassDef(typeof(String), null, null, null, new RelationshipDefCol());
            Assert.IsNull(parentClassDef.GetRelationship("wrongrel"));

            ClassDef childClassDef = new ClassDef(typeof(String), null, null, null, new RelationshipDefCol());
            childClassDef.SuperClassDef = new SuperClassDef(parentClassDef, ORMapping.ClassTableInheritance);
            Assert.IsNull(parentClassDef.GetRelationship("wrongrel"));

            PropDef propDef = new PropDef("prop", typeof(String), PropReadWriteRule.ReadWrite, null);
            RelPropDef relPropDef = new RelPropDef(propDef, "relProp");
            RelKeyDef relKeyDef = new RelKeyDef();
            relKeyDef.Add(relPropDef);
            RelationshipDef relDef = new SingleRelationshipDef("rel", typeof(MyRelatedBo), relKeyDef, true, DeleteParentAction.Prevent);
            childClassDef.RelationshipDefCol.Add(relDef);
            Assert.AreEqual(relDef, childClassDef.GetRelationship("rel"));

            childClassDef.RelationshipDefCol = new RelationshipDefCol();
            parentClassDef.RelationshipDefCol.Add(relDef);
            Assert.AreEqual(relDef, childClassDef.GetRelationship("rel"));
        }

        #region Test GetTableName


        [Test]
        public void TestGetTableName_SingleInheritance()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, 
                out grandchildClassDef, ORMapping.SingleTableInheritance);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            string parentPropTableName = parentClassDef.GetTableName();
            string childPropTableName = childClassDef.GetTableName();
            string grandchildPropTableName = grandchildClassDef.GetTableName();
            //-------------Test Result ----------------------
            Assert.AreEqual(parentClassDef.TableName, parentPropTableName);
            Assert.AreEqual(parentClassDef.TableName, childPropTableName);
            Assert.AreEqual(parentClassDef.TableName, grandchildPropTableName);
        }

        [Test]
        public void TestGetTableName_ClassInheritance()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef,
                out grandchildClassDef, ORMapping.ClassTableInheritance);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            string parentPropTableName = parentClassDef.GetTableName();
            string childPropTableName = childClassDef.GetTableName();
            string grandchildPropTableName = grandchildClassDef.GetTableName();
            //-------------Test Result ----------------------
            Assert.AreEqual(parentClassDef.TableName, parentPropTableName);
            Assert.AreEqual(childClassDef.TableName, childPropTableName);
            Assert.AreEqual(grandchildClassDef.TableName, grandchildPropTableName);
        }

        [Test]
        public void TestGetTableName_ConcreteInheritance()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, 
                out grandchildClassDef, ORMapping.ConcreteTableInheritance);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            string parentPropTableName = parentClassDef.GetTableName();
            string childPropTableName = childClassDef.GetTableName();
            string grandchildPropTableName = grandchildClassDef.GetTableName();
            //-------------Test Result ----------------------
            Assert.AreEqual(parentClassDef.TableName, parentPropTableName);
            Assert.AreEqual(childClassDef.TableName, childPropTableName);
            Assert.AreEqual(grandchildClassDef.TableName, grandchildPropTableName);
        }

        [Test]
        public void TestGetTableName_SingleInheritance_WithPropDef()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, out grandchildClassDef);
            IPropDef parentPropDef = parentClassDef.GetPropDef("MyBoID");
            IPropDef childPropDef = childClassDef.GetPropDef("Prop1");
            IPropDef grandchildPropDef = grandchildClassDef.GetPropDef("Prop2");
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            string parentPropTableName = grandchildClassDef.GetTableName(parentPropDef);
            string childPropTableName = grandchildClassDef.GetTableName(childPropDef);
            string grandchildPropTableName = grandchildClassDef.GetTableName(grandchildPropDef);
            //-------------Test Result ----------------------
            Assert.AreEqual(parentClassDef.TableName, parentPropTableName);
            Assert.AreEqual(parentClassDef.TableName, childPropTableName);
            Assert.AreEqual(parentClassDef.TableName, grandchildPropTableName);
        }

        [Test]
        public void TestGetTableName_ClassInheritance_WithPropDef()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, 
                out grandchildClassDef, ORMapping.ClassTableInheritance);
            IPropDef parentPropDef = parentClassDef.GetPropDef("MyBoID");
            IPropDef childPropDef = childClassDef.GetPropDef("Prop1");
            IPropDef grandchildPropDef = grandchildClassDef.GetPropDef("Prop2");
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            string parentPropTableName = grandchildClassDef.GetTableName(parentPropDef);
            string childPropTableName = grandchildClassDef.GetTableName(childPropDef);
            string grandchildPropTableName = grandchildClassDef.GetTableName(grandchildPropDef);
            //-------------Test Result ----------------------
            Assert.AreEqual(parentClassDef.TableName, parentPropTableName);
            Assert.AreEqual(childClassDef.TableName, childPropTableName);
            Assert.AreEqual(grandchildClassDef.TableName, grandchildPropTableName);
        }

        [Test]
        public void TestGetTableName_ConcreteInheritance_WithPropDef()
        {
            //-------------Setup Test Pack ------------------
            ClassDef.ClassDefs.Clear();
            IClassDef parentClassDef;
            IClassDef childClassDef;
            IClassDef grandchildClassDef;
            LoadInheritedClassdefStructure(out parentClassDef, out childClassDef, 
                out grandchildClassDef, ORMapping.ConcreteTableInheritance);
            IPropDef parentPropDef = parentClassDef.GetPropDef("MyBoID");
            IPropDef childPropDef = childClassDef.GetPropDef("Prop1");
            IPropDef grandchildPropDef = grandchildClassDef.GetPropDef("Prop2");
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            string parentPropTableName = grandchildClassDef.GetTableName(parentPropDef);
            string childPropTableName = grandchildClassDef.GetTableName(childPropDef);
            string grandchildPropTableName = grandchildClassDef.GetTableName(grandchildPropDef);
            //-------------Test Result ----------------------
            Assert.AreEqual(grandchildClassDef.TableName, parentPropTableName);
            Assert.AreEqual(grandchildClassDef.TableName, childPropTableName);
            Assert.AreEqual(grandchildClassDef.TableName, grandchildPropTableName);
        }

        #endregion //Test GetTableName

        [Test]
        public void TestGetMissingPropDefReturnsNull()
        {
            ClassDef classDef = new ClassDef(typeof(String), null, new PropDefCol(), null, null);
            Assert.IsNull(classDef.GetPropDef("wrongprop", false));
        }

        [Test]
        public void TestGetMissingPropDefReturnsException()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = new ClassDef(typeof(String), null, new PropDefCol(), null, null);
            //---------------Execute Test ----------------------
            try
            {
                classDef.GetPropDef("wrongprop", true);
                Assert.Fail("Expected to throw an InvalidPropertyNameException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                StringAssert.Contains("The property definition for the property 'wrongprop' could not be found", ex.Message);
            }
        }

        [Test]
        public void TestInheritedTableName()
        {
            ClassDef parentClassDef = new ClassDef("ass", "parentclass", null, null, null, null, null);
            Assert.AreEqual("parentclass", parentClassDef.InheritedTableName);

            ClassDef childClassDef = new ClassDef("ass", "childclass", null, null, null, null, null);
            childClassDef.SuperClassDef = new SuperClassDef(parentClassDef, ORMapping.SingleTableInheritance);
            Assert.AreEqual("parentclass", childClassDef.InheritedTableName);
        }

        [Test]
        public void TestProtectedSets()
        {
            FakeClassDef classDef = new FakeClassDef();

            Assert.AreEqual("Habanero.BO", classDef.AssemblyName);
            classDef.SetAssemblyName("MyAssembly");
            Assert.AreEqual("MyAssembly", classDef.AssemblyName);

            Assert.IsNull(classDef.ClassName);
            classDef.SetClassName("MyClass");
            Assert.AreEqual("MyClass", classDef.ClassName);

            Assert.AreEqual("MyClass", classDef.ClassNameFull);
            classDef.SetClassNameFull("Habanero.BO.ClassDef");
            Assert.AreEqual("Habanero.BO.ClassDef", classDef.ClassNameFull);

            classDef = new FakeClassDef();
            Assert.AreEqual(typeof(ClassDef), classDef.ClassType);
            classDef.SetClassType(typeof(PropDef));
            Assert.AreEqual(typeof(PropDef), classDef.ClassType);

            Assert.IsNull(classDef.PropDefcol);
            classDef.SetPropDefCol(new PropDefCol());
            Assert.IsNotNull(classDef.PropDefcol);
            Assert.AreEqual(0, classDef.PropDefcol.Count);

            Assert.IsNull(classDef.KeysCol);
            classDef.SetKeyCol(new KeyDefCol());
            Assert.IsNotNull(classDef.KeysCol);
            Assert.AreEqual(0, classDef.KeysCol.Count);

            Assert.IsNull(classDef.PrimaryKeyDef);
            classDef.SetPrimaryKeyDef(new PrimaryKeyDef());
            Assert.IsNotNull(classDef.PrimaryKeyDef);
            Assert.AreEqual(0, classDef.PrimaryKeyDef.Count);

            Assert.AreEqual(0, classDef.UIDefCol.Count);
            classDef.SetUIDefCol(null);
            Assert.IsNull(classDef.UIDefCol);
        }


        [Test]
        public void TestCloningAClassDef()
        {
            ClassDef originalClassDef = (ClassDef) LoadClassDef();
            ClassDef newClassDef = originalClassDef.Clone();
            Assert.AreNotSame(newClassDef, originalClassDef);
            Assert.AreEqual(newClassDef, originalClassDef);
        }

        [Test]
        public void TestClonePropertiesAreDifferentButEqual()
        {
            ClassDef originalClassDef = (ClassDef) LoadClassDef();
            ClassDef newClassDef = originalClassDef.Clone();
            Assert.AreNotSame(newClassDef.PropDefcol, originalClassDef.PropDefcol);
            Assert.AreEqual(newClassDef.PropDefcol, originalClassDef.PropDefcol);
        }

        [Test]
        public void TestClonePropertiesAreDifferentNotEqual()
        {
            ClassDef originalClassDef = (ClassDef) LoadClassDef();
            ClassDef newClassDef = originalClassDef.Clone(true);

            Assert.AreNotSame(newClassDef.PropDefcol, originalClassDef.PropDefcol);
            Assert.IsFalse(originalClassDef.PropDefcol["MyRelatedTestProp"].Compulsory);
            Assert.IsFalse(newClassDef.PropDefcol["MyRelatedTestProp"].Compulsory);

            Assert.IsTrue(newClassDef.PropDefcol.Equals(originalClassDef.PropDefcol));
            Assert.AreEqual(newClassDef.PropDefcol, originalClassDef.PropDefcol);

            originalClassDef.PropDefcol["MyRelatedTestProp"].Compulsory = true;

            Assert.IsTrue(originalClassDef.PropDefcol["MyRelatedTestProp"].Compulsory);
            Assert.IsFalse(newClassDef.PropDefcol["MyRelatedTestProp"].Compulsory);

            Assert.IsFalse(newClassDef.PropDefcol.Equals(originalClassDef.PropDefcol));
            Assert.AreNotEqual(newClassDef.PropDefcol, originalClassDef.PropDefcol);
        }

        [Test]
        public void TestTableNamesAreCloned()
        {
            ClassDef originalClassDef = (ClassDef) LoadClassDef();
            ClassDef newClassDef = originalClassDef.Clone();
            Assert.AreEqual(originalClassDef.TableName, newClassDef.TableName);
            Assert.AreEqual(originalClassDef.DisplayName, newClassDef.DisplayName);
        }

        [Test]
        public void TestUIDefColIsCloned()
        {
            //---------------Set up test pack-------------------
            ClassDef originalClassDef = (ClassDef) LoadClassDef();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ClassDef newClassDef = originalClassDef.Clone();
            //---------------Test Result -----------------------
            Assert.AreEqual(originalClassDef.UIDefCol, newClassDef.UIDefCol);
            Assert.AreNotSame(originalClassDef.UIDefCol, newClassDef.UIDefCol);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestEqualsNull()
        {
            IClassDef classDef1 = LoadClassDef();
            const ClassDef classDef2 = null;
            Assert.AreNotEqual(classDef1, classDef2);    
        }

        [Test]
        public void TestEquals()
        {
            IClassDef classDef1 = LoadClassDef();
            IClassDef classDef2 = LoadClassDef();
            Assert.AreEqual(classDef1, classDef2);
        }

        [Test]
        public void TestEqualsDifferentType()
        {
            IClassDef classDef1 = LoadClassDef();
            Assert.AreNotEqual(classDef1, "bob");
        }

        
        
        [Test]
        public void TestEquals_DifferentTypeParameter()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef1 = LoadClassDef();
            IClassDef classDef2 = LoadClassDef();
            //---------------Assert Precondition----------------
            Assert.AreEqual(classDef1, classDef2);

            //---------------Execute Test ----------------------
            classDef2.TypeParameter = TestUtil.GetRandomString();
            //---------------Test Result -----------------------
            Assert.AreNotEqual(classDef1, classDef2);
            //---------------Tear Down -------------------------

        }


        [Test]
        public void TestGetAllClassDefsInHierarchy()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDef().SuperClassDef =
    new SuperClassDef(Shape.GetClassDef(), ORMapping.SingleTableInheritance);
            ClassDef filledCircleClassDef = (ClassDef) FilledCircleNoPrimaryKey.GetClassDef();
            filledCircleClassDef.SuperClassDef =
                new SuperClassDef(CircleNoPrimaryKey.GetClassDef(), ORMapping.SingleTableInheritance);
            CircleNoPrimaryKey.GetClassDef().SuperClassDef.Discriminator = "ShapeType";
            filledCircleClassDef.SuperClassDef.Discriminator = "ShapeType";

            //---------------Execute Test ----------------------

            IList<ClassDef> classDefs = filledCircleClassDef.GetAllClassDefsInHierarchy();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, classDefs.Count);
            Assert.AreSame(filledCircleClassDef, classDefs[0]);
            Assert.AreSame(CircleNoPrimaryKey.GetClassDef(), classDefs[1]);
            Assert.AreSame(Shape.GetClassDef(), classDefs[2]);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetBaseClassOfSingleTableHierarchy()
        {
            //---------------Set up test pack-------------------
            CircleNoPrimaryKey.GetClassDef().SuperClassDef = new SuperClassDef(Shape.GetClassDef(), ORMapping.ClassTableInheritance);
            ClassDef filledCircleClassDef = (ClassDef) FilledCircleNoPrimaryKey.GetClassDef();
            filledCircleClassDef.SuperClassDef = new SuperClassDef(CircleNoPrimaryKey.GetClassDef(), ORMapping.SingleTableInheritance);
            filledCircleClassDef.SuperClassDef.Discriminator = "ShapeType";
            //---------------Execute Test ----------------------

            ClassDef baseClass = filledCircleClassDef.GetBaseClassOfSingleTableHierarchy();
            //---------------Test Result -----------------------
            Assert.AreSame(CircleNoPrimaryKey.GetClassDef(), baseClass);
            //---------------Tear Down -------------------------
        }
    
        public static IClassDef LoadClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef def =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Test"" table=""MyRelatedBoTableName"" displayName=""My Related BO Display Name"">
					<property  name=""MyRelatedBoID"" />
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" />
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
				</class>
			");
            return def;
        }

        #region Test GetPropertyType

        [Test]
        public void TestGetPropertyType_Simple()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = LoadClassDef();
            const string dateTimeProp = "TestDateTime";
            const string intProp = "TestInt";
            const string booleanProp = "TestBoolean";
            PropDef propDef = new PropDef(dateTimeProp, typeof(DateTime), PropReadWriteRule.ReadWrite, null);
            classDef.PropDefcol.Add(propDef);
            propDef = new PropDef(intProp, typeof(int), PropReadWriteRule.ReadWrite, null);
            classDef.PropDefcol.Add(propDef);
            propDef = new PropDef(booleanProp, typeof(bool), PropReadWriteRule.ReadWrite, null);
            classDef.PropDefcol.Add(propDef);
            ClassDef.ClassDefs.Add(classDef);
            //---------------Execute Test ----------------------
            Type dateTimePropertyType = classDef.GetPropertyType(dateTimeProp);
            Type intPropertyType = classDef.GetPropertyType(intProp);
            Type booleanPropertyType = classDef.GetPropertyType(booleanProp);
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(DateTime), dateTimePropertyType);
            Assert.AreSame(typeof(int), intPropertyType);
            Assert.AreSame(typeof(bool), booleanPropertyType);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestGetPropertyType_WhenLookup_ShouldReturnUnderlyingType()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = LoadClassDef();
            const string propertyName = "Lookup";
            PropDef propDef = new PropDef(propertyName, typeof(string), PropReadWriteRule.ReadWrite, null)
                          {
                              LookupList = new SimpleLookupList(new Dictionary<string, string>())
                          };
            classDef.PropDefcol.Add(propDef);
            ClassDef.ClassDefs.Add(classDef);
            //---------------Execute Test ----------------------
            Type stringPropType = classDef.GetPropertyType(propertyName);
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(string), stringPropType);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestGetPropertyType_WhenVirtualPropDouble_ShouldReturnReflectedType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithCurrencyParameterFormat_VirtualProp();
            const string virutalProp = "-MyVirtualDoubleProp-";
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            Type propertyType = classDef.GetPropertyType(virutalProp);

            //---------------Test Result -----------------------
            Assert.AreEqual(typeof (Double), propertyType);
        }
             
        [Test]
        public void TestGetPropertyType_WhenVirtualPropNullableDouble_ShouldReturnReflectedType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithCurrencyParameterFormat_VirtualProp();
            const string virutalProp = "-MyNullableVirtualDoubleProp-";
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            Type propertyType = classDef.GetPropertyType(virutalProp);

            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(Double), propertyType);
        }

        [Test]
        public void TestGetPropertyType_WhenVirtualPropString_ShouldReturnReflectedType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithCurrencyParameterFormat_VirtualProp();
            const string virutalProp = "-MyName-";
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            Type propertyType = classDef.GetPropertyType(virutalProp);

            //---------------Test Result -----------------------
            Assert.AreEqual(typeof (string), propertyType);
        }

        [Test]
        public void TestGetPropertyType_RelatedProperty()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef relatedClassDef = MyRelatedBo.LoadClassDef();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            const string dateTimeProp = "TestDateTime";
            PropDef propDef = new PropDef(dateTimeProp, typeof(DateTime), PropReadWriteRule.ReadWrite, null);
            relatedClassDef.PropDefcol.Add(propDef);
            //---------------Execute Test ----------------------
            Type dateTimePropertyType = classDef.GetPropertyType("MyRelationship." + dateTimeProp);
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(DateTime), dateTimePropertyType);
        }

        [Test]
        public void Test_GetPropertyType_WhenInheritedClass_AndPropOnRelationshipDefinedInParentType_ShouldReturnCorrectType()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef relatedClassDef = MyInheritedType.LoadInheritedTypeClassDef();
            IClassDef classDef = MyBO.LoadClassDefWithDateTime();
           
            const string dateTimeProp = "TestDateTime";
            PropDef propDef = new PropDef(dateTimeProp, typeof(DateTime), PropReadWriteRule.ReadWrite, null);
            relatedClassDef.PropDefcol.Add(propDef);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(classDef.GetPropDef(dateTimeProp, false));
            //---------------Execute Test ----------------------
            Type dateTimePropertyType = relatedClassDef.GetPropertyType("MyRelationship." + dateTimeProp);
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(DateTime), dateTimePropertyType);
        }
        #endregion //Test GetPropertyType

        [Test]
        public void TestCreatePropertyComparer()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            //---------------Execute Test ----------------------
            IPropertyComparer<MyBO> propertyComparer = classDef.CreatePropertyComparer<MyBO>("TestProp");
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(string), propertyComparer.PropertyType);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestGet()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            
            //---------------Execute Test ----------------------
            IClassDef gotClassDef = ClassDef.Get<MyBO>();
            //---------------Test Result -----------------------

            Assert.AreSame(classDef, gotClassDef);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGet_NonExistant()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();

            //---------------Execute Test ----------------------
            try
            {
                ClassDef.Get<ContactPerson>();
                Assert.Fail("Get<ContactPerson> should have thrown an error as there is no ClassDef for ContactPerson");
            }
            catch (HabaneroDeveloperException ex)
            {
            //---------------Test Result -----------------------
                StringAssert.Contains("No ClassDef has been loaded for " + typeof(ContactPerson).FullName, ex.Message);
            }
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetPropDef_WithSource()
        {
            //---------------Set up test pack-------------------

            IClassDef myRelatedClassDef = MyRelatedBo.LoadClassDef();
            IClassDef myBoClassDef = MyBO.LoadClassDefWithRelationship();
            Source source = new Source("MyRelationship");
            const string myrelatedtestpropName = "MyRelatedTestProp";
            //---------------Execute Test ----------------------
            IPropDef myRelatedTestPropDef = myBoClassDef.GetPropDef(source, myrelatedtestpropName, false);
            //---------------Test Result -----------------------
            Assert.AreSame(myRelatedClassDef.PropDefcol[myrelatedtestpropName], myRelatedTestPropDef);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetPropDef_WithSource_TwoLevels()
        {
            //---------------Set up test pack-------------------

            IClassDef engineClassDef = Engine.LoadClassDef_IncludingCarAndOwner();
            ClassDef contactPersonClassDef = new ContactPerson().ClassDef;
            Source source = Source.FromString("Car.Owner");
            const string surnamePropName = "Surname";
            //---------------Execute Test ----------------------
            IPropDef surnamePropDef = engineClassDef.GetPropDef(source, surnamePropName, false);
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonClassDef.PropDefcol[surnamePropName], surnamePropDef);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetPropDef_WithInvalidSource_ThrowError()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef myBoClassDef = MyBO.LoadDefaultClassDef();
            Source source = new Source("MyRelationship");
            const string myrelatedtestpropName = "MyRelatedTestProp";
            //---------------Execute Test ----------------------
            try
            {
                myBoClassDef.GetPropDef(source, myrelatedtestpropName, true);
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("does not contain a relationship with the name ", ex.Message);
            }
        }

        [Test]
        public void TestGetPropDef_WithInvalidSource_NoThrowError()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef myBoClassDef = MyBO.LoadDefaultClassDef();
            Source source = new Source("MyRelationship");
            const string myrelatedtestpropName = "MyRelatedTestProp";
            //---------------Execute Test ----------------------
            IPropDef myRelatedTestPropDef = myBoClassDef.GetPropDef(source, myrelatedtestpropName, false);
            //---------------Test Result -----------------------
            Assert.IsNull(myRelatedTestPropDef);
            //---------------Tear Down -------------------------

        }

        [Test]
        public void TestUIDefColHasClassDefSetup()
        {
            //---------------Execute Test ----------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            //---------------Test Result -----------------------
            Assert.AreSame(classDef, classDef.UIDefCol.ClassDef);
        }

        [Test] public void TestClassDef_ClassDefID_NullByDefault()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            //---------------Execute Test ----------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            //---------------Test Result -----------------------
            Assert.IsNull(classDef.ClassID);
            //---------------Tear Down -------------------------          
        }        
        
        [Test] public void TestClassDef_ClassModuleName_NullByDefault()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            //---------------Execute Test ----------------------
            IClassDef classDef = MyBO.LoadDefaultClassDef();
            //---------------Test Result -----------------------
            Assert.IsNull(classDef.Module);
            //---------------Tear Down -------------------------          
        }
        
        [Test] public void TestClassDef_ClassModuleName()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            //---------------Execute Test ----------------------
            IClassDef classDef = MyBO.LoadClassDefsHasModuleName();
            //---------------Test Result -----------------------
            Assert.AreEqual("MyBOModule",classDef.Module);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAutoDisplayName()
        {
            XmlClassLoader classLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef def = classLoader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual("Test Class", def.DisplayName);
        }

        [Test]
        public void Test_GetRelationship_ShouldReturnTheRelationshipDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithShape_SingleTableInheritance_Relationship();
            const string relationshipName = "Shape";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var relationshipDef = classDef.GetRelationship(relationshipName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(relationshipDef);
            Assert.AreEqual(relationshipName, relationshipDef.RelationshipName);
        }
        [Test]
        public void Test_GetRelationship_WhenOnParent_ShouldReturnTheRelationshipDefFromParent()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithShape_SingleTableInheritance_Relationship();
            IClassDef classDefSubType = MyBOSubType.LoadInheritedTypeClassDef();
            const string relationshipName = "Shape";
            //---------------Assert Precondition----------------
            Assert.IsNotNull(classDef.GetRelationship(relationshipName));
            //---------------Execute Test ----------------------
            var relationshipDef = classDefSubType.GetRelationship(relationshipName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(relationshipDef);
            Assert.AreEqual(relationshipName, relationshipDef.RelationshipName);
        }

        [Test]
        public void Test_SetRelationshipDefCol_ShouldSetClassDefOnRelDefCol()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = new FakeClassDef();
            IRelationshipDefCol relationshipDefCol = MockRepository.GenerateStub<IRelationshipDefCol>();
            //---------------Assert Precondition----------------
            Assert.IsNull(relationshipDefCol.ClassDef);
            //---------------Execute Test ----------------------
            classDef.RelationshipDefCol = relationshipDefCol;
            //---------------Test Result -----------------------
            Assert.AreSame(relationshipDefCol, classDef.RelationshipDefCol);
            Assert.AreSame(classDef, relationshipDefCol.ClassDef);
        }
        [Test]
        public void Test_SetUIDefCol_ShouldSetClassDefOnCol()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = new FakeClassDef();
            var uiDefCol = MockRepository.GenerateStub<UIDefCol>();
            //---------------Assert Precondition----------------
            Assert.IsNull(uiDefCol.ClassDef);
            //---------------Execute Test ----------------------
            classDef.UIDefCol = uiDefCol;
            //---------------Test Result -----------------------
            Assert.AreSame(uiDefCol, classDef.UIDefCol);
            Assert.AreSame(classDef, uiDefCol.ClassDef);
        }
        [Test]
        public void Test_SetKeyCol_ShouldSetClassDefOnCol()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = new FakeClassDef();
            var keyDefCol = MockRepository.GenerateStub<KeyDefCol>();
            //---------------Assert Precondition----------------
            Assert.IsNull(keyDefCol.ClassDef);
            //---------------Execute Test ----------------------
            classDef.KeysCol = keyDefCol;
            //---------------Test Result -----------------------
            Assert.AreSame(keyDefCol, classDef.KeysCol);
            Assert.AreSame(classDef, keyDefCol.ClassDef);
        }
        [Test]
        public void Test_SetPropDefCol_ShouldSetClassDefOnCol()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = new FakeClassDef();
            var propDefCol = MockRepository.GenerateStub<IPropDefCol>();
            //---------------Assert Precondition----------------
            Assert.IsNull(propDefCol.ClassDef);
            //---------------Execute Test ----------------------
            classDef.PropDefcol = propDefCol;
            //---------------Test Result -----------------------
            Assert.AreSame(propDefCol, classDef.PropDefcol);
            Assert.AreSame(classDef, propDefCol.ClassDef);
        }

        [Test]
        public void Test_GetPropDef_ShouldNotClearPropDefsClassDef()
        {
            //---------------Set up test pack-------------------
            var superClassDef = Structure.Entity.LoadDefaultClassDef();
            IClassDef classDef = Test.Structure.Part.LoadClassDef_WithClassTableInheritance();
            const string propertyName = "EntityType";
            IPropDef expectedpropdef = superClassDef.PropDefcol[propertyName];
            //---------------Assert Precondition----------------
            Assert.AreSame(superClassDef, expectedpropdef.ClassDef);
            Assert.AreSame(superClassDef, classDef.SuperClassClassDef);
            //Assert.IsTrue(classDef.PropDefColIncludingInheritance.Contains(propertyName));
            //---------------Execute Test ----------------------
            IPropDef propDef = classDef.GetPropDef(propertyName);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedpropdef, propDef);
            Assert.AreSame(superClassDef, propDef.ClassDef);
        }

        [Test]
        public void Test_CallGetPropDef_AddPropDefToPropDefCol_ThenGetPropDef_ShouldHaveNewPropDef_FixBug583()
        {
            //---------------Set up test pack-------------------
            FakeClassDef classDef = new FakeClassDef();
            classDef.SetPropDefCol(new PropDefCol());
            const string propertyName = "SomePropName";
            var initialPropDef = classDef.GetPropDef(propertyName, false);
            var addedPropDef = new PropDefFake(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsNull(initialPropDef);
            //---------------Execute Test ----------------------
            classDef.PropDefcol.Add(addedPropDef);
            var returnedPropDef = classDef.GetPropDef(propertyName, false);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnedPropDef);
            Assert.AreSame(addedPropDef, returnedPropDef);
        }
        [Test]
        public void Test_CallGetPropDef_RemovePropDefToPropDefCol_ThenGetPropDef_ShouldNotHaveRemovedPropDef_FixBug583()
        {
            //---------------Set up test pack-------------------
            FakeClassDef classDef = new FakeClassDef();
            classDef.SetPropDefCol(new PropDefCol());
            const string propertyName = "SomePropName";           
            var removedPropDef = new PropDefFake(propertyName);
            classDef.PropDefcol.Add(removedPropDef);

            var initialPropDef = classDef.GetPropDef(propertyName, false);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(initialPropDef);
            //---------------Execute Test ----------------------
            classDef.PropDefcol.Remove(removedPropDef);
            var returnedPropDef = classDef.GetPropDef(propertyName, false);
            //---------------Test Result -----------------------
            Assert.IsNull(returnedPropDef);
        }

        [Test]
        public void Test_GetPropDef_FromPropDefColIncludingInheritance_ShouldNotClearPropDefsClassDef()
        {
            //---------------Set up test pack-------------------
            var superClassDef = Structure.Entity.LoadDefaultClassDef();
            IClassDef classDef = Test.Structure.Part.LoadClassDef_WithClassTableInheritance();
            const string propertyName = "EntityType";
            IPropDef expectedpropdef = superClassDef.PropDefcol[propertyName];
            //---------------Assert Precondition----------------
            Assert.AreSame(superClassDef, expectedpropdef.ClassDef);
            Assert.AreSame(superClassDef, classDef.SuperClassClassDef);
            Assert.IsTrue(classDef.PropDefColIncludingInheritance.Contains(expectedpropdef));
            //---------------Execute Test ----------------------
            IPropDef propDef = classDef.PropDefColIncludingInheritance[propertyName];
            //---------------Test Result -----------------------
            Assert.AreSame(expectedpropdef, propDef);
            Assert.AreSame(superClassDef, propDef.ClassDef);
        }
/*
        [Test]
        public void Test_GetPK_WhenIsSubType_AndIDIsDeclaredOnParent_ShouldReturnParentsPK_FixBugBug867()
        {
            //---------------Set up test pack-------------------
            SuperClass.LoadClassDef();
            SubClass.LoadClassDef();
            var subClass = new SubClass();
            var superClass = new SuperClass();
            var superClassPKDef = superClass.ClassDef.PrimaryKeyDef;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(superClassPKDef);
            //---------------Execute Test ----------------------
            var primaryKeyDef = subClass.ClassDef.PrimaryKeyDef;
            //---------------Test Result -----------------------
            Assert.AreSame(superClassPKDef, primaryKeyDef);
        }*/
    }
    
    // This class serves to access protected methods
    internal class FakeClassDef : ClassDef
    {
        public FakeClassDef()
            : base(typeof(ClassDef), null, null, null, null, null, null)
        { }

        public void SetAssemblyName(string assemblyName)
        {
            AssemblyName = assemblyName;
        }

        public void SetClassName(string className)
        {
            ClassName = className;
        }

        public void SetClassNameFull(string className)
        {
            ClassNameFull = className;
        }

        public void SetClassType(Type type)
        {
            ClassType = type;
        }

        public void SetPropDefCol(IPropDefCol col)
        {
            PropDefcol = col;
        }

        public void SetKeyCol(KeyDefCol col)
        {
            KeysCol = col;
        }

        public void SetPrimaryKeyDef(IPrimaryKeyDef pkDef)
        {
            PrimaryKeyDef = pkDef;
        }

        public void SetUIDefCol(UIDefCol col)
        {
            UIDefCol = col;
        }
    }

    public class SuperClass : BusinessObject
    {
        public static IClassDef LoadClassDef()
        {
            const string classDefStr = @"<class name=""SuperClass"" assembly=""Habanero.Test.BO"">
                        <property name=""SuperClassID"" type=""Guid"" readWriteRule=""WriteNew"" compulsory=""true""/>
                        <property name=""SuperClassType"" />
                        <primaryKey>
                          <prop name=""SuperClassID"" />
                        </primaryKey>
                    </class>";
            return LoadClassDef(classDefStr);
        }

        protected static IClassDef LoadClassDef(string classDefStr)
        {
            var itsLoader = CreateXmlClassLoader();
            var classDef =
                itsLoader.LoadClass(classDefStr);
            ClassDef.ClassDefs.Add(classDef);
            return classDef;
        }

        private static XmlClassLoader CreateXmlClassLoader()
        {
            return new XmlClassLoader(new DtdLoader(), new DefClassFactory());
        }
        public Guid? SuperClassID { get; set; }
    }
    public class SubClass : SuperClass
    {
        public new static IClassDef LoadClassDef()
        {
            const string classDefStr = "<class name=\"SubClass\" assembly=\"Habanero.Test.BO\"> <superClass class=\"SuperClass\" assembly=\"Habanero.Test.BO\" orMapping=\"SingleTableInheritance\" discriminator=\"SuperClassType\"/> </class>";

            return LoadClassDef(classDefStr);
        }
    }
}