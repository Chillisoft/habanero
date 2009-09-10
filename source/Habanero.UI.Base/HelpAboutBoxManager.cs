//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IHelpAboutBox objects.
    /// Do not use this object in working code - rather call CreateHelpAboutBox
    /// in the appropriate control factory.
    /// </summary>
    public class HelpAboutBoxManager
    {
        private readonly IFormHabanero _FormHabanero;
        private readonly IPanel _mainPanel;

        ///<summary>
        /// Constructor for the <see cref="HelpAboutBoxManager"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="formHabanero"></param>
        ///<param name="programName"></param>
        ///<param name="producedForName"></param>
        ///<param name="producedByName"></param>
        ///<param name="versionNumber"></param>
        public HelpAboutBoxManager(IControlFactory controlFactory, IFormHabanero formHabanero, string programName, string producedForName, string producedByName, string versionNumber)
        {
            _FormHabanero = formHabanero;
            _mainPanel = controlFactory.CreatePanel();
            GridLayoutManager mainPanelManager = new GridLayoutManager(_mainPanel, controlFactory);
            mainPanelManager.SetGridSize(4, 2);
            mainPanelManager.FixAllRowsBasedOnContents();
            mainPanelManager.FixColumnBasedOnContents(0);
            mainPanelManager.FixColumnBasedOnContents(1);
            mainPanelManager.AddControl(controlFactory.CreateLabel("Programme Name:", false));
            mainPanelManager.AddControl(controlFactory.CreateLabel(programName, false));
            mainPanelManager.AddControl(controlFactory.CreateLabel("Produced For:", false));
            mainPanelManager.AddControl(controlFactory.CreateLabel(producedForName, false));
            mainPanelManager.AddControl(controlFactory.CreateLabel("Produced By:", false));
            mainPanelManager.AddControl(controlFactory.CreateLabel(producedByName, false));
            mainPanelManager.AddControl(controlFactory.CreateLabel("Version:", false));
            mainPanelManager.AddControl(controlFactory.CreateLabel(versionNumber, false));

            IButtonGroupControl buttons = controlFactory.CreateButtonGroupControl();
            buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));

            BorderLayoutManager manager = controlFactory.CreateBorderLayoutManager(formHabanero);
            manager.AddControl(_mainPanel, BorderLayoutManager.Position.Centre);
            manager.AddControl(buttons, BorderLayoutManager.Position.South);
            formHabanero.Width = 300;
            formHabanero.Height = 200;
            formHabanero.Text = "About";
        }

        ///<summary>
        /// The Main Panel
        ///</summary>
        public IPanel MainPanel 
        {
            get { return _mainPanel; }
        }

        ///<summary>
        /// the handler for when the OK button is clicked.
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        public void OKButtonClickHandler(object sender, EventArgs e)
        {
            _FormHabanero.Close();
        }
    }
}
