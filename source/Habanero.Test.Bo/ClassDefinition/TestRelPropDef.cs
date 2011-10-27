// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{


    [TestFixture]
    public class TestRelPropDef
    {
        private RelPropDef _relPropDef;
        private PropDefCol _propDefCol;

        [SetUp]
        public void init()
        {
            PropDef propDef = new PropDef("Prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            _relPropDef = new RelPropDef(propDef, "PropName");
            _propDefCol = new PropDefCol();
            _propDefCol.Add(propDef);
        }

        [Test]
        public void TestCreateRelPropDef()
        {
            Assert.AreEqual("Prop", _relPropDef.OwnerPropertyName);
            Assert.AreEqual("PropName", _relPropDef.RelatedClassPropName);
        }

        [Test]
        public void TestCreateRelProp()
        {
            IBOPropCol propCol = _propDefCol.CreateBOPropertyCol(true);
            IRelProp relProp = _relPropDef.CreateRelProp(propCol);

            Assert.AreEqual("Prop", relProp.OwnerPropertyName);
            Assert.AreEqual("PropName", relProp.RelatedClassPropName);
        }

        [Test]
        public void TestCreateRelProp_PropDoesNotExist()
        {
            //-------------Setup Test Pack ------------------
            BOPropCol propCol = new BOPropCol();
            Exception exception = null;
            IRelProp relProp = null;
            //-------------Execute test ---------------------
            try
            {
                relProp = _relPropDef.CreateRelProp(propCol);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //-------------Test Result ----------------------
            Assert.IsNotNull(exception, "An error should have been thrown because the prop does not exist.");
            Assert.IsInstanceOf(typeof(InvalidPropertyNameException), exception);
            Assert.IsNull(relProp, "The relProp should not have been created.");
        }

        [Test]
        public void TestAddNullException()
        {
            //---------------Execute Test ----------------------
            try
            {
                RelPropDef relPropDef = new RelPropDef((string)null, "");
                Assert.Fail("Expected to throw an HabaneroArgumentException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains("The argument 'ownerClassPropDefName' is not valid. ownerClassPropDefName cannot be null", ex.Message);
            }
        }

        [Test]
        public void TestProtectedGetsAndSets()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(string), PropReadWriteRule.ReadWrite, null);
            RelPropDefFake relPropDef = new RelPropDefFake(propDef);

            Assert.AreEqual(propDef, relPropDef.GetSetOwnerProperty);
            relPropDef.GetSetOwnerProperty = propDef2;
            Assert.AreEqual(propDef2, relPropDef.GetSetOwnerProperty);

            Assert.AreEqual("relprop", relPropDef.RelatedClassPropName);
            relPropDef.SetRelatedClassPropName("newrelprop");
            Assert.AreEqual("newrelprop", relPropDef.RelatedClassPropName);
        }

        [Test]
        public void Test_OwnerPropDef_ShouldReturnIPropDef()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            var relPropDef = new RelPropDef(propDef, "PropName");
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            var ownerPropDef = relPropDef.OwnerPropDef;
            //---------------Test Result -----------------------
            Assert.IsNotNull(ownerPropDef);
            Assert.AreSame(propDef, ownerPropDef);
        }
        // Grants access to protected methods
        private class RelPropDefFake : RelPropDef
        {
            public RelPropDefFake(IPropDef propDef) : base(propDef, "relprop")
            {}

            public IPropDef GetSetOwnerProperty
            {
                get { return OwnerPropDef; }
                set { OwnerPropDef = value; }
            }

            public void SetRelatedClassPropName(string name)
            {
                RelatedClassPropName = name;
            }
        }
    }
}
