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

namespace Habanero.Test.BO.ClassDefinition
{


    [TestFixture]
    public class TestRelPropDef
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
        public void TestCreateRelPropDef()
        {
            Assert.AreEqual("Prop", mRelPropDef.OwnerPropertyName);
            Assert.AreEqual("PropName", mRelPropDef.RelatedClassPropName);
        }

        [Test]
        public void TestCreateRelProp()
        {
            BOPropCol propCol = mPropDefCol.CreateBOPropertyCol(true);
            RelProp relProp = mRelPropDef.CreateRelProp(propCol);

            Assert.AreEqual("Prop", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName", relProp.RelatedClassPropName);
        }

        [Test, ExpectedException()]
        public void TestAddNullException()
        {
            RelPropDef relPropDef = new RelPropDef(null, "");
        }

        [Test]
        public void TestProtectedGetsAndSets()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(string), PropReadWriteRule.ReadWrite, null);
            RelPropDefInheritor relPropDef = new RelPropDefInheritor(propDef);

            Assert.AreEqual(propDef, relPropDef.GetSetOwnerProperty);
            relPropDef.GetSetOwnerProperty = propDef2;
            Assert.AreEqual(propDef2, relPropDef.GetSetOwnerProperty);

            Assert.AreEqual("relprop", relPropDef.RelatedClassPropName);
            relPropDef.SetRelatedClassPropName("newrelprop");
            Assert.AreEqual("newrelprop", relPropDef.RelatedClassPropName);
        }

        // Grants access to protected methods
        private class RelPropDefInheritor : RelPropDef
        {
            public RelPropDefInheritor(PropDef propDef) : base(propDef, "relprop")
            {}

            public PropDef GetSetOwnerProperty
            {
                get { return OwnerProperty; }
                set { OwnerProperty = value;}
            }

            public void SetRelatedClassPropName(string name)
            {
                RelatedClassPropName = name;
            }
        }
    }
}
