using System;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    public abstract class TestLookupComboBoxMapper
    {
        protected abstract IControlFactory GetControlFactory();
        protected const string LOOKUP_ITEM_2 = "Test2";
        protected const string LOOKUP_ITEM_1 = "Test1";

        [TestFixture]
        public class TestLookupComboBoxMapperGiz : TestLookupComboBoxMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }

            [Test]
            public void TestChangeComboBoxDoesntUpdateBusinessObject()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);
                Sample s = new Sample();
                mapper.SetLookupList(Sample.LookupCollection);
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;
                
                //---------------Test Result -----------------------
                Assert.AreEqual((Guid)Sample.LookupCollection[LOOKUP_ITEM_1], s.SampleLookupID);
                //---------------Tear Down -------------------------
            }
        }

        [TestFixture]
        public class TestLookupComboBoxMapperWin : TestLookupComboBoxMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            [Test, Ignore("To be implemented for windows")]
            public void TestChangePropValueUpdatesBusObj()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);
                Sample s = new Sample();
                mapper.SetLookupList(Sample.LookupCollection);
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;     
                //---------------Execute Test ----------------------

                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_2];
                
                //---------------Test Result -----------------------
                Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem, "Value is not set after changing bo prop");
        
                //---------------Tear Down -------------------------
            }

            [Test, Ignore("To be implemented for windows")]
            public void TestChangeComboBoxUpdatesBusinessObject()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);
                Sample s = new Sample();
                mapper.SetLookupList(Sample.LookupCollection);
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;

                //---------------Test Result -----------------------
                Assert.AreEqual((Guid)Sample.LookupCollection[LOOKUP_ITEM_2], s.SampleLookupID);
                //---------------Tear Down -------------------------
            }
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            //---------------Execute Test ----------------------
            string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);

            //---------------Test Result -----------------------
            Assert.AreSame(cmbox, mapper.Control);
            Assert.AreSame(propName, mapper.PropertyName);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetLookupList()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);
            new Sample();
            //---------------Execute Test ----------------------
            mapper.SetLookupList(Sample.LookupCollection);

            //---------------Test Result -----------------------
            Assert.AreEqual(4, cmbox.Items.Count);
            Assert.AreSame(typeof (string), cmbox.Items[0].GetType());
            Assert.IsTrue(cmbox.Items.Contains(LOOKUP_ITEM_1));

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetBusinessObj()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);
            Sample s = new Sample();
            mapper.SetLookupList(Sample.LookupCollection);
            s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
            //---------------Execute Test ----------------------
            mapper.BusinessObject = s;
            //---------------Test Result -----------------------
            Assert.AreEqual(LOOKUP_ITEM_1, cmbox.SelectedItem, "Value is not set.");
            
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestApplyChangesToBusObj()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);
            Sample s = new Sample();
            mapper.SetLookupList(Sample.LookupCollection);
            s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = LOOKUP_ITEM_2;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual((Guid) Sample.LookupCollection[LOOKUP_ITEM_2], s.SampleLookupID);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestUsingPropWithLookupSource()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookup2ID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);
      
            //---------------Execute Test ----------------------

            mapper.BusinessObject = new Sample();
            //---------------Test Result -----------------------
            Assert.AreEqual(4, cmbox.Items.Count);
            Assert.AreSame(typeof(string), cmbox.Items[0].GetType());
            Assert.IsTrue(cmbox.Items.Contains(LOOKUP_ITEM_1));
            
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestUsingPropWithBOLookupList()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookup2ID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);
            
            Sample sample = new Sample();
            sample.SetPropertyValue(propName, Sample.BOLookupCollection[LOOKUP_ITEM_2]);
                        
            //---------------Execute Test ----------------------
            mapper.SetLookupList(Sample.BOLookupCollection);
            mapper.BusinessObject = sample;
            
            //---------------Test Result -----------------------
            Assert.AreEqual(4, cmbox.Items.Count);
            Assert.AreSame(typeof(string), cmbox.Items[0].GetType());
            Assert.IsTrue(cmbox.Items.Contains(LOOKUP_ITEM_1));
            Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestUsingBOLookupListString()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookup3ID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false);
            Sample sample = new Sample();
            sample.SetPropertyValue(propName, Sample.BOLookupCollection[LOOKUP_ITEM_2]);
            
            //---------------Execute Test ----------------------
            mapper.SetLookupList(Sample.BOLookupCollection);
            mapper.BusinessObject = sample;

            //---------------Test Result -----------------------
            Assert.AreEqual(4, cmbox.Items.Count);
            Assert.AreSame(typeof(string), cmbox.Items[0].GetType());
            Assert.IsTrue(cmbox.Items.Contains(LOOKUP_ITEM_1));
            Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem);

            //---------------Tear Down -------------------------
        }
    }
}