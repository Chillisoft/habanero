using System;
using System.Data;
using System.Windows.Forms;
using Habanero.Db;
using Habanero.Generic;
using Habanero.Ui.Generic;


namespace Habanero.Ui.Misc
{
    /// <summary>
    /// Provides a pad in which raw sql can be typed and executed
    /// </summary>
    public class SqlScratchPad : UserControl, IFormControl
    {
        TextBox _scriptTextBox;
        DataGrid _resultGrid;
        TextBox _statusTextBox;
        ButtonControl _buttonControl;

        private bool _isAuthenticated;

        /// <summary>
        /// Constructor to initialise a new scratchpad
        /// </summary>
        public SqlScratchPad()
        {
            _scriptTextBox = ControlFactory.CreateTextBox();
            _scriptTextBox.Multiline = true;
            _scriptTextBox.ScrollBars = ScrollBars.Vertical;

            _resultGrid = new DataGrid();

            _statusTextBox = ControlFactory.CreateTextBox();

            _buttonControl = new ButtonControl();
            _buttonControl.AddButton("Execute", new EventHandler(ExecuteButtonClickHandler));
            Panel topPanel = new Panel();
            BorderLayoutManager topPanelManager = new BorderLayoutManager(topPanel);
            topPanelManager.AddControl(_scriptTextBox, BorderLayoutManager.Position.Centre);
            topPanelManager.AddControl(_buttonControl, BorderLayoutManager.Position.South);
            topPanel.Height = 300;

            BorderLayoutManager manager = new BorderLayoutManager(this);
            manager.AddControl(topPanel, BorderLayoutManager.Position.North, true);
            manager.AddControl(_resultGrid, BorderLayoutManager.Position.Centre);
            manager.AddControl(_statusTextBox, BorderLayoutManager.Position.South);
        }

        /// <summary>
        /// Handles the event of the "Execute" button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ExecuteButtonClickHandler(object sender, EventArgs e)
        {
            if (!_isAuthenticated)
            {
                InputBoxTextBox passwordEntry = new InputBoxTextBox("Please enter the password:", 1);
                passwordEntry.IsPasswordField = true;
                if (passwordEntry.ShowDialog() == DialogResult.OK)
                {
                    if (passwordEntry.Text != "scratch!")
                    {
                        MessageBox.Show("Invalid password.", "Incorrect", MessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        _isAuthenticated = true;
                    }
                }
                else
                {
                    return;
                }
            }
            try
            {
                if (_scriptTextBox.Text.Trim().Substring(0, 6).ToUpper() == "SELECT")
                {
                    DataTable table =
                        DatabaseConnection.CurrentConnection.LoadDataTable(
                            new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection(), _scriptTextBox.Text),
                            "", "");
                    _resultGrid.DataSource = table;
                    _statusTextBox.Text = Convert.ToString(table.Rows.Count) + " row returned.";
                }
                else
                {
                    int numRows = DatabaseConnection.CurrentConnection.ExecuteRawSql(_scriptTextBox.Text);
                    _statusTextBox.Text = Convert.ToString(numRows) + " rows affected.";
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(
                    new Exception("Problem executing statement : " + _scriptTextBox.Text, ex),
                    "There was a problem executing the statement.  Please see below for details.", "Failure");
            }
        }

        /// <summary>
        /// Sets the form
        /// </summary>
        /// <param name="form">The form to set</param>
        public void SetForm(Form form)
        {
        }
    }
}