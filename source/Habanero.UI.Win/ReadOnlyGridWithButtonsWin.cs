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

        public void SetCollection(IBusinessObjectCollection col)
        {
            throw new System.NotImplementedException();
        }
    }
}