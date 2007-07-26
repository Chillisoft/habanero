using System.Windows.Forms;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides control for panels that represent a business object
    /// in a user interface.
    /// </summary>
    public class BoPanelControl : UserControl
    {
        private BusinessObject _bo;
        private string _uiDefName;
        private PanelFactoryInfo _panelFactoryInfo;
        private Panel _boPanel;

        /// <summary>
        /// Constructor to create a new control object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <param name="uiDefName">A string name for the control</param>
        public BoPanelControl(BusinessObject bo, string uiDefName) {
            Permission.Check(this);
            _bo = bo;
            _uiDefName = uiDefName;

            BOMapper mapper = new BOMapper(bo);
			
            UIForm def = (_uiDefName.Length > 0) 
                                ? mapper.GetUIDef(_uiDefName).GetUIFormProperties() 
                                : mapper.GetUIDef().GetUIFormProperties();

            PanelFactory factory = new PanelFactory(_bo, def );
            _panelFactoryInfo = factory.CreatePanel() ;
            _boPanel = _panelFactoryInfo.Panel ;

            BorderLayoutManager manager = new BorderLayoutManager(this) ;
            manager.AddControl(_boPanel, BorderLayoutManager.Position.Centre ) ;
        }

        /// <summary>
        /// Returns the panel object being controlled
        /// </summary>
        public Panel Panel {
            get { return _boPanel; }
        }

        /// <summary>
        /// Returns the panel factory object
        /// </summary>
        public PanelFactoryInfo PanelFactoryInfo
        {
            get { return _panelFactoryInfo; }
        }

        /// <summary>
        /// Sets the business object to be represented
        /// </summary>
        /// <param name="bo">The business object</param>
        public void SetBusinessObject(BusinessObject bo)
        {
            _panelFactoryInfo.ControlMappers.BusinessObject = bo;
        }
    }
}