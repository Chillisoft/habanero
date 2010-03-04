using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;


using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    public abstract class TestCollectionComboBoxMapper
    {
        protected DataStoreInMemory _store;
        protected abstract IControlFactory GetControlFactory();

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _store = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(_store);
            Dictionary<string, string> collection = Sample.BOLookupCollection;
        }

        [SetUp]
        public void TestSetup()
        {
            ClassDef.ClassDefs.Clear();
        }

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
        public void Test_SetBusinessObj_WhenSpecificGuidPropUsed_ShouldSetTheSelectedItemToCorrectRelatedCar()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            Sample.CreateClassDefWithAGuidProp();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string sampleBOProp = "GuidProp";
            const string owningBoPropertyName = "CarId";
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, sampleBOProp, false, GetControlFactory()) 
                                                  { OwningBoPropertyName = owningBoPropertyName };

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            Sample sample = new Sample();
            sample.SetPropertyValue(sampleBOProp, car1.CarID);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, mapper.BusinessObjectCollection.Count);
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            Assert.AreEqual(owningBoPropertyName, mapper.OwningBoPropertyName);
            Assert.IsNotNull(sample.GetPropertyValue(sampleBOProp));
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
            Car car3 = new Car { CarRegNo = TestUtil.GetRandomString() };
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
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory())
                                                  {OwningBoPropertyName = "CarRegNo"};

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            string carRegNo = "MySelectedRegNo " + TestUtil.GetRandomString().Substring(0, 4);
            car1.CarRegNo = carRegNo;
            car2.CarRegNo = TestUtil.GetRandomString();
            Sample sample = new Sample { SampleText = carRegNo };
            BusinessObjectCollection<Car> newCol = new BusinessObjectCollection<Car>();
            Car car3 = new Car { CarRegNo = TestUtil.GetRandomString() };
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
            CollectionComboBoxMapper mapper = new CollectionComboBoxMapper(cmbox, propName, false, GetControlFactory())
                                                  {OwningBoPropertyName = "CarRegNo"};

            Car car1;
            Car car2;
            mapper.BusinessObjectCollection = GetCollectionWithTwoCars(out car1, out car2);
            string carRegNo = "MySelectedRegNo " + TestUtil.GetRandomString().Substring(0, 4);
            car1.CarRegNo = carRegNo;
            car2.CarRegNo = TestUtil.GetRandomString();
            Sample sample = new Sample { SampleText = carRegNo };
            BusinessObjectCollection<Car> newCol = new BusinessObjectCollection<Car>();
            Car car3 = new Car { CarRegNo = carRegNo };
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

}