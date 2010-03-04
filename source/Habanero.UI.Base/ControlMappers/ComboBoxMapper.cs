// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Wraps/Decorates a <see cref="IComboBox"/> in order to display and capture a lookup property of the business object 
    /// </summary>
    public abstract class ComboBoxMapper : ControlMapper
    {
        /// <summary>
        /// The actual <see cref="IComboBox"/> control that is being mapped to the Business Object Property identified by PropertyName.
        /// </summary>
        protected IComboBox _comboBox;
        /// <summary>
        /// The actual <see cref="Dictionary{TKey,TValue}"/> of values that will be displayed in the combo box. This is a key value dictionary
        /// where the key contains the value that will be displayed in the ComboBox and the Value is the Unique identifier for the record.
        /// An object identifier (<see cref="IBusinessObject.ID"/> - <see cref="IPrimaryKey.ObjectID"/>) in the case of a <see cref="BusinessObjectLookupList"/>.
        /// or the Primary Key (field or composite fileds) in the case of a DatabaseLookupList or the key value for a <see cref="SimpleLookupList"/>.
        /// The Application developer can of course implement any other <see cref="ILookupList"/> that they require.
        /// </summary>
        protected Dictionary<string, string> _collection;
        /// <summary>
        /// A boolean to enable or disable right click handling for this <see cref="IComboBox"/>. Right click handling allows the 
        /// user to right click and from this a form to allow the editing of Values in the <see cref="IComboBox"/>. This is only applicable 
        /// by default to <see cref="BusinessObjectLookupList"/> or a custom <see cref="ILookupList"/> defined by the user.
        /// </summary>
        protected bool _rightClickEnabled;
//        / <summary>
//        / In the case of a <see cref="BusinessObjectLookupList"/> this will be the class def of the <see cref="IBusinessObject"/>s shown
//        / in the list.
//        / </summary>
//        protected ClassDef _lookupTypeClassDef;
        //protected ComboBoxRightClickController _comboBoxRightClickController;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="comboBox">The ComboBox object to which the property is mapped</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">The control factory to be used to create controls or strategies e.g. <see cref="IComboBoxMapperStrategy"/></param>
        protected ComboBoxMapper(IComboBox comboBox, string propName, bool isReadOnly, IControlFactory factory)
            : base(comboBox, propName, isReadOnly, factory)
        {
            _comboBox = comboBox;
            _rightClickEnabled = false;
        }

        //TODO: Port
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
        
//        /// <summary>
//        /// 
//        /// </summary>
//        protected override void OnBusinessObjectChanged()
//        {
//            //if (_comboBoxRightClickController == null && _businessObject != null)
//            //{
//            //    BOMapper mapper = new BOMapper(_businessObject);
//            //    _lookupTypeClassDef = mapper.GetLookupListClassDef(_propertyName);
//            //    _comboBoxRightClickController = new ComboBoxRightClickController(_comboBox, _lookupTypeClassDef);
//            //    _comboBoxRightClickController.NewObjectCreated += NewObjectCreated;
//            //}
//        }

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
        public abstract void SetupComboBoxItems();

//        /// <summary>
//        /// Gets and sets the lookup list used to populate the items in the
//        /// ComboBox.  This method is typically called by SetupLookupList().
//        /// </summary>
//        public abstract Dictionary<string, string> LookupList { set; get;}
//        /// <summary>
//        /// Sets the <see cref="Dictionary{TKey,TValue}"/> that is being used to fill this 
//        /// combo box with values.
//        /// </summary>
//        /// <param name="lookupList"></param>
//        [Obsolete("Use Lookuplist property")]
//        public abstract void SetupLookupList(Dictionary<string, string> lookupList);

    }
}