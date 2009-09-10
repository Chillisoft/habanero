// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.BO.BusinessObjectCollection;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [Ignore("//TODO Brett 06 Feb 2009: Install oracle client on machine")]
    [TestFixture]
    public class TestRelatedBOCol_Association_WithOracleDB : TestRelatedBOCol_Association
    {
        [TestFixtureSetUp]
        public override void TestFixtureSetup()
        {
            if (DatabaseConnection.CurrentConnection != null &&
                DatabaseConnection.CurrentConnection.GetType() == typeof(DatabaseConnectionOracle))
            {
                return;
            }
            DatabaseConnection.CurrentConnection =
                new DatabaseConnectionOracle("System.Data.OracleClient", "System.Data.OracleClient.OracleConnection");
            ConnectionStringOracleFactory oracleConnectionString = new ConnectionStringOracleFactory();
            string connStr = oracleConnectionString.GetConnectionString("core1", "XE", "system", "system", "1521");
            DatabaseConnection.CurrentConnection.ConnectionString = connStr;
            DatabaseConnection.CurrentConnection.GetConnection();
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        [SetUp]
        public override void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.DeleteAllContactPeople();
            OrganisationTestBO.DeleteAllOrganisations();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }
    }
}