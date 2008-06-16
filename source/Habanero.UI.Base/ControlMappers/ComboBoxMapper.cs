//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This mapper represents a user interface ComboBox object
    /// </summary>
    public abstract class ComboBoxMapper : ControlMapper
    {
        protected IComboBox _comboBox;
        protected Dictionary<string, object> _collection;
        protected bool _rightClickEnabled;
        protected ClassDef _lookupTypeClassDef;
        //protected ComboBoxRightClickController _comboBoxRightClickController;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="comboBox">The ComboBox object to map</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
		public ComboBoxMapper(IComboBox comboBox, string propName, bool isReadOnly, IControlFactory factory)
            : base(comboBox, propName, isReadOnly, factory)
        {
            _comboBox = comboBox;
            _rightClickEnabled = false;
        }

        ///// <summary>
        ///// Gets or sets whether the user is able to right-click to
        ///// add additional items to the drop-down list
        ///// </summary>
        //public virtual bool RightClickEnabled
        //{
        //    get { return _rightClickEnabled; }
        //    set
        //    { 
        //        if (!_rightClickEnabled && value)
        //        {
        //            SetupRightClickBehaviour();
        //        }
        //        else if (_rightClickEnabled && !value)
        //        {
        //            DisableRightClickBehaviour();
        //        }
        //        _rightClickEnabled = value;
        //    }
        //}
        
        /////<summary>
        ///// The controller used to handle the right-click pop-up form behaviour
        /////</summary>
        //public ComboBoxRightClickController ComboBoxRightClickController
        //{
        //    get { return _comboBoxRightClickController; }
        //    set { _comboBoxRightClickController = value; }
        //}


        protected override void OnBusinessObjectChanged()
        {
            //if (_comboBoxRightClickController == null && _businessObject != null)
            //{
            //    BOMapper mapper = new BOMapper(_businessObject);
            //    _lookupTypeClassDef = mapper.GetLookupListClassDef(_propertyName);
            //    _comboBoxRightClickController = new ComboBoxRightClickController(_comboBox, _lookupTypeClassDef);
            //    _comboBoxRightClickController.NewObjectCreated += NewObjectCreated;
            //}
        }

        ///// <summary>
        ///// Sets up a handler so that right-clicking on the ComboBox will
        ///// allow the user to create a new business object using a form that is
        ///// provided.  A tooltip is also added to indicate this possibility to
        ///// the user.
        ///// </summary>
        //protected void SetupRightClickBehaviour()
        //{
        //    _comboBoxRightClickController.SetupRightClickBehaviour();
        //}

        ///// <summary>
        ///// When a new object is added to the combo-box, the string is
        ///// formatted correctly (especially to avoid duplication of items).  Because
        ///// of the complications of the different sorting options, any new items are
        ///// just added to the end of the list until the form is reloaded.
        ///// </summary>
        ///// <param name="businessObject"></param>
        //private void NewObjectCreated(BusinessObject businessObject)
        //{
        //    try
        //    {
        //        string newItem =
        //            BusinessObjectLookupList.GetAvailableDisplayValue(
        //            new ArrayList(_collection.Keys), businessObject.ToString());
        //        _collection.Add(newItem, businessObject);
        //        SetLookupList(_collection);
        //        _comboBox.SelectedItem = newItem;
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobalRegistry.UIExceptionNotifier.Notify(ex,
        //                                                  "There was an problem adding a new " +
        //                                                  _lookupTypeClassDef.ClassName + " to the list: ",
        //                                                  "Error adding");
        //    }
        //}

        ///// <summary>
        ///// Removes the handler that enables right-clicking on the ComboBox
        ///// </summary>
        //protected void DisableRightClickBehaviour()
        //{
        //    if (_comboBoxRightClickController != null)
        //    {
        //        _comboBoxRightClickController.DisableRightClickBehaviour();
        //    }
        //}

        /// <summary>
        /// Sets up the items to be listed in the ComboBox
        /// </summary>
        protected abstract void SetupComboBoxItems();

        /// <summary>
        /// This method is called by SetupLookupList() and populates the
        /// ComboBox with the collection of items provided
        /// </summary>
        /// <param name="value">The items used to populate the list</param>
        public abstract Dictionary<string, object> LookupList { set; get;}

    }
}