using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.UI.Base
{
    ///<summary>
    /// The Manager for the <see cref="IGroupBoxGroupControl"/> that handles the Common Logic for either VWG or Win.
    ///</summary>
    public class GroupBoxGroupControlManager
    {
        /// <summary>
        /// Constructs the <see cref="CollapsiblePanelGroupManager"/>
        /// </summary>
        // ReSharper disable SuggestBaseTypeForParameter
        public GroupBoxGroupControlManager(IGroupBoxGroupControl collapsiblePanelGroup, IControlFactory controlFactory)
        {
            this.CollapsiblePanelGroup = collapsiblePanelGroup;
            this.ControlFactory = controlFactory;
            this.LayoutManager = new ColumnLayoutManager(collapsiblePanelGroup, ControlFactory);
        }

        private LayoutManager LayoutManager { get; set; }

        private IGroupBoxGroupControl CollapsiblePanelGroup { get; set; }
        private IControlFactory ControlFactory { get; set; }

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
        public IControlHabanero AddControl
            (IControlHabanero contentControl, string headingText, int minimumControlHeight, int minimumControlWidth)
        {
            IControlFactory factory = GlobalUIRegistry.ControlFactory;
            if (factory == null)
            {
                const string errMessage =
                    "There is a serious error since the GlobalUIRegistry.ControlFactory  has not been set up.";
                throw new HabaneroDeveloperException(errMessage, errMessage);
            }
            IGroupBox groupBox = factory.CreateGroupBox(headingText);
            groupBox.Width = minimumControlWidth + 30;
            groupBox.Height = minimumControlHeight + 30;
            BorderLayoutManager layoutManager = factory.CreateBorderLayoutManager(groupBox);
            layoutManager.BorderSize = 20;
            layoutManager.AddControl(contentControl);

            CollapsiblePanelGroup.Width = groupBox.Width + LayoutManager.BorderSize * 2;
            CollapsiblePanelGroup.Height = groupBox.Height + LayoutManager.BorderSize * 2;
            LayoutManager.AddControl(groupBox);
            return groupBox;
        }
    }
}