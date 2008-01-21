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

        [Test, ExpectedException(typeof(HabaneroArgumentException))]
        public void TestAddNullException()
        {
            RelKeyDef col = new RelKeyDef();
            col.Add(null);
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

        [Test, ExpectedException(typeof(InvalidPropertyNameException))]
        public void TestThisIndexerException()
        {
            RelKeyDef relKeyDef = new RelKeyDef();
            RelPropDef relPropDef = relKeyDef["prop"];
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
