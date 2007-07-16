using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.Bo
{



    [TestFixture]
    public class TestBoPropCol
    {
        private PropDef mPropDef;
        private BOProp mProp;
        private BOPropCol mBOPropCol;

        [SetUp]
        public void init()
        {
            mBOPropCol = new BOPropCol();
            mPropDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            mBOPropCol.Add(mPropDef.CreateBOProp(false));

            mPropDef = new PropDef("Prop2", typeof(string), PropReadWriteRule.ReadOnly, null);
            mPropDef.PropRule = new PropRuleString(mPropDef.PropertyName, "Test Message", 1, 10, null, null);
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

        //		[Test]
        //		public void TestPropDefColIsValid()
        //		{
        //			mProp = _boPropCol["Prop2"];
        //			try
        //			{
        //				mProp.PropertyValue = "Prop Value fdfdfdf ff";
        //			}
        //			catch (Exception e)
        //			{}
        //			mProp = _boPropCol["PropName"];
        //			string reason;
        //			Assert.IsFalse(_boPropCol.IsValid(out reason));
        //			Assert.IsTrue(reason.Length > 0);
        //			
        //		}
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
            mPropDef.PropRule = new PropRuleString(mPropDef.PropertyName, "Test", 1, 40, null, null);
            mBOPropCol.Add(mPropDef.CreateBOProp(false));
            mProp = mBOPropCol["Prop3"];
            mProp.InitialiseProp("Prop3-new");
            Assert.IsFalse(mProp.IsDirty);
            string dirtyXml =
                "<Properties><Prop2><PreviousValue>Prop2-Orig</PreviousValue><NewValue>Prop2-New</NewValue></Prop2><PropName><PreviousValue>Propn-Orig</PreviousValue><NewValue>PropName-new</NewValue></PropName>";
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
    }

}
