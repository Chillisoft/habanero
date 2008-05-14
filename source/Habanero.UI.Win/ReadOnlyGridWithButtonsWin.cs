using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ReadOnlyGridWithButtonsWin : ControlWin, IReadOnlyGridWithButtons
    {
        public IReadOnlyGrid Grid
        {
            get { return null; }
        }

        public BusinessObject SelectedBusinessObject
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        public IReadOnlyGridButtonsControl Buttons
        {
            get { throw new System.NotImplementedException(); }
        }

        public void SetCollection(IBusinessObjectCollection boCollection)
        {
            throw new System.NotImplementedException();
        }
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}