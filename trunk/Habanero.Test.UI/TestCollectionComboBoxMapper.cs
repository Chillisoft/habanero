using System.Windows.Forms;
using Habanero.Bo;
using Habanero.Test.General;
using Habanero.Ui.BoControls;
using NUnit.Framework;

namespace Habanero.Test.Ui.BoControls
{
    /// <summary>
    /// Summary description for TestCollectionComboBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestCollectionComboBoxMapper
    {
        ComboBox itsComboBox;
        CollectionComboBoxMapper mapper;
        private BusinessObjectCollection itsCollection;

        [SetUp]
        public void SetupTest()
        {
            itsComboBox = new ComboBox();
            mapper = new CollectionComboBoxMapper(itsComboBox);
            itsCollection = new BusinessObjectCollection(Sample.GetClassDef());
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