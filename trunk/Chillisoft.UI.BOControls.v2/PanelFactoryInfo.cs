using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;
using System.Collections.Generic;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// Manages the panel object for a user interface
    /// </summary>
    public class PanelFactoryInfo
    {
        private Panel itsPanel;
        private ControlMapperCollection itsMappers;
        private readonly Control itsFirstControlToFocus;
        private int itsPreferredHeight;
        private int itsPreferredWidth;
        private IDictionary<string, IGrid> itsFormGrids;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="panel">The panel object being managed</param>
        /// <param name="mappers">The control mappers collection</param>
        /// <param name="firstControlToFocus">The first control to focus on</param>
        public PanelFactoryInfo(Panel panel, ControlMapperCollection mappers, Control firstControlToFocus)
        {
            itsPanel = panel;
            itsMappers = mappers;
            this.itsFirstControlToFocus = firstControlToFocus;
            itsFormGrids = new Dictionary<string, IGrid>();
        }

        /// <summary>
        /// A constructor as before, but with only the panel specified
        /// </summary>
        public PanelFactoryInfo(Panel panel) : this(panel, new ControlMapperCollection(), null)
        {
        }

        /// <summary>
        /// Returns the panel object
        /// </summary>
        public Panel Panel
        {
            get { return itsPanel; }
        }

        /// <summary>
        /// Returns the collection of control mappers
        /// </summary>
        public ControlMapperCollection ControlMappers
        {
            get { return itsMappers; }
        }

        /// <summary>
        /// Returns the form grid by the name specified
        /// </summary>
        /// <param name="gridName">The grid name</param>
        /// <returns>Returns the grid object if a grid by that name is
        /// found</returns>
        public IGrid GetFormGrid(string gridName)
        {
            return itsFormGrids[gridName];
        }

        /// <summary>
        /// Gets and sets the form grids
        /// </summary>
        public IDictionary<string, IGrid> FormGrids
        {
            get
            {
                return itsFormGrids;
            }
            set
            {
                itsFormGrids = value;
            }

        }

        /// <summary>
        /// Gets and sets the preferred height setting
        /// </summary>
        public int PreferredHeight
        {
            get { return itsPreferredHeight; }
            set { itsPreferredHeight = value; }
        }

        /// <summary>
        /// Gets and sets the preferred width setting
        /// </summary>
        public int PreferredWidth
        {
            get { return itsPreferredWidth; }
            set { itsPreferredWidth = value; }
        }

        /// <summary>
        /// Returns the first control to focus on in the user interface
        /// </summary>
        public Control FirstControlToFocus
        {
            get { return itsFirstControlToFocus; }
        }
    }
}