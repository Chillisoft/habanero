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

using System.Collections.Generic;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a form containing a ComboBox in order to get a single
    /// input value back from a user
    /// </summary>
    public class InputFormComboBox
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _message;
        private readonly IComboBox _comboBox;

        ///<summary>
        /// Constructor for <see cref="InputFormComboBox"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="message"></param>
        ///<param name="choices"></param>
        public InputFormComboBox(IControlFactory controlFactory, string message, List<object> choices)
        {
            _controlFactory = controlFactory;
            _message = message;
            _comboBox = _controlFactory.CreateComboBox();
            choices.ForEach(item => _comboBox.Items.Add(item));
        }

        /// <summary>
        /// Gets the control factory used to create the controls
        /// </summary>
        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        /// <summary>
        /// Gets the message to display to the user
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets the ComboBox control on the form
        /// </summary>
        public IComboBox ComboBox
        {
            get { return _comboBox; }
        }

        /// <summary>
        /// Gets or sets the selected item in the ComboBox
        /// </summary>
        public object SelectedItem
        {
            get { return _comboBox.SelectedItem; }
            set { _comboBox.SelectedItem = value; }
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
            flowLayoutManager.AddControl(_comboBox);
            panel.Height = _comboBox.Height + label.Height;
            panel.Width = _controlFactory.CreateLabel(_message, true).PreferredWidth + 20;
            _comboBox.Width = panel.Width - 30;
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