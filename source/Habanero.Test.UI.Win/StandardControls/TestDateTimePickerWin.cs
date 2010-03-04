using System;
using System.Drawing;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.StandardControls
{
    [TestFixture]
    public class TestDateTimePickerWin : TestDateTimePicker
    {
        protected override void SetBaseDateTimePickerValue(IDateTimePicker dateTimePicker, DateTime value)
        {
            System.Windows.Forms.DateTimePicker picker = (System.Windows.Forms.DateTimePicker)dateTimePicker;
            picker.Value = value;
        }

        protected override void SetBaseDateTimePickerCheckedValue(IDateTimePicker dateTimePicker, bool value)
        {
            System.Windows.Forms.DateTimePicker picker = (System.Windows.Forms.DateTimePicker)dateTimePicker;
            picker.Checked = value;
        }

        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override EventArgs GetKeyDownEventArgsForDeleteKey()
        {
            return new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.Delete);
        }

        protected override EventArgs GetKeyDownEventArgsForBackspaceKey()
        {
            return new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.Back);
        }

        protected override EventArgs GetKeyDownEventArgsForOtherKey()
        {
            return new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.A);
        }

        [Test, Ignore("Only for visual testing")]
        public void TestShowWithEvents()
        {
            //---------------Set up test pack-------------------
            System.Windows.Forms.DateTimePicker dateTimePicker = new System.Windows.Forms.DateTimePicker();
            dateTimePicker.ShowCheckBox = true;
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            form.Controls.Add(textBox);
            textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox.Multiline = true;
            form.Controls.Add(dateTimePicker);
            dateTimePicker.Dock = System.Windows.Forms.DockStyle.Top;
            dateTimePicker.ValueChanged += delegate
                                               {
                                                   textBox.Text += "EventFired";
                                               };
            System.Windows.Forms.Button button = new System.Windows.Forms.Button();
            form.Controls.Add(button);
            button.Dock = System.Windows.Forms.DockStyle.Bottom;
            button.Click += delegate
                                {
                                    dateTimePicker.Checked = !dateTimePicker.Checked;
                                };
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            form.ShowDialog();
            //---------------Test Result -----------------------

        }

        [Test, Ignore("Only for visual testing")]
        public void TestShowDatePickerForm()
        {
            //---------------Set up test pack-------------------
            IFormHabanero formWin = new FormWin();
            IDateTimePicker dateTimePicker = GetControlFactory().CreateDateTimePicker();
            dateTimePicker.Format = Habanero.UI.Base.DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = @"Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa: dd MMM yyyy";
            dateTimePicker.NullDisplayValue = "Please Click";
            //dateTimePicker.ShowCheckBox = true;
            ITextBox textBox = GetControlFactory().CreateTextBox();
            IButton button = GetControlFactory().CreateButton("Check/Uncheck", delegate
                                                                                   {
                                                                                       //dateTimePicker.Checked = !dateTimePicker.Checked;
                                                                                       if (dateTimePicker.ValueOrNull.HasValue)
                                                                                       {
                                                                                           dateTimePicker.ValueOrNull = null;
                                                                                       }
                                                                                       else
                                                                                       {
                                                                                           dateTimePicker.ValueOrNull = dateTimePicker.Value;
                                                                                       }
                                                                                   });
            IButton enableButton = GetControlFactory().CreateButton("Enable/Disable", delegate
                                                                                          {
                                                                                              dateTimePicker.Enabled = !dateTimePicker.Enabled;
                                                                                          });
            GridLayoutManager gridLayoutManager = new GridLayoutManager(formWin, GetControlFactory());
            gridLayoutManager.SetGridSize(5, 1);
            gridLayoutManager.AddControl(dateTimePicker);
            gridLayoutManager.AddControl(button);
            gridLayoutManager.AddControl(textBox);
            gridLayoutManager.AddControl(enableButton);
            gridLayoutManager.AddControl(GetControlFactory().CreateButton("ChangeColor", delegate
                                                                                             {
                                                                                                 Random random = new Random();
                                                                                                 dateTimePicker.ForeColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                                                                                                 dateTimePicker.BackColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                                                                                             }));
            dateTimePicker.ValueChanged += delegate
                                               {
                                                   textBox.Text = dateTimePicker.ValueOrNull.HasValue ? dateTimePicker.Value.ToString() : "";
                                               };
            //---------------Execute Test ----------------------
            formWin.ShowDialog();
            //---------------Test Result -----------------------

            //---------------Tear down -------------------------

        }
    }
}