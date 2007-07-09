using System.Windows.Forms;
using Habanero.Bo;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;
using Habanero.Ui.Grid;

namespace Habanero.Ui.Grid
{
    /// <summary>
    /// Manages a simple grid with buttons
    /// </summary>
    public class SimpleGridWithButtons : UserControl, IGridWithButtons
    {
        private SimpleGrid _grid;
        private EditableGridButtonControl _buttons;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public SimpleGridWithButtons()
        {
            BorderLayoutManager manager = new BorderLayoutManager(this);
            _grid = new SimpleGrid();
            _grid.Name = "GridControl";
            manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            _buttons = new EditableGridButtonControl(_grid);
            _buttons.Name = "ButtonControl";
            manager.AddControl(_buttons, BorderLayoutManager.Position.South);
        }

        /// <summary>
        /// Sets the collection of this grid, populating the grid with the collection of business objects
        /// using the ui specified to format the columns.
        /// </summary>
        /// <param name="boCol">The collection to populate the grid with</param>
        /// <param name="uiName">The ui to use</param>
        public void SetCollection(BusinessObjectCollection<BusinessObject> boCol, string uiName)
        {
            _grid.SetCollection(boCol, uiName);
        }

        /// <summary>
        /// Sets the collection of this grid, populating the grid with the collection of business objects
        /// using the default ui.
        /// </summary>
        /// <param name="boCol">The collection to populate the grid with</param>
        public void SetCollection(BusinessObjectCollection<BusinessObject> boCol)
        {
            _grid.SetCollection(boCol, "default");
        }

        /// <summary>
        /// Returns the grid object
        /// </summary>
        public GridBase Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Saves all changes made to the grid
        /// </summary>
        public void SaveChanges()
        {
            _grid.AcceptChanges();
        }

        /// <summary>
        /// Returns the button control
        /// </summary>
        public EditableGridButtonControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Returns the currently selected business object
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return this._grid.SelectedBusinessObject; }
        }
    }
}