//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    partial class MultiSelectorGiz<T>
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
            this.groupBox1 = new Gizmox.WebGUI.Forms.GroupBox();
            this._availableOptionsListbox = new Habanero.UI.WebGUI.ListBoxGiz();
            this.groupBox2 = new Gizmox.WebGUI.Forms.GroupBox();
            this._selectionsListbox = new Habanero.UI.WebGUI.ListBoxGiz();
            this._btnSelect = new Habanero.UI.WebGUI.ButtonGiz();
            this._btnSelectAll = new Habanero.UI.WebGUI.ButtonGiz();
            this._btnDeselectAll = new Habanero.UI.WebGUI.ButtonGiz();
            this._btnDeselect = new Habanero.UI.WebGUI.ButtonGiz();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((Gizmox.WebGUI.Forms.AnchorStyles)(((Gizmox.WebGUI.Forms.AnchorStyles.Top | Gizmox.WebGUI.Forms.AnchorStyles.Bottom)
                        | Gizmox.WebGUI.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this._availableOptionsListbox);
            this.groupBox1.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this.groupBox1.FlatStyle = Gizmox.WebGUI.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 369);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.Text = "Available Options";
            // 
            // _availableOptionsListbox
            // 
            this._availableOptionsListbox.Anchor = Gizmox.WebGUI.Forms.AnchorStyles.None;
            this._availableOptionsListbox.BorderStyle = Gizmox.WebGUI.Forms.BorderStyle.Fixed3D;
            this._availableOptionsListbox.Dock = Gizmox.WebGUI.Forms.DockStyle.Fill;
            this._availableOptionsListbox.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this._availableOptionsListbox.Location = new System.Drawing.Point(3, 16);
            this._availableOptionsListbox.Name = "_availableOptionsListbox";
            this._availableOptionsListbox.SelectionMode = Habanero.UI.Base.ListBoxSelectionMode.MultiExtended;
            this._availableOptionsListbox.Size = new System.Drawing.Size(194, 342);
            this._availableOptionsListbox.TabIndex = 0;
            this._availableOptionsListbox.DoubleClick += new System.EventHandler(this._availableOptionsListbox_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((Gizmox.WebGUI.Forms.AnchorStyles)(((Gizmox.WebGUI.Forms.AnchorStyles.Top | Gizmox.WebGUI.Forms.AnchorStyles.Bottom)
                        | Gizmox.WebGUI.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this._selectionsListbox);
            this.groupBox2.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this.groupBox2.FlatStyle = Gizmox.WebGUI.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(290, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 369);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.Text = "Selected Options";
            // 
            // _selectionsListbox
            // 
            this._selectionsListbox.Anchor = Gizmox.WebGUI.Forms.AnchorStyles.None;
            this._selectionsListbox.BorderStyle = Gizmox.WebGUI.Forms.BorderStyle.Fixed3D;
            this._selectionsListbox.Dock = Gizmox.WebGUI.Forms.DockStyle.Fill;
            this._selectionsListbox.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this._selectionsListbox.Location = new System.Drawing.Point(3, 16);
            this._selectionsListbox.Name = "_selectionsListbox";
            this._selectionsListbox.SelectionMode = Habanero.UI.Base.ListBoxSelectionMode.MultiExtended;
            this._selectionsListbox.Size = new System.Drawing.Size(194, 342);
            this._selectionsListbox.TabIndex = 0;
            // 
            // _btnSelect
            // 
            this._btnSelect.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this._btnSelect.Location = new System.Drawing.Point(209, 94);
            this._btnSelect.Name = "_btnSelect";
            this._btnSelect.Size = new System.Drawing.Size(75, 23);
            this._btnSelect.TabIndex = 2;
            this._btnSelect.Text = ">";
            this._btnSelect.TextImageRelation = Gizmox.WebGUI.Forms.TextImageRelation.Overlay;
            // 
            // _btnSelectAll
            // 
            this._btnSelectAll.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this._btnSelectAll.Location = new System.Drawing.Point(209, 142);
            this._btnSelectAll.Name = "_btnSelectAll";
            this._btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this._btnSelectAll.TabIndex = 3;
            this._btnSelectAll.Text = ">>";
            this._btnSelectAll.TextImageRelation = Gizmox.WebGUI.Forms.TextImageRelation.Overlay;
            // 
            // _btnDeselectAll
            // 
            this._btnDeselectAll.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this._btnDeselectAll.Location = new System.Drawing.Point(209, 238);
            this._btnDeselectAll.Name = "_btnDeselectAll";
            this._btnDeselectAll.Size = new System.Drawing.Size(75, 23);
            this._btnDeselectAll.TabIndex = 4;
            this._btnDeselectAll.Text = "<<";
            this._btnDeselectAll.TextImageRelation = Gizmox.WebGUI.Forms.TextImageRelation.Overlay;
            // 
            // _btnDeselect
            // 
            this._btnDeselect.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this._btnDeselect.Location = new System.Drawing.Point(209, 190);
            this._btnDeselect.Name = "_btnDeselect";
            this._btnDeselect.Size = new System.Drawing.Size(75, 23);
            this._btnDeselect.TabIndex = 5;
            this._btnDeselect.Text = "<";
            this._btnDeselect.TextImageRelation = Gizmox.WebGUI.Forms.TextImageRelation.Overlay;
            // 
            // MultiSelectorGiz
            // 
            this.Controls.Add(this._btnDeselect);
            this.Controls.Add(this._btnDeselectAll);
            this.Controls.Add(this._btnSelectAll);
            this.Controls.Add(this._btnSelect);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Size = new System.Drawing.Size(497, 381);
            this.Text = "MyMultiSelector";
            this.ResumeLayout(false);

        }

        #endregion

        private global::Gizmox.WebGUI.Forms.GroupBox groupBox1;
        private ListBoxGiz _availableOptionsListbox;
        private global::Gizmox.WebGUI.Forms.GroupBox groupBox2;
        private ListBoxGiz _selectionsListbox;
        private ButtonGiz _btnSelect;
        private ButtonGiz _btnSelectAll;
        private ButtonGiz _btnDeselectAll;
        private ButtonGiz _btnDeselect;



    }
}