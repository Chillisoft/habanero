using System.Windows.Forms;
using Chillisoft.Generic.v2;
using Chillisoft.Test.General.v2;
using Chillisoft.UI.BOControls.v2;
using NUnit.Framework;

namespace Chillisoft.Test.UI.BOControls.v2
{
    /// <summary>
    /// Summary description for TestComboBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestLookupComboBoxMapper : TestUsingDatabase
    {
        private ComboBox cbx;
        private LookupComboBoxMapper mapper;
        private Sample s;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        [SetUp]
        public void SetupTest()
        {
            cbx = new ComboBox();
            mapper = new LookupComboBoxMapper(cbx, "SampleLookupID", false);
            s = new Sample();
            mapper.SetLookupList(Sample.LookupCollection);
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(cbx, mapper.Control);
            Assert.AreSame("SampleLookupID", mapper.PropertyName);
        }

        [Test]
        public void TestSetLookupList()
        {
            Assert.AreEqual(4, cbx.Items.Count);
            Assert.AreSame(typeof (StringGuidPair), cbx.Items[0].GetType());
            Assert.IsTrue(cbx.Items.Contains(Sample.LookupCollection[1]));
        }

        [Test]
        public void TestComboBoxValue()
        {
            s.SampleLookupID = Sample.LookupCollection[0].Id;
            mapper.BusinessObject = s;
            Assert.AreEqual(Sample.LookupCollection[0].Id, ((StringGuidPair) cbx.SelectedItem).Id, "Value is not set.");
            s.SampleLookupID = Sample.LookupCollection[1].Id;
            Assert.AreEqual(Sample.LookupCollection[1].Id, ((StringGuidPair) cbx.SelectedItem).Id,
                            "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingComboValueUpdatesBO()
        {
            s.SampleLookupID = Sample.LookupCollection[0].Id;
            mapper.BusinessObject = s;
            cbx.SelectedIndex = 2;
            StringGuidPair selected = (StringGuidPair) cbx.SelectedItem;
            Assert.AreEqual(selected.Id, s.SampleLookupID,
                            "BO property value isn't changed when control value is changed.");
        }

//		[Test, ExpectedException(typeof (LookupListNotSetException), "You must set the lookup list before using a control that requires it.")]
//		public void TestNotSettingLookupList() {
//			cbx = new ComboBox();
//			mapper = new LookupComboBoxMapper(cbx, "SampleLookupID", false);
//			s.SampleLookupID = Sample.LookupCollection[0].Id;
//			mapper.BusinessObject = s;
//		}

        [Test]
        public void TestUsingPropWithLookupSource()
        {
            cbx = new ComboBox();
            mapper = new LookupComboBoxMapper(cbx, "SampleLookup2ID", false);
            s = new Sample();
            s.SetPropertyValue("SampleLookup2ID", Sample.LookupCollection[1].Id);
            mapper.BusinessObject = s;
            Assert.AreEqual(4, cbx.Items.Count);
            Assert.AreSame(typeof (StringGuidPair), cbx.Items[0].GetType());
            Assert.IsTrue(cbx.Items.Contains(Sample.LookupCollection[1]));
            Assert.AreEqual("Test2", ((StringGuidPair) cbx.SelectedItem).Str);
        }
    }
}