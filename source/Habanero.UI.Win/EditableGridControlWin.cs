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
            _editableGridManager = new EditableGridControlManager(this);
            _grid = _controlFactory.CreateEditableGrid();
        }

        public IGridBase Grid
        {
            get { return _grid; }
        }

        IControlCollection IControlChilli.Controls
        {
            get { throw new System.NotImplementedException(); }
        }

        public void Initialise(ClassDef classDef)
        {
            _editableGridManager.Initialise(classDef);
            
        }

        public void Initialise(ClassDef classDef, string uiDefName)
        {
            _editableGridManager.Initialise(classDef, uiDefName);
        }


        public string UiDefName
        {
            get { return _editableGridManager.UiDefName; }
            set { _editableGridManager.UiDefName = value; }
        }

        public ClassDef ClassDef
        {
            get { return _editableGridManager.ClassDef; }
            set { _editableGridManager.ClassDef = value; }
        }
    }
}