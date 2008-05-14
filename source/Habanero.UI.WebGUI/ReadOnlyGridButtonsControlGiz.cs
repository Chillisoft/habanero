using System;
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
            IButton addButton = AddButton("Add", FireAddButtonClicked);
            addButton.Visible = true;

            IButton editButton = AddButton("Edit", FireEditButtonClicked);
            editButton.Visible = true;

            _deleteButton = AddButton("Delete", FireDeleteButtonClicked);
            _deleteButton.Visible = false;
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