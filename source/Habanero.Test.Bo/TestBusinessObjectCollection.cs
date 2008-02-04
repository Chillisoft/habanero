//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
using NMock;
using NUnit.Framework;
using Rhino.Mocks;
using BusinessObject = Habanero.BO.BusinessObject;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBusinessObjectCollection.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectCollection : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.SetupDBConnection();
            ClassDef.ClassDefs.Clear();
            ClassDef myboClassDef = MyBO.LoadDefaultClassDef();
        }

        [Test]
        public void TestInstantiate()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            Assert.AreSame(ClassDef.ClassDefs[typeof(MyBO)], col.ClassDef);
        }

        [Test]
        public void TestFindByGuid()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo1 = new MyBO();
            col.Add(bo1);
            col.Add(new MyBO());
            Assert.AreSame(bo1, col.FindByGuid(bo1.MyBoID));
        }

        [Test]
        public void TestCreateLoadSqlStatementLimitClauseAtEnd()
        {
            MyBO bo1 = new MyBO();
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(bo1, ClassDef.ClassDefs[typeof (MyBO)], null, 10, null);
            Assert.AreEqual("SELECT `MyBO`.`MyBoID`, `MyBO`.`TestProp`, `MyBO`.`TestProp2` FROM `MyBO` limit 10", statement.Statement.ToString());
        }

        [Test]
        public void TestCreateLoadSqlStatementLimitClauseAtBeginning()
        {
            MyBO bo1 = new MyBO();
            bo1.SetDatabaseConnection(new MyDatabaseConnection());
            ISqlStatement statement = BusinessObjectCollection<BusinessObject>.CreateLoadSqlStatement(bo1, ClassDef.ClassDefs[typeof(MyBO)], null, 10, null);
            Assert.AreEqual("SELECT TOP 10 MyBO.MyBoID, MyBO.TestProp, MyBO.TestProp2 FROM MyBO", statement.Statement.ToString());
        }

        public class MyDatabaseConnection : DatabaseConnection
        {
            public MyDatabaseConnection() : base("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection") { }

            public override string GetLimitClauseForBeginning(int limit)
            {
                return "TOP " + limit;
            }

            public override string LeftFieldDelimiter
            {
                get { return ""; }
            }

            public override string RightFieldDelimiter
            {
                get { return ""; }
            }
        }

    }
}