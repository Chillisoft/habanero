using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Used to group a collections of Group Box Controls. This implements the <see cref="IGroupControl"/>.
    /// The <see cref="IGroupControl"/> is a control for grouping a number of child controls
    /// Typically properties of a Business object that are grouped together can be grouped together using this
    /// control.
    /// </summary>
    public class GroupBoxGroupControlVWG: PanelVWG, IGroupBoxGroupControl
    {
        private readonly GroupBoxGroupControlManager _controlManager;

        ///<summary>
        /// Constructor for <see cref="GroupBoxGroupControlVWG"/>
        ///</summary>
        public GroupBoxGroupControlVWG(IControlFactory controlFactory)
        {
            _controlManager = new GroupBoxGroupControlManager(this, controlFactory);
        }

        /// <summary>
        /// Adds an <see cref="IControlHabanero"/> to this control. The <paramref name="contentControl"/> is
        ///    wrapped in the appropriate Child Control Type.
        /// </summary>
        /// <param name="contentControl">The control that is being placed as a child within this control. The content control could be 
        ///  a Panel of <see cref="IBusinessObject"/>.<see cref="IBOProp"/>s or any other child control</param>
        /// <param name="headingText">The heading text that will be shown as the Header for this Group e.g. For a <see cref="ITabControl"/>
        ///   this will be the Text shown in the Tab for a <see cref="ICollapsiblePanelGroupControl"/> this will be the text shown
        ///   on the Collapse Panel and for an <see cref="IGroupBox"/> this will be the title of the Group Box.</param>
        /// <param name="minimumControlHeight">The minimum height that the <paramref name="contentControl"/> can be.
        ///   This height along with any other spacing required will be used as the minimum height for the ChildControlCreated</param>
        ///  <param name="minimumControlWidth">The minimum width that the <paramref name="contentControl"/> can be</param>
        ///  <returns></returns>
        public IControlHabanero AddControl(IControlHabanero contentControl, string headingText, int minimumControlHeight, int minimumControlWidth)
        {
            return _controlManager.AddControl(contentControl, headingText, minimumControlHeight, minimumControlWidth);
//            IControlFactory factory = GlobalUIRegistry.ControlFactory;
//            if (factory == null)
//            {
//                const string errMessage = "There is a serious error since the GlobalUIRegistry.ControlFactory  has not been set up.";
//                throw new HabaneroDeveloperException(errMessage, errMessage);
//            }
//            IGroupBox groupBox = factory.CreateGroupBox(headingText);
//            groupBox.Width = minimumControlWidth + 20;
//            groupBox.Height = minimumControlHeight + 20;
//            BorderLayoutManager layoutManager = factory.CreateBorderLayoutManager(groupBox);
//            layoutManager.BorderSize = 10;
//            layoutManager.AddControl(contentControl);
//
//            ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(this, factory);
//            this.Width = groupBox.Width + columnLayoutManager.BorderSize*2;
//            this.Height = groupBox.Height + columnLayoutManager.BorderSize*2;
//            columnLayoutManager.AddControl(groupBox);
//            return groupBox;
        }
//
//        /// <summary>
//        /// Adds an <see cref="IControlHabanero"/> to this control. The <paramref name="contentControl"/> is
//        ///    wrapped in the appropriate Child Control Type.
//        /// </summary>
//        /// <param name="contentControl">The control that is being placed as a child within this control. The content control could be 
//        ///  a Panel of <see cref="IBusinessObject"/>.<see cref="IBOProp"/>s or any other child control</param>
//        /// <param name="headingText">The heading text that will be shown as the Header for this Group e.g. For a <see cref="ITabControl"/>
//        ///   this will be the Text shown in the Tab for a <see cref="ICollapsiblePanelGroupControl"/> this will be the text shown
//        ///   on the Collapse Panel and for an <see cref="IGroupBox"/> this will be the title of the Group Box.</param>
//        /// <param name="minimumControlHeight">The minimum height that the <paramref name="contentControl"/> can be.
//        ///   This height along with any other spacing required will be used as the minimum height for the ChildControlCreated</param>
//        ///  <param name="minimumControlWidth">The minimum width that the <paramref name="contentControl"/> can be</param>
//        ///  <returns></returns>
//        public IControlHabanero AddControl(IControlHabanero contentControl, string headingText, int minimumControlHeight, int minimumControlWidth)
//        {
//            IControlFactory factory = GlobalUIRegistry.ControlFactory;
//            if (factory == null)
//            {
//                const string errMessage = "There is a serious error since the GlobalUIRegistry.ControlFactory  has not been set up.";
//                throw new HabaneroDeveloperException(errMessage, errMessage);
//            }
//            this.Text = headingText;
//            this.Width = minimumControlWidth;
//            this.Height = minimumControlHeight;
//            BorderLayoutManager layoutManager = factory.CreateBorderLayoutManager(this);
//            layoutManager.AddControl(contentControl);
//            return this;
//        }
    }
}