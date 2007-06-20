
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Chillisoft.Test.Setup.v2;
using Habanero.Ui.Generic;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Generic.v2
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