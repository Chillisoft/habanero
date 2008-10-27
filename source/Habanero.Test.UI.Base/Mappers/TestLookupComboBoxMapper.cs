//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.VWG;
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
        public class TestLookupComboBoxMapperVWG : TestLookupComboBoxMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            [Test]
            public void TestChangeComboBoxDoesntUpdateBusinessObject()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
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

            [Test]
            public void TestChangePropValueUpdatesBusObj()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;     
                //---------------Execute Test ----------------------

                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_2];
                mapper.UpdateControlValueFromBusinessObject();
                
                //---------------Test Result -----------------------
                Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem, "Value is not set after changing bo prop");
        
                //---------------Tear Down -------------------------
            }


            [Test]
            public void TestChangePropValueUpdatesBusObj_WithoutCallingUpdateControlValue()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------

                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_2];

                //---------------Test Result -----------------------
                Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem, "Value is not set after changing bo prop");

                //---------------Tear Down -------------------------
            }

            [Test]
            public void TestChangeComboBoxUpdatesBusinessObject()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;
                mapper.ApplyChangesToBusinessObject();
                //---------------Test Result -----------------------
                Assert.AreEqual((Guid)Sample.LookupCollection[LOOKUP_ITEM_2], s.SampleLookupID);
                //---------------Tear Down -------------------------
            }


            [Test]
            public void TestChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(LookupComboBoxDefaultMapperStrategyWin), mapper.MapperStrategy);
                Assert.AreEqual((Guid)Sample.LookupCollection[LOOKUP_ITEM_2], s.SampleLookupID);
                //---------------Tear Down -------------------------
            }


            [Test]
            public void TestKeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
            {
                //---------------Set up test pack-------------------
                ComboBoxWinStub cmbox = new ComboBoxWinStub();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.Text = "Test2";
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(LookupComboBoxDefaultMapperStrategyWin), mapper.MapperStrategy);
                Assert.AreEqual((Guid)Sample.LookupCollection[LOOKUP_ITEM_2], s.SampleLookupID);
                //---------------Tear Down -------------------------
            }


            [Test]
            public void Test_KeyPressStrategy_UpdatesBusinessObject_WhenEnterKeyPressed()
            {
                //---------------Set up test pack-------------------
                ComboBoxWinStub cmbox = new ComboBoxWinStub();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.Text = "Test2";
                cmbox.CallSendKeyBob();

                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(LookupComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
                Assert.AreEqual((Guid)Sample.LookupCollection[LOOKUP_ITEM_2], s.SampleLookupID);
                //---------------Tear Down -------------------------
            }


            [Test]
            public void Test_KeyPressStrategy_DoesNotUpdateBusinessObject_SelectedIndexChanged()
            {
                //---------------Set up test pack-------------------
                ComboBoxWinStub cmbox = new ComboBoxWinStub();
                string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;

                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(LookupComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
                Assert.AreEqual((Guid)Sample.LookupCollection[LOOKUP_ITEM_1], s.SampleLookupID);
                //---------------Tear Down -------------------------
            }

            private class ComboBoxWinStub : ComboBoxWin
            {
                public void CallSendKeyBob()
                {
                    this.OnKeyPress(new System.Windows.Forms.KeyPressEventArgs((char)13));
                }
            }
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            //---------------Execute Test ----------------------
            string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());

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
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            new Sample();
            //---------------Execute Test ----------------------
            mapper.LookupList = Sample.LookupCollection;

            //---------------Test Result -----------------------
            Assert.AreEqual(4, cmbox.Items.Count);
            //Assert.AreSame(typeof (string), cmbox.Items[0].GetType());
            //Assert.IsTrue(cmbox.Items.Contains(LOOKUP_ITEM_1));

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetBusinessObj()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            s.SampleLookupID = (Guid)Sample.LookupCollection[LOOKUP_ITEM_1];
            //---------------Execute Test ----------------------
            mapper.BusinessObject = s;
            //---------------Test Result -----------------------
            Assert.AreEqual(LOOKUP_ITEM_1, cmbox.SelectedItem, "Item is not set.");
            Assert.AreEqual(s.SampleLookupID, cmbox.SelectedValue, "Value is not set");
            
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestApplyChangesToBusObj()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
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
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
      
            //---------------Execute Test ----------------------

            mapper.BusinessObject = new Sample();
            cmbox.SelectedIndex = 2;
            //---------------Test Result -----------------------
            Assert.AreEqual(4, cmbox.Items.Count);
            Assert.AreSame(typeof(string), cmbox.SelectedItem.GetType());
            Assert.AreSame(typeof(Guid), cmbox.SelectedValue.GetType());
            
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestUsingPropWithBOLookupList()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookup2ID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            
            Sample sample = new Sample();
            Sample sampleToSelect = (Sample) Sample.BOLookupCollection[LOOKUP_ITEM_2];
            sample.SetPropertyValue(propName, sampleToSelect);
                        
            //---------------Execute Test ----------------------
            mapper.LookupList = Sample.BOLookupCollection;
            mapper.BusinessObject = sample;
            
            //---------------Test Result -----------------------
            Assert.AreEqual(4, cmbox.Items.Count);
            Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem);
            Assert.AreSame(sampleToSelect, cmbox.SelectedValue);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestUsingBOLookupListString()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookup3ID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample sample = new Sample();
            object sampleToSelect = Sample.BOLookupCollection[LOOKUP_ITEM_2];
            sample.SetPropertyValue(propName, sampleToSelect);
            
            //---------------Execute Test ----------------------
            mapper.LookupList = Sample.BOLookupCollection;
            mapper.BusinessObject = sample;

            //---------------Test Result -----------------------
            Assert.AreEqual(4, cmbox.Items.Count);
            Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem);
            Assert.AreSame(sampleToSelect, cmbox.SelectedValue);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCustomiseLookupList_Add()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookup2ID";
            CustomAddLookupComboBoxMapper mapper = new CustomAddLookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample sample = new Sample();

            //---------------Execute Test ----------------------
            mapper.BusinessObject = sample;

            //---------------Test Result -----------------------
            Assert.AreEqual(5, cmbox.Items.Count);
            Assert.AreEqual("ExtraLookupItem", cmbox.Items[cmbox.Items.Count - 1].ToString());

            //---------------Tear Down -------------------------
            // This test changes the static class def, so force a reload
            ClassDef.ClassDefs.Remove(typeof(Sample));
        }

        [Test]
        public void TestCustomiseLookupList_Remove()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            string propName = "SampleLookup2ID";
            CustomRemoveLookupComboBoxMapper mapper = new CustomRemoveLookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample sample = new Sample();

            //---------------Execute Test ----------------------
            mapper.BusinessObject = sample;

            //---------------Test Result -----------------------
            Assert.AreEqual(3, cmbox.Items.Count);
            
            //---------------Tear Down -------------------------
            // This test changes the static class def, so force a reload
            ClassDef.ClassDefs.Remove(typeof(Sample));
        }
    }

    internal class CustomAddLookupComboBoxMapper : LookupComboBoxMapper
    {
        public CustomAddLookupComboBoxMapper(IComboBox cbx, string propName, bool isReadOnly, IControlFactory factory)
            : base(cbx, propName, isReadOnly, factory) {}

        protected override void CustomiseLookupList(Dictionary<string, object> col)
        {
            Sample additionalBO = new Sample();
            additionalBO.SampleText = "ExtraLookupItem";

            col.Add(additionalBO.SampleText, additionalBO);
        }
    }

    internal class CustomRemoveLookupComboBoxMapper : LookupComboBoxMapper
    {
        public CustomRemoveLookupComboBoxMapper(IComboBox cbx, string propName, bool isReadOnly, IControlFactory factory)
            : base(cbx, propName, isReadOnly, factory) { }

        protected override void CustomiseLookupList(Dictionary<string, object> col)
        {
            string lastKey = "";
            foreach (string key in col.Keys)
            {
                lastKey = key;
            }

            col.Remove(lastKey);
        }
    }
}