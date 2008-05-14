using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ReadOnlyGridWithButtonsGiz : ControlGiz, IReadOnlyGridWithButtons
    {
        private readonly IControlFactory _controlfactory;
        private readonly ReadOnlyGridGiz _grid;
        private readonly IReadOnlyGridButtonsControl _buttons;
        //private readonly GridSelectionController _gridSelectionController;

        //private IBusinessObjectCollection _collection;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public ReadOnlyGridWithButtonsGiz(IControlFactory controlfactory)
        {
            if (controlfactory == null) throw new ArgumentNullException("controlfactory");

            _controlfactory = controlfactory;
            //BorderLayoutManager manager = new BorderLayoutManager(this);
            _grid = new ReadOnlyGridGiz();
            _grid.Name = "GridControl";
            this.Controls.Add(_grid);
            //manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            _buttons = _controlfactory.CreateReadOnlyGridButtonsControl();
            _buttons.AddClicked += Buttons_AddClicked;
            _buttons.EditClicked += Buttons_EditClicked;
            //_buttons.Name = "ButtonControl";
            //manager.AddControl(_buttons, BorderLayoutManager.Position.South);

            //_gridSelectionController = new GridSelectionController(_grid);
            //_gridSelectionController.DelayedItemSelected = DelayedItemSelected;

            //this.Buttons.ObjectEditor = new DefaultBOEditor();
            //this.Buttons.ObjectCreator = new DefaultBOCreator(_provider.ClassDef);
        }

        private void Buttons_EditClicked(object sender, EventArgs e)
        {
            BusinessObject selectedBo = SelectedBusinessObject;
            if (selectedBo != null)
            {
                //TODO_Port: CheckEditorExists();
                IObjectEditor objectEditor = new DefaultBOEditor(_controlfactory);
                objectEditor.EditObject(selectedBo, "default");
                //				{
                //					_readOnlyGrid.RefreshRow(selectedBo) ;
                //				}
            }
        }

        void Buttons_AddClicked(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
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

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        public IReadOnlyGridButtonsControl Buttons
        {
            get { return _buttons; }
        }


        public void SetCollection(IBusinessObjectCollection boCollection)
        {
            _grid.SetCollection(boCollection);
            //this.Buttons.ObjectEditor = new DefaultBOEditor();
            //this.Buttons.ObjectCreator = new DefaultBOCreator(_collection.ClassDef);
        }
    }
}