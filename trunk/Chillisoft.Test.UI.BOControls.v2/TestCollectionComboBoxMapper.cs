using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Test.General.v2;
using Chillisoft.UI.BOControls.v2;
using NUnit.Framework;

namespace Chillisoft.Test.UI.BOControls.v2
{
    /// <summary>
    /// Summary description for TestCollectionComboBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestCollectionComboBoxMapper
    {
        ComboBox itsComboBox;
        CollectionComboBoxMapper mapper;
        private BusinessObjectBaseCollection itsCollection;

        [SetUp]
        public void SetupTest()
        {
            itsComboBox = new ComboBox();
            mapper = new CollectionComboBoxMapper(itsComboBox);
            itsCollection = new BusinessObjectBaseCollection(Sample.GetClassDef());
            itsCollection.Add(new Sample());
            itsCollection.Add(new Sample());
            itsCollection.Add(new Sample());
            mapper.SetCollection(itsCollection, false);

        }

        [Test]
        public void TestSetCollection()
        {
            Assert.AreEqual(3, itsComboBox.Items.Count);
        }

        [Test]
        public void TestGetBusinessObject()
        {
            itsComboBox.SelectedItem = itsCollection[2];
            Assert.IsNotNull(mapper.SelectedBusinessObject );
            Assert.AreSame(itsCollection[2], mapper.SelectedBusinessObject);
        }

        [Test]
        public void TestAddBoToCollection()
        {
            itsCollection.Add(new Sample());
            Assert.AreEqual(4, itsComboBox.Items.Count);
        }

        [Test]
        public void TestRemoveBoFromCollection()
        {
            itsCollection.RemoveAt(0);
            Assert.AreEqual(2, itsComboBox.Items.Count);
        }
    }
}