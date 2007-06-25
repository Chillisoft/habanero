using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Generic;
using Habanero.Ui.Forms;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// This mapper represents a user interface ComboBox object
    /// </summary>
    public abstract class ComboBoxMapper : ControlMapper
    {
        protected ClassDef _lookupTypeClassDef;
        protected ComboBox _comboBox;
        protected StringGuidPairCollection _collection;

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
            _comboBox = comboBox;
        }

        /// <summary>
        /// Sets up a handler so that right-clicking on the ComboBox will
        /// allow the user to create a new business object using a form that is
        /// provided.  A tooltip is also added to indicate this possibility to
        /// the user.
        /// </summary>
        protected void SetupRightClickBehaviour()
        {
            BOMapper mapper = new BOMapper(_businessObject);
            _lookupTypeClassDef = mapper.GetLookupListClassDef(_propertyName);
            if (_lookupTypeClassDef != null)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(_comboBox, "Right click to add a new entry.");
                _comboBox.MouseUp += new MouseEventHandler(ComboBoxMouseUpHandler);
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
            BusinessObject lookupBo = _lookupTypeClassDef.CreateNewBusinessObject();
            BoPanelControl boCtl = new BoPanelControl(lookupBo, "");
            boCtl.Height = 180;
            boCtl.Width = 240;
            OKCancelDialog dialog =
                new OKCancelDialog(boCtl, "Add a new entry", _comboBox.PointToScreen(new Point(0, 0)));
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    lookupBo.ApplyEdit();
                    SetupComboBoxItems();
                    _comboBox.SelectedItem = lookupBo;
                }
                catch (Exception ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex,
                                                              "There was an problem adding a new " +
                                                              _lookupTypeClassDef.ClassName + " to the list: ",
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