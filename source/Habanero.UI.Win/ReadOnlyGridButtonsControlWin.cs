using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ReadOnlyGridButtonsControlWin : ButtonGroupControlWin, IReadOnlyGridButtonsControl
    {
        public event EventHandler DeleteClicked;
        public event EventHandler AddClicked;
        public event EventHandler EditClicked;
        private readonly ReadOnlyGridButtonsControlManager _manager;

        public ReadOnlyGridButtonsControlWin(IControlFactory controlFactory)
            : base(controlFactory)
        {
            _manager = new ReadOnlyGridButtonsControlManager(this);
            _manager.CreateDeleteButton(delegate { if (DeleteClicked != null) DeleteClicked(this, new EventArgs()); });
            _manager.CreateEditButton(delegate { if (EditClicked != null) EditClicked(this, new EventArgs()); });
            _manager.CreateAddButton(delegate { if (AddClicked != null) AddClicked(this, new EventArgs()); });
        }

        public bool ShowDefaultDeleteButton
        {
            get { return _manager.DeleteButton.Visible; }
            set { _manager.DeleteButton.Visible = value; }
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}