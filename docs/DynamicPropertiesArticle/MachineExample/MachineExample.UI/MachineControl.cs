using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using MachineExample.BO;

namespace MachineExample.UI
{
    public class MachineControl : UserControlWin, IFormControl
    {
        private readonly IControlFactory _controlFactory;
        private IReadOnlyGridControl machinesGrid;
        private IComboBox machineTypeComboBox;
        private IPanel selectMachineTypePanel;

        public MachineControl(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;

            CreateSelectMachineTypePanel();
            CreateMachinesGrid();

            BorderLayoutManager manager = _controlFactory.CreateBorderLayoutManager(this);
            manager.AddControl(selectMachineTypePanel, BorderLayoutManager.Position.North);
            manager.AddControl(machinesGrid, BorderLayoutManager.Position.Centre);

        }

        public void SetForm(IFormHabanero form) { }

        private void CreateMachinesGrid()
        {
            machinesGrid = _controlFactory.CreateReadOnlyGridControl();
            machinesGrid.Buttons["Add"].Visible = false;
            machinesGrid.FilterControl.Visible = false;
            machineTypeComboBox.SelectedIndexChanged +=
                (sender, e) =>
                    {
                        var machineType = (MachineType) machineTypeComboBox.SelectedItem;
                        if (machineType == null) return;
                        BusinessObjectCollection<Machine> machines = machineType.Machines;

                        ClassDef machineClassDef = machineType.GetMachineClassDef();
                        machines.ClassDef = machineClassDef;
                        machinesGrid.Initialise(machineClassDef);
                        machinesGrid.SetBusinessObjectCollection(machines);
                    };
        }

        private void CreateSelectMachineTypePanel()
        {
            selectMachineTypePanel = _controlFactory.CreatePanel();
            var selectMachineTypePanelManager = new FlowLayoutManager(selectMachineTypePanel, _controlFactory);
            selectMachineTypePanel.Height = 50;

            CreateMachineTypeComboBox();

            selectMachineTypePanelManager.AddControl(_controlFactory.CreateLabel("Machine Type: "));
            selectMachineTypePanelManager.AddControl(machineTypeComboBox);
            IButton button = CreateNewMachineButton();

            selectMachineTypePanelManager.AddControl(button);
        }

        private void CreateMachineTypeComboBox()
        {
            machineTypeComboBox = _controlFactory.CreateComboBox();
            Broker.GetBusinessObjectCollection<MachineType>("").ForEach(type => machineTypeComboBox.Items.Add(type));
        }

        private IButton CreateNewMachineButton()
        {
            IButton button = _controlFactory.CreateButton("Create New Machine Instance");
            button.Click +=
                (sender, e) =>
                    {
                        var machineType = (MachineType) machineTypeComboBox.SelectedItem;
                        if (machineType == null) return;
                        Machine newMachine = machineType.CreateMachine();
                        IDefaultBOEditorForm editorFormWin = new DefaultBOEditorFormWin(newMachine, "default",
                                                                                        _controlFactory);
                        if (editorFormWin.ShowDialog())
                        {
                            machinesGrid.SetBusinessObjectCollection(machineType.Machines);
                        }
                    };
            return button;
        }
    }
}