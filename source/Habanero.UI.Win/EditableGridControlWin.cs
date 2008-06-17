using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Base.Grid;

namespace Habanero.UI.Win
{
    public class EditableGridControlWin : UserControlWin, IEditableGridControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly IEditableGrid _grid;
        private readonly EditableGridControlManager _editableGridManager;

        public EditableGridControlWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _editableGridManager = new EditableGridControlManager(this, controlFactory);
            _grid = _controlFactory.CreateEditableGrid();
            BorderLayoutManager manager = controlFactory.CreateBorderLayoutManager(this);
            manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
        }

        public IGridBase Grid
        {
            get { return _grid; }
        }

        public void Initialise(IClassDef classDef)
        {
            _editableGridManager.Initialise(classDef);
            
        }

        public void Initialise(IClassDef classDef, string uiDefName)
        {
            _editableGridManager.Initialise(classDef, uiDefName);
        }


        public string UiDefName
        {
            get { return _editableGridManager.UiDefName; }
            set { _editableGridManager.UiDefName = value; }
        }

        public IClassDef ClassDef
        {
            get { return _editableGridManager.ClassDef; }
            set { _editableGridManager.ClassDef = value; }
        }
    }
}