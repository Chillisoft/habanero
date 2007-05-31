using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Application.v2
{
    /// <summary>
    /// Manages a simple grid with buttons
    /// </summary>
    public class SimpleGridWithButtons : UserControl, IGridWithButtons
    {
        private SimpleGrid itsGrid;
        private EditableGridButtonControl itsButtons;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public SimpleGridWithButtons()
        {
            BorderLayoutManager manager = new BorderLayoutManager(this);
            itsGrid = new SimpleGrid();
            itsGrid.Name = "GridControl";
            manager.AddControl(itsGrid, BorderLayoutManager.Position.Centre);
            itsButtons = new EditableGridButtonControl(itsGrid);
            itsButtons.Name = "ButtonControl";
            manager.AddControl(itsButtons, BorderLayoutManager.Position.South);
        }

        /// <summary>
        /// Constructor to initialise a new grid with a data provider
        /// </summary>
        /// <param name="provider">The data provider</param>
        public SimpleGridWithButtons(IGridDataProvider provider) : this()
        {
            itsGrid.SetGridDataProvider(provider);
        }

        /// <summary>
        /// Returns the grid object
        /// </summary>
        public GridBase Grid
        {
            get { return itsGrid; }
        }

        /// <summary>
        /// Saves all changes made to the grid
        /// </summary>
        public void SaveChanges()
        {
            itsGrid.AcceptChanges();
        }

        /// <summary>
        /// Returns the button control
        /// </summary>
        public EditableGridButtonControl Buttons
        {
            get { return itsButtons; }
        }

        /// <summary>
        /// Returns the currently selected business object
        /// </summary>
        public BusinessObjectBase SelectedBusinessObject
        {
            get { return this.itsGrid.SelectedBusinessObject; }
        }
    }
}