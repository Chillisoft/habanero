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

using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    ///<summary>
    /// A <see cref="ComboBox"/> with a <see cref="Button"/> next to it on the right with a '...' displayed as the text.
    ///</summary>
    public class ExtendedComboBoxWin : UserControlWin, IExtendedComboBox
    {
        private readonly IControlFactory _controlFactory;
        private readonly IComboBox _comboBox;
        private readonly IButton _button;

        ///<summary>
        /// Constructs the <see cref="ExtendedComboBoxWin"/> with the default <see cref="IControlFactory"/>.
        ///</summary>
        public ExtendedComboBoxWin() : this(GlobalUIRegistry.ControlFactory) { }

        ///<summary>
        /// Constructs the <see cref="ExtendedComboBoxWin"/> with the specified <see cref="IControlFactory"/>.
        ///</summary>
        public ExtendedComboBoxWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            IUserControlHabanero userControlHabanero = this;
            _comboBox = _controlFactory.CreateComboBox();
            _button = _controlFactory.CreateButton("...");
            BorderLayoutManager borderLayoutManager = controlFactory.CreateBorderLayoutManager(userControlHabanero);
            borderLayoutManager.AddControl(_comboBox, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(_button, BorderLayoutManager.Position.East);
        }


        ///<summary>
        /// Returns the <see cref="IExtendedComboBox.ComboBox"/> in the control
        ///</summary>
        public IComboBox ComboBox
        {
            get { return _comboBox; }
        }

        ///<summary>
        /// Returns the <see cref="IExtendedComboBox.Button"/> in the control
        ///</summary>
        public IButton Button
        {
            get { return _button; }
        }
    }
}
