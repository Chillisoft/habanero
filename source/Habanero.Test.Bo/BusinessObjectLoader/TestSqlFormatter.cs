#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    [TestFixture]
    public class TestSqlFormatter
    {
        [Test]
        public void TestCreateSqlFormatter()
        {
            //-------------Setup Test Pack ------------------
            const string leftFieldDelimiter = "LEFT_DELIMIT";
            const string rightFieldDelimiter = "RIGHT_DELIMIT";
            //-------------Execute test ---------------------
            SqlFormatter sqlFormatter = new SqlFormatter(leftFieldDelimiter, rightFieldDelimiter, "","");

            //-------------Test Result ----------------------
            Assert.AreEqual(leftFieldDelimiter, sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual(rightFieldDelimiter, sqlFormatter.RightFieldDelimiter);
        }

        [Test]
        public void TestDelimitField()
        {
            //-------------Setup Test Pack ------------------
            const string leftDelimiter = "LEFT_DELIMIT";
            const string rightDelimiter = "RIGHT_DELIMIT";
            SqlFormatter sqlFormatter = new SqlFormatter(leftDelimiter, rightDelimiter,"","");
            const string fieldName = "MY_FIELD";
            //-------------Execute test ---------------------
            string delimitedField = sqlFormatter.DelimitField(fieldName);
            //-------------Test Result ----------------------
            Assert.AreEqual(String.Format("{0}{1}{2}", leftDelimiter, fieldName, rightDelimiter), delimitedField);
        }

        [Test]
        public void TestDelimitTable()
        {
            //-------------Setup Test Pack ------------------
            const string leftDelimiter = "LEFT_DELIMIT";
            const string rightDelimiter = "RIGHT_DELIMIT";
            SqlFormatter sqlFormatter = new SqlFormatter(leftDelimiter, rightDelimiter,"","");
            const string tableName = "MY_TABLE";
            //-------------Execute test ---------------------
            string delimitedField = sqlFormatter.DelimitTable(tableName);
            //-------------Test Result ----------------------
            Assert.AreEqual(String.Format("{0}{1}{2}", leftDelimiter, tableName, rightDelimiter), delimitedField);
        }

        [Test]
        public void Test_DelimitTable_WithSchemaPrefix_ShouldReturnWellFormattedDelimiters()
        {
            //-------------Setup Test Pack ------------------
            const string leftDelimiter = "[";
            const string rightDelimiter = "]";
            const string expected = "[MY_SCHEMA].[MY_TABLE]";
            SqlFormatter sqlFormatter = new SqlFormatter(leftDelimiter, rightDelimiter, "", "");
            const string tableName = "MY_SCHEMA.MY_TABLE";
            //-------------Execute test ---------------------
            string delimitedField = sqlFormatter.DelimitTable(tableName);
            
            //-------------Test Result ----------------------
            Assert.AreEqual(expected, delimitedField);
        }

        [Test]
        public void Test_GetLimitClauseForEnd()
        {
            //---------------Set up test pack-------------------
            const string limitClauseAtEnd = "LIMIT";
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", limitClauseAtEnd);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string limitClauseForEnd = sqlFormatter.GetLimitClauseCriteriaForEnd(10);
            //---------------Test Result -----------------------
            const string expectedLimitClauseForEnd = limitClauseAtEnd + " 10";
            Assert.AreEqual(expectedLimitClauseForEnd, limitClauseForEnd);
        }

        [Test]
        public void Test_CreateSqlFormatter()
        {
            //---------------Set up test pack-------------------
            const string limitClauseAtBeginning = "TOP";
            const string limitClauseAtEnd = "LIMIT";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter sqlFormatter = new SqlFormatter("`", "z", limitClauseAtBeginning, limitClauseAtEnd);
            //---------------Test Result -----------------------
            Assert.AreEqual("`", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("z", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual(limitClauseAtEnd, sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual(limitClauseAtBeginning, sqlFormatter.LimitClauseAtBeginning);
        }

        [Test]
        public void Test_GetLimitClauseForEnd_EndHasNoClauseSet()
        {
            //---------------Set up test pack-------------------
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", "");
            //---------------Assert Precondition----------------
            Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
            //---------------Execute Test ----------------------
            string limitClauseForEnd = sqlFormatter.GetLimitClauseCriteriaForEnd(10);
            //---------------Test Result -----------------------
            const string expectedLimitClauseForEnd = "";
            Assert.AreEqual(expectedLimitClauseForEnd, limitClauseForEnd);
        }

        [Test]
        public void Test_GetLimitClauseForBegin()
        {
            //---------------Set up test pack-------------------
            const string limitClauseAtBeginning = "TOP";
            SqlFormatter sqlFormatter = new SqlFormatter("", "", limitClauseAtBeginning, "");
            //---------------Assert Precondition----------------
            Assert.AreEqual(limitClauseAtBeginning, sqlFormatter.LimitClauseAtBeginning);
            //---------------Execute Test ----------------------
            string limitClauseForBegin = sqlFormatter.GetLimitClauseCriteriaForBegin(10);
            //---------------Test Result -----------------------
            const string expectedLimitClauseForEnd = limitClauseAtBeginning + " 10";
            Assert.AreEqual(expectedLimitClauseForEnd, limitClauseForBegin);
        }

        [Test]
        public void Test_GetLimitClauseForBegin_HasNotClauseSet()
        {
            //---------------Set up test pack-------------------
            const string limitClauseAtBeginning = "";
            SqlFormatter sqlFormatter = new SqlFormatter("", "", limitClauseAtBeginning, "");
            //---------------Assert Precondition----------------
            Assert.AreEqual("", sqlFormatter.LimitClauseAtBeginning);
            //---------------Execute Test ----------------------
            string limitClauseForBegin = sqlFormatter.GetLimitClauseCriteriaForBegin(10);
            //---------------Test Result -----------------------
            Assert.AreEqual("", limitClauseForBegin);
        }

        [Test]
        public void Test_PrepareValue_WithGuid()
        {
            //---------------Set up test pack-------------------
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", "");
            Guid g = Guid.NewGuid();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object preparedValue = sqlFormatter.PrepareValue(g);
            //---------------Test Result -----------------------
            string strg = g.ToString("B").ToUpper();
            Assert.AreEqual(strg, preparedValue, "PrepareValue is not preparing guids correctly.");
        }

        [Test]
        public void Test_PrepareValue_WithTimeSpan_ShouldReturnDateTime()
        {
            //---------------Set up test pack-------------------
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", "");
            var value = new TimeSpan(0,23,59,59,999);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
			object preparedValue = sqlFormatter.PrepareValue(value);
            //---------------Test Result -----------------------
        	var expected = new DateTime(1900,1,1,23,59,59,999);
			Assert.AreEqual(expected, preparedValue, "PrepareValue is not preparing timespans correctly.");
        }

        [Test]
        public void Test_PrepareValue_WithTimeSpan_WhenHasDays_ShouldReturnDateTime()
        {
            //---------------Set up test pack-------------------
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", "");
            var value = new TimeSpan(1,23,59,59,999);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
			object preparedValue = sqlFormatter.PrepareValue(value);
            //---------------Test Result -----------------------
        	var expected = new DateTime(1900,1,2,23,59,59,999);
			Assert.AreEqual(expected, preparedValue, "PrepareValue is not preparing timespans correctly.");
        }
    }
}