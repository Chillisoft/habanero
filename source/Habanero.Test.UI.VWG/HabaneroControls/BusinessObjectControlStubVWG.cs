using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    internal class BusinessObjectControlStubVWG : UserControlVWG, IBusinessObjectControl
    {
        public IBusinessObject BusinessObject { get; set; }
    }
}