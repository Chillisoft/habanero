using System;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Displays project information when the user chooses "About" from the
    /// "Help" menu
    /// </summary>
    public class HelpAboutBox : Form
    {
        /// <summary>
        /// Constructor to initialise a new About form with the given
        /// information
        /// </summary>
        /// <param name="programmeName">The program name</param>
        /// <param name="producedForName">Who the program is produced for</param>
        /// <param name="producedByName">Who produced the program</param>
        /// <param name="versionNumber">The version number</param>
        /// TODO ERIC - rename param to programName
        public HelpAboutBox(string programmeName, string producedForName, string producedByName, string versionNumber)
        {
            Panel mainPanel = new Panel();
            GridLayoutManager mainPanelManager = new GridLayoutManager(mainPanel);
            mainPanelManager.SetGridSize(4, 2);
            mainPanelManager.FixAllRowsBasedOnContents();
            mainPanelManager.FixColumnBasedOnContents(0);
            mainPanelManager.FixColumnBasedOnContents(1);
            mainPanelManager.AddControl(ControlFactory.CreateLabel("Programme Name:", false));
            mainPanelManager.AddControl(ControlFactory.CreateLabel(programmeName, false));
            mainPanelManager.AddControl(ControlFactory.CreateLabel("Produced For:", false));
            mainPanelManager.AddControl(ControlFactory.CreateLabel(producedForName, false));
            mainPanelManager.AddControl(ControlFactory.CreateLabel("Produced By:", false));
            mainPanelManager.AddControl(ControlFactory.CreateLabel(producedByName, false));
            mainPanelManager.AddControl(ControlFactory.CreateLabel("Version:", false));
            mainPanelManager.AddControl(ControlFactory.CreateLabel(versionNumber, false));

            ButtonControl buttons = new ButtonControl();
            buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));

            BorderLayoutManager manager = new BorderLayoutManager(this);
            manager.AddControl(mainPanel, BorderLayoutManager.Position.Centre);
            manager.AddControl(buttons, BorderLayoutManager.Position.South);
            this.Width = 300;
            this.Height = 200;
            this.Text = "About";
        }

        /// <summary>
        /// Handles the event of the OK button being pressed, which closes
        /// the form
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void OKButtonClickHandler(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}