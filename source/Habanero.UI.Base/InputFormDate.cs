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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a form containing a DateTimePicker in order to get a single
    /// DateTime value back from a user
    /// </summary>
    public class InputFormDate
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _message;
        private IDateTimePicker _dateTimePicker;

        public InputFormDate(IControlFactory controlFactory, string message)
        {
            _controlFactory = controlFactory;
            _message = message;
            _dateTimePicker = _controlFactory.CreateDateTimePicker(DateTime.Now);
        }

        /// <summary>
        /// Gets the DateTimePicker control
        /// </summary>
        public IDateTimePicker DateTimePicker
        {
            get { return _dateTimePicker; }
        }

        /// <summary>
        /// Gets the message to display to the user
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets or sets the DateTime value held in the DateTimePicker control
        /// </summary>
        public DateTime Value
        {
            get { return this.DateTimePicker.Value; }
            set { this.DateTimePicker.Value = value; }
        }

        /// <summary>
        /// Creates the panel on the form
        /// </summary>
        /// <returns>Returns the panel created</returns>
        public IPanel createControlPanel()
        {
            IPanel panel = _controlFactory.CreatePanel();
            ILabel label = _controlFactory.CreateLabel(_message, false);
            FlowLayoutManager flowLayoutManager = new FlowLayoutManager(panel, _controlFactory);
            flowLayoutManager.AddControl(label);
            flowLayoutManager.AddControl(_dateTimePicker);
            panel.Height = _dateTimePicker.Height + label.Height;
            panel.Width = _controlFactory.CreateLabel(_message, true).PreferredWidth + 20;
            return panel;
        }

        //this is Currently untestable, the layout has been tested in the createControlPanel method.
        /// <summary>
        /// Shows the form to the user
        /// </summary>
        public DialogResult ShowDialog()
        {
            IPanel panel = createControlPanel();
            IOKCancelDialogFactory okCancelDialogFactory = _controlFactory.CreateOKCancelDialogFactory();
            IFormHabanero form = okCancelDialogFactory.CreateOKCancelForm(panel, "");
            return form.ShowDialog();
        }
    }
}