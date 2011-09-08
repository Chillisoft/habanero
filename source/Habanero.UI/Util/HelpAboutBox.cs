//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------


using System;
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.Forms;

namespace Habanero.UI.Util
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
        /// <param name="programName">The program name</param>
        /// <param name="producedForName">Who the program is produced for</param>
        /// <param name="producedByName">Who produced the program</param>
        /// <param name="versionNumber">The version number</param>
        public HelpAboutBox(string programName, string producedForName, string producedByName, string versionNumber)
        {
            Panel mainPanel = new Panel();
            GridLayoutManager mainPanelManager = new GridLayoutManager(mainPanel);
            mainPanelManager.SetGridSize(4, 2);
            mainPanelManager.FixAllRowsBasedOnContents();
            mainPanelManager.FixColumnBasedOnContents(0);
            mainPanelManager.FixColumnBasedOnContents(1);
            mainPanelManager.AddControl(ControlFactory.CreateLabel("Programme Name:", false));
            mainPanelManager.AddControl(ControlFactory.CreateLabel(programName, false));
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