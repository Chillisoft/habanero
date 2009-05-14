using System.Windows.Forms;
using Habanero.Base;
using Invoicing.UI;

namespace Invoicing
{
    public partial class ProgramForm : Form
    {
		private InvoicingFormController _formController;
		
        public ProgramForm()
        {
            SetBounds(0, 0, 640, 480);
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
            _formController = new InvoicingFormController(this);

            MainMenu mainMenu = new MainMenu();
            MenuItem fileMenu = new MenuItem("&Data");
            //fileMenu.MenuItems.Add(new MenuItem("&Object Name", delegate
            //    { _formController.SetCurrentControl(InvoicingFormController.OBJECT_NAME); }));
            mainMenu.MenuItems.Add(fileMenu);

			MenuItem windowMenu = new MenuItem("&Window");
        	windowMenu.MdiList = true;
        	mainMenu.MenuItems.Add(windowMenu);

            Menu = mainMenu;
        }
    }
}