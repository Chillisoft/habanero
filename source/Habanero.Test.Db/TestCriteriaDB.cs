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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestCriteriaDB
    {
        [TestFixtureSetUp]
        public void SetupDatabaseConnection()
        {
            DatabaseConnection.CurrentConnection = TestUtil.GetDatabaseConfig().GetDatabaseConnection();
        }
        [Test]
        public void TestConstructor()
        {
            //-------------Setup Test Pack ------------------
            const string surname = "Surname";
            string surnameValue = TestUtil.GetRandomString();
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Equals, surnameValue);

            //-------------Execute test ---------------------
            CriteriaDB criteriaDB = new CriteriaDB(criteria);
            //-------------Test Result ----------------------
            Assert.AreEqual(criteria, criteriaDB);
        }

        [Test]
        public void TestToString_BlankSource()
        {
            //-------------Setup Test Pack ------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Equals, surnameValue);
            criteria.Field.Source = new Source("");
            CriteriaDB surnameCriteria = new CriteriaDB(criteria);

            //-------------Execute test ---------------------
            string tostring = surnameCriteria.ToString(new SqlFormatter("<<", ">>", "", ""),
                                                       delegate(object value) { return Convert.ToString(value); });
            //-------------Test Result ----------------------

            Assert.AreEqual(string.Format("<<{0}>> = {1}", surname, surnameValue), tostring);
        }

        [Test]
        public void TestToString_IsNullCriteria()
        {
            //-------------Setup Test Pack ------------------
            const string surnameField = "Surname";
            Criteria criteria = new Criteria(surnameField, Criteria.ComparisonOp.Is, null);
            const string surnameTable = "surname_table";
            criteria.Field.Source = new Source(surnameTable);
            CriteriaDB surnameCriteria = new CriteriaDB(criteria);

            //-------------Execute test ---------------------
            string tostring = surnameCriteria.ToString(new SqlFormatter("<<", ">>", "",""),
                                                       delegate(object value) { return Convert.ToString(value); });
            //-------------Test Result ----------------------

            Assert.AreEqual(string.Format("<<{0}>>.<<{1}>> IS NULL", surnameTable, surnameField), tostring);
        }


        [Test]
        public void TestToString_EqualsNullCriteria()
        {
            //-------------Setup Test Pack ------------------
            const string surnameField = "Surname";
            Criteria criteria = new Criteria(surnameField, Criteria.ComparisonOp.Equals, null);
            const string surnameTable = "surname_table";
            criteria.Field.Source = new Source(surnameTable);
            CriteriaDB surnameCriteria = new CriteriaDB(criteria);

            //-------------Execute test ---------------------
            string tostring = surnameCriteria.ToString(new SqlFormatter("<<", ">>", "", ""),
                                                       Convert.ToString);
            //-------------Test Result ----------------------

            Assert.AreEqual(string.Format("<<{0}>>.<<{1}>> IS NULL", surnameTable, surnameField), tostring);
        }
        
        [Test]
        public void TestToString_NotEqualsNullCriteria()
        {
            //-------------Setup Test Pack ------------------
            const string surnameField = "Surname";
            Criteria criteria = new Criteria(surnameField, Criteria.ComparisonOp.NotEquals, null);
            const string surnameTable = "surname_table";
            criteria.Field.Source = new Source(surnameTable);
            CriteriaDB surnameCriteria = new CriteriaDB(criteria);

            //-------------Execute test ---------------------
            string tostring = surnameCriteria.ToString(new SqlFormatter("<<", ">>", "", ""),
                                                       Convert.ToString);
            //-------------Test Result ----------------------

            Assert.AreEqual(string.Format("<<{0}>>.<<{1}>> IS NOT NULL", surnameTable, surnameField), tostring);
        }

        [Test]
        public void TestToString_NoSource()
        {
            //-------------Setup Test Pack ------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Equals, surnameValue);
            CriteriaDB surnameCriteria = new CriteriaDB(criteria);

            //-------------Execute test ---------------------
            string tostring = surnameCriteria.ToString(new SqlFormatter("<<", ">>","", ""),
                                                       delegate(object value) { return Convert.ToString(value); });
            //-------------Test Result ----------------------

            Assert.AreEqual(string.Format("<<{0}>> = {1}", surname, surnameValue), tostring);
        }

        [Test]
        public void TestToString_UsesFormatter()
        {
            //-------------Setup Test Pack ------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            Criteria criteria = new Criteria(surname, Criteria.ComparisonOp.Equals, surnameValue);
            const string surnameTable = "surname_table";
            criteria.Field.Source = new Source(surnameTable);
            CriteriaDB surnameCriteria = new CriteriaDB(criteria);

            //-------------Execute test ---------------------
            string tostring = surnameCriteria.ToString(new SqlFormatter("<<", ">>","",""),
                                                       delegate(object value) { return Convert.ToString(value); });
            //-------------Test Result ----------------------

            Assert.AreEqual(string.Format("<<{0}>>.<<{1}>> = {2}", surnameTable, surname, surnameValue), tostring);
        }

        [Test]
        public void TestToString_UsingDelegates()
        {
            //---------------Set up test pack-------------------
            string surnameValue = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            CriteriaDB surnameCriteria =
                new CriteriaDB(new Criteria(surname, Criteria.ComparisonOp.Equals, surnameValue));
            DateTime dateTimeValue = DateTime.Now;
            const string datetimePropName = "DateTime";
            CriteriaDB dateTimeCriteria =
                new CriteriaDB(new Criteria(datetimePropName, Criteria.ComparisonOp.GreaterThan, dateTimeValue));

            CriteriaDB andCriteria =
                new CriteriaDB(new Criteria(surnameCriteria, Criteria.LogicalOp.And, dateTimeCriteria));

            //---------------Execute Test ----------------------
            int i = 0;
            string criteriaAsString = andCriteria.ToString(new SqlFormatter("", "","",""), delegate { return "param" + i++; });

            //---------------Test Result -----------------------
            const string expectedString = "(Surname = param0) AND (DateTime > param1)";
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestToString_In()
        {
            //---------------Set up test pack-------------------
            string surnameValue1 = Guid.NewGuid().ToString("N");
            string surnameValue2 = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            CriteriaDB surnameCriteria =
                new CriteriaDB(new Criteria(surname, Criteria.ComparisonOp.In, new object[] { surnameValue1, surnameValue2}));
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            string criteriaAsString = surnameCriteria.ToString(new SqlFormatter("","","",""), value => value.ToString());

            //---------------Test Result -----------------------
            string expectedString = string.Format("Surname IN ('{0}', '{1}')", surnameValue1, surnameValue2);
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestToString_NotIn()
        {
            //---------------Set up test pack-------------------
            string surnameValue1 = Guid.NewGuid().ToString("N");
            string surnameValue2 = Guid.NewGuid().ToString("N");
            const string surname = "Surname";
            CriteriaDB surnameCriteria =
                new CriteriaDB(new Criteria(surname, Criteria.ComparisonOp.NotIn, new object[] { surnameValue1, surnameValue2 }));
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            string criteriaAsString = surnameCriteria.ToString(new SqlFormatter("", "", "", ""), value => value.ToString());

            //---------------Test Result -----------------------
            string expectedString = string.Format("Surname NOT IN ('{0}', '{1}')", surnameValue1, surnameValue2);
            StringAssert.AreEqualIgnoringCase(expectedString, criteriaAsString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestToString_ShouldUseAliases()
        {
            //---------------Set up test pack-------------------
            const string sourceName = "mysource";
            var source1 = new Source(sourceName);
            Source field1Source = source1;
            QueryField field1 = new QueryField("testfield", "testfield", field1Source);
            Criteria criteria = new Criteria(field1, Criteria.ComparisonOp.Equals, "myvalue");
            IDictionary<Source, string> aliases = new Dictionary<Source, string>() {{ source1, "a1"}};
            CriteriaDB criteriaDb = new CriteriaDB(criteria);
            SqlFormatter sqlFormatter = new SqlFormatter("[", "]", "", "LIMIT");
            //---------------Execute Test ----------------------
            string whereClause = criteriaDb.ToString(sqlFormatter, value => "Param", aliases);
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("a1.[testfield] = Param", whereClause);
        }
    }
}