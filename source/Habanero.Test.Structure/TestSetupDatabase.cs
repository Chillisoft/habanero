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
using System.IO;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.Structure
{
    [TestFixture, Ignore("This fixture is only run to create the database schema")]
    public class TestSetupDatabase
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
        }

        #endregion

        private SetupDatabase _setupDatabase;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            DatabaseConfig databaseConfig = new DatabaseConfig("Mysql", "localhost", "habanero_temp", "root", "root",
                                                               "3306");
            IDatabaseConnection databaseConnection = databaseConfig.GetDatabaseConnection();
            _setupDatabase = new SetupDatabase(databaseConnection);
            _setupDatabase.ClearDatabase();
        }

        [Test, Ignore("Not Part of this project")]
        public void TestAll()
        {
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(new StreamReader("Classdefs.xml").ReadToEnd(), new DtdLoader(),new DefClassFactory()));
            _setupDatabase.CreateDatabase(ClassDef.ClassDefs);
        }

        [Test, Ignore("Not Part of this project")]
        public void TestCreateAll_ClassTableInheritance()
        {
            Entity.LoadDefaultClassDef();
            Part.LoadClassDef_WithClassTableInheritance();
            Engine.LoadClassDef_WithClassTableInheritance();
            Vehicle.LoadClassDef_WithClassTableInheritance();
            Car.LoadClassDef_WithClassTableInheritance();
            LegalEntity.LoadClassDef_WithClassTableInheritance();
            Person.LoadClassDef_WithClassTableInheritance();
            Organisation.LoadClassDef_WithClassTableInheritance();
            OrganisationPerson.LoadDefaultClassDef();
            _setupDatabase.CreateDatabase(ClassDef.ClassDefs);
        }

        [Test, Ignore("Not Part of this project")]
        public void TestCreateAll_NoInheritance()
        {
            Entity.LoadDefaultClassDef();
            Part.LoadDefaultClassDef();
            Engine.LoadDefaultClassDef();
            Vehicle.LoadDefaultClassDef();
            Car.LoadDefaultClassDef();
            LegalEntity.LoadDefaultClassDef();
            Person.LoadDefaultClassDef();
            Organisation.LoadDefaultClassDef();
            OrganisationPerson.LoadDefaultClassDef();
            _setupDatabase.CreateDatabase(ClassDef.ClassDefs);
        }
    }
}