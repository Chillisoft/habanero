using System;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.BOControls.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.BOControls.v2
{
	/// <summary>
	/// Provides control for panels that represent a business object
    /// in a user interface.
	/// </summary>
	public class BoPanelControl : UserControl
	{
		private BusinessObjectBase itsBo;
		private string itsUiDefName;
		private PanelFactoryInfo itsPanelFactoryInfo;
		private Panel itsBoPanel;

        /// <summary>
        /// Constructor to create a new control object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <param name="uiDefName">A string name for the control</param>
		public BoPanelControl(BusinessObjectBase bo, string uiDefName) {
			itsBo = bo;
			itsUiDefName = uiDefName;

			BOMapper mapper = new BOMapper(bo);
			
			UIFormDef def = (itsUiDefName.Length > 0) 
				? mapper.GetUserInterfaceMapper(itsUiDefName).GetUIFormProperties() 
				: mapper.GetUserInterfaceMapper().GetUIFormProperties();

			PanelFactory factory = new PanelFactory(itsBo, def );
			itsPanelFactoryInfo = factory.CreatePanel() ;
			itsBoPanel = itsPanelFactoryInfo.Panel ;

			BorderLayoutManager manager = new BorderLayoutManager(this) ;
			manager.AddControl(itsBoPanel, BorderLayoutManager.Position.Centre ) ;
		}

        /// <summary>
        /// Returns the panel object being controlled
        /// </summary>
        public Panel Panel {
            get { return itsBoPanel; }
        }

        /// <summary>
        /// Returns the panel factory object
        /// </summary>
        public PanelFactoryInfo PanelFactoryInfo
        {
            get { return itsPanelFactoryInfo; }
        }

        /// <summary>
        /// Sets the business object to be represented
        /// </summary>
        /// <param name="bo">The business object</param>
        public void SetBusinessObject(BusinessObjectBase bo)
        {
            itsPanelFactoryInfo.ControlMappers.BusinessObject = bo;
        }
	}
}
