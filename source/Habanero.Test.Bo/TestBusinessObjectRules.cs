using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
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
            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsTrue(ruleStub.IsValid());
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
            string msg;
            IList<IBOError> msgList;
            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsFalse(ruleStub.IsValid());
            Assert.IsTrue(bo.Status.IsValid(out msg));
            Assert.IsTrue(bo.Status.IsValid(out msgList));
            //---------------Execute Test ----------------------
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
            Assert.IsTrue(bo.Status.HasWarnings(out msgList));
            Assert.AreEqual(1, msgList.Count);
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
            bo.AddBusinessRule(ruleStub);
            //---------------Test Result -----------------------
            Assert.IsTrue(bo.Status.IsValid());
            Assert.IsTrue(bo.Status.IsValid(out msg));
            Assert.IsTrue(bo.Status.IsValid(out msgList));
            Assert.AreEqual(0, msgList.Count);
            TestUtil.AssertStringEmpty(msg, "msg");
            Assert.IsTrue(bo.Status.HasWarnings(out msgList));
            Assert.AreEqual(1, msgList.Count);
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
        public string Name { get; set; }

        /// <summary>
        /// Returns the error message for if the rule fails.
        /// </summary>
        public string Message { get; set; }

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


        #endregion
    }
}