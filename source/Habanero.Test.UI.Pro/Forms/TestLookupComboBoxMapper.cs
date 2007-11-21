using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Test.General;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
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
            Assert.AreSame(typeof (string), cbx.Items[0].GetType());
            Assert.IsTrue(cbx.Items.Contains("Test1"));
        }

        [Test]
        public void TestComboBoxValue()
        {
            s.SampleLookupID = new Guid("{6E8B3DDB-1B13-4566-868D-57478C1F4BEE}");
            mapper.BusinessObject = s;
            Assert.AreEqual("Test1", (string)cbx.SelectedItem, "Value is not set.");
            s.SampleLookupID = new Guid("{7209B956-96A0-4720-8E49-DE154FA0E096}");
            Assert.AreEqual("Test2", (string)cbx.SelectedItem, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingComboValueUpdatesBO()
        {
            s.SampleLookupID = new Guid("{6E8B3DDB-1B13-4566-868D-57478C1F4BEE}");
            mapper.BusinessObject = s;
            cbx.SelectedIndex = 2;
            string selected = (string) cbx.SelectedItem;
            Assert.AreEqual(Sample.LookupCollection[selected], s.SampleLookupID,
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
            s.SetPropertyValue("SampleLookup2ID", new Guid("{7209B956-96A0-4720-8E49-DE154FA0E096}"));
            mapper.BusinessObject = s;
            Assert.AreEqual(4, cbx.Items.Count);
            Assert.AreSame(typeof (string), cbx.Items[0].GetType());
            Assert.IsTrue(cbx.Items.Contains("Test1"));
            Assert.AreEqual("Test2", (string) cbx.SelectedItem);
        }

        [Test]
        public void TestUsingBOLookupList()
        {
            cbx = new ComboBox();
            mapper = new LookupComboBoxMapper(cbx, "SampleLookup2ID", false);
            mapper.SetLookupList(Sample.BOLookupCollection);
            s = new Sample();
            s.SetPropertyValue("SampleLookup2ID", Sample.BOLookupCollection["Test2"]);
            mapper.BusinessObject = s;
            Assert.AreEqual(4, cbx.Items.Count);
            Assert.AreSame(typeof (string), cbx.Items[0].GetType());
            Assert.IsTrue(cbx.Items.Contains("Test1"));
            Assert.AreEqual("Test2", (string) cbx.SelectedItem);
        }

        [Test]
        public void TestUsingBOLookupListStr()
        {
            cbx = new ComboBox();
            mapper = new LookupComboBoxMapper(cbx, "SampleLookup3ID", false);
            mapper.SetLookupList(Sample.BOLookupCollection);
            s = new Sample();
            s.SetPropertyValue("SampleLookup3ID", Sample.BOLookupCollection["Test2"]);
            mapper.BusinessObject = s;
            Assert.AreEqual(4, cbx.Items.Count);
            Assert.AreSame(typeof(string), cbx.Items[0].GetType());
            Assert.IsTrue(cbx.Items.Contains("Test1"));
            Assert.AreEqual("Test2", (string)cbx.SelectedItem);
        }
    }
}