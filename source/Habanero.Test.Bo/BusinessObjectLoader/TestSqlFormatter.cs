//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
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