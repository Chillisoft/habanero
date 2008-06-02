using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class EditableGridGiz : GridBaseGiz, IEditableGrid
    {
        public EditableGridGiz()
        {
            this.AllowUserToAddRows = true;
        }

        public override IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col)
        {
            return new EditableDataSetProvider(col);
        }
    }
}