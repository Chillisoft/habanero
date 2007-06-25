
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Test;
using Habanero.Ui.Base;
using NUnit.Framework;

namespace Habanero.Test.Ui.Generic
{
    /// <summary>
    /// Summary description for TestCollectionGridDataProvider.
    /// </summary>
    [TestFixture]
    public class TestCollectionGridDataProvider
    {
        private ClassDef itsClassDef;
        private BusinessObjectCollection itsCollection;

        private MyBo itsBo1;
        private MyBo itsBo2;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.GetClassDefCol.Clear();
            itsClassDef = MyBo.LoadDefaultClassDef();
            itsCollection = new BusinessObjectCollection(itsClassDef);

            itsBo1 = MyBo.Create();
            itsBo2 = MyBo.Create();
        }

        [Test]
        public void TestSimpleConstructor()
        {
            CollectionGridDataProvider provider = new CollectionGridDataProvider(itsCollection);
            Assert.AreSame(itsCollection, provider.GetCollection());
            Assert.AreSame(itsBo1.GetUserInterfaceMapper().GetUIGridProperties(), provider.GetUIGridDef());
        }

        [Test]
        public void TestWithNonDefaultUiDef()
        {
            CollectionGridDataProvider provider = new CollectionGridDataProvider(itsCollection, "Alternate");
            Assert.AreSame(itsCollection, provider.GetCollection());
            Assert.AreSame(itsBo1.GetUserInterfaceMapper("Alternate").GetUIGridProperties(), provider.GetUIGridDef());
        }
    }
}