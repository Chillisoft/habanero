using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    public class ReadOnlyGridButtonsControlManager
    {
        private readonly IReadOnlyGridButtonsControl _buttonsControl;
        private IButton _deleteButton;

        public ReadOnlyGridButtonsControlManager(IReadOnlyGridButtonsControl buttonsControl)
        {
            _buttonsControl = buttonsControl;
        }

        public IButton DeleteButton
        {
            get { return _deleteButton; }
        }

        public void CreateDeleteButton(EventHandler eventHandler)
        {
            _deleteButton = _buttonsControl.AddButton("Delete", eventHandler);
            _deleteButton.Visible = false;
        }

        public void CreateEditButton(EventHandler eventHandler)
        {
            IButton editButton = _buttonsControl.AddButton("Edit", eventHandler);
            editButton.Visible = true;
        }

        public void CreateAddButton(EventHandler eventHandler)
        {
            IButton addButton = _buttonsControl.AddButton("Add", eventHandler);
            addButton.Visible = true;
        }

    }
}
