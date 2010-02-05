using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    internal class BusinessObjectControlStubWin : UserControlWin, IBusinessObjectControl
    {
        public IBusinessObject BusinessObject { get; set; }

    }
}