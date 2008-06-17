using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestCriteria
    {
        [Test]
        public void TestCriteria_IsMatch_OneProp_Equals_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, Guid.NewGuid().ToString("N"));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCriteria_IsMatch_OneProp_Equals()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, cp.Surname);
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCriteria_IsMatch_OneProp_GreaterThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.GreaterThan, DateTime.Now.AddDays(-1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCriteria_IsMatch_OneProp_GreaterThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.GreaterThan, DateTime.Now.AddDays(1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestCriteria_IsMatch_OneProp_LessThan()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.LessThan, DateTime.Now.AddDays(1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestCriteria_IsMatch_OneProp_LessThan_NoMatch()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.DateOfBirth = DateTime.Now;

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.LessThan, DateTime.Now.AddDays(-1));
            bool isMatch = criteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsFalse(isMatch, "The object should not be a match since it does not match the criteria given.");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestCriteria_IsMatch_TwoProps_And()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            DateTime dob = DateTime.Now;
            cp.DateOfBirth = dob;
            string surname = Guid.NewGuid().ToString("N");
            cp.Surname = surname;

            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.Op.Equals, dob);
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.Equals, surname);

            //---------------Execute Test ----------------------
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.And, nameCriteria);
            bool isMatch = twoPropCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestIsComposite_False()
        {
            //---------------Set up test pack-------------------
            Criteria criteria = new Criteria("bob", Criteria.Op.LessThan, "hello");
            //---------------Execute Test ----------------------
            bool isComposite = criteria.IsComposite();
            //---------------Test Result -----------------------
            Assert.IsFalse(isComposite, "A single criteria should not be composite");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestIsComposite_True()
        {
            //---------------Set up test pack-------------------
            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.Op.Equals, "sfd");
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.Equals, "dfsd");
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.And, nameCriteria);
            //---------------Execute Test ----------------------
            bool isComposite = twoPropCriteria.IsComposite();
            //---------------Test Result -----------------------
            Assert.IsTrue(isComposite, "A criteria made up of two others should be composite");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCriteria_IsMatch_TwoProps_Or()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            DateTime dob = DateTime.Now;
            cp.DateOfBirth = dob;
            string surname = Guid.NewGuid().ToString("N");
            cp.Surname = surname;

            Criteria dobCriteria = new Criteria("DateOfBirth", Criteria.Op.Equals, dob.AddDays(2));
            Criteria nameCriteria = new Criteria("Surname", Criteria.Op.Equals, surname);

            //---------------Execute Test ----------------------
            Criteria twoPropCriteria = new Criteria(dobCriteria, Criteria.LogicalOp.Or, nameCriteria);
            bool isMatch = twoPropCriteria.IsMatch(cp);
            //---------------Test Result -----------------------
            Assert.IsTrue(isMatch, "The object should be a match since it matches the criteria given.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString_LeafCriteria_String_Equals()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.Op.Equals, surnameValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("Surname = '" + surnameValue + "'", criteriaAsString);
        }

        [Test]
        public void TestToString_LeafCriteria_DateTime_GreaterThan()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria criteria = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);

            //---------------Execute Test ----------------------
            string criteriaAsString = criteria.ToString();

            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("DateTime > '" + dateTimeValue.ToString(Criteria.DATE_FORMAT) + "'", criteriaAsString);
        }

        [Test]
        public void TestToString_And()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            string surname = "Surname";
            Criteria surnameCriteria = new Criteria(surname, Criteria.Op.Equals, surnameValue);
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria dateTimeCriteria = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);

            Criteria andCriteria = new Criteria(surnameCriteria, Criteria.LogicalOp.And, dateTimeCriteria);
            //---------------Execute Test ----------------------
            string criteriaAsString = andCriteria.ToString();
            //---------------Test Result -----------------------
            string expectedString = string.Format("(Surname = '{0}') AND (DateTime > '{1}')", surnameValue, dateTimeValue.ToString(Criteria.DATE_FORMAT));
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_Null()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);
           
            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(null);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_WrongType()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);

            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(6);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_Leaf_True()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);
            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(criteria2);
            //---------------Test Result -----------------------
            Assert.IsTrue(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_Leaf_False()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue = DateTime.Now;
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue.AddDays(34));
            //---------------Execute Test ----------------------
            bool areEquals = criteria1.Equals(criteria2);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_CompositeAndLeaf()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue1 = DateTime.Now;
            DateTime dateTimeValue2 = DateTime.Now.AddDays(20);
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue1);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue2);
            Criteria composite1 = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);

            Criteria criteria3 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue1);
            //---------------Execute Test ----------------------
            bool areEquals = composite1.Equals(criteria3);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEquals);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestEquals_Composite_True()
        {
            //---------------Set up test pack-------------------
            DateTime dateTimeValue1 = DateTime.Now;
            DateTime dateTimeValue2 = DateTime.Now.AddDays(20);
            string datetimePropName = "DateTime";
            Criteria criteria1 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue1);
            Criteria criteria2 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue2);
            Criteria composite1 = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);

            Criteria criteria3 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue1);
            Criteria criteria4 = new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue2);
            Criteria composite2 = new Criteria(criteria3, Criteria.LogicalOp.And, criteria4);
            //---------------Execute Test ----------------------
            bool areEquals = composite1.Equals(composite2);
            //---------------Test Result -----------------------
            Assert.IsTrue(areEquals);
            //---------------Tear Down -------------------------
        }


    }
}
