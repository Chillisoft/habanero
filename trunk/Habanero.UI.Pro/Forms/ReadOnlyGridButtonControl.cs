using System;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Ui.Forms;
using Habanero.Ui.Grid;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Ui.Forms
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

        private IObjectEditor _objectEditor;
        private IObjectCreator _objectCreator;
        private RowDoubleClickedHandler _doubleClickedDelegate;

        /// <summary>
        /// Constructor to initialise a new button control
        /// </summary>
        /// <param name="readOnlyGrid">The read-only grid</param>
        public ReadOnlyGridButtonControl(IReadOnlyGrid readOnlyGrid)
        {
            this.AddButton("Add", new EventHandler(AddButtonClickHandler));
            this.AddButton("Edit", new EventHandler(EditButtonClickHandler));
            this._readOnlyGrid = readOnlyGrid;
            _doubleClickedDelegate = new RowDoubleClickedHandler(RowDoubleClickedHandler);
            this._readOnlyGrid.RowDoubleClicked += _doubleClickedDelegate;
        }

        /// <summary>
        /// Disables the default double-click behaviour
        /// </summary>
        public void DisableDefaultDoubleClickBehaviour()
        {
            this._readOnlyGrid.RowDoubleClicked -= _doubleClickedDelegate;
        }

        /// <summary>
        /// Handles the event of a user double-clicking on a row
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RowDoubleClickedHandler(object sender, BOEventArgs e)
        {
            if (e.BusinessObject != null)
            {
                if (_objectEditor == null)
                {
                    throw new NullReferenceException("There was an attempt to edit " +
                                                     "a business object when the object editor has not been " +
                                                     "set.  When the ReadOnlyGridWithButtons is instantiated, " +
                                                     "either use the single-parameter constructor that assigns a " +
                                                     "default editor or create a customised object editor and " +
                                                     "assign that through the appropriate constructor.");
                }

                _objectEditor.EditObject(e.BusinessObject);
            }
        }

        /// <summary>
        /// Handles the event of the "Edit" button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void EditButtonClickHandler(object sender, EventArgs e)
        {
            BusinessObject selectedBo = _readOnlyGrid.SelectedBusinessObject;
            if (selectedBo != null)
            {
                if (_objectEditor == null)
                {
                    throw new NullReferenceException("There was an attempt to edit " +
                                                     "a business object when the object editor has not been " +
                                                     "set.  When the ReadOnlyGridWithButtons is instantiated, " +
                                                     "either use the single-parameter constructor that assigns a " +
                                                     "default editor or create a customised object editor and " +
                                                     "assign that through the appropriate constructor.");
                }

                //if
                _objectEditor.EditObject(selectedBo);
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
            if (_objectCreator == null)
            {
                throw new NullReferenceException("There was an attempt to create " +
                                                 "a new business object when the object creator has not been " +
                                                 "set.  When the ReadOnlyGridWithButtons is instantiated, " +
                                                 "either use the single-parameter constructor that assigns a " +
                                                 "default creator or create a customised object creator and " +
                                                 "assign that through the appropriate constructor.");
            }

            BusinessObject newObject = (BusinessObject) _objectCreator.CreateObject(this._objectEditor);
            if (newObject != null)
            {
                _readOnlyGrid.SelectedBusinessObject = null;
                _readOnlyGrid.AddBusinessObject(newObject);
                _readOnlyGrid.SelectedBusinessObject = newObject;
            }
        }

        /// <summary>
        /// Sets the object editor.  This editor would typically be called to edit
        /// the currently selected object on the grid if such a provision is made 
        /// on the grid.
        /// </summary>
        public IObjectEditor ObjectEditor
        {
            set { _objectEditor = value; }
        }

        /// <summary>
        /// Sets the object creator.  This creator would typically be called to 
        /// add a business object if such a provision is made on the grid.
        /// </summary>
        public IObjectCreator ObjectCreator
        {
            set { _objectCreator = value; }
        }
    }
}