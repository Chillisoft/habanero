using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using MachineExample.BO;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace MachineExample.UI
{
    public class MachinePropertiesControl : UserControlWin, IFormControl
    {
        public MachinePropertiesControl(IControlFactory factory)
        {
            BorderLayoutManager manager = factory.CreateBorderLayoutManager(this);
            IReadOnlyGridControl grid = factory.CreateReadOnlyGridControl();
            grid.Buttons.Visible = false;
            grid.FilterControl.Visible = false;
            grid.SetBusinessObjectCollection(Broker.GetBusinessObjectCollection<MachineProperty>(""));
            
            ILabel label = factory.CreateLabel(
                    "Note: this screen is simply to demonstrate that items have been 'saved' into the correct database structure.");
            label.Height = 40;

            manager.AddControl(grid, BorderLayoutManager.Position.Centre);
            manager.AddControl(label, BorderLayoutManager.Position.North);
        }

        public void SetForm(IFormHabanero form)
        {

        }
    }
}