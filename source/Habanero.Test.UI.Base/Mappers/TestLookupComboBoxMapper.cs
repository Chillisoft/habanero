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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
        [TestFixture]
        public class TestLookupComboBoxMapperVWG
            
        {
        protected const string LOOKUP_ITEM_2 = "Test2";
        protected const string LOOKUP_ITEM_1 = "Test1";
        protected DataStoreInMemory _store;
        


            protected virtual IControlFactory GetControlFactory()
            {
                ControlFactoryVWG factory = new ControlFactoryVWG();
                GlobalUIRegistry.ControlFactory = factory;
                return factory;
            }
#pragma warning disable 168
            [TestFixtureSetUp]
            public void TestFixtureSetup()
            {
                _store = new DataStoreInMemory();
                BORegistry.DataAccessor = new DataAccessorInMemory(_store);
                Dictionary<string, string> collection = Sample.BOLookupCollection;
            }
#pragma warning restore 168

            [Test]
            public virtual void TestChangeComboBoxDoesntUpdateBusinessObject()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                Guid guidResult;
                StringUtilities.GuidTryParse(Sample.LookupCollection[LOOKUP_ITEM_1], out guidResult);
                s.SampleLookupID = guidResult;
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;
                
                //---------------Test Result -----------------------
                Assert.AreEqual(Sample.LookupCollection[LOOKUP_ITEM_1], s.SampleLookupID.ToString());
                //---------------Tear Down -------------------------
            }
            [Test]
            public void TestConstructor()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                //---------------Execute Test ----------------------
                const string propName = "SampleLookupID";
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
                const string propName = "SampleLookupID";
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
            //
            [Test]
            public void TestSetBusinessObj()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                //---------------Assert Precondition----------------
                Assert.AreEqual(3, Sample.LookupCollection.Count);
                Assert.IsNull(cmbox.SelectedItem);
                //---------------Execute Test ----------------------
                mapper.BusinessObject = s;
                //---------------Test Result -----------------------
                Assert.IsNotNull(cmbox.SelectedItem);
                Assert.AreEqual(LOOKUP_ITEM_1, cmbox.SelectedItem, "Item is not set.");
                Assert.AreEqual(s.SampleLookupID.ToString(), cmbox.SelectedValue, "Value is not set");
            }

            [Test]
            public void TestSetBusinessObject_Null_DoesNotRaiseError_BUGFIX()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                //---------------Assert Precondition----------------
                Assert.AreEqual(3, Sample.LookupCollection.Count);
                Assert.IsNull(cmbox.SelectedItem);
                //---------------Execute Test ----------------------
                mapper.BusinessObject = null;
                //---------------Test Result -----------------------
                Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(cmbox.SelectedItem)));
            }
            [Test]
            public void TestSetBusinessObject_Null_NullLookupListSet_DoesNotRaiseError_BUGFIX()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                //---------------Assert Precondition----------------
                Assert.IsNull(mapper.LookupList);
                Assert.AreEqual(0, cmbox.Items.Count);
                Assert.IsNull(cmbox.SelectedItem);
                //---------------Execute Test ----------------------
                mapper.BusinessObject = null;
                //---------------Test Result -----------------------
                Assert.IsNull(cmbox.SelectedItem);
                Assert.AreEqual(1, cmbox.Items.Count, "Should have only the null item in it.");
            }
            //        protected static string GuidToUpper(Guid guid)
            //        {
            //            return guid.ToString("B").ToUpperInvariant();
            //        }
            protected static object GetGuidValue(IDictionary<string, string> collection, string lookupItem)
            {
                BOPropGuidDataMapper guidDataMapper = new BOPropGuidDataMapper();
                object returnValue;
                guidDataMapper.TryParsePropValue(collection[lookupItem], out returnValue);
                return returnValue;
            }

            [Test]
            public void TestApplyChangesToBusObj()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;
                mapper.ApplyChangesToBusinessObject();
                //---------------Test Result -----------------------
                //            Assert.AreEqual((Guid) Sample.LookupCollection[LOOKUP_ITEM_2], s.SampleLookupID);
                Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);

                //---------------Tear Down -------------------------
            }

            [Test]
            public void TestUsingPropWithLookupSource()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookup2ID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());

                //---------------Execute Test ----------------------

                mapper.BusinessObject = new Sample();
                cmbox.SelectedIndex = 2;
                //---------------Test Result -----------------------
                Assert.AreEqual(4, cmbox.Items.Count);
                Assert.AreSame(typeof(string), cmbox.SelectedItem.GetType());
                Assert.AreSame(typeof(string), cmbox.SelectedValue.GetType());
            }


            [Test]
            public void TestUsingPropWithBOLookupList()
            {
                //---------------Set up test pack-------------------
                DataStoreInMemory store = _store;
                BORegistry.DataAccessor = new DataAccessorInMemory(store);
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookup2ID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
//                Sample.BOLookupCollection = null;
                Dictionary<string, string> collection = Sample.BOLookupCollection;
                Sample sample = new Sample();
                sample.Save();
                string boId = Sample.BOLookupCollection[LOOKUP_ITEM_2];
                Assert.AreEqual(4, store.Count);
                IBusinessObject businessObject = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue<Sample>(boId);
                Sample sampleToSelect = (Sample)businessObject;
                sample.SetPropertyValue(propName, sampleToSelect);
                            
                //--------------Assert Preconditions -------------
                Assert.AreEqual(3, collection.Count);
                Assert.AreEqual(4, _store.Count);
                //---------------Execute Test ----------------------
                mapper.LookupList = collection;
                mapper.BusinessObject = sample;
                
                //---------------Test Result -----------------------
                Assert.AreEqual(4, cmbox.Items.Count);
                Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem);
                Assert.AreEqual(sampleToSelect.ToString(), cmbox.SelectedValue);
            }

            [Test]
            public void TestUsingBOLookupListString()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookup3ID";
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
            }

            [Test]
            public void TestCustomiseLookupList_Add()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookup2ID";
                CustomAddLookupComboBoxMapper mapper = new CustomAddLookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample sample = new Sample();

                //---------------Execute Test ----------------------
                mapper.BusinessObject = sample;

                //---------------Test Result -----------------------
                Assert.AreEqual(5, cmbox.Items.Count);
                Assert.AreEqual("ExtraLookupItem", LastComboBoxItem(cmbox).ToString());

                //---------------Tear Down -------------------------
                // This test changes the static class def, so force a reload
                ClassDef.ClassDefs.Remove(typeof(Sample));
            }
            [Test]
            public void TestCustomiseLookupList_Add_SelectAdded()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookup2ID";
                CustomAddLookupComboBoxMapper mapper = new CustomAddLookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample sample = new Sample();
                mapper.BusinessObject = sample;
                //---------------Assert Preconditions---------------
                Assert.AreEqual(5, cmbox.Items.Count);
                Assert.AreEqual("ExtraLookupItem", LastComboBoxItem(cmbox).ToString());
                //---------------Execute Test ----------------------
                cmbox.SelectedIndex = cmbox.Items.Count - 1;
                mapper.ApplyChangesToBusinessObject();
                //---------------Test Result -----------------------
                object value = sample.GetPropertyValue(propName);
                Assert.IsNotNull(value);

                //---------------Tear Down -------------------------
                // This test changes the static class def, so force a reload
                ClassDef.ClassDefs.Remove(typeof(Sample));
            }
            [Test]
            public void Test_LookupList_AddItemToComboBox_SelectAdditionalItem_SetsBOPropValueToNull()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookup2ID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample sample = new Sample();
                mapper.BusinessObject = sample;
                cmbox.Items.Add("SomeItem");
                //---------------Assert Preconditions---------------
                Assert.AreEqual(5, cmbox.Items.Count);
                Assert.AreEqual("SomeItem", LastComboBoxItem(cmbox).ToString());
                //---------------Execute Test ----------------------
                cmbox.SelectedIndex = cmbox.Items.Count - 1;
                mapper.ApplyChangesToBusinessObject();
                //---------------Test Result -----------------------
                object value = sample.GetPropertyValue(propName);
                Assert.IsNull(value);
            }
            private static object LastComboBoxItem(IComboBox cmbox)
            {
                return cmbox.Items[cmbox.Items.Count - 1];
            }

            [Test]
            public void TestCustomiseLookupList_Remove()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookup2ID";
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

        [TestFixture]
        public class TestLookupComboBoxMapperWin : TestLookupComboBoxMapperVWG
        {
            protected override IControlFactory GetControlFactory()
            {
                ControlFactoryWin factory = new ControlFactoryWin();
                GlobalUIRegistry.ControlFactory = factory;
                return factory;
            }

            [Test]
            public void TestChangePropValueUpdatesBusObj()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                Guid guidResult;
                StringUtilities.GuidTryParse(Sample.LookupCollection[LOOKUP_ITEM_1], out guidResult);
                s.SampleLookupID = guidResult;
                mapper.BusinessObject = s;
                //---------------Test Preconditions-------------------
                Assert.AreEqual(3, Sample.LookupCollection.Count);
                Assert.IsNotNull(mapper.LookupList);
                Assert.IsNotNull(cmbox.SelectedItem, "There should be a selected item to start with");
                //---------------Execute Test ----------------------

                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2);
                mapper.UpdateControlValueFromBusinessObject();
                
                //---------------Test Result -----------------------
                Assert.IsNotNull(cmbox.SelectedItem );
                Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem, "Value is not set after changing bo prop Value");
            }

            [Test]
            public void TestChangePropValueUpdatesBusObj_WithoutCallingUpdateControlValue()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------

                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2);

                //---------------Test Result -----------------------
                Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem, "Value is not set after changing bo prop");

                //---------------Tear Down -------------------------
            }
            [Test]
            public override void TestChangeComboBoxDoesntUpdateBusinessObject()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                Guid guidResult;
                StringUtilities.GuidTryParse(Sample.LookupCollection[LOOKUP_ITEM_1], out guidResult);
                s.SampleLookupID = guidResult;
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;

                //---------------Test Result -----------------------
                Assert.AreEqual(Sample.LookupCollection[LOOKUP_ITEM_1], s.SampleLookupID.ToString());

            }
            [Test]
            public void TestChangeComboBoxUpdatesBusinessObject()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;
                mapper.ApplyChangesToBusinessObject();
                //---------------Test Result -----------------------
                Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
            }
            //            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            //            Sample s = new Sample();
            //            mapper.LookupList = Sample.LookupCollection;
            //            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            //            //---------------Assert Precondition----------------
            //            Assert.AreEqual(3, Sample.LookupCollection.Count);
            //            Assert.IsNull(cmbox.SelectedItem);
            //            //---------------Execute Test ----------------------
            //            mapper.BusinessObject = s;
            [Test]
            public void TestChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges()
            {
                //---------------Set up test pack-------------------
                IComboBox cmbox = GetControlFactory().CreateComboBox();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(LookupComboBoxDefaultMapperStrategyWin), mapper.MapperStrategy);
                Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
            }

            [Test]
            public void TestKeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
            {
                //---------------Set up test pack-------------------
                ComboBoxWinStub cmbox = new ComboBoxWinStub();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.Text = "Test2";
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(LookupComboBoxDefaultMapperStrategyWin), mapper.MapperStrategy);
                Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
            }


            [Test]
            public void Test_KeyPressStrategy_UpdatesBusinessObject_WhenEnterKeyPressed()
            {
                //---------------Set up test pack-------------------
                ComboBoxWinStub cmbox = new ComboBoxWinStub();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.Text = "Test2";
                cmbox.CallSendKeyBob();

                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(LookupComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
                Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
            }


            [Test]
            public void Test_KeyPressStrategy_DoesNotUpdateBusinessObject_SelectedIndexChanged()
            {
                //---------------Set up test pack-------------------
                ComboBoxWinStub cmbox = new ComboBoxWinStub();
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
                mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
                Sample s = new Sample();
                mapper.LookupList = Sample.LookupCollection;
                s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
                mapper.BusinessObject = s;
                //---------------Execute Test ----------------------
                cmbox.SelectedItem = LOOKUP_ITEM_2;

                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(LookupComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
                Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1), s.SampleLookupID);
                //---------------Tear Down -------------------------
            }
//
            private class ComboBoxWinStub : ComboBoxWin
            {
                public void CallSendKeyBob()
                {
                    this.OnKeyPress(new System.Windows.Forms.KeyPressEventArgs((char)13));
                }
            }
        }

    internal class CustomAddLookupComboBoxMapper : LookupComboBoxMapper
    {
        public CustomAddLookupComboBoxMapper(IComboBox cbx, string propName, bool isReadOnly, IControlFactory factory)
            : base(cbx, propName, isReadOnly, factory) {}

        protected override void CustomiseLookupList(Dictionary<string, string> col)
        {
            Sample additionalBO = new Sample {SampleText = "ExtraLookupItem"};
            col.Add(additionalBO.SampleText, additionalBO.ToString());
        }
    }

    internal class CustomRemoveLookupComboBoxMapper : LookupComboBoxMapper
    {
        public CustomRemoveLookupComboBoxMapper(IComboBox cbx, string propName, bool isReadOnly, IControlFactory factory)
            : base(cbx, propName, isReadOnly, factory) { }

        protected override void CustomiseLookupList(Dictionary<string, string> col)
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