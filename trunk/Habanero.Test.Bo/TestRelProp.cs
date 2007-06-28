using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.Bo
{

    [TestFixture]
    public class RelPropTester
    {
        private RelPropDef mRelPropDef;
        private PropDefCol mPropDefCol;

        [SetUp]
        public void init()
        {
            PropDef propDef = new PropDef("Prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            mRelPropDef = new RelPropDef(propDef, "PropName");
            mPropDefCol = new PropDefCol();
            mPropDefCol.Add(propDef);
        }

        [Test]
        public void TestCreateRelProp()
        {
            BOPropCol propCol = mPropDefCol.CreateBOPropertyCol(true);
            RelProp relProp = mRelPropDef.CreateRelProp(propCol);

            Assert.AreEqual("Prop", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName", relProp.RelatedClassPropName);

            Assert.IsTrue(relProp.IsNull);
        }

        [Test]
        public void TestCreateRelPropNotNull()
        {
            PropDef propDef = new PropDef("Prop1", typeof(string), PropReadWriteRule.ReadWrite, "1");
            RelPropDef relPropDef = new RelPropDef(propDef, "PropName1");
            PropDefCol propDefCol = new PropDefCol();

            propDefCol.Add(propDef);
            BOPropCol propCol = propDefCol.CreateBOPropertyCol(true);
            RelProp relProp = relPropDef.CreateRelProp(propCol);

            Assert.AreEqual(relPropDef.OwnerPropertyName, relProp.OwnerPropertyName);
            Assert.AreEqual(relPropDef.RelatedClassPropName, relProp.RelatedClassPropName);

            Assert.IsFalse(relProp.IsNull);
        }
    }
}
