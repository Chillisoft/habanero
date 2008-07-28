using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestSqlFormatter
    {
        [Test]
        public void TestCreateSqlFormatter()
        {
            //-------------Setup Test Pack ------------------
            string leftFieldDelimiter = "LEFT_DELIMIT";
            string rightFieldDelimiter = "RIGHT_DELIMIT";
            //-------------Execute test ---------------------
            SqlFormatter sqlFormatter = new SqlFormatter(leftFieldDelimiter, rightFieldDelimiter);

            //-------------Test Result ----------------------
            Assert.AreEqual(leftFieldDelimiter, sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual(rightFieldDelimiter, sqlFormatter.RightFieldDelimiter);
        }

        [Test]
        public void TestDelimitField()
        {
            //-------------Setup Test Pack ------------------
            string leftDelimiter = "LEFT_DELIMIT";
            string rightDelimiter = "RIGHT_DELIMIT";
            SqlFormatter sqlFormatter = new SqlFormatter(leftDelimiter, rightDelimiter);
            string fieldName = "MY_FIELD";
            //-------------Execute test ---------------------
            string delimitedField = sqlFormatter.DelimitField(fieldName);
            //-------------Test Result ----------------------
            Assert.AreEqual(String.Format("{0}{1}{2}", leftDelimiter, fieldName, rightDelimiter), delimitedField);
        }

        [Test]
        public void TestDelimitTable()
        {
            //-------------Setup Test Pack ------------------
            string leftDelimiter = "LEFT_DELIMIT";
            string rightDelimiter = "RIGHT_DELIMIT";
            SqlFormatter sqlFormatter = new SqlFormatter(leftDelimiter, rightDelimiter);
            string tableName = "MY_TABLE";
            //-------------Execute test ---------------------
            string delimitedField = sqlFormatter.DelimitTable(tableName);
            //-------------Test Result ----------------------
            Assert.AreEqual(String.Format("{0}{1}{2}", leftDelimiter, tableName, rightDelimiter), delimitedField);
        }
    }
}
