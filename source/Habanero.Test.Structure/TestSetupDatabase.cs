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
        [Test, Ignore("Not Part of this project")]
        public void Test()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(new StreamReader("Classdefs.xml").ReadToEnd(), new DtdLoader()));
            DatabaseConfig databaseConfig = new DatabaseConfig("Mysql", "localhost", "habanero_temp", "root", "root", "3306");
            IDatabaseConnection databaseConnection = databaseConfig.GetDatabaseConnection();
            SetupDatabase setupDatabase = new SetupDatabase(databaseConnection);
            setupDatabase.ClearDatabase();
            setupDatabase.CreateDatabase(ClassDef.ClassDefs);
        }

    }
}