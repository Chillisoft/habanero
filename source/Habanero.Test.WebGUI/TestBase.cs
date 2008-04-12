using System.Collections.Generic;
using System.IO;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.WebGUI
{
    [TestFixture]
    public class TestBase 
    {
        protected static List<BusinessObject> _objectsToDelete = new List<BusinessObject>();
        //TODO: SetupDatabaseConfig
        protected DatabaseConfig _databaseConfig = new DatabaseConfig(
            "Access", null, @"C:\Systems\NScape\EarlyEducation\source\EarlyEducationASP\App_Data\EarlyEducationTest.mdb", null, null, null);

        public TestBase()
        {
            if (DatabaseConnection.CurrentConnection == null)
            {
                DatabaseConnection.CurrentConnection = _databaseConfig.GetDatabaseConnection();
                ClassDef.ClassDefs.Clear();
                ClassDef.LoadClassDefs(
                    new XmlClassDefsLoader(new StreamReader("Classdefs.xml").ReadToEnd(), new DtdLoader()));
            }
        }

        [TearDown]
        public void TearDownTest()
        {
            DeleteObjects(_objectsToDelete);
        }
        public static void DeleteObjects(List<BusinessObject> objectsToDelete)
        {
            int count = objectsToDelete.Count;
            Dictionary<BusinessObject, int> failureHistory = new Dictionary<BusinessObject, int>();
            while (objectsToDelete.Count > 0)
            {
                BusinessObject thisBo = objectsToDelete[objectsToDelete.Count - 1];
                try
                {
                    if (!thisBo.State.IsNew)
                    {
                        thisBo.Restore();
                        thisBo.Delete();
                        thisBo.Save();
                    }
                    objectsToDelete.Remove(thisBo);
                }
                catch
                {
                    int failureCount = 0;
                    if (failureHistory.ContainsKey(thisBo))
                    {
                        failureCount = failureHistory[thisBo]++;
                    }
                    else
                    {
                        failureHistory.Add(thisBo, failureCount + 1);
                    }
                    objectsToDelete.Remove(thisBo);
                    if (failureCount <= count)
                    {
                        objectsToDelete.Insert(0, thisBo);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}