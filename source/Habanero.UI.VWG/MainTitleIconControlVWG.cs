using System;
using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using Color=System.Drawing.Color;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// This control is the Title Control that is is present at the top of the right hand panel
    /// This control is used to provide context for the Business Object being shown in the Panel.
    /// </summary>
    public class MainTitleIconControlVWG : UserControlVWG, IMainTitleIconControl
    {
        /// <summary>
        /// The panel that contains two labels one showing the Icon and one showing the
        /// Title.
        /// </summary>
        private readonly IPanel _panel;
        private readonly ILabel _icon;
        private readonly ILabel _title;
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// Constructs a <see cref="MainTitleIconControlVWG"/>
        /// </summary>
        public MainTitleIconControlVWG() : this(GlobalUIRegistry.ControlFactory)
        { }
        /// <summary>
        /// Constructs a <see cref="MainTitleIconControlVWG"/>
        /// </summary>
        public MainTitleIconControlVWG(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _controlFactory = controlFactory;
            _panel = _controlFactory.CreatePanel();
            ((PanelVWG)_panel).BackgroundImage = "Images.headergradient.png";
            _panel.BackColor = Color.Transparent;
            _panel.Dock = Habanero.UI.Base.DockStyle.Top;

            _panel.Height = 23;

            _icon = _controlFactory.CreateLabel();
            ((LabelVWG)_icon).BackgroundImage = "";
            _icon.BackColor = Color.Transparent;
            ((LabelVWG)_icon).BackgroundImageLayout = ImageLayout.Center;
            _icon.Dock = Habanero.UI.Base.DockStyle.Left;
            _icon.Size = new Size(20, 20);

            _title = _controlFactory.CreateLabel();
            _title.Dock = Habanero.UI.Base.DockStyle.Fill;
            _title.BackColor = Color.Transparent;
            _title.TextAlign = ContentAlignment.MiddleLeft;
            _title.ForeColor = Color.White;

            _panel.Controls.Add(_title);
            _panel.Controls.Add(_icon);

            this.Dock = DockStyleVWG.GetDockStyle(DockStyle.Top);
            this.Controls.Add((PanelVWG)_panel);
            this.Height = 23;
        }

        /// <summary>
        /// Gets the Panel that the Title Lable and Icon Label are added to.
        /// </summary>
        public IPanel Panel
        {
            get { return _panel; }
        }
        /// <summary>
        /// The <see cref="ILabel"/> that contains the Icon being displayed.
        /// </summary>
        public ILabel Icon
        {
            get { return _icon; }
        }
        /// <summary>
        /// The <see cref="ILabel"/> that contains the Title being displayed.
        /// </summary>
        public ILabel Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets the control factory that is used by this control to create its Icon and Panel
        /// </summary>
        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        /// <summary>
        /// Sets the Image that is shown on the <see cref="Icon"/> label.
        /// </summary>
        /// <param name="image"></param>
        public void SetIconImage(string image)
        {
            ((LabelVWG)_icon).BackgroundImage = image;
        }
        /// <summary>
        /// Removes any Image shown on the <see cref="Icon"/> Label
        /// </summary>
        public void RemoveIconImage()
        {
            SetIconImage("");
        }

        /// <summary>
        /// Sets the Image to a standard valid image.
        /// </summary>
        public void SetValidImage()
        {
            //            ((LabelVWG)_icon).BackgroundImage = "Images.Valid.gif";
            SetIconImage("Images.Valid.gif");
            _icon.Size = new System.Drawing.Size(20, 20);
        }
        /// <summary>
        /// Sets the Image to a standard invalid image.
        /// </summary>
        public void SetInvalidImage()
        {
            //            ((LabelVWG)_icon).BackgroundImage = "Images.Invalid.gif";
            SetIconImage("Images.Invalid.gif");
            _icon.Size = new System.Drawing.Size(20, 20);
        }

    }
}