using System.Threading.Tasks;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.DB.ConcurrencyTests
{
    [TestFixture]
    public class TestBOSave_Concurrency_FirebirdEmbedded: FirebirdEmbeddedTestsBase
    {
        [Test]
        [Ignore("Intermittently failing on the CI server")] 
        public void Test_ConcurrentSave()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            var classDef = ContactPersonTestBO.LoadDefaultClassDef();
            FixtureEnvironment.ClearBusinessObjectManager();
            TestUtil.WaitForGC();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Parallel.For(0, 1000, i =>
                                  {
                                      var person = new ContactPersonTestBO();
                                      person.Surname = RandomValueGen.GetRandomString(1, 10);
                                      person.Save();
                                  });

            //---------------Test Result -----------------------
        }
    }
}