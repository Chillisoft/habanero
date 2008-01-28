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

using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBoKey
    {
        private BOPropCol _boPropCol1;
        private KeyDef _keyDef1;

        private BOPropCol _boPropCol2;
        private KeyDef _keyDef2;

        [SetUp]
        public void Init()
        {
        	_boPropCol1 = new BOPropCol();
        	_keyDef1 = new KeyDef();
        	_boPropCol2 = new BOPropCol();
        	_keyDef2 = new KeyDef();

            //Props for KeyDef 1
            PropDef lPropDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            _boPropCol1.Add(lPropDef.CreateBOProp(false));
            _keyDef1.Add(lPropDef);

            lPropDef = new PropDef("PropName1", typeof(string), PropReadWriteRule.ReadOnly, null);
            _boPropCol1.Add(lPropDef.CreateBOProp(false));
            _keyDef1.Add(lPropDef);

            //Props for KeyDef 2
            lPropDef = new PropDef("PropName1", typeof(string), PropReadWriteRule.ReadOnly, null);
            _boPropCol2.Add(lPropDef.CreateBOProp(false));
            _keyDef2.Add(lPropDef);

            lPropDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            _boPropCol2.Add(lPropDef.CreateBOProp(false));
            _keyDef2.Add(lPropDef);
        }

        [Test]
        public void TestBOKeyEqual()
        {
            //Set values for Key1
            BOKey lBOKey1 = _keyDef1.CreateBOKey(_boPropCol1);
            BOProp lProp = _boPropCol1["PropName"];
            lProp.Value = "Prop Value";

            lProp = _boPropCol1["PropName1"];
            lProp.Value = "Value 2";

            //Set values for Key2
            BOKey lBOKey2 = _keyDef2.CreateBOKey(_boPropCol2);
            lProp = _boPropCol2["PropName"];
            lProp.Value = "Prop Value";

            lProp = _boPropCol2["PropName1"];
            lProp.Value = "Value 2";

            //Assert.AreEqual(lBOKey1, lBOKey2);
            Assert.IsTrue(lBOKey1 == lBOKey2);

            Assert.AreEqual(lBOKey1.GetHashCode(), lBOKey2.GetHashCode());
        }

        [Test]
        public void TestSortedValues()
        {
            BOKey lBOKey1 = _keyDef1.CreateBOKey(_boPropCol2);
            BOProp lProp = _boPropCol2["PropName"];
            Assert.AreSame(lProp, lBOKey1.SortedValues[0]);
        }

        [Test, ExpectedException(typeof(InvalidPropertyNameException))]
        public void TestIndexerPropertyNotFound()
        {
            BOKey boKey = _keyDef1.CreateBOKey(_boPropCol1);
            BOProp prop = boKey["invalidpropname"];
        }

        [Test, ExpectedException(typeof(HabaneroArgumentException))]
        public void TestAddNullBOProp()
        {
            BOKey boKey = _keyDef1.CreateBOKey(_boPropCol1);
            boKey.Add(null);
        }

        [Test, ExpectedException(typeof(InvalidPropertyException))]
        public void TestAddDuplicateBOProp()
        {
            BOKey boKey = _keyDef1.CreateBOKey(_boPropCol1);
            boKey.Add(boKey["PropName"]);
        }

        [Test]
        public void TestEquality()
        {
            BOKey boKey = _keyDef1.CreateBOKey(_boPropCol1);

            // Test when property count is different
            KeyDef keyDef = new KeyDef();
            BOKey otherKey = new BOKey(keyDef);
            Assert.IsFalse(boKey == otherKey);

            // Same property count, but different prop names
            PropDef propDef1 = new PropDef("PropName5", typeof(string), PropReadWriteRule.ReadOnly, null);
            PropDef propDef2 = new PropDef("PropName6", typeof(string), PropReadWriteRule.ReadOnly, null);
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef1.CreateBOProp(false));
            propCol.Add(propDef2.CreateBOProp(false));
            keyDef.Add(propDef1);
            keyDef.Add(propDef2);
            otherKey = keyDef.CreateBOKey(propCol);
            Assert.IsFalse(boKey == otherKey);

            // Same props but different values (with one null)
            otherKey = _keyDef1.CreateBOKey(_boPropCol2);
            otherKey["PropName"].Value = "blah";
            Assert.IsFalse(boKey == otherKey);

            // Same props but different values (neither are null)
            otherKey = _keyDef1.CreateBOKey(_boPropCol2);
            boKey["PropName"].Value = "diblah";
            Assert.IsFalse(boKey == otherKey);
            Assert.IsFalse(boKey.Equals(otherKey));

            // False when different type of object
            Assert.IsFalse(boKey.Equals(keyDef));

            // Finally, when they are equal
            boKey["PropName"].Value = "blah";
            Assert.IsTrue(boKey == otherKey);
            Assert.IsTrue(boKey.Equals(otherKey));
        }
    }
}