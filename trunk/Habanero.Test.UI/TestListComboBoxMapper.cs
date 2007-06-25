using System.Windows.Forms;
using Chillisoft.Test;
using Habanero.Generic;
using Habanero.Test.General;
using Habanero.Ui.Forms;
using NUnit.Framework;

namespace Habanero.Test.Ui.BoControls
{
    /// <summary>
    /// Summary description for TestComboBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestListComboBoxMapper : TestUsingDatabase
    {
        private ComboBox cbx;
        private ListComboBoxMapper mapper;
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
            mapper = new ListComboBoxMapper(cbx, "SampleText", false);
            s = new Sample();
            mapper.SetList("One;Two;Three;Four", false);
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(cbx, mapper.Control);
            Assert.AreSame("SampleText", mapper.PropertyName);
        }

        [Test]
        public void TestSetList()
        {
            Assert.AreEqual(4, cbx.Items.Count);
            Assert.AreSame(typeof(string), cbx.Items[0].GetType());
            Assert.IsTrue(cbx.Items.Contains("Two"));
        }

        [Test]
        public void TestComboBoxValue()
        {
            s.SampleText = "Three";
            mapper.BusinessObject = s;
            Assert.AreEqual("Three", cbx.SelectedItem, "Value is not set.");
            s.SampleText = "Four";
            Assert.AreEqual("Four", cbx.SelectedItem, "Value is not set after changing bo prop");
        }

        [Test]
        public void TestSettingComboValueUpdatesBO()
        {
            s.SampleText = "Three";
            mapper.BusinessObject = s;
            cbx.SelectedIndex = 0;
            //string selected = (string)cbx.SelectedItem;
            Assert.AreEqual(cbx.SelectedItem, s.SampleText, "BO property value isn't changed when control value is changed.");
        }
    }
}