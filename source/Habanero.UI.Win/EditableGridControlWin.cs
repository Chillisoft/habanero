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

        public IEditableGrid Grid
        {
            get { return _grid; }
        }

        public void Initialise(ClassDef classDef)
        {
            _editableGridManager.Initialise(classDef);
            
        }

        public void Initialise(ClassDef classDef, string uiDefName)
        {
            _editableGridManager.Initialise(classDef, uiDefName);
        }
    }
}