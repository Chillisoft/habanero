using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ReadOnlyGridWithButtonsGiz : ControlGiz, IReadOnlyGridWithButtons
    {
        private readonly ReadOnlyGridGiz _grid;
        //private readonly ReadOnlyGridButtonControl _buttons;
        //private readonly GridSelectionController _gridSelectionController;

        private IBusinessObjectCollection _collection;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public ReadOnlyGridWithButtonsGiz()
        {
            //BorderLayoutManager manager = new BorderLayoutManager(this);
            _grid = new ReadOnlyGridGiz();
            _grid.Name = "GridControl";
            this.Controls.Add(_grid);
            //manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            //_buttons = new ReadOnlyGridButtonControl(_grid);
            //_buttons.Name = "ButtonControl";
            //manager.AddControl(_buttons, BorderLayoutManager.Position.South);

            //_gridSelectionController = new GridSelectionController(_grid);
            //_gridSelectionController.DelayedItemSelected = DelayedItemSelected;

            //this.Buttons.ObjectEditor = new DefaultBOEditor();
            //this.Buttons.ObjectCreator = new DefaultBOCreator(_provider.ClassDef);
        }
        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>
        public IReadOnlyGrid Grid
        {
            get { return _grid; }
        }
        /// <summary>
        /// Gets or sets the single selected business object (null if none are selected)
        /// denoted by where the current selected cell is
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return _grid.SelectedBusinessObject; }
            set { _grid.SelectedBusinessObject = value; }
        }
        ///// <summary>
        ///// Returns the button control held. This property can be used
        ///// to access a range of functionality for the button control
        ///// (eg. myGridWithButtons.Buttons.AddButton(...)).
        ///// </summary>
        //public ReadOnlyGridButtonControl Buttons
        //{
        //    get { return _buttons; }
        //}

        public void SetCollection(IBusinessObjectCollection col)
        {
            _grid.SetCollection(col);
            //this.Buttons.ObjectEditor = new DefaultBOEditor();
            //this.Buttons.ObjectCreator = new DefaultBOCreator(_collection.ClassDef);
        }
    }
}