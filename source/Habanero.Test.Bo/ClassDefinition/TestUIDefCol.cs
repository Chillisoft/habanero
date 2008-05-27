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

using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIDefCol
    {
        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestAddDuplicateNameException()
        {
            UIDef uiDef1 = new UIDef("defname", null, null);
            UIDef uiDef2 = new UIDef("defname", null, null);
            UIDefCol col = new UIDefCol();
            col.Add(uiDef1);
            col.Add(uiDef2);
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

        [Test, ExpectedException(typeof(HabaneroApplicationException))]
        public void TestDefaultUIDefMissingException()
        {
            UIDefCol col = new UIDefCol();
            UIDef uiDef = col["default"];
        }

        [Test, ExpectedException(typeof(HabaneroApplicationException))]
        public void TestOtherUIDefMissingException()
        {
            UIDefCol col = new UIDefCol();
            UIDef uiDef = col["otherdef"];
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
        public void TestEqualsNull()
        {
            UIDefCol uIDefCol1 = new UIDefCol();
            UIDefCol uIDefCol2 = null;
            Assert.AreNotEqual(uIDefCol1, uIDefCol2);
        }

        [Test]
        public void TestEquals()
        {
            //---------------Execute Test ----------------------
            UIDefCol uIDefCol1 = new UIDefCol();
            UIDef def = new UIDef("UiDefname", null, null);
            uIDefCol1.Add(def);
            UIDefCol uIDefCol2 = new UIDefCol();
            uIDefCol2.Add(def);
            //---------------Test Result -----------------------
            Assert.AreEqual(uIDefCol1, uIDefCol2);
        }
        //TODO test where uiDefname is same but uiDefObject is different and children objects are different
        [Test]
        public void TestEqualsDifferentType()
        {
            UIDefCol uIDefCol1 = new UIDefCol();
            Assert.AreNotEqual(uIDefCol1, "bob");
        }

        //TODO:
        [Test, Ignore("under dev")]
        public void TestCloneUIDefCol()
        {
            //---------------Set up test pack-------------------
             ClassDef originalClassDef = LoadClassDef();           
            //---------------Execute Test ----------------------
            UIDefCol newUIDefCol = originalClassDef.UIDefCol.Clone();
            //---------------Test Result -----------------------

            Assert.AreNotSame(newUIDefCol, originalClassDef.UIDefCol);
            Assert.AreEqual(newUIDefCol, originalClassDef.UIDefCol);
            //---------------Tear Down -------------------------
        }


        public static ClassDef LoadClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef def =
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
    }

}
