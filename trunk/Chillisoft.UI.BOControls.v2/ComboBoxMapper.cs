using System.Drawing;
using System.Windows.Forms;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Misc.v2;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// This mapper represents a user interface ComboBox object
    /// </summary>
    public abstract class ComboBoxMapper : ControlMapper
    {
        protected ClassDef itsLookupTypeClassDef;
        protected ComboBox itsComboBox;
        protected StringGuidPairCollection itsCollection;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="comboBox">The ComboBox object to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnceOnly">Whether this object can be read once only</param>
        /// TODO ERIC - what's this last one, review all by similar name
        public ComboBoxMapper(ComboBox comboBox, string propName, bool isReadOnceOnly)
            : base(comboBox, propName, isReadOnceOnly)
        {
            itsComboBox = comboBox;
        }

        /// <summary>
        /// Sets up a handler so that right-clicking on the ComboBox will
        /// allow the user to create a new business object using a form that is
        /// provided.  A tooltip is also added to indicate this possibility to
        /// the user.
        /// </summary>
        protected void SetupRightClickBehaviour()
        {
            BOMapper mapper = new BOMapper(itsBusinessObject);
            itsLookupTypeClassDef = mapper.GetLookupListClassDef(itsPropertyName);
            if (itsLookupTypeClassDef != null)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(itsComboBox, "Right click to add a new entry.");
                itsComboBox.MouseUp += new MouseEventHandler(ComboBoxMouseUpHandler);
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
            BusinessObjectBase lookupBo = itsLookupTypeClassDef.CreateNewBusinessObject();
            BoPanelControl boCtl = new BoPanelControl(lookupBo, "");
            boCtl.Height = 180;
            boCtl.Width = 240;
            OKCancelDialog dialog =
                new OKCancelDialog(boCtl, "Add a new entry", itsComboBox.PointToScreen(new Point(0, 0)));
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    lookupBo.ApplyEdit();
                    SetupComboBoxItems();
                    itsComboBox.SelectedItem = lookupBo;
                }
                catch (BaseApplicationException ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex,
                                                              "There was an problem adding a new " +
                                                              itsLookupTypeClassDef.ClassName + " to the list: ",
                                                              "Error adding");
                }
            }
        }

        /// <summary>
        /// Sets up the items to be listed in the ComboBox
        /// </summary>
        protected abstract void SetupComboBoxItems();
    }
}