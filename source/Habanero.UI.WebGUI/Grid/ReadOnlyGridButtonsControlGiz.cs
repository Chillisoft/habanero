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

using System;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
 public class ReadOnlyGridButtonsControlGiz :ButtonGroupControlGiz, IReadOnlyGridButtonsControl
    {
        private readonly IButton _deleteButton;
        public event EventHandler DeleteClicked;
        public event EventHandler AddClicked;
        public event EventHandler EditClicked;

        public bool ShowDefaultDeleteButton
        {
            get { return _deleteButton.Visible; }
            set { _deleteButton.Visible = value; }
        }

        public ReadOnlyGridButtonsControlGiz(IControlFactory controlFactory) : base(controlFactory)
        {
            _deleteButton = AddButton("Delete", FireDeleteButtonClicked);
            _deleteButton.Visible = false;
           
            IButton editButton = AddButton("Edit", FireEditButtonClicked);
            editButton.Visible = true;
            IButton addButton = AddButton("Add", FireAddButtonClicked);
            addButton.Visible = true;
            
        }

        void FireDeleteButtonClicked(object sender, EventArgs e)
        {
            if (DeleteClicked != null)
            {
                this.DeleteClicked(this, new EventArgs());
            }
        }
        void FireAddButtonClicked(object sender, EventArgs e)
        {
            if (AddClicked != null)
            {
                this.AddClicked(this, new EventArgs());
            }
        }
        void FireEditButtonClicked(object sender, EventArgs e)
        {
            if (EditClicked != null)
            {
                this.EditClicked(this, new EventArgs());
            }
        }
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }
}