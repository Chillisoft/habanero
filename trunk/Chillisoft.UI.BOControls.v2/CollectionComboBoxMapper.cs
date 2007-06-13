using System;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// This mapper represents a user interface ComboBox object that represents
    /// a business object collection
    /// </summary>
    /// TODO ERIC - need to compare more with ComboBoxMaper
    public class CollectionComboBoxMapper
    {
        private readonly ComboBox itsComboBox;
        private BusinessObjectBaseCollection itsCollection;
        private string itsUIDefName;
        private MouseEventHandler itsMouseClickHandler;

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
            itsComboBox = comboBox;
            itsUIDefName = uiDefName;
        }

        /// <summary>
        /// Sets the collection being represented to a specific collection
        /// of business objects
        /// </summary>
        /// <param name="collection">The collection to represent</param>
        /// <param name="includeBlank">Whether to a put a blank item at the
        /// top of the list</param>
        public void SetCollection(BusinessObjectBaseCollection collection, bool includeBlank)
        {
            if (itsCollection != null)
            {
                itsCollection.BusinessObjectAdded -= new BusinessObjectEventHandler(BusinessObjectAddedHandler);
                itsCollection.BusinessObjectRemoved -= new BusinessObjectEventHandler(BusinessObjectRemovedHandler);
            }
            itsCollection = collection;
            SetComboBoxCollection(itsComboBox, itsCollection, includeBlank);
//			_comboBox.Items.Clear();
//			foreach (BusinessObjectBase businessObjectBase in _collection) {
//				itsComboBox.Items.Add(businessObjectBase);
//			}

            itsCollection.BusinessObjectAdded += new BusinessObjectEventHandler(BusinessObjectAddedHandler);
            itsCollection.BusinessObjectRemoved += new BusinessObjectEventHandler(BusinessObjectRemovedHandler);
        }

        /// <summary>
        /// This handler is called when a business object has been removed from
        /// the collection - it subsequently removes the item from the ComboBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BusinessObjectEventArgs e)
        {
            itsComboBox.Items.Remove(e.BusinessObject);
        }

        /// <summary>
        /// This handler is called when a business object has been added to
        /// the collection - it subsequently adds the item to the ComboBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BusinessObjectEventArgs e)
        {
            itsComboBox.Items.Add(e.BusinessObject);
        }

        /// <summary>
        /// Returns the business object, in object form, that is currently 
        /// selected in the ComboBox list
        /// </summary>
        public BusinessObjectBase SelectedBusinessObject
        {
            get
            {
                //if (itsComboBox.SelectedText == "")
                //{
                //    return null;
                //}
                //else
                //{
                if (itsComboBox.SelectedIndex == -1)
                {
                    return null;
                } else if (itsComboBox.SelectedItem is string && (itsComboBox.SelectedItem == null || (string)itsComboBox.SelectedItem == ""))
                {
                    return null;
                }
                else
                {
                    return (BusinessObjectBase)itsComboBox.SelectedItem;
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
        public static void SetComboBoxCollection(ComboBox cbx, BusinessObjectBaseCollection col, bool includeBlank)
        {
            int width = cbx.Width;
            
            Label lbl = ControlFactory.CreateLabel("", false);
            cbx.Items.Clear();
            if (includeBlank)
            {
                cbx.Items.Add("");
            }
            foreach (BusinessObjectBase businessObjectBase in col)
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
        /// As above, but specifies the list of items in the ComboBox with a 
        /// set of string-Guid pairs
        /// </summary>
        public static void SetComboBoxCollection(ComboBox cbx, StringGuidPairCollection col, bool includeBlank)
        {
            int width = cbx.Width;
            ;
            Label lbl = ControlFactory.CreateLabel("", false);
            cbx.Items.Clear();
            if (includeBlank)
            {
                cbx.Items.Add("");
            }
            foreach (StringGuidPair stringguidpair in col)
            {
                lbl.Text = stringguidpair.Str;
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                cbx.Items.Add(stringguidpair);
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
            BOMapper mapper = new BOMapper(itsCollection.SampleBo);
            if (mapper.GetUserInterfaceMapper(itsUIDefName) != null)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(itsComboBox, "Right click to add a new entry.");
                itsMouseClickHandler = new MouseEventHandler(ComboBoxMouseUpHandler);
                itsComboBox.MouseUp += itsMouseClickHandler; 
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
            BusinessObjectBase newBo = itsCollection.ClassDef.CreateNewBusinessObject();
            DefaultBOEditorForm form = new DefaultBOEditorForm(newBo, itsUIDefName);
            if (form.ShowDialog() == DialogResult.OK)
            {
                itsCollection.Add(newBo);
                itsComboBox.SelectedItem = newBo;
            }
        }
    }
}