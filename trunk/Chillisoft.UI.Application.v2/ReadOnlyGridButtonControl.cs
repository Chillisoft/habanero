using System;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Application.v2
{
    /// <summary>
    /// Manages buttons in a grid that cannot be directly edited (although
    /// other means such as an "Edit" and "Add" button can be used to edit the
    /// data)
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
        private void RowDoubleClickedHandler(object sender, BusinessObjectEventArgs e)
        {
            if (e.BusinessObject != null)
            {
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
            BusinessObjectBase selectedBo = _readOnlyGrid.SelectedBusinessObject;
            if (selectedBo != null)
            {
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
            BusinessObjectBase newObject = (BusinessObjectBase) _objectCreator.CreateObject(this._objectEditor);
            if (newObject != null)
            {
                _readOnlyGrid.AddBusinessObject(newObject);
            }
        }

        /// <summary>
        /// Sets the object editor.  This editor would typically be called to edit
        /// an object if such a provision is made.
        /// </summary>
        public IObjectEditor ObjectEditor
        {
            set { _objectEditor = value; }
        }

        /// <summary>
        /// Sets the object creator.  This creator would typically be called to 
        /// add a business object if such a provision is made.
        /// </summary>
        public IObjectCreator ObjectCreator
        {
            set { _objectCreator = value; }
        }
    }
}