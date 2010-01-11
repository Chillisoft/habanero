using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    [TestFixture]
    public class TestCollectionComboBoxMapperVWG
    {
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
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            //---------------Execute Test ----------------------
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());

            //---------------Test Result -----------------------
            Assert.AreSame(cmbox, mapper.Control);
            Assert.AreSame(propName, mapper.PropertyName);
            Assert.IsFalse(mapper.IsReadOnly);
        }

        [Test]
        public void TestSetBOCollection()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());

            Sample bo = new Sample();
            IBusinessObjectCollection col = new BusinessObjectCollection<Sample> {bo};
            //---------------Assert Precondition ---------------
            Assert.AreEqual(0, cmbox.Items.Count);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, cmbox.Items.Count, "The Sample item and the blank");
            Assert.AreSame(typeof (string), cmbox.Items[0].GetType(), "First Item should be blank");
            Assert.IsTrue(cmbox.Items.Contains(bo), "Second Item should be the Business Object");
        }

        [Test]
        public void Test_SetBusinessObj_ShouldSetTheSelectedItemToCorrectRelatedCar()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample { SampleLookupID = car1.CarID };
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, mapper.BusinessObjectCollection.Count);
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            Assert.IsNull(mapper.OwningBoPropertyName);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = s;
            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(car1, cmbox.SelectedItem, "Combo Box selected item is not set.");
            Assert.AreEqual(s.SampleLookupID.ToString(), cmbox.SelectedValue.ToString(), "Combo Box Value is not set");
        }

        [Test]
        public void Test_SetBusinessObj_WhenSpecificPropUsed_ShouldSetTheSelectedItemToCorrectRelatedCar()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            mapper.OwningBoPropertyName = "CarRegNo";

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            string carRegNo = "MySelectedRegNo " + TestUtil.GetRandomString().Substring(0, 4);
            car1.CarRegNo = carRegNo;
            car2.CarRegNo = TestUtil.GetRandomString();
            Sample sample = new Sample { SampleText = carRegNo };
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, mapper.BusinessObjectCollection.Count);
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            Assert.AreEqual("CarRegNo", mapper.OwningBoPropertyName);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = sample;
            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(car1, cmbox.SelectedItem, "Combo Box selected item is not set.");
        }

        [Test]
        public void Test_BusinessObjectCollection_WhenSet_WithNewCollection_WhenItemAlreadySelected_AndAlsoInNewList_ShouldStillHaveTheSameItemSelected()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            mapper.OwningBoPropertyName = "CarRegNo";

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            string carRegNo = "MySelectedRegNo " + TestUtil.GetRandomString().Substring(0, 4);
            car1.CarRegNo = carRegNo;
            car2.CarRegNo = TestUtil.GetRandomString();
            Sample sample = new Sample { SampleText = carRegNo };
            BusinessObjectCollection<Car> newCol = new BusinessObjectCollection<Car>();
            Car car3 = new Car() { CarRegNo = TestUtil.GetRandomString() };
            newCol.Add(car1, car2, car3);
            mapper.BusinessObject = sample;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, mapper.BusinessObjectCollection.Count);
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.AreEqual(car1, cmbox.SelectedItem, "Combo Box selected item should be set.");
            Assert.AreEqual("CarRegNo", mapper.OwningBoPropertyName);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = newCol;
            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(car1, cmbox.SelectedItem, "Combo Box selected item should still be set.");
            Assert.AreSame(carRegNo, sample.SampleText);
        }

        [Test]
        public void Test_BusinessObjectCollection_WhenSet_WithNewCollection_WhenItemAlreadySelected_AndNotInNewList_ShouldHaveNothingSelected()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            mapper.OwningBoPropertyName = "CarRegNo";

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            string carRegNo = "MySelectedRegNo " + TestUtil.GetRandomString().Substring(0, 4);
            car1.CarRegNo = carRegNo;
            car2.CarRegNo = TestUtil.GetRandomString();
            Sample sample = new Sample { SampleText = carRegNo };
            BusinessObjectCollection<Car> newCol = new BusinessObjectCollection<Car>();
            Car car3 = new Car() { CarRegNo = TestUtil.GetRandomString() };
            newCol.Add(car2, car3);
            mapper.BusinessObject = sample;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, mapper.BusinessObjectCollection.Count);
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.AreEqual(car1, cmbox.SelectedItem, "Combo Box selected item should be set.");
            Assert.AreEqual("CarRegNo", mapper.OwningBoPropertyName);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = newCol;
            //---------------Test Result -----------------------
            Assert.IsNull(cmbox.SelectedItem);
            Assert.IsNull(sample.SampleText);
        }

        [Test]
        [Ignore("This needs to be determined what the correct action is here, different test result for Win/VWG")]
        //TODO Mark 11 Jan 2010: Ignored Test - This needs to be determined what the correct action is here, different test result for Win/VWG
        public void Test_BusinessObjectCollection_WhenSet_WithNewCollection_WhenItemAlreadySelected_AndDifferentMatchInNewList_ShouldSelectNewMatch()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            mapper.OwningBoPropertyName = "CarRegNo";

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            string carRegNo = "MySelectedRegNo " + TestUtil.GetRandomString().Substring(0, 4);
            car1.CarRegNo = carRegNo;
            car2.CarRegNo = TestUtil.GetRandomString();
            Sample sample = new Sample { SampleText = carRegNo };
            BusinessObjectCollection<Car> newCol = new BusinessObjectCollection<Car>();
            Car car3 = new Car() { CarRegNo = carRegNo };
            newCol.Add(car2, car3);
            mapper.BusinessObject = sample;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, mapper.BusinessObjectCollection.Count);
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.AreEqual(car1, cmbox.SelectedItem, "Combo Box selected item should be set.");
            Assert.AreEqual("CarRegNo", mapper.OwningBoPropertyName);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = newCol;
            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(car3, cmbox.SelectedItem, "Combo Box selected item should now be the new match.");
            Assert.AreSame(carRegNo, sample.SampleText);
        }

        //This test is specific for VWG where you do not want the BO updated on every event.
        [Test]
        public virtual void Test_ChangeComboBoxDoesntUpdateBusinessObject()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = car2;
            //---------------Test Result -----------------------
            Assert.AreEqual(car1.CarID.ToString(), s.SampleLookupID.ToString());
        }

        protected static BusinessObjectCollection<Car> GetCollectionWithTwoCars(out Car car1, out Car car2)
        {
            car1 = new Car();
            car2 = new Car();
            return new BusinessObjectCollection<Car> {car1, car2};
        }


        [Test]
        public void
            Test_ResetBusinessObj_WhenHasNullValueForProperty_WhenPreviousBOHadAValue_ShouldSetSelectedItemNull_BUGFIX()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample {SampleLookupID = car1.CarID};

            mapper.BusinessObject = s;
            Sample sWithNullPropValue = new Sample();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(car1, cmbox.SelectedItem, "Combo Box selected item is not set.");
            Assert.AreEqual(s.SampleLookupID.ToString(), cmbox.SelectedValue.ToString(), "Combo Box Value is not set");

            Assert.IsNull(sWithNullPropValue.SampleLookupID);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = sWithNullPropValue;
            //---------------Test Result -----------------------
//            Assert.IsNull(cmbox.SelectedItem);
            TestUtil.AssertStringEmpty(Convert.ToString(cmbox.SelectedItem), "cmbox.SelectedItem",
                                       "there should be no item selected");
        }

        [Test]
        public void Test_setBusinessObj_WhenHasNullCollection_ShouldNotRaiseErrro()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());

            mapper.BusinessObjectCollection = null;
            Car car1 = new Car();
            Sample s = new Sample {SampleLookupID = car1.CarID};
            //---------------Assert Precondition----------------
            Assert.IsNull(cmbox.SelectedItem);
            Assert.IsNull(mapper.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            try
            {
                mapper.BusinessObject = s;
                Assert.Fail("expected developer exception");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(
                    "The BusinessObjectCollection is null when in the CollectionComboBoxMapper when the BusinessObject is set ",
                    ex.Message);
                StringAssert.Contains(
                    "The BusinessObjectCollection is null when in the CollectionComboBoxMapper when the BusinessObject is set ",
                    ex.DeveloperMessage);
            }
        }


        [Test]
        public void TestSetBusinessObject_Null_DoesNotRaiseError_BUGFIX()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            //---------------Assert Precondition----------------
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
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsNull(mapper.BusinessObjectCollection);
            Assert.AreEqual(0, cmbox.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(cmbox.SelectedItem);
            Assert.AreEqual(0, cmbox.Items.Count, "Should have only the null item in it.");
        }


        [Test]
        public void TestApplyChangesToBusObj()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = car2;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            //            Assert.AreEqual((Guid) Sample.LookupCollection[LOOKUP_ITEM_2], s.SampleLookupID);
            Assert.AreEqual(car2.CarID, s.SampleLookupID);
        }


        [Test]
        public void Test_LookupList_AddItemToComboBox_SelectAdditionalItem_SetsBOPropValueToNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookup2ID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample sample = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = sample;
            cmbox.Items.Add("SomeItem");
            //---------------Assert Preconditions---------------
            Assert.AreEqual(4, cmbox.Items.Count);
            Assert.AreEqual("SomeItem", LastComboBoxItem(cmbox).ToString());
            //---------------Execute Test ----------------------
            cmbox.SelectedIndex = cmbox.Items.Count - 1;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            object value = sample.GetPropertyValue(propName);
            Assert.IsNull(value);
        }

        // If add item to BOCollection them must update combo box.

        [Test]
        public void Test_AddItemToCollection_ShouldAddItemToComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookup2ID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1;
            Car car2;
            IBusinessObjectCollection collection =
                mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample sample = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = sample;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(3, cmbox.Items.Count);
            //---------------Execute Test ----------------------
            collection.Add(new Car());
            //---------------Test Result -----------------------
            Assert.AreEqual(4, cmbox.Items.Count);
        }


        private static object LastComboBoxItem(IComboBox cmbox)
        {
            return cmbox.Items[cmbox.Items.Count - 1];
        }
    }

    [TestFixture]
    public class TestCollectionComboBoxMapperWin : TestCollectionComboBoxMapperVWG
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
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, true, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = s;
            //---------------Test Preconditions-------------------
            Assert.AreEqual(3, Sample.LookupCollection.Count);
            Assert.IsNotNull(mapper.BusinessObjectCollection);
            Assert.IsNotNull(cmbox.SelectedItem, "There should be a selected item to start with");
            //---------------Execute Test ----------------------
            s.SampleLookupID = car2.CarID;
            mapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(s.SampleLookupID.ToString(), cmbox.SelectedItem.ToString(),
                            "Value is not set after changing bo prop Value");
        }

        [Test]
        public void TestChangePropValueUpdatesBusObj_WithoutCallingUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------

            s.SampleLookupID = car2.CarID;

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(s.SampleLookupID.ToString(), cmbox.SelectedItem.ToString(),
                            "Value is not set after changing bo prop Value");
        }

        [Test]
        public override void Test_ChangeComboBoxDoesntUpdateBusinessObject()
        {
            //For Windows the value should is changed (see TestChangeComboBox_UpdatesBusinessObject).
        }

        [Test]
        public void TestChangeComboBox_UpdatesBusinessObject()
        {
            //For Windows the value should be changed.
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = car2;
            //---------------Test Result -----------------------
            Assert.AreEqual(car2.CarID.ToString(), s.SampleLookupID.ToString(),
                            "For Windows the value should be changed");
        }

//        private static Dictionary<string, string> GetLookupList()
//        {
//            Sample sample1 = new Sample();
//            sample1.Save();
//            Sample sample2 = new Sample();
//            sample2.Save();
//            Sample sample3 = new Sample();
//            sample3.Save();
//            return new Dictionary<string, string>
//                        {
//                            {"Test3", sample3.ID.GetAsValue().ToString()},
//                            {"Test2", sample2.ID.GetAsValue().ToString()},
//                            {"Test1", sample1.ID.GetAsValue().ToString()}
//                        };
//        }
//
//
        [Test]
        public void TestKeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.Text = car2.ToString();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (ComboBoxDefaultMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreEqual(car2.CarID, s.SampleLookupID);
        }

        [Test]
        public void Test_KeyPressStrategy_UpdatesBusinessObject_WhenEnterKeyPressed()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory())
                                                  {
                                                      MapperStrategy =
                                                          GetControlFactory().CreateLookupKeyPressMapperStrategy()
                                                  };
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.Text = car2.ToString();
            cmbox.CallSendKeyBob();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreEqual(car2.CarID, s.SampleLookupID);
        }


        [Test]
        public void Test_KeyPressStrategy_DoesNotUpdateBusinessObject_SelectedIndexChanged()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            const string propName = "SampleLookupID";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample s = new Sample {SampleLookupID = car1.CarID};
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = car2;

            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreEqual(car1.CarID, s.SampleLookupID);
        }

        [Test]
        public void Test_WhenUseToStringSet_ShouldUseBoToString()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Car car1; Car car2;
            IBusinessObjectCollection collection =
                mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            car1.CarRegNo = "MyCarRegNo";
            Sample sample = new Sample { SampleLookupID = car1.CarID };
            mapper.BusinessObjectCollection = collection;
            mapper.BusinessObject = sample;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, cmbox.Items.Count);
            //---------------Execute Test ----------------------
            mapper.OwningBoPropertyName = "CarRegNo";
            cmbox.SelectedItem = car1;
            //---------------Test Result -----------------------
            Assert.AreEqual(car1.CarRegNo, sample.SampleText);
        }

        private class ComboBoxWinStub : ComboBoxWin
        {
            public void CallSendKeyBob()
            {
                this.OnKeyPress(new System.Windows.Forms.KeyPressEventArgs((char) 13));
            }
        }
    }

//    internal class CustomAddCollectionComboBoxMapperStub : CollectionComboBoxMapper
//    {
//        public CustomAddCollectionComboBoxMapperStub(IComboBox cbx, string propName, bool isReadOnly,
//                                                     IControlFactory factory)
//            : base(cbx, propName, isReadOnly, factory)
//        {
//        }
//
//        protected override void CustomiseLookupList(Dictionary<string, string> col)
//        {
//            Sample additionalBO = new Sample {SampleText = "ExtraLookupItem"};
//            col.Add(additionalBO.SampleText, additionalBO.ToString());
//        }
//    }
//
//    internal class CustomRemoveCollectionComboBoxMapperStub : CollectionComboBoxMapper
//    {
//        public CustomRemoveCollectionComboBoxMapperStub(IComboBox cbx, string propName, bool isReadOnly,
//                                                        IControlFactory factory)
//            : base(cbx, propName, isReadOnly, factory)
//        {
//        }
//
//        protected override void CustomiseLookupList(Dictionary<string, string> col)
//        {
//            string lastKey = "";
//            foreach (string key in col.Keys)
//            {
//                lastKey = key;
//            }
//
//            col.Remove(lastKey);
//        }
//    }
}