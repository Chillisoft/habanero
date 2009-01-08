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

using System.Data;
using Habanero.Base;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestBusinessObjectISqlParameterInfo.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectISqlParameterInfo : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void TestSqlParameterInfo()
        {
            new Address(); new Engine(); new Car();
            IExpression exp = Expression.CreateExpression("PK3Prop = 'test'");
            ContactPerson cp = new ContactPerson();
            SqlCriteriaCreator creator = new SqlCriteriaCreator(exp, cp);
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection);
            creator.AppendCriteriaToStatement(statement);
            Assert.AreEqual("contact_person.PK3_Prop = ?Param0", statement.Statement.ToString());
            Assert.AreEqual("test", statement.Parameters[0].Value);
        }
    }
}