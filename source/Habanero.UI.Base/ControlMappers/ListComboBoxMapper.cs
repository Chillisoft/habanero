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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Wraps a ComboBox in order to display and capture a property of the business object 
    /// </summary>
    public class ListComboBoxMapper : ControlMapper
    {
        private readonly IComboBox _comboBox;
        private readonly IListComboBoxMapperStrategy _mapperStrategy;

        public ListComboBoxMapper(IControlHabanero ctl, string propName, bool isReadOnly, IControlFactory factory)
            : base(ctl, propName, isReadOnly, factory)
        {
            _comboBox = (IComboBox)ctl;
            _mapperStrategy = factory.CreateListComboBoxMapperStrategy();
            _mapperStrategy.AddItemSelectedEventHandler(this);
        }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        public override void ApplyChangesToBusinessObject()
        {
            SetPropertyValue(_comboBox.SelectedItem);
        }

        /// <summary>
        /// Updates the value in the control from its business object.
        /// </summary>
        protected  override void InternalUpdateControlValueFromBo()
        {
            _comboBox.SelectedItem = GetPropertyValue();
        }

        /// <summary>
        /// Populates the Items list based on the pipe (|) seperated
        /// string list.
        /// </summary>
        /// <param name="list">A pipe (|) seperated string representing 
        /// the list of string options to populate the list e.g Mr|Mrs|Dr </param>
        public void SetList(string list)
        {
            foreach (string item in list.Split('|'))
            {
                _comboBox.Items.Add(item);
            }
        }
    }
}