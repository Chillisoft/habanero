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
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// This mapper represents a user interface ComboBox object
    /// </summary>
    public abstract class ComboBoxMapper : ControlMapper
    {
        protected ClassDef _lookupTypeClassDef;
        protected ComboBox _comboBox;
        protected Dictionary<string, object> _collection;
        protected bool _rightClickEnabled;

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
            if (_lookupTypeClassDef != null && _lookupTypeClassDef.UIDefCol["default"].UIForm != null)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(_comboBox, "Right click to add a new entry.");
                _comboBox.MouseUp += new MouseEventHandler(ComboBoxMouseUpHandler);
            }
        }

        /// <summary>
        /// Removes the handler that enables right-clicking on the ComboBox
        /// </summary>
        protected void DisableRightClickBehaviour()
        {
            //BOMapper mapper = new BOMapper(_businessObject);
            //_lookupTypeClassDef = mapper.GetLookupListClassDef(_propertyName);
            //if (_lookupTypeClassDef != null)
            //{
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(_comboBox, "");
                _comboBox.MouseUp -= ComboBoxMouseUpHandler;
            //}
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
            boCtl.Height = _lookupTypeClassDef.UIDefCol["default"].UIForm.Height;
            boCtl.Width = _lookupTypeClassDef.UIDefCol["default"].UIForm.Width;
            OKCancelDialog dialog =
                new OKCancelDialog(boCtl, "Add a new entry", _comboBox.PointToScreen(new Point(0, 0)));
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ArrayList originalCol = new ArrayList();
                    foreach (object item in _comboBox.Items)
                    {
                        originalCol.Add(item.ToString());
                    }
                    lookupBo.Save();
                    SetupComboBoxItems();

                    string newItem = lookupBo.ToString();
                    foreach (object item in _comboBox.Items)
                    {
                        if (!originalCol.Contains(item))
                        {
                            newItem = item.ToString();
                            break;
                        }
                    }
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
        }

        /// <summary>
        /// Sets up the items to be listed in the ComboBox
        /// </summary>
        protected abstract void SetupComboBoxItems();
    }
}