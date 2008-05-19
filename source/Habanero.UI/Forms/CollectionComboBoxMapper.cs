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


using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// This class provides mapping from a business object collection to a
    /// user interface ComboBox.  This mapper is used at code level when
    /// you are explicitly providing a business object collection.
    /// </summary>
    public class CollectionComboBoxMapper
    {
        private readonly ComboBox _comboBox;
		private IBusinessObjectCollection _collection;
        private MouseEventHandler _mouseClickHandler;
        private ComboBoxRightClickController _comboBoxRightClickController;


        /// <summary>
        /// Constructor to create a new collection ComboBox mapper object.
        /// </summary>
        /// <param name="comboBox">The ComboBox object to map</param>
        public CollectionComboBoxMapper(ComboBox comboBox)
        {
            _comboBox = comboBox;
        }

        /// <summary>
        /// Sets the collection being represented to a specific collection
        /// of business objects
        /// </summary>
        /// <param name="collection">The collection to represent</param>
        /// <param name="includeBlank">Whether to a put a blank item at the
        /// top of the list</param>
		public void SetCollection(IBusinessObjectCollection collection, bool includeBlank)
        {
            if (_collection != null)
            {
                _collection.BusinessObjectAdded -= new EventHandler<BOEventArgs>(BusinessObjectAddedHandler);
                _collection.BusinessObjectRemoved -= new EventHandler<BOEventArgs>(BusinessObjectRemovedHandler);
            }
            _collection = collection;
            SetupComboBoxRightClickController();
            SetComboBoxCollection(_comboBox, _collection, includeBlank);
//			_comboBox.Items.Clear();
//			foreach (BusinessObjectBase businessObjectBase in _collection) {
//				_comboBox.Items.Add(businessObjectBase);
//			}

            _collection.BusinessObjectAdded += new EventHandler<BOEventArgs>(BusinessObjectAddedHandler);
            _collection.BusinessObjectRemoved += new EventHandler<BOEventArgs>(BusinessObjectRemovedHandler);
        }

        private void SetupComboBoxRightClickController()
        {
            _comboBoxRightClickController = new ComboBoxRightClickController(_comboBox, _collection.ClassDef);
            _comboBoxRightClickController.NewObjectCreated += NewComboBoxObjectCreated;
        }

        private void NewComboBoxObjectCreated(IBusinessObject businessObject)
        {
            _collection.Add(businessObject);
            _comboBox.SelectedItem = businessObject;
        }

        ///<summary>
        /// The controller used to handle the right-click pop-up form behaviour
        ///</summary>
        public ComboBoxRightClickController ComboBoxRightClickController
        {
            get { return _comboBoxRightClickController; }
            set { _comboBoxRightClickController = value; }
        }

        /// <summary>
        /// This handler is called when a business object has been removed from
        /// the collection - it subsequently removes the item from the ComboBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            _comboBox.Items.Remove(e.BusinessObject);
        }

        /// <summary>
        /// This handler is called when a business object has been added to
        /// the collection - it subsequently adds the item to the ComboBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            _comboBox.Items.Add(e.BusinessObject);
        }

        /// <summary>
        /// Returns the business object, in object form, that is currently 
        /// selected in the ComboBox list
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get
            {
                //if (_comboBox.SelectedText == "")
                //{
                //    return null;
                //}
                //else
                //{
                if (_comboBox.SelectedIndex == -1)
                {
                    return null;
                } else if (_comboBox.SelectedItem is string && (_comboBox.SelectedItem == null || (string)_comboBox.SelectedItem == ""))
                {
                    return null;
                }
                else
                {
                    return (BusinessObject)_comboBox.SelectedItem;
                }
                //}
            }
        }

        /// <summary>
        /// Set the list of objects in the ComboBox to a specific collection of
        /// business objects.<br/>
        /// NOTE: If you are changing the business object collection,
        /// use the SetBusinessObjectCollection method instead, which will call this method
        /// automatically.
        /// </summary>
        /// <param name="cbx">The ComboBox being mapped</param>
        /// <param name="col">The business object collection being represented</param>
        /// <param name="includeBlank">Whether to include a blank item at the
        /// top of the list</param>
		public static void SetComboBoxCollection(ComboBox cbx, IBusinessObjectCollection col, bool includeBlank)
        {
            int width = cbx.Width;
            
            Label lbl = ControlFactory.CreateLabel("", false);
            cbx.Items.Clear();
            if (includeBlank)
            {
                cbx.Items.Add("");
            }
            foreach (IBusinessObject businessObjectBase in col)
            {
                lbl.Text = businessObjectBase.ToString();
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                cbx.Items.Add(businessObjectBase);
            }
            cbx.DropDownWidth = width;
        }

        /// <summary>
        /// Sets up a handler so that right-clicking on the ComboBox will
        /// allow the user to create a new business object using a form that is
        /// provided.  A tooltip is also added to indicate this possibility to
        /// the user.
        /// </summary>
        public void SetupRightClickBehaviour()
        {
            _comboBoxRightClickController.SetupRightClickBehaviour();
        }
        
    }
}