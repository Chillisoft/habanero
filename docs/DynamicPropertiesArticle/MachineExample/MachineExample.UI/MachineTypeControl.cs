using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using MachineExample.BO;

namespace MachineExample.UI
{
    public class MachineTypeControl : UserControlWin, IFormControl
    {
        private ReadOnlyGridControlWin _machineTypesGrid;
        private ReadOnlyGridControlWin _machinePropertyDefsGrid;
        private IControlFactory _controlFactory;

        public MachineTypeControl(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(this);

            IGroupBox machinePropertyDefsGroupBox = CreateMachinePropertyDefsGroupBox();
            layoutManager.AddControl(machinePropertyDefsGroupBox, BorderLayoutManager.Position.Centre);

            IGroupBox machineTypesGroupBox = CreateMachineTypesGroupBox();
            layoutManager.AddControl(machineTypesGroupBox, BorderLayoutManager.Position.North);

            _machineTypesGrid.SetBusinessObjectCollection(Broker.GetBusinessObjectCollection<MachineType>("", "Name"));
        }

        private IGroupBox CreateMachineTypesGroupBox()
        {
            IGroupBox machineTypesGroupBox = _controlFactory.CreateGroupBox();
            machineTypesGroupBox.Text = "Machine Types";
            _machineTypesGrid = new ReadOnlyGridControlWin(_controlFactory);
            _machineTypesGrid.FilterControl.Visible = false;
            _machineTypesGrid.Dock = System.Windows.Forms.DockStyle.Fill;

            machineTypesGroupBox.Controls.Add(_machineTypesGrid);
            machineTypesGroupBox.Height = 150;

            _machineTypesGrid.Grid.BusinessObjectSelected += 
                (sender, e) =>
                     {
                         MachineType machineType = (MachineType)_machineTypesGrid.SelectedBusinessObject;
                         if (machineType == null) return;
                         _machinePropertyDefsGrid.SetBusinessObjectCollection(machineType.MachinePropertyDefs);
                     };
            return machineTypesGroupBox;
        }

        private IGroupBox CreateMachinePropertyDefsGroupBox()
        {
            IGroupBox machinePropertyDefsGroupBox = _controlFactory.CreateGroupBox();
            machinePropertyDefsGroupBox.Text = "Machine Property Definitions";
            _machinePropertyDefsGrid = new ReadOnlyGridControlWin(_controlFactory);
            _machinePropertyDefsGrid.FilterControl.Visible = false;
            _machinePropertyDefsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            machinePropertyDefsGroupBox.Controls.Add(_machinePropertyDefsGrid);
            return machinePropertyDefsGroupBox;
        }

        public void SetForm(IFormHabanero form) { }
    }
}