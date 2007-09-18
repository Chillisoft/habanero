using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Application
{
	[TestFixture]
	public class TestDateTimePickerController
	{
		private DateTimePicker _dateTimePicker;
		private DateTimePickerController _dateTimePickerController;
		private Control _nullDisplayControl;

		[SetUp]
		public void SetupTest()
		{
			_dateTimePicker = new DateTimePicker();
			_dateTimePickerController = new DateTimePickerController(_dateTimePicker);
			if (_dateTimePicker.Controls.Count > 0)
			{
				_nullDisplayControl = _dateTimePicker.Controls[0];
			}
			Assert.IsNotNull(_nullDisplayControl, "The Date Time Picker was not set up correctly.");
		}

		[Test]
		public void TestSetPickerValue()
		{
			DateTime sampleDate = new DateTime(2001, 01, 01, 01, 01, 01);
			_dateTimePicker.Value = sampleDate;
			Assert.AreEqual(sampleDate, _dateTimePickerController.Value);
			Assert.IsFalse(_nullDisplayControl.Visible, "Null display value control should not be visible when there is a value.");
		}

		[Test]
		public void TestSetControllerValue()
		{
			DateTime sampleDate = new DateTime(2002, 02, 02, 02, 02, 02);
			_dateTimePickerController.Value = sampleDate;
			Assert.AreEqual(sampleDate, _dateTimePicker.Value);
			Assert.IsFalse(_nullDisplayControl.Visible, "Null display value control should not be visible when there is a value.");
		}

		[Test]
		public void TestSetNullValue()
		{
			_dateTimePickerController.Value = null;
			Assert.AreEqual(null, _dateTimePickerController.Value);
			Assert.IsTrue(_nullDisplayControl.Visible, "Null display value control should be visible when there is a null value.");
		}


		[Test]
		public void TestSetNullThenControllerValue()
		{
			TestSetNullValue();
			TestSetControllerValue();
		}

		[Test]
		public void TestSetNullThenPickerValue()
		{
			TestSetNullValue();
			TestSetPickerValue();
		}

		[Test]
		public void TestSetControllerValueFiresEvent()
		{
			DateTime sampleDate = new DateTime(2003, 03, 03, 03, 03, 03);
			bool isFired = false;
			EventHandler handleValueChanged = delegate
			{
				isFired = true;
			};
			_dateTimePickerController.ValueChanged += handleValueChanged;
			_dateTimePickerController.Value = sampleDate;
			Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
			isFired = false;
			_dateTimePickerController.Value = null;
			Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value to null.");
			_dateTimePickerController.ValueChanged -= handleValueChanged;
		}

		[Test]
		public void TestSetPickerValueFiresEvent()
		{
			DateTime sampleDate = new DateTime(2004, 04, 04, 04, 04, 04);
			bool isFired = false;
			EventHandler handleValueChanged = delegate
			{
				isFired = true;
			};
			_dateTimePickerController.ValueChanged += handleValueChanged;
			_dateTimePicker.Value = sampleDate;
			Assert.IsTrue(isFired, "The ValueChanged event should have fired after setting the value.");
			_dateTimePickerController.ValueChanged -= handleValueChanged;
		}

		[Test]
		public void TestSetNullDisplayValue()
		{
			string nullDisplayValue = "<TEST_NULL_DISPLAY_VALUE>";
			_dateTimePickerController.NullDisplayValue = nullDisplayValue;
			Assert.AreEqual(nullDisplayValue, _dateTimePickerController.NullDisplayValue);
			Assert.AreEqual(1, _nullDisplayControl.Controls.Count, "Null display panel should have a control.");
			Control displayText = _nullDisplayControl.Controls[0];
			Assert.AreEqual(nullDisplayValue, displayText.Text, "Null display text should equal whas has just been set.");
		}
	}
}
