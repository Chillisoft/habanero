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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{

    [TestFixture]
    public class TestRelKeyDef
    {
        private RelKeyDef mRelKeyDef;
        private PropDefCol mPropDefCol;

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            mRelKeyDef = new RelKeyDef();
            mPropDefCol = new PropDefCol();

            PropDef propDef = new PropDef("Prop", typeof(string), PropReadWriteRule.ReadWrite, "1");

            mPropDefCol.Add(propDef);
            RelPropDef lRelPropDef = new RelPropDef(propDef, "PropName");
            mRelKeyDef.Add(lRelPropDef);

            propDef = new PropDef("Prop2", typeof(string), PropReadWriteRule.ReadWrite, "2");

            mPropDefCol.Add(propDef);
            lRelPropDef = new RelPropDef(propDef, "PropName2");
            mRelKeyDef.Add(lRelPropDef);
        }

        [Test]
        public void Test_Contains_WhenHas_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var relKeyDef = new RelKeyDef();
            const string ownerClassPropDefName = "fdafads";
            IRelPropDef relPropDef = new RelPropDef(ownerClassPropDefName, "fdafasd");
            relKeyDef.Add(relPropDef);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, relKeyDef.Count);
            //---------------Execute Test ----------------------
            var contains = relKeyDef.Contains(ownerClassPropDefName);
            //---------------Test Result -----------------------
            Assert.IsTrue(contains);
        }
        [Test]
        public void Test_Contains_WhenNotHas_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var relKeyDef = new RelKeyDef();
            const string ownerClassPropDefName = "fdafads";
            IRelPropDef relPropDef = new RelPropDef(ownerClassPropDefName, "fdafasd");
            relKeyDef.Add(relPropDef);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, relKeyDef.Count);
            //---------------Execute Test ----------------------
            var contains = relKeyDef.Contains("SomeOtherName");
            //---------------Test Result -----------------------
            Assert.IsFalse(contains);
        }

        [Test]
        public void TestAddPropDef()
        {
            Assert.AreEqual(2, mRelKeyDef.Count);
        }

        [Test]
        public void TestContainsPropDef()
        {
            Assert.IsTrue(mRelKeyDef.Contains("Prop"));
            IRelPropDef lPropDef = mRelKeyDef["Prop"];
            Assert.AreEqual("Prop", lPropDef.OwnerPropertyName);
        }

        [Test]
        public void TestCreateRelKey()
        {
            IBOPropCol propCol = mPropDefCol.CreateBOPropertyCol(true);
            IRelKey relKey = mRelKeyDef.CreateRelKey(propCol);
            Assert.IsTrue(relKey.Contains("Prop"));
            Assert.IsTrue(relKey.Contains("Prop2"));
            IRelProp relProp = relKey["Prop"];
            Assert.AreEqual("Prop", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName", relProp.RelatedClassPropName);
            relProp = relKey["Prop2"];
            Assert.AreEqual("Prop2", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName2", relProp.RelatedClassPropName);

            Assert.IsTrue(relKey.HasRelatedObject(),
                          "This is true since the values for the properties should have defaulted to 1 each");

            Assert.AreEqual("(PropName = '1') AND (PropName2 = '2')", relKey.Criteria.ToString());
        }

        [Test]
        public void TestAddNullException()
        {
            //---------------Set up test pack-------------------
            RelKeyDef col = new RelKeyDef();
            //---------------Execute Test ----------------------
            try
            {
                col.Add(null);
                Assert.Fail("Expected to throw an HabaneroArgumentException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains("You cannot add a null prop def to a classdef", ex.Message);
            }
        }

        [Test]
        public void TestRemove()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            RelPropDef relPropDef = new RelPropDef(propDef, "prop");
            RelKeyDefDefInheritor relKeyDef = new RelKeyDefDefInheritor();

            relKeyDef.CallRemove(relPropDef);
            relKeyDef.Add(relPropDef);
            Assert.AreEqual(1, relKeyDef.Count);
            relKeyDef.CallRemove(relPropDef);
            Assert.AreEqual(0, relKeyDef.Count);
        }

        [Test]
        public void TestThisIndexerException()
        {
            //---------------Set up test pack-------------------
            RelKeyDef relKeyDef = new RelKeyDef();
            //---------------Execute Test ----------------------
            try
            {
                IRelPropDef relPropDef = relKeyDef["prop"];
                Assert.Fail("Expected to throw an InvalidPropertyNameException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                StringAssert.Contains("In a relationship property definition, the property with the name 'prop' does not exist in the collection of properties", ex.Message);
            }
        }

        // Grants access to protected methods
        private class RelKeyDefDefInheritor : RelKeyDef
        {
            public void CallRemove(RelPropDef relPropDef)
            {
                Remove(relPropDef);
            }
        }
    }
}
