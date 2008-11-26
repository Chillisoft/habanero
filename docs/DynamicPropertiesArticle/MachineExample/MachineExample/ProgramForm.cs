using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using MachineExample.BO;
using MachineExample.UI;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace MachineExample
{
    public partial class ProgramForm : FormWin
    {
        public ProgramForm()
        {
            SetBounds(0, 0, 455, 400);
            IsMdiContainer = true;
            CreateMainMenu();
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            Text = GlobalRegistry.ApplicationName + " " + GlobalRegistry.ApplicationVersion;
        }

        private void CreateMainMenu()
        {
            var menu = new HabaneroMenu("Main", this, GlobalUIRegistry.ControlFactory);

            HabaneroMenu dataMenu = menu.AddSubmenu("&Data");
            HabaneroMenu.Item machineTypeItem = dataMenu.AddMenuItem("Machine Types");
            machineTypeItem.FormControlCreator += () => new MachineTypeControl(GlobalUIRegistry.ControlFactory);

            HabaneroMenu.Item machinesItem = dataMenu.AddMenuItem("Machines");
            machinesItem.FormControlCreator += () => new MachineControl(GlobalUIRegistry.ControlFactory);

            HabaneroMenu otherMenu = menu.AddSubmenu("&Other");
            HabaneroMenu.Item machinePropertiesItem = otherMenu.AddMenuItem("Machine Properties");
            machinePropertiesItem.FormControlCreator += () => new MachinePropertiesControl(GlobalUIRegistry.ControlFactory);


            var menuBuilderWin = new MenuBuilderWin();
            menuBuilderWin.BuildMainMenu(menu).DockInForm(this);
        }
    }


}