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

namespace Habanero.UI.VWG
{
    partial class MultiSelectorVWG<T>
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
            this._availableOptionsGroupBox = new GroupBoxVWG();
            this._availableOptionsListbox = new ListBoxVWG();
            this._selectionsGroupBox = new GroupBoxVWG();
            this._selectionsListbox = new ListBoxVWG();
            this._btnSelect = new ButtonVWG();
            this._btnSelectAll = new ButtonVWG();
            this._btnDeselectAll = new ButtonVWG();
            this._btnDeselect = new ButtonVWG();
            this.SuspendLayout();
            // 
            // _availableOptionsGroupBox
            // 
            this._availableOptionsGroupBox.Anchor = ((Gizmox.WebGUI.Forms.AnchorStyles)(((Gizmox.WebGUI.Forms.AnchorStyles.Top | Gizmox.WebGUI.Forms.AnchorStyles.Bottom)
                        | Gizmox.WebGUI.Forms.AnchorStyles.Left)));
            this._availableOptionsGroupBox.Controls.Add(this._availableOptionsListbox);
            this._availableOptionsGroupBox.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this._availableOptionsGroupBox.FlatStyle = Gizmox.WebGUI.Forms.FlatStyle.Flat;
            this._availableOptionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this._availableOptionsGroupBox.Name = "_availableOptionsGroupBox";
            this._availableOptionsGroupBox.Size = new System.Drawing.Size(200, 369);
            this._availableOptionsGroupBox.TabIndex = 0;
            this._availableOptionsGroupBox.Text = "Available AllOptions";
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
            // _selectionsGroupBox
            // 
            this._selectionsGroupBox.Anchor = ((Gizmox.WebGUI.Forms.AnchorStyles)(((Gizmox.WebGUI.Forms.AnchorStyles.Top | Gizmox.WebGUI.Forms.AnchorStyles.Bottom)
                        | Gizmox.WebGUI.Forms.AnchorStyles.Right)));
            this._selectionsGroupBox.Controls.Add(this._selectionsListbox);
            this._selectionsGroupBox.DragTargets = new Gizmox.WebGUI.Forms.Component[0];
            this._selectionsGroupBox.FlatStyle = Gizmox.WebGUI.Forms.FlatStyle.Flat;
            this._selectionsGroupBox.Location = new System.Drawing.Point(290, 3);
            this._selectionsGroupBox.Name = "_selectionsGroupBox";
            this._selectionsGroupBox.Size = new System.Drawing.Size(200, 369);
            this._selectionsGroupBox.TabIndex = 1;
            this._selectionsGroupBox.Text = "Selected AllOptions";
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
            // MultiSelectorVWG
            // 
            //this.Controls.Add(this._btnDeselect);
            //this.Controls.Add(this._btnDeselectAll);
            //this.Controls.Add(this._btnSelectAll);
            //this.Controls.Add(this._btnSelect);
            //this.Controls.Add(this._selectionsGroupBox);
            //this.Controls.Add(this._availableOptionsGroupBox);
            this.Size = new System.Drawing.Size(497, 381);
            this.Text = "MyMultiSelector";
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBoxVWG _availableOptionsGroupBox;
        private ListBoxVWG _availableOptionsListbox;
        private GroupBoxVWG _selectionsGroupBox;
        private ListBoxVWG _selectionsListbox;
        private ButtonVWG _btnSelect;
        private ButtonVWG _btnSelectAll;
        private ButtonVWG _btnDeselectAll;
        private ButtonVWG _btnDeselect;



    }
}