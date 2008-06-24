using System;
using System.Windows.Forms;
using Habanero.UI;
using Habanero.UI.Base;
using Habanero.UI.Base.Habanero.UI.Win;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;


namespace Habanero.Test.UI.Base
{
    public abstract class TestDateTimePickerController //:TestBase
    {
        protected abstract IControlFactory GetControlFactory();

        //[TestFixture]
        //public class TestDateTimePickerControllerWin : TestDateTimePickerController
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryWin();
        //    }

            
        //}


        //public class TestDateTimePickerControllerGiz : TestDateTimePickerController
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryGizmox();
        //    }
        //    //There are not tests for Giz since this functionality is very specific to windows
        //}

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        private IDateTimePicker CreateDateTimePicker()
        {
            return GetControlFactory().CreateDateTimePicker();
        }

        private DateTimePickerController GetDateTimePickerController(IDateTimePicker dateTimePicker)
        {
            return new DateTimePickerController(GetControlFactory(), dateTimePicker);
        }

        //-----------------------------------TO BE MOVED TO GIZ AS WELL----------------------------
        

        

        //[Test]
        //public void TestCreateDateTimePickerController()
        //{
        //    //---------------Set up test pack-------------------
        //    IDateTimePicker dateTimePicker = CreateDateTimePicker();
        //    //---------------Execute Test ----------------------
        //    DateTimePickerController dateTimePickerController = GetDateTimePickerController(dateTimePicker);

        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(dateTimePickerController);
        //    Assert.IsNotNull(dateTimePickerController.DateTimePicker);
        //    //Assert.IsTrue(dateTimePicker.Controls.Count > 0);
        //    //---------------Tear Down   -----------------------
        //}



        


        //[Test]
        //public void TestSetDateTimePickerValueViaController()
        //{
        //    IDateTimePicker dateTimePicker = CreateDateTimePicker();
        //    DateTimePickerController dateTimePickerController = GetDateTimePickerController(dateTimePicker);
        //    DateTime testDate = new DateTime(2007, 01, 01, 01, 01, 01);
        //    //---------------Execute Test ----------------------
        //    dateTimePickerController.Value = testDate;

        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(testDate, dateTimePicker.Value);
        //    Assert.AreEqual(dateTimePickerController.Value, dateTimePicker.Value);
        //    //---------------Tear Down -------------------------          
        //}

        //-----------------------------------For Windows Only----------------------------
        

        //[Test]
        //public void TestSetPickerControllerValue()
        //{
        //    //---------------Set up test pack-------------------
        //    IDateTimePicker dateTimePicker = CreateDateTimePicker();
        //    DateTimePickerController dateTimePickerController = GetDateTimePickerController(dateTimePicker);
        //    DateTime sampleDate = new DateTime(2001, 01, 01, 01, 01, 01);
        //    //---------------Execute Test ----------------------
        //    dateTimePicker.Value = sampleDate;
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(sampleDate, dateTimePickerController.Value);
        //    IControlChilli nullDisplayControl = dateTimePicker.Controls[0];
        //    Assert.IsFalse(nullDisplayControl.Visible,
        //                   "Null display value control should not be visible when there is a value.");
        //}


        

        //[Test]
        //public void TestSetNullThenControllerValue()
        //{
        //    TestSetNullValue();
        //    TestSetControllerValue();
        //}

        

        

        //[Test]
        //public void TestSetControllerValueFiresEvent()
        //{
        //    IDateTimePicker dateTimePicker = CreateDateTimePicker();
        //    DateTimePickerController dateTimePickerController = GetDateTimePickerController(dateTimePicker);
        //    DateTime sampleDate = new DateTime(2003, 03, 03, 03, 03, 03);
        //    bool isFired = false;
        //    EventHandler handleValueChanged = delegate { isFired = true; };
        //    dateTimePickerController.ValueChanged += handleValueChanged;
        //    dateTimePickerController.Value = sampleDate;
        //    Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
        //    isFired = false;
        //    dateTimePickerController.Value = null;
        //    Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value to null.");
        //    dateTimePickerController.ValueChanged -= handleValueChanged;
        //}

        //[Test]
        //public void TestSetPickerValueFiresEvent()
        //{
        //    IDateTimePicker dateTimePicker = CreateDateTimePicker();
        //    DateTimePickerController dateTimePickerController = GetDateTimePickerController(dateTimePicker);
        //    DateTime sampleDate = new DateTime(2004, 04, 04, 04, 04, 04);
        //    bool isFired = false;
        //    EventHandler handleValueChanged = delegate { isFired = true; };
        //    dateTimePickerController.ValueChanged += handleValueChanged;
        //    dateTimePicker.Value = sampleDate;
        //    Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
        //    dateTimePickerController.ValueChanged -= handleValueChanged;
        //}

    }
}