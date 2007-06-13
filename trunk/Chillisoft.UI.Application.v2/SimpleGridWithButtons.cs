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
        /// Constructor to initialise a new grid with a data provider
        /// </summary>
        /// <param name="provider">The data provider</param>
        public SimpleGridWithButtons(IGridDataProvider provider) : this()
        {
            _grid.SetGridDataProvider(provider);
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