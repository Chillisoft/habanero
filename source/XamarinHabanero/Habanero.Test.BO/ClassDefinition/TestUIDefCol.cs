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
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIDefCol
    {
        [Test]
        public void TestAddDuplicateNameException()
        {
            //---------------Set up test pack-------------------
            UIDef uiDef1 = new UIDef("defname", null, null);
            UIDef uiDef2 = new UIDef("defname", null, null);
            UIDefCol col = new UIDefCol();
            col.Add(uiDef1);
            //---------------Execute Test ----------------------
            try
            {
                col.Add(uiDef2);

                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("a definition with that name already exists", ex.Message);
            }
        }

        [Test]
        public void TestContains()
        {
            UIDef uiDef = new UIDef("defname", null, null);
            UIDefCol col = new UIDefCol();

            Assert.IsFalse(col.Contains(uiDef));
            col.Add(uiDef);
            Assert.IsTrue(col.Contains(uiDef));
        }

        [Test]
        public void TestRemove()
        {
            UIDef uiDef = new UIDef("defname", null, null);
            UIDefCol col = new UIDefCol();
            col.Add(uiDef);

            Assert.IsTrue(col.Contains(uiDef));
            col.Remove(uiDef);
            Assert.IsFalse(col.Contains(uiDef));
        }

        [Test]
        public void TestDefaultUIDefMissingException()
        {
            //---------------Set up test pack-------------------
            UIDefCol col = new UIDefCol();
            //---------------Execute Test ----------------------
            try
            {
                IUIDef uiDef = col["default"];
                Assert.Fail("Expected to throw an HabaneroApplicationException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                StringAssert.Contains("No default 'ui' definition exists (a definition with no name attribute)", ex.Message);
            }
        }

        [Test]
        public void TestOtherUIDefMissingException()
        {
            UIDefCol col = new UIDefCol();
            try
            {
                IUIDef uiDef = col["otherdef"];
                Assert.Fail("Expected to throw an HabaneroApplicationException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                StringAssert.Contains("The ui definition with the name 'otherdef' does not " +
                                                               "exist in the collection of definitions for the", ex.Message);
            }
        }

        [Test]
        public void TestEnumerator()
        {
            UIDef uiDef1 = new UIDef("defname1", null, null);
            UIDef uiDef2 = new UIDef("defname2", null, null);
            UIDefCol col = new UIDefCol();
            col.Add(uiDef1);
            col.Add(uiDef2);

            int count = 0;
            foreach (object def in col)
            {
                count++;
            }
            Assert.AreEqual(2, count);
        }

        [Test]
        public void Test_NotEqualsNull()
        {
            UIDefCol uIDefCol1 = new UIDefCol();
            UIDefCol uIDefCol2 = null;
            AssertNotEqual(uIDefCol1, uIDefCol2);
        }

        [Test]
        public void TestEquals_SameUIDef()
        {
            //---------------Execute Test ----------------------
            UIDefCol uIDefCol1 = new UIDefCol();
            UIDef def = new UIDef("UiDefname", null, null);
            uIDefCol1.Add(def);
            UIDefCol uIDefCol2 = new UIDefCol();
            uIDefCol2.Add(def);
            //---------------Test Result -----------------------
            AssertAreEqual(uIDefCol1, uIDefCol2);
//            Assert.AreEqual(uIDefCol1, uIDefCol2);
        }

        [Test]
        public void Test_HashCode_Equals()
        {
            //--------------- Set up test pack ------------------
            UIDefCol uIDefCol1 = new UIDefCol();
            UIDef def = new UIDef("UiDefname", null, null);
            uIDefCol1.Add(def);
            UIDefCol uIDefCol2 = new UIDefCol();
            uIDefCol2.Add(def);
            //--------------- Test Preconditions ----------------
            AssertAreEqual(uIDefCol1, uIDefCol2);
            //--------------- Execute Test ----------------------

            //--------------- Test Result -----------------------
            Assert.AreEqual(uIDefCol1.GetHashCode(), uIDefCol2.GetHashCode());
        }

        [Test]
        public void Test_HashCode_NotEquals()
        {
            //--------------- Set up test pack ------------------
            UIDefCol uIDefCol1 = new UIDefCol();
            UIDef def = new UIDef("UiDefname", null, null);
            UIDef def2 = new UIDef("UiDefName2", null, null);
            uIDefCol1.Add(def);
            UIDefCol uIDefCol2 = new UIDefCol();
            uIDefCol2.Add(def2);
            //--------------- Test Preconditions ----------------
            AssertNotEqual(uIDefCol1, uIDefCol2);
            //--------------- Execute Test ----------------------

            //--------------- Test Result -----------------------
            Assert.AreNotEqual(uIDefCol1.GetHashCode(), uIDefCol2.GetHashCode());
        }

        [Test]
        public void Test_NotEqualsWrongType()
        {
            UIDefCol uIDefCol1 = new UIDefCol();
            Assert.IsFalse(uIDefCol1.Equals("FFFF"));
        }

        [Test]
        public void Test_NotEqual_DifferentCount()
        {
            UIDefCol uIDefCol1 = new UIDefCol();
            UIDef def = new UIDef("UiDefname", null, null);
            uIDefCol1.Add(def);
            UIDefCol uIDefCol2 = new UIDefCol();
            uIDefCol2.Add(def);
            UIDef def2 = new UIDef("UiDefname2", null, null);
            uIDefCol2.Add(def2);
            AssertNotEqual(uIDefCol1, uIDefCol2);
        }
        [Test]
        public void Test_NotEqual_DifferentDefs()
        {
            UIDefCol uIDefCol1 = new UIDefCol();
            UIDef def = new UIDef("UiDefname", null, null);
            uIDefCol1.Add(def);
            UIDefCol uIDefCol2 = new UIDefCol();
            uIDefCol2.Add(def);
            UIDef def2 = new UIDef("UiDefname2", null, null);
            uIDefCol2.Add(def2);

            UIDef def3 = new UIDef("UiDefname3", null, null);
            uIDefCol1.Add(def3);

            AssertNotEqual(uIDefCol1, uIDefCol2);
        }
        private static void AssertAreEqual(UIDefCol uiDefCol, UIDefCol uiDefCol2)
        {
            Assert.IsTrue(uiDefCol.Equals(uiDefCol2));
            Assert.IsTrue(uiDefCol == uiDefCol2);
            Assert.IsFalse(uiDefCol != uiDefCol2);
        }

        private static void AssertNotEqual(UIDefCol uiDefCol, UIDefCol uiDefCol2)
        {
            Assert.IsFalse(uiDefCol.Equals(uiDefCol2));
            Assert.IsFalse(uiDefCol == uiDefCol2);
            Assert.IsTrue(uiDefCol != uiDefCol2);
        }

        [Test]
        public void TestCloneUIDefCol()
        {
            //---------------Set up test pack-------------------
             IClassDef originalClassDef = LoadClassDef();           
            //---------------Execute Test ----------------------
            UIDefCol newUIDefCol = originalClassDef.UIDefCol.Clone();
            //---------------Test Result -----------------------

            Assert.AreNotSame(newUIDefCol, originalClassDef.UIDefCol);
            Assert.IsTrue(newUIDefCol.Equals(originalClassDef.UIDefCol));
            Assert.IsTrue(newUIDefCol == originalClassDef.UIDefCol);
            Assert.AreEqual(newUIDefCol["default"], originalClassDef.UIDefCol["default"]);
            Assert.AreNotSame(newUIDefCol["default"], originalClassDef.UIDefCol["default"]);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestAddUIDef()
        {
            //---------------Set up test pack-------------------
            UIDef uiDef = new UIDef("test", null, null);
            UIDefCol uiDefcol = new UIDefCol();
            //---------------Assert Precondition----------------
Assert.IsNull(uiDef.UIDefCol);
            //---------------Execute Test ----------------------
            uiDefcol.Add(uiDef);
            //---------------Test Result -----------------------
            Assert.AreSame(uiDefcol, uiDef.UIDefCol);

        }
        [Test]
        public void TestClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            UIDefCol uiDefCol = new UIDefCol();
            IClassDef classdef = MyBO.LoadDefaultClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            uiDefCol.ClassDef = classdef;
            //---------------Test Result -----------------------
            Assert.AreSame(classdef, uiDefCol.ClassDef);

        }


        [Test]
        public void Test_ClassName_WhenClassDefNull_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            UIDefCol uiDefCol = new UIDefCol();
            //---------------Assert Precondition----------------
            Assert.IsNull(uiDefCol.ClassDef);
            //---------------Execute Test ----------------------
            string className = uiDefCol.ClassName;
            //---------------Test Result -----------------------
            Assert.IsEmpty(className);
        }
        [Test]
        public void Test_ClassName_WhenClassDefSet_ShouldReturnClassDefClassName()
        {
            //---------------Set up test pack-------------------
            UIDefCol uiDefCol = new UIDefCol();
            var classDef = MockRepository.GenerateStub<IClassDef>();
            classDef.ClassName = GetRandomString();
            uiDefCol.ClassDef = classDef;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(uiDefCol.ClassDef);
            Assert.IsNotNullOrEmpty(classDef.ClassName);
            //---------------Execute Test ----------------------
            string className = uiDefCol.ClassName;
            //---------------Test Result -----------------------
            Assert.AreSame(classDef.ClassName, className);
        }

        private static string GetRandomString()
        {
            return TestUtil.GetRandomString();
        }

        public static IClassDef LoadClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef def =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Test"" table=""MyRelatedBo"">
					<property  name=""MyRelatedBoID"" />
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" />
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
                    <ui>
                      <grid>
                        <column heading=""MyRelatedTestProp"" property=""MyRelatedTestProp"" width=""200"" />
                      </grid>
                      <form width=""400"">
                        <columnLayout width=""350"">
                          <field label=""MyRelatedTestProp :"" property=""MyRelatedTestProp"" />
                        </columnLayout>
                      </form>
                    </ui>
				</class>
			");
            return def;
        }

        [Test]
        public void Test_SetClassDef_ShouldSet()
        {
            //---------------Set up test pack-------------------
            var classDef = MockRepository.GenerateStub<IClassDef>();
            UIDefCol uiDefCol = new UIDefCol();
            //---------------Assert Precondition----------------
            Assert.IsNull(uiDefCol.ClassDef);
            //---------------Execute Test ----------------------
            uiDefCol.ClassDef = classDef;
            //---------------Test Result -----------------------
            Assert.AreSame(classDef, uiDefCol.ClassDef);
        }
        [Test]
        public void Test_Add_ShouldSetUIDefssClassDef()
        {
            //---------------Set up test pack-------------------
            var uiDef = new UIDefStub();
            var col = new UIDefCol();
            var expectedClassDef = MockRepository.GenerateStub<IClassDef>();
            col.ClassDef = expectedClassDef;
            //---------------Assert Preconditions---------------
            Assert.IsNull(uiDef.ClassDef);
            //---------------Execute Test ----------------------
            col.Add(uiDef);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedClassDef, uiDef.ClassDef);
        }
        [Test]
        public void Test_Add_ShouldSetUIDefsGridDefsClassDef()
        {
            //---------------Set up test pack-------------------
            var uiDef = new UIDefStub();
            UIGrid uiGrid = new UIGrid();
            uiDef.SetUIGrid(uiGrid);
            var col = new UIDefCol();
            var expectedClassDef = MockRepository.GenerateStub<IClassDef>();
            col.ClassDef = expectedClassDef;
            //---------------Assert Preconditions---------------
            Assert.IsNull(uiDef.ClassDef);
            //---------------Execute Test ----------------------
            col.Add(uiDef);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedClassDef, uiGrid.ClassDef);
        }
    }

}
