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
    /// Indicates how to dock the control within its container and how the control
    /// resizes when its parent is resized 
    /// </summary>
    public enum DockStyle
    {
        /// <summary>
        /// The control is not docked.
        /// </summary>
        None = 0,
        /// <summary>
        /// All the control's edges are docked to the all edges of its containing control and sized appropriately.
        /// </summary>
        Fill = 1,
        /// <summary>
        /// The control's top edge is docked to the top of its containing control.
        /// </summary>
        Top = 2,
        /// <summary>
        /// The control's right edge is docked to the right edge of its containing control.
        /// </summary>
        Right = 3,
        /// <summary>
        /// The control's bottom edge is docked to the bottom of its containing control.
        /// </summary>
        Bottom = 4,
        /// <summary>
        /// The control's left edge is docked to the left edge of its containing control.
        /// </summary>
        Left = 5
    }

    /// <summary>
    /// Control anchoring styles
    /// </summary>
    [Flags]
    public enum AnchorStyles
    {
        // Fields
        Bottom = 2,
        Left = 4,
        None = 0,
        Right = 8,
        Top = 1
    }


    /// <summary>
    /// Defines controls, which are components with visual representation
    /// </summary>
    public interface IControlHabanero
    {
        /// <summary>
        /// Occurs when the control is clicked
        /// </summary>
        event EventHandler Click;

        /////// <summary>
        /////// Occurs when a key is pressed while the control has focus
        /////// </summary>
        //event KeyPressEventHandler KeyPress;
        /////// <summary>
        /////// Occurs when a key is released while the control has focus
        /////// </summary>
        //event KeyPressEventHandler KeyUp;
        /////// <summary>
        /////// Occurs when a key is pressed while the control has focus
        /////// </summary>
        //event KeyPressEventHandler KeyDown;

        /// <summary>
        /// Occurs when the control is double-clicked
        /// </summary>
        event EventHandler DoubleClick;

        /// <summary>
        /// Occurs when the control is resized
        /// </summary>
        event EventHandler Resize;

        /// <summary>
        /// Occurs when the Visible property value changes
        /// </summary>
        event EventHandler VisibleChanged;

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        AnchorStyles Anchor { get; set; }

        /// <summary>
        /// Gets or sets the width of the control
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Width { get; set; }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection Controls { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control is displayed
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the tab order of the control within its container
        /// </summary>
        int TabIndex { get; set; }

        /// <summary>
        /// Sets focus to this control
        /// </summary>
        /// <returns>true if the input focus request was successful; otherwise, false.</returns>
        bool Focus();

        /// <summary>Gets a value indicating whether the control has input focus.</summary>
        /// <returns>true if the control has focus; otherwise, false.</returns>
        bool Focused { get; }

        /// <summary>
        /// Gets or sets the height of the control
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the distance, in pixels, between the top edge of the
        /// control and the top edge of its container's client area
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Top { get; set; }

        /// <summary>
        /// Gets the distance, in pixels, between the bottom edge of the
        /// control and the top edge of its container's client area
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Bottom { get; }

        /// <summary>
        /// Gets or sets the distance, in pixels, between the left edge of the
        /// control and the left edge of its container's client area
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Left { set; get; }

        /// <summary>
        /// Gets the distance, in pixels, between the right edge of the
        /// control and the left edge of its container's client area
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int Right { get; }

        /// <summary>
        /// Gets or sets the text associated with this control
        /// </summary>
        [System.ComponentModel.DefaultValue("")]
        [System.ComponentModel.Localizable(true)]
        [System.ComponentModel.Bindable(true)]
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the name of the control
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DefaultValue("")]
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the control
        /// </summary>
        Color ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the background color for the control
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the
        /// focus to this control using the TAB key
        /// </summary>
        /// <value>true if the user can give the focus to the control using the
        /// TAB key; otherwise, false. The default is true.This property will
        /// always return true for an instance of the Form class.
        /// </value>
        [System.ComponentModel.DefaultValue(true)]
        bool TabStop { get; set; }

        /// <summary>
        /// Gets or sets the height and width of the control
        /// </summary>
        /// <value>The System.Drawing.Size that represents the height
        /// and width of the control in pixels</value>
        Size Size { get; set; }

        /// <summary>
        /// Activates the control  
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
        /// Gets a value indicating whether the control contains one or more child controls
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this control has children; otherwise, <c>false</c>.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        bool HasChildren { get; }

        /// <summary>
        /// Gets or sets the size that is the upper limit that
        /// GetPreferredSize(System.Drawing.Size) can specify
        /// </summary>
        /// <returns>An ordered pair of type System.Drawing.Size
        /// representing the width and height of a rectangle</returns>
        /// <filterpriority>1</filterpriority>
        [System.ComponentModel.AmbientValue(typeof (Size), "0, 0")]
        Size MaximumSize { get; set; }

        /// <summary>
        /// Gets or sets the size that is the lower limit that
        /// GetPreferredSize(System.Drawing.Size) can specify
        /// </summary>
        /// <returns>An ordered pair of type System.Drawing.Size
        /// representing the width and height of a rectangle</returns>
        /// <filterpriority>1</filterpriority>
        Size MinimumSize { get; set; }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control
        /// </summary>
        /// <value>The System.Drawing.Font to apply to the text displayed
        /// by the control. The default is the value of the DefaultFont property.</value>
        Font Font { get; set; }

        ///// <summary>
        ///// Gets or sets the width of the border.
        ///// </summary>
        ///// <value></value>
        //[System.ComponentModel.DefaultValue(1)]
        //int BorderWidth { get; set; }

        /// <summary>
        /// Temporarily suspends the layout logic for the control
        /// </summary>
        void SuspendLayout();

        /// <summary>
        /// Resumes usual layout logic, optionally forcing an immediate
        /// layout of pending layout requests
        /// </summary>
        void ResumeLayout(bool performLayout);

        /// <summary>
        /// Invalidates the entire surface of the control and causes the control to be redrawn
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the
        /// control relative to the upper-left corner of its container
        /// </summary>
        /// <value>The System.Drawing.Point that represents the upper-left
        /// corner of the control relative to the upper-left corner of its container</value>
        Point Location { get; set; }

        ///// <summary>
        ///// Gets or sets the border style.
        ///// </summary>
        ///// <value></value>
        //ControlBorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        DockStyle Dock { get; set; }

        /// <summary>
        /// Releases all resources used by the Component.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Occurs when the .Text property value changes
        /// </summary>
        event EventHandler TextChanged;
    }
}