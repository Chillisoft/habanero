using System.Collections.Concurrent;
using System.Threading.Tasks;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using System.Linq;
using System.Reflection;
using Habanero.Base.Util;
using Habanero.BO;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBusinessObject.
    /// </summary>
    [TestFixture]
    public sealed class TestBusinessObject_Concurrency
    {

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var asm = Assembly.GetExecutingAssembly();
            ConfigurationManager.Initialise(asm);

            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [Test]
        public void TestSaveUpdatesAutoIncrementingField()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();
            var newIds = new ConcurrentBag<int?>();
            //---------------Execute Test ----------------------
            Parallel.For(0, 1000, i => {
                                           //---------------Set up test pack-------------------
                                           var bo = new TestAutoInc();
                                           bo.SetPropertyValue("testfield", "testing 123");
                                           //---------------Assert Precondition----------------
                                           Assert.IsFalse(bo.TestAutoIncID.HasValue);
                                           //---------------Execute Test ----------------------
                                           bo.Save();
                                           //---------------Test Result -----------------------
                                           newIds.Add(bo.TestAutoIncID);
            });
            //---------------Test Result -----------------------
            Assert.IsTrue(newIds.All(i => i.HasValue));
            Assert.IsTrue(newIds.All(i => i > 0));
            Assert.That(newIds.Distinct().Count(), Is.EqualTo(1000), "Every generated ID must be unique");
        }
    }
}