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
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.WebGUI
{
    /// <summary>
    /// Manages the buttons attached to a ReadOnlyGridWithButtons. By default,
    /// an "Add" and "Edit" button are added which allow the user to either add
    /// a new object or edit the currently selected object.  You can add other
    /// buttons with a command like: 
    /// "AddButton("buttonName", new EventHandler(handlerMethodToCall));".
    /// You can also manipulate the behaviour of this control by accessing it
    /// through the grid with an accessor like "myGrid.Buttons.someMethod".
    /// </summary>
    public class ReadOnlyGridButtonControl : ButtonControl
    {
        private readonly IReadOnlyGrid _readOnlyGrid;

        private IBusinessObjectEditor _BusinessObjectEditor;
        private IBusinessObjectCreator _BusinessObjectCreator;
        private IObjectInitialiser _objectInitialiser;
        //private RowDoubleClickedHandler _doubleClickedDelegate;
        private readonly Button _deleteButton;
        private bool _confirmDeletion;

        /// <summary>
        /// Constructor to initialise a new button control
        /// </summary>
        /// <param name="readOnlyGrid">The read-only grid</param>
        public ReadOnlyGridButtonControl(IReadOnlyGrid readOnlyGrid)
        {

            AddButton("Add", AddButtonClickHandler);
            AddButton("Edit", EditButtonClickHandler);
            _deleteButton = AddButton("Delete", DeleteButtonClickHandler);
            _deleteButton.Visible = false;
            _readOnlyGrid = readOnlyGrid;
            //_doubleClickedDelegate = new RowDoubleClickedHandler(RowDoubleClickedHandler);
            //this._readOnlyGrid.RowDoubleClicked += _doubleClickedDelegate;

            _confirmDeletion = true;
        }

        ///// <summary>
        ///// Disables the default double-click behaviour
        ///// </summary>
        //public void DisableDefaultDoubleClickBehaviour()
        //{
        //    this._readOnlyGrid.RowDoubleClicked -= _doubleClickedDelegate;
        //}

        ///// <summary>
        ///// Handles the event of a user double-clicking on a row
        ///// </summary>
        ///// <param name="sender">The object that notified of the event</param>
        ///// <param name="e">Attached arguments regarding the event</param>
        //private void RowDoubleClickedHandler(object sender, BOEventArgs e)
        //{
        //    if (e.BusinessObject != null)
        //    {
        //        CheckEditorExists();
        //        _BusinessObjectEditor.EditObject(e.BusinessObject, _readOnlyGrid.UIName);
        //    }
        //}

        /// <summary>
        /// Handles the event of the "Edit" button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void EditButtonClickHandler(object sender, EventArgs e)
        {
            IBusinessObject selectedBo = _readOnlyGrid.SelectedBusinessObject;
            if (selectedBo != null)
            {
                CheckEditorExists();
                //if
                _BusinessObjectEditor.EditObject(selectedBo, _readOnlyGrid.UIName);
                //				{
                //					_readOnlyGrid.RefreshRow(selectedBo) ;
                //				}
            }
        }

        /// <summary>
        /// Handles the event of the "Add" button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void AddButtonClickHandler(object sender, EventArgs e)
        {
            IBusinessObject newBO;
            if (this._BusinessObjectCreator == null)
            {
                newBO = _readOnlyGrid.BusinessObjectCollection.CreateBusinessObject();
            } else
            {
                newBO = (BusinessObject) this._BusinessObjectCreator.CreateBusinessObject();
            }

            _BusinessObjectEditor.EditObject(newBO, _readOnlyGrid.UIName);
            
//           CheckCreatorExists();
//            BusinessObject newObject = (BusinessObject)_BusinessObjectCreator.CreateBusinessObject(_BusinessObjectEditor, _objectInitialiser, _readOnlyGrid.UIName);
//            if (newObject != null)
//            {
//                _readOnlyGrid.SelectedBusinessObject = null;
//                _readOnlyGrid.AddBusinessObject(newObject);
//                _readOnlyGrid.SelectedBusinessObject = newObject;
//            }
        }

        /// <summary>
        /// Handles the event of the "Delete" button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DeleteButtonClickHandler(object sender, EventArgs e)
        {
            IList boCol = _readOnlyGrid.SelectedBusinessObjects;
            if (boCol.Count > 0)
            {
                //if (ConfirmDeletion && MessageBox.Show("Are you sure you want to delete the selected row(s)?",
                //    "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                //{
                //    return;
                //}

                while (boCol.Count > 0)
                {
                    IBusinessObject bo = (BusinessObject) boCol[0];
                    bo.Delete();
                    try
                    {
                        bo.Save();
                    }
                    catch (Exception ex)
                    {
                        GlobalRegistry.UIExceptionNotifier.Notify(ex,
                                                                  "A selected row could not be deleted.",
                                                                  "Deletion Error");
                        bo.Restore();
                        return;
                    }
                }
                _readOnlyGrid.SelectedBusinessObject = null;
               // _readOnlyGrid.SelectedBusinessObject = _readOnlyGrid.SelectedBusinessObject;
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the "Delete" button.  When
        /// visible, clicking the delete button will require "yes"
        /// confirmation from the user and will then delete the
        /// current row if possible.
        /// </summary>
        public bool ShowDefaultDeleteButton
        {
            get { return _deleteButton.Visible; }
            set { _deleteButton.Visible = value; }
        }

        /// <summary>
        /// Gets or sets the boolean value that determines whether to confirm
        /// deletion with the user when they have clicked the Delete button
        /// </summary>
        public bool ConfirmDeletion
        {
            get { return _confirmDeletion; }
            set { _confirmDeletion = value; }
        }

        /// <summary>
        /// Gets and sets the object editor, which is the control used to edit
        /// the selected business object
        /// </summary>
        public IBusinessObjectEditor BusinessObjectEditor
        {
            get { return _BusinessObjectEditor; }
            set { _BusinessObjectEditor = value; }
        }

        /// <summary>
        /// Gets and sets the object creator, which is the control used to create
        /// a new business object
        /// </summary>
        public IBusinessObjectCreator BusinessObjectCreator
        {
            get { return _BusinessObjectCreator; }
            set { _BusinessObjectCreator = value; }
        }

        /// <summary>
        /// Sets the object initialiser, which is used to initialise a business
        /// object when it is created
        /// </summary>
        public IObjectInitialiser ObjectInitialiser
        {
            get { return _objectInitialiser; }
            set { _objectInitialiser = value; }
        }

        ///// <summary>
        ///// Checks that the object creator has been defined and throws
        ///// an exception if not
        ///// </summary>
        //private void CheckCreatorExists()
        //{
        //    if (_BusinessObjectCreator == null)
        //    {
        //        throw new NullReferenceException("There was an attempt to create " +
        //                                         "a new business object when the object creator has not been " +
        //                                         "set.  When the ReadOnlyGridWithButtons is instantiated, " +
        //                                         "either use the single-parameter constructor that assigns a " +
        //                                         "default creator or create a customised object creator and " +
        //                                         "assign that through the appropriate constructor.");
        //    }
        //}

        /// <summary>
        /// Checks that the object editor has been defined and throws
        /// an exception if not
        /// </summary>
        private void CheckEditorExists()
        {
            if (_BusinessObjectEditor == null)
            {
                throw new NullReferenceException("There was an attempt to edit " +
                                                 "a business object when the object editor has not been " +
                                                 "set.  When the ReadOnlyGridWithButtons is instantiated, " +
                                                 "either use the single-parameter constructor that assigns a " +
                                                 "default editor or create a customised object editor and " +
                                                 "assign that through the appropriate constructor.");
            }
        }
    }
}