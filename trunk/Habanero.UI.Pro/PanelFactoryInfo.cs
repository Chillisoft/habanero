using System.Windows.Forms;
using Habanero.Ui.Generic;
using System.Collections.Generic;

namespace Habanero.Ui.BoControls
{
    /// <summary>
    /// Manages the panel object for a user interface
    /// </summary>
    public class PanelFactoryInfo
    {
        private Panel _panel;
        private ControlMapperCollection _mappers;
        private readonly Control _firstControlToFocus;
        private int _preferredHeight;
        private int _preferredWidth;
        private IDictionary<string, IGrid> _formGrids;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="panel">The panel object being managed</param>
        /// <param name="mappers">The control mappers collection</param>
        /// <param name="firstControlToFocus">The first control to focus on</param>
        public PanelFactoryInfo(Panel panel, ControlMapperCollection mappers, Control firstControlToFocus)
        {
            _panel = panel;
            _mappers = mappers;
            this._firstControlToFocus = firstControlToFocus;
            _formGrids = new Dictionary<string, IGrid>();
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
            get { return _panel; }
        }

        /// <summary>
        /// Returns the collection of control mappers
        /// </summary>
        public ControlMapperCollection ControlMappers
        {
            get { return _mappers; }
        }

        /// <summary>
        /// Returns the form grid by the name specified
        /// </summary>
        /// <param name="gridName">The grid name</param>
        /// <returns>Returns the grid object if a grid by that name is
        /// found</returns>
        public IGrid GetFormGrid(string gridName)
        {
            return _formGrids[gridName];
        }

        /// <summary>
        /// Gets and sets the form grids
        /// </summary>
        public IDictionary<string, IGrid> FormGrids
        {
            get
            {
                return _formGrids;
            }
            set
            {
                _formGrids = value;
            }

        }

        /// <summary>
        /// Gets and sets the preferred height setting
        /// </summary>
        public int PreferredHeight
        {
            get { return _preferredHeight; }
            set { _preferredHeight = value; }
        }

        /// <summary>
        /// Gets and sets the preferred width setting
        /// </summary>
        public int PreferredWidth
        {
            get { return _preferredWidth; }
            set { _preferredWidth = value; }
        }

        /// <summary>
        /// Returns the first control to focus on in the user interface
        /// </summary>
        public Control FirstControlToFocus
        {
            get { return _firstControlToFocus; }
        }
    }
}