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
using Habanero.BO.ClassDefinition;
using Habanero.BO.Rules;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.PropRule
{
    [TestFixture]
    public class TestInterPropRule
    {

        [Test]
        public void Test_CreateInterPropRule()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.GreaterThan, prop2);

            //---------------Test Result -----------------------
            Assert.IsNotNull(rule);
            Assert.AreEqual(ErrorLevel.Error, rule.ErrorLevel);
            Assert.AreEqual(prop1.PropertyName + " Is GreaterThan " + prop2.PropertyName, rule.Name);
        }
        [Test]
        public void Test_CreateInterPropRule_WhenEqualsOperator_NameShouldUseEquals()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.EqualTo, prop2);

            //---------------Test Result -----------------------
            Assert.IsNotNull(rule);
            Assert.AreEqual(prop1.PropertyName + " Is EqualTo " + prop2.PropertyName, rule.Name);
        }
        
        [Test]
        public void Test_Construct_WithNullCompareToProp_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new InterPropRule(new PropDefFake(), ComparisonOperator.LessThanOrEqual, null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("propRight", ex.ParamName);
            }
        }

        [Test]
        public void Test_Construct_ShouldSetPropsAndOperator()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            const ComparisonOperator comparisonOperator = ComparisonOperator.GreaterThan;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            InterPropRule rule = new InterPropRule(prop1, comparisonOperator, prop2);

            //---------------Test Result -----------------------
            Assert.AreSame(prop1, rule.LeftProp);
            Assert.AreSame(prop2, rule.RightProp);
            Assert.AreEqual(comparisonOperator, rule.ComparisonOp);
        }
        [Test]
        public void Test_Construct_WithNullCompareFromProp_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new InterPropRule(null, ComparisonOperator.LessThanOrEqual, new PropDefFake());
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("propLeft", ex.ParamName);
            }
        }

        [Test]
        public void Test_IsValid_WhenPasses_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.GreaterThan, prop2);
            IBusinessObject bo = GetBOWithPropValueSet(prop1, DateTime.Now, prop2, DateTime.Now.AddDays(-1));
            //---------------Assert Precondition----------------
            Assert.Greater((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Test_IsValid_WhenFails_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.GreaterThan, prop2);
            DateTime prop1Value;
            DateTime prop2Value;
            IBusinessObject bo = GetBOWithProp1LTProp2(prop1, prop2, out prop1Value, out prop2Value);
            //---------------Assert Precondition----------------
            Assert.LessOrEqual((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsFalse(isValid);
            string  expectedMessage = string.Format("Property '{0}' with value '{1}'  should "
                                                    + "be GreaterThan property '{2}' with value '{3}'"
                                                    , prop1.PropertyName, prop1Value, prop2.PropertyName, prop2Value);

            Assert.AreEqual(expectedMessage, rule.Message);
        }


        [Test]
        public void Test_IsValid_WhenLTPasses_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.LessThan, prop2);
            IBusinessObject bo = GetBOWithPropValueSet(prop1, DateTime.Now.AddDays(-1), prop2, DateTime.Now);
            //---------------Assert Precondition----------------
            Assert.Less((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Test_IsValid_WhenLtEq_WhenEqual_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.LessThanOrEqual, prop2);
            DateTime propValue = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, propValue, prop2, propValue);
            //---------------Assert Precondition----------------
            Assert.AreEqual((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid);
        }
        [Test]
        public void Test_IsValid_WhenEq_WhenEqual_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.EqualTo, prop2);
            DateTime propValue = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, propValue, prop2, propValue);
            //---------------Assert Precondition----------------
            Assert.AreEqual((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid);
        }
        [Test]
        public void Test_IsValid_WhenEq_WhenNotEqual_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.EqualTo, prop2);
            DateTime prop1Value = DateTime.Now.AddDays(1);
            DateTime prop2Value = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, prop1Value, prop2, prop2Value);
            //---------------Assert Precondition----------------
            Assert.AreNotEqual((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsFalse(isValid);
            string expectedMessage = string.Format("Property '{0}' with value '{1}'  should "
                                                   + "be EqualTo property '{2}' with value '{3}'"
                                                   , prop1.PropertyName, prop1Value, prop2.PropertyName, prop2Value);

            Assert.AreEqual(expectedMessage, rule.Message);
        }
        [Test]
        public void Test_IsValid_WhenLTFails_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.LessThan, prop2);
            DateTime prop1Value = DateTime.Now.AddDays(1);
            DateTime prop2Value = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, prop1Value, prop2, prop2Value);
            //---------------Assert Precondition----------------
            Assert.Greater((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsFalse(isValid);
            string expectedMessage = string.Format("Property '{0}' with value '{1}'  should "
                                                   + "be LessThan property '{2}' with value '{3}'"
                                                   , prop1.PropertyName, prop1Value, prop2.PropertyName, prop2Value);

            Assert.AreEqual(expectedMessage, rule.Message);
        }

        [Test]
        public void Test_IsValid_WhenGtEq_WhenEqual_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.GreaterThanOrEqual, prop2);
            DateTime propValue = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, propValue, prop2, propValue);
            //---------------Assert Precondition----------------
            Assert.AreEqual((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Test_IsValid_WhenGtEq_WhenGT_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.GreaterThanOrEqual, prop2);
            DateTime propValue = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, propValue.AddDays(1), prop2, propValue);
            //---------------Assert Precondition----------------
            Assert.Greater((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid);
        }
        [Test]
        public void Test_IsValid_WhenGtEq_WhenNotGtEqual_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.GreaterThanOrEqual, prop2);
            DateTime prop1Value = DateTime.Now.AddDays(-1);
            DateTime prop2Value = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, prop1Value, prop2, prop2Value);
            //---------------Assert Precondition----------------
            Assert.Less((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsFalse(isValid);
            string expectedMessage = string.Format("Property '{0}' with value '{1}'  should "
                                                   + "be GreaterThanOrEqual property '{2}' with value '{3}'"
                                                   , prop1.PropertyName, prop1Value, prop2.PropertyName, prop2Value);

            Assert.AreEqual(expectedMessage, rule.Message);
        }
        [Test]
        public void Test_IsValid_WhenGt_WhenGT_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.GreaterThan, prop2);
            DateTime propValue = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, propValue.AddDays(1), prop2, propValue);
            //---------------Assert Precondition----------------
            Assert.Greater((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid);
        }
        [Test]
        public void Test_IsValid_WhenGt_WhenEq_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.GreaterThan, prop2);
            DateTime propValue = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, propValue, prop2, propValue);
            //---------------Assert Precondition----------------
            Assert.AreEqual((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsFalse(isValid);
        }
        [Test]
        public void Test_IsValid_WhenGt_WhenNotGtEqual_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            InterPropRule rule = new InterPropRule(prop1, ComparisonOperator.GreaterThan, prop2);
            DateTime prop1Value = DateTime.Now.AddDays(-1);
            DateTime prop2Value = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, prop1Value, prop2, prop2Value);
            //---------------Assert Precondition----------------
            Assert.Less((DateTime)bo.GetPropertyValue(prop1.PropertyName), (DateTime)bo.GetPropertyValue(prop2.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsFalse(isValid);
            string expectedMessage = string.Format("Property '{0}' with value '{1}'  should "
                                                   + "be GreaterThan property '{2}' with value '{3}'"
                                                   , prop1.PropertyName, prop1Value, prop2.PropertyName, prop2Value);

            Assert.AreEqual(expectedMessage, rule.Message);
        }

        [Test]
        public void Test_IsValid_WhenRightPropNull_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            PropDefFake propLeft = new PropDefFake();
            PropDefFake propRight = new PropDefFake();
            InterPropRule rule = new InterPropRule(propLeft, ComparisonOperator.GreaterThan, propRight);
            DateTime prop1Value = DateTime.Now.AddDays(-1);
            DateTime? prop2Value = null;
            IBusinessObject bo = GetBOWithPropValueSet(propLeft, prop1Value, propRight, prop2Value);
            //---------------Assert Precondition----------------
            Assert.IsNull(bo.GetPropertyValue(propRight.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid);
        }
        [Test]
        public void Test_IsValid_WhenLeftPropNull_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            PropDefFake propLeft = new PropDefFake();
            PropDefFake propRight = new PropDefFake();
            InterPropRule rule = new InterPropRule(propLeft, ComparisonOperator.GreaterThan, propRight);
            DateTime? prop1Value = null;
            DateTime? prop2Value = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(propLeft, prop1Value, propRight, prop2Value);
            //---------------Assert Precondition----------------
            Assert.IsNull(bo.GetPropertyValue(propLeft.PropertyName));
            //---------------Execute Test ----------------------
            bool isValid = rule.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid);
        }
        private static IBusinessObject GetBOWithProp1LTProp2(IPropDef prop1, IPropDef prop2, out DateTime prop1Value, out DateTime prop2Value)
        {
            prop1Value = DateTime.Now.AddDays(-1);
            prop2Value = DateTime.Now;
            IBusinessObject bo = GetBOWithPropValueSet(prop1, prop1Value, prop2, prop2Value);
            return bo;
        }

        private static IBusinessObject GetBOWithPropValueSet(IPropDef prop1, DateTime? prop1Value, IPropDef prop2, DateTime? prop2Value)
        {
            IBusinessObject bo = MockRepository.GenerateMock<IBusinessObject>();
            bo.Stub(busObj => busObj.GetPropertyValue(prop1.PropertyName)).Return(prop1Value);
            bo.Stub(busObj => busObj.GetPropertyValue(prop2.PropertyName)).Return(prop2Value);
            return bo;
        }
        /// <summary>
        /// Fake so that can use simple constructor.
        /// </summary>
        private class PropDefFake : PropDef
        {
            public PropDefFake()
                : base(TestUtil.GetRandomString(), typeof(int), PropReadWriteRule.ReadWrite, null)
            {
            }
        }  
    }
}