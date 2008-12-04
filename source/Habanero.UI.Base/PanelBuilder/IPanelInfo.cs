using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IPanelInfo
    {
        IPanel Panel { get; set; }
        GridLayoutManager LayoutManager { get; set; }
        PanelInfo.FieldInfoCollection FieldInfos { get; }
        IBusinessObject BusinessObject { get; set; }
        bool ControlsEnabled { set; }
        IList<IPanelInfo> PanelInfos { get; }
        void ApplyChangesToBusinessObject();
        void ClearErrorProviders();
    }
}