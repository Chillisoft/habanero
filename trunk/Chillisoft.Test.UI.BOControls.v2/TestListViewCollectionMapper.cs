using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.Bo;
using Chillisoft.Test.General.v2;
using Habanero.Ui.BoControls;
using NUnit.Framework;

namespace Chillisoft.Test.UI.BOControls.v2
{
    [TestFixture ]
    public class TestListViewCollectionMapper
    {

        ListView itsListView;
        ListViewCollectionMapper mapper;
        private BusinessObjectCollection itsCollection;

        [SetUp]
        public void SetupTest()
        {
            itsListView = new ListView();
            mapper = new ListViewCollectionMapper(itsListView);
            itsCollection = new BusinessObjectCollection(Sample.GetClassDef());
            itsCollection.Add(new Sample());
            itsCollection.Add(new Sample());
            itsCollection.Add(new Sample());
            mapper.SetCollection(itsCollection);
        }

        [Test]
        public void TestSetCollection()
        {
            Assert.AreEqual(3, itsListView.Items.Count);
        }

        
        //TODO: this test works in debug mode, but not in nunit.
        //[Test]
        //public void TestGetBusinessObject()
        //{
        //    itsListView.Focus();
        //    itsListView.Items[2].Selected = true;
        //    itsListView.Items[0].Focused = true;
        //    Assert.IsNotNull(mapper.SelectedBusinessObject);
        //    Assert.AreSame(_collection[2], mapper.SelectedBusinessObject);
        //}

        [Test]
        public void TestAddBoToCollection()
        {
            itsCollection.Add(new Sample());
            Assert.AreEqual(4, itsListView .Items.Count);
        }

        [Test]
        public void TestRemoveBoFromCollection()
        {
            itsCollection.RemoveAt(0);
            Assert.AreEqual(2, itsListView.Items.Count);
        }        
    }

}
