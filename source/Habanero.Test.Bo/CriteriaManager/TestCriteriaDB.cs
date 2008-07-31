using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestCriteriaDB
    {
        
        
        [Test]
        public void TestConstructor()
        {
            //-------------Setup Test Pack ------------------
            const string surname = "Surname";
            string surnameValue = TestUtil.CreateRandomString();
            Criteria criteria = new Criteria(surname, Criteria.Op.Equals, surnameValue);

            //-------------Execute test ---------------------
            CriteriaDB criteriaDB = new CriteriaDB(criteria);
            //-------------Test Result ----------------------
            Assert.AreEqual(criteria, criteriaDB);
        }

        [Test]
        public void TestToString_UsingDelegates()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            CriteriaDB surnameCriteria = new CriteriaDB(new Criteria(surname, Criteria.Op.Equals, surnameValue));
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            CriteriaDB dateTimeCriteria = new CriteriaDB(new Criteria(datetimePropName, Criteria.Op.GreaterThan, dateTimeValue));

            CriteriaDB andCriteria = new CriteriaDB(new Criteria(surnameCriteria, Criteria.LogicalOp.And, dateTimeCriteria));

            //---------------Execute Test ----------------------
            int i = 0;
            string criteriaAsString = andCriteria.ToString(new SqlFormatter("",""), delegate { return "param" + i++; });

            //---------------Test Result -----------------------
            const string expectedString = "(Surname = param0) AND (DateTime > param1)";
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString_UsesFormatter()
        {
            //-------------Setup Test Pack ------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.Op.Equals, surnameValue);
            string surnameTable = "surname_table";
            criteria.Field.Source = new Source(surnameTable);
            CriteriaDB surnameCriteria = new CriteriaDB(criteria);

            //-------------Execute test ---------------------
            string tostring = surnameCriteria.ToString(new SqlFormatter("<<", ">>"), delegate(object value) { return Convert.ToString(value); });
            //-------------Test Result ----------------------
            
            Assert.AreEqual(string.Format("<<{0}>>.<<{1}>> = {2}", surnameTable, surname, surnameValue), tostring);

        }

        [Test]
        public void TestToString_NoSource()
        {
            //-------------Setup Test Pack ------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.Op.Equals, surnameValue);
            CriteriaDB surnameCriteria = new CriteriaDB(criteria);

            //-------------Execute test ---------------------
            string tostring = surnameCriteria.ToString(new SqlFormatter("<<", ">>"), delegate(object value) { return Convert.ToString(value); });
            //-------------Test Result ----------------------

            Assert.AreEqual(string.Format("<<{0}>> = {1}", surname, surnameValue), tostring);
        }

        [Test]
        public void TestToString_BlankSource()
        {
            //-------------Setup Test Pack ------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.Op.Equals, surnameValue);
            criteria.Field.Source = new Source("");
            CriteriaDB surnameCriteria = new CriteriaDB(criteria);

            //-------------Execute test ---------------------
            string tostring = surnameCriteria.ToString(new SqlFormatter("<<", ">>"), delegate(object value) { return Convert.ToString(value); });
            //-------------Test Result ----------------------

            Assert.AreEqual(string.Format("<<{0}>> = {1}", surname, surnameValue), tostring);
        }

       
    }
}
