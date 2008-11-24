using System.Collections.Generic;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IPanelInfo
    {
        IPanel Panel { get; set; }
        GridLayoutManager LayoutManager { get; set; }
        PanelInfo.FieldInfoCollection FieldInfos { get; }
        BusinessObject BusinessObject { get; set; }
        bool ControlsEnabled { set; }
        IList<IPanelInfo> PanelInfos { get; }
        void ApplyChangesToBusinessObject();
        void ClearErrorProviders();
    }
}