using System;
using System.Data;
using System.Windows.Forms;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a pad in which raw sql can be typed and executed
    /// </summary>
    public class SqlScratchPad : UserControl, FormControl
    {
        TextBox itsScriptTextBox;
        DataGrid itsResultGrid;
        TextBox itsStatusTextBox;
        ButtonControl itsButtonControl;

        private bool itsIsAuthenticated;

        /// <summary>
        /// Constructor to initialise a new scratchpad
        /// </summary>
        public SqlScratchPad()
        {
            itsScriptTextBox = ControlFactory.CreateTextBox();
            itsScriptTextBox.Multiline = true;
            itsScriptTextBox.ScrollBars = ScrollBars.Vertical;

            itsResultGrid = new DataGrid();

            itsStatusTextBox = ControlFactory.CreateTextBox();

            itsButtonControl = new ButtonControl();
            itsButtonControl.AddButton("Execute", new EventHandler(ExecuteButtonClickHandler));
            Panel topPanel = new Panel();
            BorderLayoutManager topPanelManager = new BorderLayoutManager(topPanel);
            topPanelManager.AddControl(itsScriptTextBox, BorderLayoutManager.Position.Centre);
            topPanelManager.AddControl(itsButtonControl, BorderLayoutManager.Position.South);
            topPanel.Height = 300;

            BorderLayoutManager manager = new BorderLayoutManager(this);
            manager.AddControl(topPanel, BorderLayoutManager.Position.North, true);
            manager.AddControl(itsResultGrid, BorderLayoutManager.Position.Centre);
            manager.AddControl(itsStatusTextBox, BorderLayoutManager.Position.South);
        }

        /// <summary>
        /// Handles the event of the "Execute" button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ExecuteButtonClickHandler(object sender, EventArgs e)
        {
            if (!itsIsAuthenticated)
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
                        itsIsAuthenticated = true;
                    }
                }
                else
                {
                    return;
                }
            }
            try
            {
                if (itsScriptTextBox.Text.Trim().Substring(0, 6).ToUpper() == "SELECT")
                {
                    DataTable table =
                        DatabaseConnection.CurrentConnection.LoadDataTable(
                            new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection(), itsScriptTextBox.Text),
                            "", "");
                    itsResultGrid.DataSource = table;
                    itsStatusTextBox.Text = Convert.ToString(table.Rows.Count) + " row returned.";
                }
                else
                {
                    int numRows = DatabaseConnection.CurrentConnection.ExecutePlainSql(itsScriptTextBox.Text);
                    itsStatusTextBox.Text = Convert.ToString(numRows) + " rows affected.";
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(
                    new BaseApplicationException("Problem executing statement : " + itsScriptTextBox.Text, ex),
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