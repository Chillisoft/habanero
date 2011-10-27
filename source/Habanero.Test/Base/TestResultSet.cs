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
using System.Linq;
using System.Text;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestResultSet
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            var resultSet = new ResultSet();
            //---------------Test Result -----------------------
            Assert.IsNotNull(resultSet.Fields);
            Assert.IsNotNull(resultSet.Rows);
            Assert.AreEqual(0, resultSet.Fields.Count());
            Assert.AreEqual(0, resultSet.Rows.Count());
        }

        [Test]
        public void ConstructField()
        {
            //---------------Set up test pack-------------------
            var propName = TestUtil.GetRandomString();
            var index = TestUtil.GetRandomInt();
            //---------------Execute Test ----------------------
            var field = new ResultSet.Field(propName, index);
            //---------------Test Result -----------------------
            Assert.AreEqual(propName, field.PropertyName);
            Assert.AreEqual(index, field.Index);
        }

        [Test]
        public void AddField_ShouldAddFieldToFields()
        {
            //---------------Set up test pack-------------------
            var resultSet = new ResultSet();
            var propertyName = "Name";
            //---------------Execute Test ----------------------
            resultSet.AddField(propertyName);
            //---------------Test Result -----------------------
            var fields = resultSet.Fields.ToList();
            Assert.AreEqual(1, fields.Count);
            Assert.AreEqual(propertyName, fields[0].PropertyName);
            Assert.AreEqual(0, fields[0].Index);
        }

        [Test]
        public void AddField_ShouldSetIndexOfAddedField()
        {
            //---------------Set up test pack-------------------
            var resultSet = new ResultSet();
            var propertyName1 = "Name";
            resultSet.AddField(propertyName1);
            var propertyName2 = "Name2";
            resultSet.AddField(propertyName2);
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            var fields = resultSet.Fields.ToList();
            Assert.AreEqual(2, fields.Count);
            Assert.AreEqual(propertyName1, fields[0].PropertyName);
            Assert.AreEqual(0, fields[0].Index); 
            Assert.AreEqual(propertyName2, fields[1].PropertyName);
            Assert.AreEqual(1, fields[1].Index);
        }


        [Test]
        public void AddResult_ShouldAddRowToRowsList()
        {
            //---------------Set up test pack-------------------
            var resultSet = new ResultSet();
            resultSet.AddField("Name");
            string name = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            resultSet.AddResult(new object[] {name});
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual(1, rows.Count);
            Assert.AreEqual(name, rows[0].RawValues[0]);
        }

        [Test]
        public void ConstructRow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            var row = new ResultSet.Row();
            //---------------Test Result -----------------------
            Assert.IsNotNull(row.RawValues);
            Assert.IsNotNull(row.Values);
            Assert.AreEqual(0, row.RawValues.Count);
            Assert.AreEqual(0, row.Values.Count);

        }

        [Test]
        public void ConstructRow_WithRawValues()
        {
            //---------------Set up test pack-------------------
            object[] rawValues = new object[] {"somevalues", 34};
            //---------------Execute Test ----------------------
            var row = new ResultSet.Row(rawValues);
            //---------------Test Result -----------------------
            Assert.AreEqual(rawValues.Length, row.RawValues.Count);
            Assert.AreEqual(rawValues[0], row.RawValues[0]);
            Assert.AreEqual(rawValues[1], row.RawValues[1]);
        }

        [Test]
        public void Sort_OneFieldAscending()
        {
            //---------------Set up test pack-------------------
            var resultSet = new ResultSet();
            resultSet.AddField("Name");
            var lastValue = "zzzzz";
            var firstValue = "aaaaa";
            resultSet.AddResult(new object[] { lastValue });
            resultSet.AddResult(new object[] { firstValue });
            OrderCriteria orderCriteria = new OrderCriteria();
            orderCriteria.Add("Name");
            //---------------Execute Test ----------------------
            resultSet.Sort(orderCriteria);
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual(firstValue, rows[0].Values[0]);
            Assert.AreEqual(lastValue, rows[1].Values[0]);
        }

        [Test]
        public void Sort_TwoFieldsAscending()
        {
            //---------------Set up test pack-------------------
            var resultSet = new ResultSet();
            resultSet.AddField("Name");
            resultSet.AddField("Age");
            resultSet.AddResult(new object[] { "Bob", 21 });
            resultSet.AddResult(new object[] { "Bob", 19 });
            resultSet.AddResult(new object[] { "Peter", 40 });
            resultSet.AddResult(new object[] { "Peter", 30 });
            var orderCriteria = OrderCriteria.FromString("Name, Age");
            //---------------Execute Test ----------------------
            resultSet.Sort(orderCriteria);
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual("Bob", rows[0].Values[0]);
            Assert.AreEqual(19, rows[0].Values[1]); 
            Assert.AreEqual("Bob", rows[1].Values[0]);
            Assert.AreEqual(21, rows[1].Values[1]);
            Assert.AreEqual("Peter", rows[2].Values[0]);
            Assert.AreEqual(30, rows[2].Values[1]);
            Assert.AreEqual("Peter", rows[3].Values[0]);
            Assert.AreEqual(40, rows[3].Values[1]);
        }

        [Test]
        public void Sort_OneFieldDescending()
        {
            //---------------Set up test pack-------------------
            var resultSet = new ResultSet();
            resultSet.AddField("Name");
            resultSet.AddResult(new object[] { "aaaaa" });
            resultSet.AddResult(new object[] { "zzzzz" });
            var orderCriteria = OrderCriteria.FromString("Name DESC");
            //---------------Execute Test ----------------------
            resultSet.Sort(orderCriteria);
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual("zzzzz", rows[0].Values[0]);
            Assert.AreEqual("aaaaa", rows[1].Values[0]);
        }

        
    }
}
