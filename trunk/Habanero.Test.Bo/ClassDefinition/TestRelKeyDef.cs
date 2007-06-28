using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.Bo.ClassDefinition
{

    [TestFixture]
    public class TestRelKeyDef
    {
        private RelKeyDef mRelKeyDef;
        private PropDefCol mPropDefCol;

        [SetUp]
        public void init()
        {
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
        public void TestAddPropDef()
        {
            Assert.AreEqual(2, mRelKeyDef.Count);
        }

        [Test]
        public void TestContainsPropDef()
        {
            Assert.IsTrue(mRelKeyDef.Contains("Prop"));
            RelPropDef lPropDef = mRelKeyDef["Prop"];
            Assert.AreEqual("Prop", lPropDef.OwnerPropertyName);
        }

        [Test]
        public void TestCreateRelKey()
        {
            BOPropCol propCol = mPropDefCol.CreateBOPropertyCol(true);
            RelKey relKey = mRelKeyDef.CreateRelKey(propCol);
            Assert.IsTrue(relKey.Contains("Prop"));
            Assert.IsTrue(relKey.Contains("Prop2"));
            RelProp relProp = relKey["Prop"];
            Assert.AreEqual("Prop", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName", relProp.RelatedClassPropName);
            relProp = relKey["Prop2"];
            Assert.AreEqual("Prop2", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName2", relProp.RelatedClassPropName);

            Assert.IsTrue(relKey.HasRelatedObject(),
                          "This is true since the values for the properties should have defaulted to 1 each");

            Assert.AreEqual("(PropName = '1' AND PropName2 = '2')", relKey.RelationshipExpression().ExpressionString());
        }
    }
}
