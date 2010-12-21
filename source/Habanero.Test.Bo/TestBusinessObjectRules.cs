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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
#pragma warning disable 612,618
    /// <summary>
    /// Summary description for TestBusinessObject.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectRules
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [Test]
        public void Test_BOIsValid_WhenBORuleIsValid_ShouldBeValid()
        {
            //---------------Set up test pack-------------------
            MyBO bo = new MyBO();
            BusinessObjectRuleStub ruleStub = new BusinessObjectRuleStub();
            string msg;
            IList<IBOError> msgList;

            //---------------Assert Precondition----------------
            Assert.IsTrue(ruleStub.IsValid());

            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsTrue(bo.Status.IsValid(out msg));
            Assert.IsTrue(bo.Status.IsValid(out msgList));
            //---------------Execute Test ----------------------
            bo.AddBusinessRule(ruleStub);
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsTrue(bo.Status.IsValid(out msg));
            Assert.IsTrue(bo.Status.IsValid(out msgList));
            Assert.AreEqual(0,msgList.Count);
            TestUtil.AssertStringEmpty(msg, "msg");
            Assert.IsFalse(bo.Status.HasWarnings(out msgList));
        }
        [Test]
        public void Test_BOIsValid_WhenBORuleIsNotValid_ShouldBeNotValid()
        {
            //---------------Set up test pack-------------------
            MyBO bo = new MyBO();
            BusinessObjectRuleStub ruleStub = new BusinessObjectRuleStub(false);
            ruleStub.Message = TestUtil.GetRandomString();
            string msg;
            IList<IBOError> msgList;
            //---------------Assert Precondition----------------
            Assert.IsFalse(ruleStub.IsValid());

            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsTrue(bo.Status.IsValid(out msg));
            Assert.IsTrue(bo.Status.IsValid(out msgList));
            //---------------Execute Test ----------------------
            bo = new MyBO();
            bo.AddBusinessRule(ruleStub);
            //---------------Test Result -----------------------
            Assert.IsFalse(bo.Status.IsValid());
            Assert.IsFalse(bo.Status.IsValid(out msg));
            Assert.IsFalse(bo.Status.IsValid(out msgList));
            Assert.AreEqual(1, msgList.Count);
            TestUtil.AssertStringNotEmpty(msg, "msg");
            IBOError error = msgList[0];
            Assert.AreEqual(error.Level, ErrorLevel.Error);
            Assert.AreSame(bo, error.BusinessObject);
            IList<IBOError> warningList;
            Assert.IsTrue(bo.Status.HasWarnings(out warningList));
            Assert.AreEqual(1, warningList.Count);
            Assert.AreSame(bo, warningList[0].BusinessObject);
        }
        [Test]
        public void Test_BOIsValid_WithBO_WhenRuleIsNotValid_ShouldBeNotValid()
        {
            //---------------Set up test pack-------------------
            MyBO bo = new MyBO();
            BusinessObjectRuleStub ruleStub = new BusinessObjectRuleStub(false);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool isValid = ruleStub.IsValid(bo);
            //---------------Test Result -----------------------
            Assert.IsFalse(isValid);

        }

        [Test]
        public void Test_BOIsValid_WithBO_WhenRuleWithBOIsValid_ShouldBeValid()
        {
            //---------------Set up test pack-------------------
            MyBO bo = new MyBO();
            var mockRule = MockRepository.GenerateMock<IBusinessObjectRule>();
            mockRule.Stub(rule => rule.IsValid(bo)).Return(true);
            mockRule.Stub(rule => rule.IsValid()).Return(false);

            bo.AddBusinessRule(mockRule);
            //---------------Assert Precondition----------------
            Assert.True(mockRule.IsValid(bo));
            Assert.IsFalse(mockRule.IsValid());
            //---------------Execute Test ----------------------
            IList<IBOError> message;
            bool isValid = bo.Status.IsValid(out message);
            //---------------Test Result -----------------------
            Assert.IsTrue(isValid, "Should be Valid");
        }

        [Test]
        public void Test_ErrorLevel_WhenRuleIsWarning_ShouldNotBeError()
        {
            //---------------Set up test pack-------------------
            MyBO bo = new MyBO();
            BusinessObjectRuleStub ruleStub = new BusinessObjectRuleStub(false, ErrorLevel.Warning);
            IList<IBOError> msgList;
            string msg;
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsFalse(ruleStub.IsValid());
            //---------------Execute Test ----------------------
            bo = new MyBO();
            bo.AddBusinessRule(ruleStub);
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsTrue(bo.Status.IsValid(out msg));
            Assert.IsTrue(bo.Status.IsValid(out msgList));
            Assert.AreEqual(0, msgList.Count);
            TestUtil.AssertStringEmpty(msg, "msg");
            IList<IBOError> warningList;
            Assert.IsTrue(bo.Status.HasWarnings(out warningList));
            Assert.AreEqual(1, warningList.Count);
            Assert.AreSame(bo, warningList[0].BusinessObject);
        }

        [Test]
        public void Test_BOIsValid_WhenRuleIsNull_ShouldNotRaiseError()
        {
            //---------------Set up test pack-------------------
            MyBO bo = new MyBO();
            BusinessObjectRuleStub ruleStub = new BusinessObjectRuleStub(false, ErrorLevel.Error);
            IList<IBOError> msgList;
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsFalse(ruleStub.IsValid());
            //---------------Execute Test ----------------------
            bo = new MyBO();
            bo.AddBusinessRule(null);
            bo.AddBusinessRule(ruleStub);
            //---------------Test Result -----------------------
            Assert.IsFalse(bo.Status.IsValid(out msgList));
            Assert.AreEqual(1, msgList.Count);
        }
        [Test]
        public void Test_BOIsValid_WhenRuleTwoFailingRules_ShouldReturnTwoErrors()
        {
            //---------------Set up test pack-------------------
            MyBO bo = new MyBO();
            BusinessObjectRuleStub ruleStub = new BusinessObjectRuleStub(false, ErrorLevel.Error);
            IList<IBOError> msgList;
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsFalse(ruleStub.IsValid());
            //---------------Execute Test ----------------------
            bo = new MyBO();
            bo.AddBusinessRule(new BusinessObjectRuleStub(false));
            bo.AddBusinessRule(new BusinessObjectRuleStub(true));
            bo.AddBusinessRule(ruleStub);
            //---------------Test Result -----------------------
            Assert.IsFalse(bo.Status.IsValid(out msgList));
            Assert.AreEqual(2, msgList.Count);
        }
    }

    internal class BusinessObjectRuleStub : IBusinessObjectRule
    {
        private readonly ErrorLevel _errorLevel = Habanero.Base.ErrorLevel.Error;
        private readonly bool _isValid;

        public BusinessObjectRuleStub(bool isValid)
        {
            _isValid = isValid;
        }

        public BusinessObjectRuleStub():this(true)
        {
        }

        public BusinessObjectRuleStub(bool isValid, ErrorLevel errorLevel):this(isValid)
        {
            _errorLevel = errorLevel;
        }

        #region Implementation of IBusinessObjectRule

        /// <summary>
        /// Returns the rule name
        /// </summary>
// ReSharper disable UnusedAutoPropertyAccessor.Global
        public string Name { get; set; }

        /// <summary>
        /// Returns the error message for if the rule fails.
        /// </summary>
        public string Message { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Global

        /// <summary>
        /// The <see cref="IBusinessObjectRule.ErrorLevel"/> for this BusinessObjectRule e.g. Warning, Error. 
        /// </summary>
        public ErrorLevel ErrorLevel
        {
            get { return _errorLevel; }
        }

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <returns>Returns true if valid</returns>
        public bool IsValid()
        {
            return _isValid;
        }

        public bool IsValid(IBusinessObject bo)
        {
            return _isValid;
        }

        #endregion
    }

#pragma warning restore 612,618
}