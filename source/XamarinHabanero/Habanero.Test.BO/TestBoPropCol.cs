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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBoPropCol
    {
        private PropDef mPropDef;
        private IBOProp mProp;
        private BOPropCol mBOPropCol;

        [SetUp]
        public void init()
        {
            mBOPropCol = new BOPropCol();
            mPropDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadWrite, null);
            mBOPropCol.Add(mPropDef.CreateBOProp(false));

            mPropDef = new PropDef("Prop2", typeof(string), PropReadWriteRule.ReadWrite, null);
            mPropDef.AddPropRule(new PropRuleString(mPropDef.PropertyName, "Test Message", 1, 10, null));
            mBOPropCol.Add(mPropDef.CreateBOProp(false));

            BOPropCol anotherPropCol = new BOPropCol();
            PropDef anotherPropDef =
                new PropDef("TestAddPropCol", typeof(string), PropReadWriteRule.ReadWrite, null);
            anotherPropCol.Add(anotherPropDef.CreateBOProp(false));

            mBOPropCol.Add(anotherPropCol);
        }

        [Test]
        public void TestSetBOPropValue()
        {
            mProp = mBOPropCol["Prop2"];
            mProp.Value = "Prop Value";
            Assert.AreEqual("Prop Value", mProp.Value);

            mProp = mBOPropCol["PropName"];
            mProp.Value = "Value 2";
            Assert.AreEqual("Value 2", mProp.Value);
        }

        [Test]
        public void TestPropDefColIsValid()
        {
            mProp = mBOPropCol["Prop2"];
            try
            {
                mProp.Value = "Prop Value fdfdfdf ff";
            }
            catch (InvalidPropertyValueException)
            {
            }
            mProp = mBOPropCol["PropName"];
            string reason;
            Assert.IsFalse(mBOPropCol.IsValid(out reason));
            Assert.IsTrue(reason.Length > 0);
        }

        [Test]
        public void TestAddBOPropColToBOPropCol()
        {
            Assert.AreEqual(3, mBOPropCol.Count,
                            "There should be 3 items in the BOPropCol after adding the other BOPropCol to it.");
        }

        [Test]
        public void TestDirtyXml()
        {
            mProp = mBOPropCol["Prop2"];
            mProp.InitialiseProp("Prop2-Orig");
            mProp.Value = "Prop2-New";
            Assert.IsTrue(mProp.IsDirty);

            mProp = mBOPropCol["PropName"];
            mProp.InitialiseProp("Propn-Orig");
            mProp.Value = "PropName-new";
            Assert.IsTrue(mProp.IsDirty);

            mPropDef = new PropDef("Prop3", typeof(string), PropReadWriteRule.ReadOnly, null);
            mPropDef.AddPropRule( new PropRuleString(mPropDef.PropertyName, "Test", 1, 40, null));
            mBOPropCol.Add(mPropDef.CreateBOProp(false));
            mProp = mBOPropCol["Prop3"];
            mProp.InitialiseProp("Prop3-new");
            Assert.IsFalse(mProp.IsDirty);
            const string dirtyXml = "<Properties><Prop2><PreviousValue>Prop2-Orig</PreviousValue><NewValue>Prop2-New</NewValue></Prop2><PropName><PreviousValue>Propn-Orig</PreviousValue><NewValue>PropName-new</NewValue></PropName></Properties>";
            Assert.AreEqual(dirtyXml, mBOPropCol.DirtyXml);
        }

        [Test]
        public void TestRemove()
        {
            PropDef propDef = new PropDef("Prop3", typeof(string), PropReadWriteRule.ReadOnly, null);
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef.CreateBOProp(false));
            Assert.AreEqual(1, propCol.Count);
            Assert.IsTrue(propCol.Contains("Prop3"), "BOPropCol should contain Prop3 after adding it.");
            propCol.Remove("Prop3");
            Assert.AreEqual(0, propCol.Count, "Remove should remove a BOProp from a BOPropCol");
        }

        [Test]
        public void Test_IsValid_ErrorDescriptions()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            MyBO myBO = new MyBO();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IList<IBOError> errors;
            bool isValid = myBO.Props.IsValid(out errors);
            //--------------- Test Result -----------------------
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, errors.Count);
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", errors[0].Message);
            Assert.AreEqual(ErrorLevel.Error, errors[0].Level);
            Assert.AreSame(null, errors[0].BusinessObject);
        }
//
//        [Test]
//        public void Test_SetBO_ShouldSetBOForAllBOProps()
//        {
            //---------------Set up test pack-------------------
//            IBusinessObject bo = MockRepository.GenerateMock<IBusinessObject>();
//            PropDef propDef = new PropDef("Prop3", typeof(string), PropReadWriteRule.ReadOnly, null);
//            BOPropCol propCol = new BOPropCol();
//            IBOProp boProp = propDef.CreateBOProp(false);
//            propCol.Add(boProp);
            //---------------Assert Precondition----------------
//            Assert.AreEqual(1, propCol.Count);
//            Assert.AreSame(boProp, propCol[boProp.PropertyName]);
            //---------------Execute Test ----------------------
//            propCol.BusinessObject = bo;
            //---------------Test Result -----------------------
//            Assert.AreSame(bo, propCol.BusinessObject);
//            Assert.AreSame(bo, boProp.BusinessObject);
//        }
//        [Test]
//        public void Test_SetBO_When2BOs_ShouldSetBOForAllBOProps()
//        {
            //---------------Set up test pack-------------------
//            IBusinessObject bo = MockRepository.GenerateMock<IBusinessObject>();
//            PropDef propDef = new PropDef("Prop3", typeof(string), PropReadWriteRule.ReadOnly, null);
//            PropDef propDef2 = new PropDef("Prop4", typeof(string), PropReadWriteRule.ReadOnly, null);
//            BOPropCol propCol = new BOPropCol();
//            IBOProp boProp = propDef.CreateBOProp(false);
//            propCol.Add(boProp);
//            IBOProp boProp2 = propDef2.CreateBOProp(false);
//            propCol.Add(boProp2);
            //---------------Assert Precondition----------------
//            Assert.AreEqual(2, propCol.Count);
//            Assert.AreSame(boProp, propCol[boProp.PropertyName]);
//            Assert.AreSame(boProp2, propCol[boProp2.PropertyName]);
            //---------------Execute Test ----------------------
//            propCol.BusinessObject = bo;
            //---------------Test Result -----------------------
//            Assert.AreSame(bo, propCol.BusinessObject);
//            Assert.AreSame(bo, boProp.BusinessObject);
//            Assert.AreSame(bo, boProp2.BusinessObject);
//        }

    }

}
