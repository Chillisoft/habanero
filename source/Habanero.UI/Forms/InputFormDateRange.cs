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
using System.Windows.Forms;
using Habanero.UI;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a form in which a user can enter or edit a date range with
    /// a start and end date
    /// </summary>
    public class InputFormDateRange
    {
        private readonly string _message;
        private DateTimePicker _startDateTimePicker;
        private DateTimePicker _endDateTimePicker;
        private string _title;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title</param>
        public InputFormDateRange(string message, string title)
        {
            _message = message;
            _title = title;
            _startDateTimePicker = ControlFactory.CreateStandardDateTimePicker();
            _endDateTimePicker = ControlFactory.CreateStandardDateTimePicker();
        }

        /// <summary>
        /// Sets up the form and makes it visible to the user
        /// </summary>
        /// <returns>Returns a DialogResult object which indicates the user's 
        /// response to the dialog. See System.Windows.Forms.DialogResult for 
        /// more detail.</returns>
        public DialogResult ShowDialog()
        {
            Panel mainPanel = new Panel();
            BorderLayoutManager manager = new BorderLayoutManager(mainPanel);
            manager.AddControl(ControlFactory.CreateLabel(_message, false), BorderLayoutManager.Position.North);

            Panel messagePanel = new Panel();
            GridLayoutManager messagePanelManager = new GridLayoutManager(messagePanel);
            messagePanelManager.SetGridSize(2, 2);
            messagePanelManager.FixAllRowsBasedOnContents();
            messagePanelManager.FixColumnBasedOnContents(0);
            messagePanelManager.AddControl(ControlFactory.CreateLabel("Start Date", false));
            messagePanelManager.AddControl(_startDateTimePicker);
            messagePanelManager.AddControl(ControlFactory.CreateLabel("End Date", false));
            messagePanelManager.AddControl(_endDateTimePicker);
            messagePanel.Height = messagePanelManager.GetFixedHeightIncludingGaps();
            messagePanel.Width = Math.Max(250, ControlFactory.CreateLabel(_message, false).PreferredWidth + 10);
            mainPanel.Height = messagePanel.Height + ControlFactory.CreateLabel("Test", true).Height + 5;
            mainPanel.Width = messagePanel.Width;
            manager.AddControl(messagePanel, BorderLayoutManager.Position.Centre);
            return new OKCancelDialog(mainPanel, _title).ShowDialog();
        }

        /// <summary>
        /// Gets and sets the start date in the date range
        /// </summary>
        public DateTime StartDate
        {
            get { return _startDateTimePicker.Value; }
            set { _startDateTimePicker.Value = value; }
        }

        /// <summary>
        /// Gets and sets the end date in the date range
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDateTimePicker.Value; }
            set { _endDateTimePicker.Value = value; }
        }
    }
}