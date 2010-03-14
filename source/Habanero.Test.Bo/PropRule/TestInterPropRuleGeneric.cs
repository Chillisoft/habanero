using System;
using AutoMappingHabanero;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Rules;
using NUnit.Framework;

namespace Habanero.Test.BO.PropRule
{
    [TestFixture]
    public class TestInterPropRuleGeneric
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var classDef = typeof (FakeBO).MapClass();
            ClassDef.ClassDefs.Add(classDef);
        }
        [Test]
        public void Test_CreateInterPropRule()
        {
            //---------------Set up test pack-------------------
            PropDefFake prop1 = new PropDefFake();
            PropDefFake prop2 = new PropDefFake();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            InterPropRule<MyBO> rule = new InterPropRule<MyBO>(prop1, ComparisonOperator.GreaterThan, prop2);

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
            InterPropRule<MyBO> rule = new InterPropRule<MyBO>(prop1, ComparisonOperator.EqualTo, prop2);

            //---------------Test Result -----------------------
            Assert.IsNotNull(rule);
            Assert.AreEqual(prop1.PropertyName + " Is EqualTo " + prop2.PropertyName, rule.Name);
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
            InterPropRule<MyBO> rule = new InterPropRule<MyBO>(prop1, comparisonOperator, prop2);

            //---------------Test Result -----------------------
            Assert.AreSame(prop1, rule.LeftProp);
            Assert.AreSame(prop2, rule.RightProp);
            Assert.AreEqual(comparisonOperator, rule.ComparisonOp);
        }

        [Test]
        public void Test_Construct_WithNullCompareToProp_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new InterPropRule<MyBO>(new PropDefFake(), ComparisonOperator.LessThanOrEqual, null);
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
        public void Test_Construct_WithNullCompareFromProp_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new InterPropRule<MyBO>(null, ComparisonOperator.LessThanOrEqual, new PropDefFake());
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
        public void Test_Construct_WithNullRightExpression_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new InterPropRule<FakeBO>(bo => bo.EconomicLife, ComparisonOperator.LessThanOrEqual, null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("propExpressionRight", ex.ParamName);
            }
        }

        [Test]
        public void Test_Construct_WithNullLeftExpression_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new InterPropRule<FakeBO>(null, ComparisonOperator.LessThanOrEqual, bo => bo.EconomicLife);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("propExpressionLeft", ex.ParamName);
            }
        }
        [Test]
        public void Test_Create_WithExpressions_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            const string propNameLeft = "EconomicLife";
            const string propNameRight = "EngineeringLife";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            InterPropRule<FakeBO> rule = new InterPropRule<FakeBO>(bo => bo.EconomicLife, ComparisonOperator.EqualTo, bo => bo.EngineeringLife);
            //---------------Test Result -----------------------
            Assert.IsNotNull(rule);
            Assert.AreEqual(propNameLeft + " Is EqualTo " + propNameRight, rule.Name);
        }
        [Test]
        public void Test_Create_WithExpressions_ShouldConstructWithPropDefs()
        {
            //---------------Set up test pack-------------------
            const string propNameLeft = "EconomicLife";
            const string propNameRight = "EngineeringLife";
            IClassDef classDef = ClassDef.ClassDefs[typeof (FakeBO)];
            IPropDef propDefLeft = classDef.PropDefcol[propNameLeft];
            IPropDef propDefRight = classDef.PropDefcol[propNameRight];
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            InterPropRule<FakeBO> rule = new InterPropRule<FakeBO>(bo => bo.EconomicLife, ComparisonOperator.EqualTo, bo => bo.EngineeringLife);
            //---------------Test Result -----------------------
            Assert.AreSame(propDefLeft, rule.LeftProp);
            Assert.AreSame(propDefRight, rule.RightProp);
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
        private class FakeBO: BusinessObject
        {
            public virtual System.Int32? EconomicLife
            {
                get
                {
                    return ((System.Int32?)(base.GetPropertyValue("EconomicLife")));
                }
/*                set
                {
                    base.SetPropertyValue("EconomicLife", value);
                }*/
            }

            public virtual System.Int32? EngineeringLife
            {
                get
                {
                    return ((System.Int32?)(base.GetPropertyValue("EngineeringLife")));
                }
/*                set
                {
                    base.SetPropertyValue("EngineeringLife", value);
                }*/
            }
        }
    }

}