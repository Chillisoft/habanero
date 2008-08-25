using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Habanero.Base;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;
using Habanero.BO.ClassDefinition;

namespace Habanero.Test.Structure
{
    [TestFixture]
    public class TestSetupDatabase
    {
        private SetupDatabase _setupDatabase;

        [SetUpFixture]
        public void SetupFixture()
        {
            DatabaseConfig databaseConfig = new DatabaseConfig("Mysql", "localhost", "habanero_temp", "root", "root", "3306");
            IDatabaseConnection databaseConnection = databaseConfig.GetDatabaseConnection();
            _setupDatabase = new SetupDatabase(databaseConnection);
            _setupDatabase.ClearDatabase();
        }

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
        }

        [Test, Ignore("Not Part of this project")]
        public void TestAll()
        {
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(new StreamReader("Classdefs.xml").ReadToEnd(), new DtdLoader()));
            _setupDatabase.CreateDatabase(ClassDef.ClassDefs);
        }

        [Test]
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