using System;

namespace Habanero.UI.Base
{
    public class HelpAboutBoxManager
    {
        private IFormChilli _formChilli;
        private IPanel _mainPanel;

        public HelpAboutBoxManager(IControlFactory controlFactory, IFormChilli formChilli, string programName, string producedForName, string producedByName, string versionNumber)
        {
            _formChilli = formChilli;
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

            BorderLayoutManager manager = controlFactory.CreateBorderLayoutManager(formChilli);
            manager.AddControl(_mainPanel, BorderLayoutManager.Position.Centre);
            manager.AddControl(buttons, BorderLayoutManager.Position.South);
            formChilli.Width = 300;
            formChilli.Height = 200;
            formChilli.Text = "About";
        }

        public IPanel MainPanel 
        {
            get { return _mainPanel; }
        }

        public void OKButtonClickHandler(object sender, EventArgs e)
        {
            _formChilli.Close();
        }
    }
}
