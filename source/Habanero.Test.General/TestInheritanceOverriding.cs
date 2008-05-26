using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public class TestInheritanceOverriding
    {

        [Test]
        public void TestLoadFromClasDef()
        {
            //-------------Setup Test Pack ------------------

            //-------------Test Pre-conditions --------------

            //-------------Execute test ---------------------

            //-------------Test Result ----------------------
        }

        //[Test]
        //public void TestCircleOverridingProperty()
        //{
        //    //-------------Setup Test Pack ------------------
        //    PropDef propDefOverride = new PropDef("ShapeName", typeof(string), PropReadWriteRule.WriteNew, "OverriddenDefault");
        //    //-------------Test Pre-conditions --------------
        //    Assert.IsFalse(_classDefCircleNoPrimaryKey.PropDefcol.Contains("ShapeName"));
        //    //-------------Execute test ---------------------
        //    _classDefCircleNoPrimaryKey.PropDefcol.Add(propDefOverride);
        //    //-------------Test Result ----------------------
        //    Assert.IsTrue(_classDefCircleNoPrimaryKey.PropDefcol.Contains("ShapeName"));
        //}
    }
}
