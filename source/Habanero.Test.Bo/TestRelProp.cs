//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
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