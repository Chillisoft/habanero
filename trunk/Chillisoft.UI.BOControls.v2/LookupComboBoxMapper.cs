using System;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// Maps the lookup ComboBox
    /// </summary>
    /// TODO ERIC - what's the dif btw these? what is lookup?
    public class LookupComboBoxMapper : ComboBoxMapper
    {

        /// <summary>
        /// Constructor to initialise the mapper
        /// </summary>
        /// <param name="cbx">The ComboBox to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether the object is read once only</param>
        public LookupComboBoxMapper(ComboBox cbx, string propName, bool isReadOnceOnly)
            : base(cbx, propName, isReadOnceOnly)
        {
            itsComboBox = cbx;
            //itsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            itsComboBox.SelectedIndexChanged += new EventHandler(ValueChangedHandler);
            itsComboBox.KeyPress += delegate(object sender, KeyPressEventArgs e)
                                        {
                                            if (e.KeyChar == 13)
                                            {
                                                ValueChangedHandler(sender, e);
                                            }
                                        };
        }

        //
        //		private void ComboBoxDoubleClickHandler(object sender, EventArgs e) {
        //
        //		}

        /// <summary>
        /// A handler to carry out changes to the business object when the
        /// value has changed in the user interface
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ValueChangedHandler(object sender, EventArgs e)
        {
            //log.Debug("ValueChanged in LookupComboBoxMapper") ;
            if (itsBusinessObject != null && itsComboBox.SelectedIndex != -1)
            {
                Guid newValue = ((StringGuidPair) itsComboBox.SelectedItem).Id;
                if (newValue.Equals(Guid.Empty))
                {
                    if (itsBusinessObject.GetPropertyValue(itsPropertyName) != null)
                    {
                        itsBusinessObject.SetPropertyValue(itsPropertyName, null);
                    }
                }
                else if (itsBusinessObject.GetPropertyValue(itsPropertyName) == null ||
                         !newValue.Equals(itsBusinessObject.GetPropertyValue(itsPropertyName)))
                {
                    itsBusinessObject.SetPropertyValue(itsPropertyName, newValue);
                }
            }
            //log.Debug("ValueChanged in LookupComboBoxMapper complete") ;
        }

        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void ValueUpdated()
        {
            if (itsCollection == null)
            {
                SetupLookupList();
            }
            else
            {
                if (itsBusinessObject.GetPropertyValue(itsPropertyName) == null)
                {
                    itsComboBox.SelectedIndex = -1;
                }
                else
                {
                    try
                    {
                        itsComboBox.SelectedItem =
                            itsCollection.FindByGuid((Guid) itsBusinessObject.GetPropertyValue(itsPropertyName));
                        itsComboBox.SelectionStart = 0;
                        itsComboBox.SelectionLength = 0;
                    }
                    catch (System.ObjectDisposedException)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Sets up the list of items to display and calls SetLookupList()
        /// to populate the ComboBox with this list
        /// </summary>
        private void SetupLookupList()
        {
            BOMapper mapper = new BOMapper(itsBusinessObject);
            StringGuidPairCollection col = mapper.GetLookupList(itsPropertyName);
            if (itsLookupTypeClassDef == null)
            {
                SetupRightClickBehaviour();
            }
            //if (col.Count == 0) {
            //throw new LookupListNotSetException();
            //} else {
            SetLookupList(col);
            //}
            if (col.Count > 0 && itsBusinessObject.GetPropertyValue(itsPropertyName) != null)
            {
                itsComboBox.SelectedItem =
                    itsCollection.FindByGuid((Guid) itsBusinessObject.GetPropertyValue(itsPropertyName));
                itsComboBox.SelectionStart = 0;
                itsComboBox.SelectionLength = 0;
            }
        }

        /// <summary>
        /// This method is called by SetupLookupList() and populates the
        /// ComboBox with the collection of items provided
        /// </summary>
        /// <param name="col">The items used to populate the list</param>
        public void SetLookupList(StringGuidPairCollection col)
        {
            int width = itsComboBox.Width;
            Label lbl = ControlFactory.CreateLabel("", false);
            itsCollection = col;
            itsComboBox.Items.Clear();
            itsComboBox.Items.Add(new StringGuidPair("", Guid.Empty));
            foreach (StringGuidPair pair in col)
            {
                lbl.Text = pair.Str;
                if (lbl.PreferredWidth > width)
                {
                    width = lbl.PreferredWidth;
                }
                itsComboBox.Items.Add(pair);
            }
            itsComboBox.DropDownWidth = width;
        }

        /// <summary>
        /// An overridden method from the parent that simply redirects to
        /// SetupLookupList()
        /// </summary>
        protected override void SetupComboBoxItems()
        {
            SetupLookupList();
        }
    }
}