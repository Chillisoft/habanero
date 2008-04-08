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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Forms;

namespace Habanero.UI.Forms
{
    ///<summary>
    /// This controller sets up the right-click pop-up form behaviour for a combo box.
    ///</summary>
    public class ComboBoxRightClickController
    {
        ///<summary>
        /// This delegate returns the object that has just been created.
        ///</summary>
        ///<param name="businessObject">The object that has just been created</param>
        public delegate void NewObjectCreatedDelegate(BusinessObject businessObject);

        /// <summary>
        /// This event is fried when a new object has been created and saved using the right-click popup form. 
        /// </summary>
        public event NewObjectCreatedDelegate NewObjectCreated;

        private ClassDef _lookupTypeClassDef;
        private ComboBox _comboBox;
        private string _uiDefName;
        private IObjectCreator _objectCreator;
        private IObjectEditor _objectEditor;
        private IObjectInitialiser _objectInitialiser;
        
        ///<summary>
        /// Creates a new combo box right-click controller, which pops up a screen for the user 
        /// to create a new item to add to the combo box list.
        ///</summary>
        ///<param name="comboBox">The combo box for which to enable this feature</param>
        ///<param name="lookupTypeClassDef">The class definition of the class that the combo box represents</param>
        public ComboBoxRightClickController(ComboBox comboBox, ClassDef lookupTypeClassDef)
        {
            _lookupTypeClassDef = lookupTypeClassDef;
            _comboBox = comboBox;
            _objectCreator = new DefaultBOCreator(_lookupTypeClassDef);
            _objectEditor = new DefaultBOEditor();
            _objectInitialiser = null; 
        }
        
        ///<summary>
        /// The name of the UI Definition to use for the pop-up form
        ///</summary>
        public string UiDefName
        {
            get { return _uiDefName; }
            set { _uiDefName = value; }
        }
        
        ///<summary>
        /// The Object Creator that will be used to create a new object for this combo box
        ///</summary>
        public IObjectCreator ObjectCreator
        {
            get { return _objectCreator; }
            set { _objectCreator = value; }
        }

        ///<summary>
        /// The Object Editor that will be used to edit the newly created object
        ///</summary>
        public IObjectEditor ObjectEditor
        {
            get { return _objectEditor; }
            set { _objectEditor = value; }
        }

        /// <summary>
        /// The Object Initialiser used to initialise the object when it is being created
        /// </summary>
        public IObjectInitialiser ObjectInitialiser
        {
            get { return _objectInitialiser; }
            set { _objectInitialiser = value; }
        }

        /// <summary>
        /// Sets up a handler so that right-clicking on the ComboBox will
        /// allow the user to create a new business object using a form that is
        /// provided.  A tooltip is also added to indicate this possibility to
        /// the user.
        /// </summary>
        public virtual void SetupRightClickBehaviour()
        {
            if (_lookupTypeClassDef != null && _lookupTypeClassDef.UIDefCol.Contains(GetUiDefName()) &&
                _lookupTypeClassDef.UIDefCol[GetUiDefName()].UIForm != null)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(_comboBox, "Right click to add a new entry.");
                _comboBox.MouseUp += ComboBoxMouseUpHandler;
            }
        }

        private string GetUiDefName()
        {
            string uiDefName = _uiDefName;
            if (String.IsNullOrEmpty(uiDefName))
            {
                uiDefName = "default";
            }
            return uiDefName;
        }

        /// <summary>
        /// Removes the handler that enables right-clicking on the ComboBox
        /// </summary>
        public virtual void DisableRightClickBehaviour()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(_comboBox, "");
            _comboBox.MouseUp -= ComboBoxMouseUpHandler;
        }

        /// <summary>
        /// A handler to deal with the release of a mouse button on the
        /// ComboBox, allowing the user to add a new business object.
        /// See SetupRightClickBehaviour() for more detail.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected virtual void ComboBoxMouseUpHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                OnComboBoxRightClick();
            }
        }

        /// <summary>
        /// A handler to deal with a right mouse click on the
        /// ComboBox, allowing the user to add a new business object.
        /// See SetupRightClickBehaviour() for more detail.
        /// </summary>
        protected virtual void OnComboBoxRightClick()
        {
            BusinessObject newBo;
            if (OpenCreateDialog(out newBo))
            {
                if (NewObjectCreated != null)
                {
                    NewObjectCreated(newBo);
                }
            }
        }

        private bool OpenCreateDialog(out BusinessObject newBo)
        {
            try
            {
                if (_objectCreator != null && _objectEditor != null)
                {
                    newBo = (BusinessObject)_objectCreator.CreateObject(_objectEditor, _objectInitialiser, GetUiDefName());
                    return newBo != null;
                }
                else
                {
                    newBo = _lookupTypeClassDef.CreateNewBusinessObject();
                    BoPanelControl boCtl = new BoPanelControl(newBo, GetUiDefName());
                    UIForm uiForm = _lookupTypeClassDef.UIDefCol[GetUiDefName()].UIForm;
                    boCtl.Height = uiForm.Height;
                    boCtl.Width = uiForm.Width;
                    OKCancelDialog dialog =
                        new OKCancelDialog(boCtl, "Add a new entry", _comboBox.PointToScreen(new Point(0, 0)));
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        newBo.Save();
                        return true;
                    }
                    else
                    {
                        newBo = null;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "Cannot create a new item for the combo box", "Error");
                newBo = null;
                return false;
            }
        }
    }
}
