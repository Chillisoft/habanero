// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    ///<summary>
    /// Provides an implementation of a control for
    /// the interface <see cref="IBOCollapsiblePanelSelector"/> for a control that specialises 
    /// in showing a list of 
    /// Business Objects <see cref="IBusinessObjectCollection"/>.
    /// This control shows each business object in its own collapsible Panel.
    /// This is a very powerfull control for easily adding or viewing a fiew items E.g. for 
    /// a list of addresses for a person.
    ///</summary>
    public class CollapsiblePanelSelectorVWG : CollapsiblePanelGroupControlVWG, IBOCollapsiblePanelSelector
    {

        ///<summary>
        /// Constructor for <see cref="CollapsiblePanelSelectorVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<exception cref="NotImplementedException"></exception>
        public CollapsiblePanelSelectorVWG(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            this.ItemSelected += delegate { FireBusinessObjectSelected(); };
            this.AutoSelectFirstItem = true;
        }
        private IBusinessObjectCollection _businessObjectCollection;
        private readonly IControlFactory _controlFactory;
        /// <summary>
        /// Gets and Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command or from the
        /// <see cref="IBusinessObjectLoader"/>.
        /// The default UI definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return _businessObjectCollection; }
            set
            {
                UnregisterForEvents();
                ClearPanel();
                _businessObjectCollection = value;
                if (_businessObjectCollection == null) return;

                RegisterForEvents();


                if (!CollectionHasItems()) return;

                CreateBOCollapsiblePanels();
                SelectFirstItem();
            }
        }
        private bool CollectionHasItems()
        {
            return _businessObjectCollection != null && _businessObjectCollection.Count > 0;
        }
        private void SelectFirstItem()
        {
            if (AutoSelectFirstItem)
            {
                SelectedBusinessObject = _businessObjectCollection[0];
            }
        }
        private void ClearPanel()
        {
            SelectedBusinessObject = null;
            this.PanelsList.Clear();
            this.Controls.Clear();
        }
        private void UnregisterForEvents()
        {
            if (_businessObjectCollection == null) return;
            _businessObjectCollection.BusinessObjectAdded -= BusinessObjectAddedHandler;
            _businessObjectCollection.BusinessObjectRemoved -= BusinessObjectRemovedHandler;
        }

        private void RegisterForEvents()
        {
            if (_businessObjectCollection == null) return;
            _businessObjectCollection.BusinessObjectAdded += BusinessObjectAddedHandler;
            _businessObjectCollection.BusinessObjectRemoved += BusinessObjectRemovedHandler;
        }
        private void CreateBOCollapsiblePanels()
        {
            //TODO  02 Mar 2009: Need some serious testing here.
            foreach (IBusinessObject businessObject in _businessObjectCollection)
            {
                AddBOPanel(businessObject);
                //                    PanelBuilder builder = new PanelBuilder(_controlFactory);
                //                    UIForm form = ((ClassDef)businessObject.ClassDef).GetUIDef("default").UIForm;
                //                    IPanelInfo form1 = builder.BuildPanelForForm(form);
                //                    this.AddControl(form1.Panel, businessObject.ToString(), form1.Panel.Height);
            }
        }
        /// <summary>
        /// This handler is called when a business object has been added to
        /// the collection - it subsequently adds the item to the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            AddBOPanel(e.BusinessObject);
        }
        private void AddBOPanel(IBusinessObject businessObject)
        {
            IPanel panel = _controlFactory.CreatePanel();
            panel.Name = businessObject.ID.ObjectID.ToString();
            ICollapsiblePanel control = this.AddControl(panel, businessObject.ToString(), 100);
            control.Name = businessObject.ID.ObjectID.ToString();
        }
        /// <summary>
        /// This handler is called when a business object has been removed from
        /// the collection - it subsequently removes the item from the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            RemoveBOPanel(e.BusinessObject);
        }
        private void RemoveBOPanel(IBusinessObject businessObject)
        {
            ICollapsiblePanel panel = FindCollapsiblePanel(businessObject);
            this.Controls.Remove((Control)panel);
            this.PanelsList.Remove(panel);
        }
        private ICollapsiblePanel FindCollapsiblePanel(IBusinessObject businessObject)
        {
            if (businessObject == null) return null;
            foreach (ICollapsiblePanel panel in this.PanelsList)
            {
                if (panel.Name == businessObject.ID.ObjectID.ToString())
                {
                    return panel;
                }
            }
            return null;
        }
        private IBusinessObject _selectedBusinessObject;
        /// <summary>
        /// Gets and sets the currently selected business object in the selector
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get { return _selectedBusinessObject; }
            set
            {
                ICollapsiblePanel panel = FindCollapsiblePanel(value);
                if (panel == null)
                {
                    _selectedBusinessObject = null;
                    return;
                }
                panel.Collapsed = false;
                _selectedBusinessObject = value;
            }
        }

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;
        
        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        public void Clear()
        {
            BusinessObjectCollection = null;
        }

        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>Gets the number of rows displayed in the <see cref="IBOColSelectorControl"></see>.</summary>
        /// <returns>The number of rows in the <see cref="IBOColSelectorControl"></see>.</returns>
        public int NoOfItems
        {
            get { return this.PanelsList.Count; }
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            return IndexOutOfRange(row) ? null : BusinessObjectCollection[row];
        }

        private bool IndexOutOfRange(int row)
        {
            return row < 0 || row >= NoOfItems;
        }
        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem { get; set; }
    }
}