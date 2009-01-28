using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    ///<summary>
    /// A panel info is a class that wraps the panel and provides functionality 
    ///  for linking a business object a layout manager and a panel.
    ///</summary>
    public interface IPanelInfo
    {
        ///<summary>
        /// The panel that this panel info is controlling
        ///</summary>
        IPanel Panel { get; set; }
        ///<summary>
        /// Gets and sets the layout manager used for this Panel
        ///</summary>
        GridLayoutManager LayoutManager { get; set; }
        ///<summary>
        /// Returns a list of Field infos (info on the fields controlled by this panel.
        ///</summary>
        PanelInfo.FieldInfoCollection FieldInfos { get; }
        ///<summary>
        /// Sets the business object for this panel.
        ///</summary>
        IBusinessObject BusinessObject { get; set; }
        ///<summary>
        /// Sets whether the controls on this panel are enabled or not
        ///</summary>
        bool ControlsEnabled { set; }
        ///<summary>
        /// A list of all panel infos containd in this panel info.
        ///</summary>
        IList<IPanelInfo> PanelInfos { get; }
        ///<summary>
        /// Applies any changes in any control on this panel to the business object
        ///</summary>
        void ApplyChangesToBusinessObject();
        ///<summary>
        /// Clears any error providers for all controls visible on this panel
        ///</summary>
        void ClearErrorProviders();

        /// <summary>
        /// Gets the UIFormTab definition used to construct the panel
        /// for a single tab in the form.  By default, there is one
        /// tab for a form, even if it has not been explicitly defined.
        /// </summary>
        UIFormTab UIFormTab { get; }

        /// <summary>
        /// Gets  the minimum height for the panel
        /// </summary>
        int MinimumPanelHeight { get; }

        /// <summary>
        /// Gets the UIForm definition used to construct the
        /// panel - this is taken from the class definitions for the
        /// business object
        /// </summary>
        UIForm UIForm { get;  }

        /// <summary>
        /// Gets the text for the panel tab (UIFormTab.Name)
        /// </summary>
        string PanelTabText { get; }
    }
}