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

using System.Linq;
using Habanero.Base.Data;
using NUnit.Framework;

namespace Habanero.Test.Base.Data
{
    [TestFixture]
    public class TestQueryResult
    {
        [Test]
        public void Construct()
        {
            //---------------Execute Test ----------------------
            var result = new QueryResult();
            //---------------Test Result -----------------------
            Assert.IsNotNull(result.Fields);
            Assert.IsNotNull(result.Rows);
            Assert.AreEqual(0, result.Fields.Count());
            Assert.AreEqual(0, result.Rows.Count());
        }

        [Test]
        public void AddField_ShouldAddFieldToFields()
        {
            //---------------Set up test pack-------------------
            var resultSet = new QueryResult();
            const string propertyName = "Name";
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
            var resultSet = new QueryResult();
            const string propertyName1 = "Name";
            resultSet.AddField(propertyName1);
            const string propertyName2 = "Name2";
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
            var resultSet = new QueryResult();
            resultSet.AddField("Name");
            var name = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            resultSet.AddResult(new object[] {name});
            //---------------Test Result -----------------------
            var rows = resultSet.Rows.ToList();
            Assert.AreEqual(1, rows.Count);
            Assert.AreEqual(name, rows[0].RawValues[0]);
        }
    }
}
