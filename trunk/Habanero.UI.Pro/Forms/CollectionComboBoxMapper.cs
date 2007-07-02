using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Ui.Base;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// This mapper represents a user interface ComboBox object that represents
    /// a business object collection
    /// </summary>
    /// TODO ERIC - need to compare more with ComboBoxMaper
    public class CollectionComboBoxMapper
    {
        private readonly ComboBox _comboBox;
        private BusinessObjectCollection _collection;
        private string _uiDefName;
        private MouseEventHandler _mouseClickHandler;

        /// <summary>
        /// Constructor to create a new collection ComboBox mapper object.
        /// Sets the UIDefName to "default".
        /// </summary>
        /// <param name="comboBox">The ComboBox object to map</param>
        public CollectionComboBoxMapper(ComboBox comboBox) : this(comboBox, "default")
        {
        }

        /// <summary>
        /// A constructor as before, but with the provision of a string name
        /// for the object
        /// </summary>
        public CollectionComboBoxMapper(ComboBox comboBox, string uiDefName)
        {
            _comboBox = comboBox;
            _uiDefName = uiDefName;
        }

        /// <summary>
        /// Sets the collection being represented to a specific collection
        /// of business objects
        /// </summary>
        /// <param name="collection">The collection to represent</param>
        /// <param name="includeBlank">Whether to a put a blank item at the
        /// top of the list</param>
        public void SetCollection(BusinessObjectCollection collection, bool includeBlank)
        {
            if (_collection != null)
            {
                _collection.BusinessObjectAdded -= new BusinessObjectEventHandler(BusinessObjectAddedHandler);
                _collection.BusinessObjectRemoved -= new BusinessObjectEventHandler(BusinessObjectRemovedHandler);
            }
            _collection = collection;
            SetComboBoxCollection(_comboBox, _collection, includeBlank);
//			_comboBox.Items.Clear();
//			foreach (BusinessObjectBase businessObjectBase in _collection) {
//				_comboBox.Items.Add(businessObjectBase);
//			}

            _collection.BusinessObjectAdded += new BusinessObjectEventHandler(BusinessObjectAddedHandler);
            _collection.BusinessObjectRemoved += new BusinessObjectEventHandler(BusinessObjectRemovedHandler);
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
        /// use the SetCollection method instead, which will call this method
        /// automatically.
        /// </summary>
        /// <param name="cbx">The ComboBox being mapped</param>
        /// <param name="col">The business object collection being represented</param>
        /// <param name="includeBlank">Whether to include a blank item at the
        /// top of the list</param>
        public static void SetComboBoxCollection(ComboBox cbx, BusinessObjectCollection col, bool includeBlank)
        {
            int width = cbx.Width;
            
            Label lbl = ControlFactory.CreateLabel("", false);
            cbx.Items.Clear();
            if (includeBlank)
            {
                cbx.Items.Add("");
            }
            foreach (BusinessObject businessObjectBase in col)
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
            BOMapper mapper = new BOMapper(_collection.SampleBo);
            if (mapper.GetUserInterfaceMapper(_uiDefName) != null)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(_comboBox, "Right click to add a new entry.");
                _mouseClickHandler = new MouseEventHandler(ComboBoxMouseUpHandler);
                _comboBox.MouseUp += _mouseClickHandler; 
            }
        }

        /// <summary>
        /// A handler to deal with the release of a mouse button on the
        /// ComboBox, allowing the user to add a new business object.
        /// See SetupRightClickBehaviour() for more detail.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ComboBoxMouseUpHandler(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            BusinessObject newBo = _collection.ClassDef.CreateNewBusinessObject();
            DefaultBOEditorForm form = new DefaultBOEditorForm(newBo, _uiDefName);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _collection.Add(newBo);
                _comboBox.SelectedItem = newBo;
            }
        }
    }
}