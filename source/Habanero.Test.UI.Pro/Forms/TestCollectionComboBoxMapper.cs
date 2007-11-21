using System.Windows.Forms;
using Habanero.BO;
using Habanero.Test.General;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestCollectionComboBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestCollectionComboBoxMapper
    {
        ComboBox itsComboBox;
        CollectionComboBoxMapper mapper;
        private BusinessObjectCollection<BusinessObject> itsCollection;

        [SetUp]
        public void SetupTest()
        {
            itsComboBox = new ComboBox();
            mapper = new CollectionComboBoxMapper(itsComboBox);
            itsCollection = new BusinessObjectCollection<BusinessObject>(Sample.GetClassDef());
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