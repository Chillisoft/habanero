using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Base.LayoutManagers;

namespace Habanero.UI.WebGUI
{
    public class ReadOnlyGridWithButtonsGiz : ControlGiz, IReadOnlyGridWithButtons
    {
        private readonly IControlFactory _controlFactory;
        private readonly ReadOnlyGridGiz _grid;
        private readonly IReadOnlyGridButtonsControl _buttons;
        private IBusinessObjectEditor _businessObjectEditor;
        private IBusinessObjectCreator _businessObjectCreator;
        private IBusinessObjectDeletor _businessObjectDeletor;
        //private readonly GridSelectionController _gridSelectionController;

        //private IBusinessObjectCollection _collection;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public ReadOnlyGridWithButtonsGiz(IControlFactory controlfactory)
        {
            if (controlfactory == null) throw new ArgumentNullException("controlfactory");

            _controlFactory = controlfactory;
            BorderLayoutManager manager = new BorderLayoutManagerGiz(this, _controlFactory);
            _grid = new ReadOnlyGridGiz();
            _grid.Name = "GridControl";
            manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            
            _buttons = _controlFactory.CreateReadOnlyGridButtonsControl();
            _buttons.AddClicked += Buttons_AddClicked;
            _buttons.EditClicked += Buttons_EditClicked;
            _buttons.DeleteClicked += Buttons_DeleteClicked;
            _buttons.Name = "ButtonControl";
            manager.AddControl(_buttons, BorderLayoutManager.Position.South);

            //_gridSelectionController = new GridSelectionController(_grid);
            //_gridSelectionController.DelayedItemSelected = DelayedItemSelected;

            //this.Buttons.BusinessObjectEditor = new DefaultBOEditor();
            //this.Buttons.BusinessObjectCreator = new DefaultBOCreator(_provider.ClassDef);
        }

        private void Buttons_DeleteClicked(object sender, EventArgs e)
        {
            IBusinessObject selectedBo = SelectedBusinessObject;
            if (selectedBo != null)
            {
                try
                {
                    _businessObjectDeletor.DeleteBusinessObject(selectedBo);
                } catch (Exception ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem deleting", "Problem Deleting");
                }
            }
        }

        private void Buttons_EditClicked(object sender, EventArgs e)
        {
            IBusinessObject selectedBo = SelectedBusinessObject;
            if (selectedBo != null)
            {
                //TODO_Port: CheckEditorExists();
                _businessObjectEditor.EditObject(selectedBo, "default");
                //IBusinessObjectEditor objectEditor = new DefaultBOEditor(_controlFactory);
                //objectEditor.EditObject(selectedBo, "default");
                //				{
                //					_readOnlyGrid.RefreshRow(selectedBo) ;
                //				}
            }
        }

        void Buttons_AddClicked(object sender, EventArgs e)
        {
            IBusinessObject newBo = _businessObjectCreator.CreateBusinessObject();
            _businessObjectEditor.EditObject(newBo, "default");
            
                
            
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

        public IBusinessObjectEditor BusinessObjectEditor
        {
            get { return _businessObjectEditor; }
            set { _businessObjectEditor = value; }
        }

        public IBusinessObjectCreator BusinessObjectCreator
        {
            get { return _businessObjectCreator; }
            set { _businessObjectCreator = value; }
        }

        public IBusinessObjectDeletor BusinessObjectDeletor
        {
            get { return _businessObjectDeletor; }
            set { _businessObjectDeletor = value; }
        }


        public void SetCollection(IBusinessObjectCollection boCollection)
        {
            _grid.SetCollection(boCollection);
            this.BusinessObjectEditor = new DefaultBOEditor(_controlFactory);
            this.BusinessObjectCreator = new DefaultBOCreator(boCollection.ClassDef);
            this.BusinessObjectDeletor = new DefaultBODeletor();
        }
    }
}