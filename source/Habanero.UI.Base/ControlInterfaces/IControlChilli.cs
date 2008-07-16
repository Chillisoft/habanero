//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Drawing;
using Habanero.UI.Base.ControlInterfaces;

namespace Habanero.UI.Base
{

    ///// <summary>
    ///// Border style types
    ///// </summary>
    //public enum ControlBorderStyle
    //{
    //    Clear,
    //    FixedSingle,
    //    Dashed,
    //    Dotted,
    //    Inset,
    //    Outset,
    //    Fixed3D,
    //    None
    //}

    /// <summary>
    /// The DockStyle of a control, which indicates how to dock the control within its container.
    /// </summary>
    public enum DockStyle
    {
        None = 0,
        Fill = 1,
        Top = 2,
        Right = 3,
        Bottom = 4,
        Left = 5
    }

    public interface IControlChilli 
    {
        /// <summary>
        /// Occurs on clicking the button etc.
        /// </summary>
        event EventHandler Click;

        /////// <summary>
        /////// Occurs on key press.
        /////// </summary>
        //event KeyPressEventHandler KeyPress;
        /////// <summary>
        /////// Occurs on key Up.
        /////// </summary>
        //event KeyPressEventHandler KeyUp;
        /////// <summary>
        /////// Occurs on key Down.
        /////// </summary>
        //event KeyPressEventHandler KeyDown;

        /// <summary>
        /// Occurs when the control is double clicked.
        /// </summary>
        event EventHandler DoubleClick;

        event EventHandler Resize;
        event EventHandler VisibleChanged;
        /// <summary>
        /// Gets/Sets the width position
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Width { get; set; }

        IControlCollection Controls { get; }
        /// <summary>
        /// Gets or sets the control visability.  
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        bool Visible { get; set; }

        /// <summary>
        /// The order in which tabbing through the form will tab to this control
        /// </summary>
        int TabIndex { get; set; }

        /// <summary>
        /// Gets/Sets the height position
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Height { get; set; }

        /// <summary>
        /// Gets/Sets the top position
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Top { get; set; }
        /// <summary>
        /// Gets/Sets the bottom position
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Bottom { get; }

        /// <summary>
        /// Gets/Sets the left position
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Left { set; get; }

        /// <summary>
        /// Gets/Sets the right position
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Right { get; }
        /// <summary>
        /// Gets or sets the text associated with this control.  
        /// </summary>
        [System.ComponentModel.DefaultValue("")]
        [System.ComponentModel.Localizable(true)]
        [System.ComponentModel.Bindable(true)]
        string Text { get; set; }
        /// <summary>
        /// Gets or sets the name associated with this control.  
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DefaultValue("")]
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the control enabled state.  
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        bool Enabled { get; set; }

        Color ForeColor { get; set; }

        Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tab stop is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if tab stop is enabled; otherwise, <c>false</c>.
        /// </value>
        [System.ComponentModel.DefaultValue(true)]
        bool TabStop { get; set; }
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value></value>
        Size Size { get; set; }
        /// <summary>
        /// Activates the control.  
        /// </summary>
        void Select();

        ///// <summary>
        ///// Gets a value indicating whether this instance has controls.
        ///// </summary>
        ///// <value>
        ///// 	<c>true</c> if this instance has controls; otherwise, <c>false</c>.
        ///// </value>
        //[System.ComponentModel.Browsable(false)]
        //bool HasControls { get; }
        /// <summary>
        /// Gets a value indicating whether this control has children.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this control has children; otherwise, <c>false</c>.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        bool HasChildren { get; }

        /// <summary>Gets or sets the size that is the upper limit that can specify.</summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size"></see> representing the width and height of a rectangle.</returns>
        /// <filterpriority>1</filterpriority>
        [System.ComponentModel.AmbientValue(typeof(Size), "0, 0")]
        Size MaximumSize { get; set; }

        /// <summary>Gets or sets the size that is the lower limit that can specify.</summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size"></see> representing the width and height of a rectangle.</returns>
        /// <filterpriority>1</filterpriority>
        Size MinimumSize { get; set; }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value></value>
        Font Font { get; set; }

        ///// <summary>
        ///// Gets or sets the width of the border.
        ///// </summary>
        ///// <value></value>
        //[System.ComponentModel.DefaultValue(1)]
        //int BorderWidth { get; set; }

        /// <summary>
        /// Suspends the layout.
        /// </summary>
        void SuspendLayout();

        /// <summary>
        /// Resumes the layout.
        /// </summary>
        void ResumeLayout(bool performLayout);

        /// <summary>Invalidates the entire surface of the control and causes the control to be redrawn.</summary>
        void Invalidate();
        
        /// <summary>
        /// Gets or sets the control location.
        /// </summary>
        /// <value></value>
        Point Location{ get; set; }

        ///// <summary>
        ///// Gets or sets the border style.
        ///// </summary>
        ///// <value></value>
        //ControlBorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the docking style of this control - this can be none, top, bottom, left, right or fill, 
        /// depending on how you want your  control to dock inside its container control
        /// See <see cref="DockStyle"/>
        /// </summary>
        DockStyle Dock { get; set; }
        
    }
}
