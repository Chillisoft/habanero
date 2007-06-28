using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Bo.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.Bo.ClassDefinition
{



    [TestFixture]
    public class TestKeyDef
    {
        private KeyDef mKeyDef;

        [SetUp]
        public void init()
        {
            mKeyDef = new KeyDef();
            mKeyDef.IgnoreIfNull = true;
            PropDef lPropDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            mKeyDef.Add(lPropDef);
        }

        [Test]
        public void TestAddPropDef()
        {
            Assert.AreEqual(1, mKeyDef.Count);
        }

        [Test]
        public void TestContainsPropDef()
        {
            Assert.IsTrue(mKeyDef.Contains("PropName"));
            PropDef lPropDef = mKeyDef["PropName"];
            Assert.AreEqual("PropName", lPropDef.PropertyName);
            Assert.IsTrue(mKeyDef.IgnoreIfNull);
        }
    }

}
