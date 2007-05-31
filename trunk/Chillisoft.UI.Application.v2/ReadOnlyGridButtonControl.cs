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
        private readonly IReadOnlyGrid itsReadOnlyGrid;

        private IObjectEditor itsObjectEditor;
        private IObjectCreator itsObjectCreator;
        private RowDoubleClickedHandler itsDoubleClickedDelegate;

        /// <summary>
        /// Constructor to initialise a new button control
        /// </summary>
        /// <param name="readOnlyGrid">The read-only grid</param>
        public ReadOnlyGridButtonControl(IReadOnlyGrid readOnlyGrid)
        {
            this.AddButton("Add", new EventHandler(AddButtonClickHandler));
            this.AddButton("Edit", new EventHandler(EditButtonClickHandler));
            this.itsReadOnlyGrid = readOnlyGrid;
            itsDoubleClickedDelegate = new RowDoubleClickedHandler(RowDoubleClickedHandler);
            this.itsReadOnlyGrid.RowDoubleClicked += itsDoubleClickedDelegate;
        }

        /// <summary>
        /// Disables the default double-click behaviour
        /// </summary>
        public void DisableDefaultDoubleClickBehaviour()
        {
            this.itsReadOnlyGrid.RowDoubleClicked -= itsDoubleClickedDelegate;
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
                itsObjectEditor.EditObject(e.BusinessObject);
            }
        }

        /// <summary>
        /// Handles the event of the "Edit" button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void EditButtonClickHandler(object sender, EventArgs e)
        {
            BusinessObjectBase selectedBo = itsReadOnlyGrid.SelectedBusinessObject;
            if (selectedBo != null)
            {
                //if
                itsObjectEditor.EditObject(selectedBo);
                //				{
                //					itsReadOnlyGrid.RefreshRow(selectedBo) ;
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
            BusinessObjectBase newObject = (BusinessObjectBase) itsObjectCreator.CreateObject(this.itsObjectEditor);
            if (newObject != null)
            {
                itsReadOnlyGrid.AddBusinessObject(newObject);
            }
        }

        /// <summary>
        /// Sets the object editor.  This editor would typically be called to edit
        /// an object if such a provision is made.
        /// </summary>
        public IObjectEditor ObjectEditor
        {
            set { itsObjectEditor = value; }
        }

        /// <summary>
        /// Sets the object creator.  This creator would typically be called to 
        /// add a business object if such a provision is made.
        /// </summary>
        public IObjectCreator ObjectCreator
        {
            set { itsObjectCreator = value; }
        }
    }
}