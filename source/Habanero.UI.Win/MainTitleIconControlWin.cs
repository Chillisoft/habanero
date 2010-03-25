// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.UI.Base;
using DockStyle=Habanero.UI.Base.DockStyle;

namespace Habanero.UI.Win
{
    /// <summary>
    /// This control is the Title Control that is is present at the top of the right hand panel
    /// This control is used to provide context for the Business Object being shown in the Panel.
    /// </summary>
    public class MainTitleIconControlWin : UserControlWin, IMainTitleIconControl
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
        /// Constructs a <see cref="MainTitleIconControlWin"/>
        /// </summary>
        public MainTitleIconControlWin(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _controlFactory = controlFactory;
            _panel = _controlFactory.CreatePanel();
            ((PanelWin)_panel).BackgroundImage = CollapsiblePanelResource.headergradient;
            _panel.BackColor = Color.Transparent;
            _panel.Dock = Habanero.UI.Base.DockStyle.Top;

            _panel.Height = 23;

            _icon = _controlFactory.CreateLabel();
            ((LabelWin)_icon).BackgroundImage = CollapsiblePanelResource.headergradient;
            _icon.BackColor = Color.Transparent;
            ((LabelWin)_icon).BackgroundImageLayout = ImageLayout.Center;
            _icon.Dock = Habanero.UI.Base.DockStyle.Left;
            _icon.Size = new Size(20, 20);

            _title = _controlFactory.CreateLabel();
            _title.Dock = Habanero.UI.Base.DockStyle.Fill;
            _title.BackColor = Color.Transparent;
            _title.TextAlign = ContentAlignment.MiddleLeft;
            _title.ForeColor = Color.White;

            _panel.Controls.Add(_title);
            _panel.Controls.Add(_icon);
            _panel.MaximumSize = new Size(2000, 23);
            this.Dock = DockStyleWin.GetDockStyle(DockStyle.Top);
            this.Controls.Add((PanelWin)_panel);
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
            //TODO Brett 20 Apr 2009: Nubb To figure out:  ((LabelWin)_icon).BackgroundImage = image;
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
            //            ((LabelWin)_icon).BackgroundImage = "Images.Valid.gif";
            SetIconImage("Images.Valid.gif");
            _icon.Size = new System.Drawing.Size(20, 20);
        }
        /// <summary>
        /// Sets the Image to a standard invalid image.
        /// </summary>
        public void SetInvalidImage()
        {
            //            ((LabelWin)_icon).BackgroundImage = "Images.Invalid.gif";
            SetIconImage("Images.Invalid.gif");
            _icon.Size = new System.Drawing.Size(20, 20);
        }

    }
}