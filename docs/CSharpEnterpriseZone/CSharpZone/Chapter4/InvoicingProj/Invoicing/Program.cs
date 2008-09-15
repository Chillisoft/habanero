using System;
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.Base;

namespace Invoicing
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            HabaneroAppForm mainApp = new HabaneroAppForm("Invoicing", "v1.0");
            if (!mainApp.Startup()) return;

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ProgramForm());
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex,
					"An error has occurred in the application.",
					"Application Error");
            }
        }
    }
}