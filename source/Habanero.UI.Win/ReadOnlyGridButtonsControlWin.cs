using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a set of buttons for use on an <see cref="IReadOnlyGridControl"/>.
    /// By default, Add and Edit buttons are available, but you can also make the standard
    /// Delete button visible by setting the <see cref="ShowDefaultDeleteButton"/>
    /// property to true.
    /// </summary>
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

        /// <summary>
        /// Indicates whether the default delete button is visible.  This
        /// is false by default.
        /// </summary>
        public bool ShowDefaultDeleteButton
        {
            get { return _manager.DeleteButton.Visible; }
            set { _manager.DeleteButton.Visible = value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}