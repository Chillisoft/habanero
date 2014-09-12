using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestBusinessObjectLoaderDB_FirebirdEmbedded : FirebirdEmbeddedTestsBase
    {
        [Test]
        public void Test_GetCount()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            FixtureEnvironment.ClearBusinessObjectManager();
            TestUtil.WaitForGC();
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");
            ContactPersonTestBO.CreateSavedContactPerson("bbbb", "bbb");
            ContactPersonTestBO.CreateSavedContactPerson("cccc", "ccc");
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            int count = BORegistry.DataAccessor.BusinessObjectLoader.GetCount(classDef, null);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, count);
        }

    }
}