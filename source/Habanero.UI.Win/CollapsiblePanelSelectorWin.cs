using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    ///<summary>
    /// Provides an implementation of the <see cref="IBOCollapsiblePanelSelector"/> an interface 
    /// for a control that specialises in showing a list of 
    /// Business Objects <see cref="IBusinessObjectCollection"/>.
    /// This control shows each business object in its own collapsible Panel.
    /// This is a very powerfull control for easily adding or viewing a fiew items E.g. for 
    /// a list of addresses for a person.
    ///</summary>
    public class CollapsiblePanelSelectorWin : CollapsiblePanelGroupControlWin, IBOCollapsiblePanelSelector
    {
        private readonly IControlFactory _controlFactory;

        ///<summary>
        /// Constructor for <see cref="CollapsiblePanelSelectorWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<exception cref="NotImplementedException"></exception>
        public CollapsiblePanelSelectorWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            this.ItemSelected += delegate { FireBusinessObjectSelected(); };
            this.AutoSelectFirstItem = true;
        }

        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        private IBusinessObjectCollection _businessObjectCollection;

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

        private void SelectFirstItem()
        {
            if (AutoSelectFirstItem)
            {
                SelectedBusinessObject = _businessObjectCollection[0]; 
            }  
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

        private void ClearPanel()
        {
            SelectedBusinessObject = null;
            this.PanelsList.Clear();
            this.Controls.Clear();
        }

        private void RegisterForEvents()
        {
            if (_businessObjectCollection == null) return;
            _businessObjectCollection.BusinessObjectAdded += BusinessObjectAddedHandler;
            _businessObjectCollection.BusinessObjectRemoved += BusinessObjectRemovedHandler;
        }

        private void UnregisterForEvents()
        {
            if (_businessObjectCollection == null) return;
            _businessObjectCollection.BusinessObjectAdded -= BusinessObjectAddedHandler;
            _businessObjectCollection.BusinessObjectRemoved -= BusinessObjectRemovedHandler;
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
            this.Controls.Remove((Control) panel);
            this.PanelsList.Remove(panel);
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

        private bool CollectionHasItems()
        {
            return _businessObjectCollection != null && _businessObjectCollection.Count > 0;
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

//        private void FireBusinessObjectSelected()
//        {
//            if (this.BusinessObjectSelected != null)
//            {
//                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
//            }
//        }
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

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        /// <summary>
        /// Clears the business object collection and the rows in the selector
        /// </summary>
        public void Clear()
        {
            BusinessObjectCollection = null;
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

        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem { get; set; }

        private bool IndexOutOfRange(int row)
        {
            return row < 0 || row >= NoOfItems;
        }
    }
}