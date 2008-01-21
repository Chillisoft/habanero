using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Forms;
using Habanero.UI.Pro.Forms;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// This mapper represents a user interface ComboBox object
    /// </summary>
    public abstract class ComboBoxMapper : ControlMapper
    {
        protected ComboBox _comboBox;
        protected Dictionary<string, object> _collection;
        protected bool _rightClickEnabled;
        protected ClassDef _lookupTypeClassDef;
        protected ComboBoxRightClickController _comboBoxRightClickController;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="comboBox">The ComboBox object to map</param>
        /// <param name="propName">The property name</param>
		/// <param name="isReadOnly">Whether this control is read only</param>
		public ComboBoxMapper(ComboBox comboBox, string propName, bool isReadOnly)
            : base(comboBox, propName, isReadOnly)
        {
            Permission.Check(this);
            _comboBox = comboBox;
            _rightClickEnabled = false;
        }

        /// <summary>
        /// Gets or sets whether the user is able to right-click to
        /// add additional items to the drop-down list
        /// </summary>
        public virtual bool RightClickEnabled
        {
            get { return _rightClickEnabled; }
            set
            { 
                if (!_rightClickEnabled && value)
                {
                    SetupRightClickBehaviour();
                }
                else if (_rightClickEnabled && !value)
                {
                    DisableRightClickBehaviour();
                }
                _rightClickEnabled = value;
            }
        }
        
        ///<summary>
        /// The controller used to handle the right-click pop-up form behaviour
        ///</summary>
        public ComboBoxRightClickController ComboBoxRightClickController
        {
            get { return _comboBoxRightClickController; }
            set { _comboBoxRightClickController = value; }
        }


        protected override void OnBusinessObjectChanged()
        {
            if (_comboBoxRightClickController == null && _businessObject != null)
            {
                BOMapper mapper = new BOMapper(_businessObject);
                _lookupTypeClassDef = mapper.GetLookupListClassDef(_propertyName);
                _comboBoxRightClickController = new ComboBoxRightClickController(_comboBox, _lookupTypeClassDef);
                _comboBoxRightClickController.NewObjectCreated += NewObjectCreated;
            }
        }

        /// <summary>
        /// Sets up a handler so that right-clicking on the ComboBox will
        /// allow the user to create a new business object using a form that is
        /// provided.  A tooltip is also added to indicate this possibility to
        /// the user.
        /// </summary>
        protected void SetupRightClickBehaviour()
        {
            _comboBoxRightClickController.SetupRightClickBehaviour();
        }

        private void NewObjectCreated(BusinessObject businessObject)
        {
            try
            {
                SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
                foreach (KeyValuePair<string, object> keyValuePair in _collection)
                {
                    sortedDictionary.Add(keyValuePair.Key,keyValuePair.Value);
                }
                string newItem = businessObject.ToString();
                newItem = BusinessObjectLookupList.GetAvailableDisplayValue(sortedDictionary, newItem);
                sortedDictionary.Add(newItem, businessObject);
                Dictionary<string, object> dictionary = new Dictionary<string, object>(sortedDictionary.Count);
                foreach (KeyValuePair<string, object> keyValuePair in sortedDictionary)
                {
                    dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                }
                SetLookupList(dictionary);
                _comboBox.SelectedItem = newItem;
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex,
                                                          "There was an problem adding a new " +
                                                          _lookupTypeClassDef.ClassName + " to the list: ",
                                                          "Error adding");
            }
        }

        /// <summary>
        /// Removes the handler that enables right-clicking on the ComboBox
        /// </summary>
        protected void DisableRightClickBehaviour()
        {
            if (_comboBoxRightClickController != null)
            {
                _comboBoxRightClickController.DisableRightClickBehaviour();
            }
        }

        /// <summary>
        /// Sets up the items to be listed in the ComboBox
        /// </summary>
        protected abstract void SetupComboBoxItems();

        /// <summary>
        /// This method is called by SetupLookupList() and populates the
        /// ComboBox with the collection of items provided
        /// </summary>
        /// <param name="col">The items used to populate the list</param>
        public abstract void SetLookupList(Dictionary<string, object> col);
    }
}