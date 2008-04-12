namespace Habanero.WebGUI
{
    partial class ButtonControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Visual WebGui UserControl Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new Gizmox.WebGUI.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = Gizmox.WebGUI.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.Dock = Gizmox.WebGUI.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this.flowLayoutPanel1.FlowDirection = Gizmox.WebGUI.Forms.FlowDirection.LeftToRight;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(391, 306);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // ButtonControl
            // 
            this.Controls.Add(this.flowLayoutPanel1);
            this.Size = new System.Drawing.Size(391, 306);
            this.Text = "ButtonControl";
            this.ResumeLayout(false);

        }

        #endregion

        private Gizmox.WebGUI.Forms.FlowLayoutPanel flowLayoutPanel1;


    }
}