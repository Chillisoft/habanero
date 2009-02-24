namespace Habanero.UI.Base
{
    ///<summary>
    /// The Manager for the <see cref="ICollapsiblePanel"/> that handles the Common Logic for either VWG or Win.
    ///</summary>
    public class CollapsiblePanelManager
    {
        private readonly ICollapsiblePanel _collapsiblePanel;
        private readonly IControlFactory _controlFactory;
        private bool _collapsed;
        private readonly IButton _collapseButton;
        private bool _pinned;
        private readonly ILabel _pinLabel;
        private readonly BorderLayoutManager _layoutManager;
        private IControlHabanero _contentControl;

        /// <summary>
        /// Returns the Height required by the Panel when it is Expanded.
        /// </summary>
        public int ExpandedHeight { get; private set; }

        ///<summary>
        /// Constructor for <see cref="CollapsiblePanelManager"/>
        ///</summary>
        ///<param name="collapsiblePanel"></param>
        ///<param name="controlFactory"></param>
        public CollapsiblePanelManager(ICollapsiblePanel collapsiblePanel, IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _collapsiblePanel = collapsiblePanel;
            _collapseButton = _controlFactory.CreateButtonCollapsibleStyle();


            _collapseButton.Click += delegate { Collapsed = !Collapsed; };
            _pinLabel = controlFactory.CreateLabelPinOffStyle();
            _pinLabel.Click += delegate { Pinned = !Pinned; };

            IPanel buttonPanel = _controlFactory.CreatePanel();
            BorderLayoutManager buttonLayoutManager =
                _controlFactory.CreateBorderLayoutManager(buttonPanel);
            buttonPanel.Height = _collapseButton.Height;

            buttonLayoutManager.AddControl(_collapseButton, BorderLayoutManager.Position.Centre);
            buttonLayoutManager.AddControl(_pinLabel, BorderLayoutManager.Position.East);

            _layoutManager = _controlFactory.CreateBorderLayoutManager(collapsiblePanel);
            _layoutManager.AddControl(buttonPanel, BorderLayoutManager.Position.North);


            _collapseButton.BackColor = System.Drawing.Color.Transparent;
            _collapseButton.ForeColor = System.Drawing.Color.Transparent;
        }

        ///<summary>
       /// Gets and Sets whether the <see cref="IPanel"/> is collapsed or expanded.
       ///</summary>
        public bool Collapsed
        {
            get { return _collapsed; }
            set
            {
                _collapsed = value;
                if (_collapsed)
                {
                    ExpandedHeight = _collapsiblePanel.Height;
                    _collapsiblePanel.Height = _collapseButton.Height;
                    Pinned = false;
                }
                else
                {
                    _collapsiblePanel.Height = ExpandedHeight;
                    FireUncollapsed();
                }
            }
        }
        /// <summary>
        /// Gets and Sets the <see cref="IControlHabanero"/> that is placed on the Panel.
        /// </summary>
        public IControlHabanero ContentControl
        {
            get { return _contentControl; }
            set
            {
                _contentControl = value;
                _layoutManager.AddControl(_contentControl, BorderLayoutManager.Position.Centre);
            }
        }

        private void FireUncollapsed()
        {
            _collapsiblePanel.FireUncollapsedEvent();
        }

        ///<summary>
        /// The 
        ///</summary>
        public IButton CollapseButton
        {
            get { return _collapseButton; }
        }
        /// <summary>
        /// Gets and Sets whether the Panel is Pinned or not.
        /// </summary>
        public bool Pinned
        {
            get { return _pinned; }
            set
            {
                _pinned = value;
                if (_pinned)
                {
                    Collapsed = false;
                    _controlFactory.ConfigurePinOffStyleLabel(_pinLabel);
                    //_pinLabel.Text = "Unpin";
                }
                else
                {
                    _controlFactory.ConfigurePinOnStyleLabel(_pinLabel);
                    //_pinLabel.Text = "Pin";
                }
            }
        }
        /// <summary>
        /// Returns the PinLabel <see cref="ILabel"/> so that the Image can be changed on it for styling.
        /// </summary>
        public ILabel PinLabel
        {
            get { return _pinLabel; }
        }
    }
    
}