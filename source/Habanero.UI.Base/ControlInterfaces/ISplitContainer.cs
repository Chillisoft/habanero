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
namespace Habanero.UI.Base
{

    //TODO Mark 17 Jul 2009: Need to implement this interface's properties

    ///// <summary>
    ///// Specifies the orientation of controls or elements of controls.
    ///// </summary>
    ////[Serializable()]
    //public enum Orientation
    //{
    //    /// <summary>
    //    /// The control or element is oriented horizontally.
    //    /// </summary>
    //    Horizontal,
    //    /// <summary>
    //    /// The control or element is oriented vertically.
    //    /// </summary>
    //    Vertical
    //}

    ////[Serializable()]
    /////<summary>
    ///// Specifies that ISplitContainer.Panel1, ISplitContainer.Panel2, or neither panel is fixed.
    /////</summary>
    //public enum FixedPanel
    //{
    //    /// <summary>
    //    /// Specifies that neither ISplitContainer.Panel1, ISplitContainer.Panel2 is fixed. A Control.Resize event affects both panels.
    //    /// </summary>
    //    None,
    //    /// <summary>
    //    /// Specifies that ISplitContainer.Panel1 is fixed. A Control.Resize event affects only ISplitContainer.Panel2.
    //    /// </summary>
    //    Panel1,
    //    /// <summary>
    //    /// Specifies that ISplitContainer.Panel2 is fixed. A Control.Resize event affects only ISplitContainer.Panel1.
    //    /// </summary>
    //    Panel2
    //}

    /// <summary>
    /// Represents a Container that has a Left Panel (Panel1) and a 
    /// Right Panel (Panel2) with a splitter bar seperting these two panels.
    /// </summary>
    public interface ISplitContainer : IControlHabanero
    {
        //////[SRDescription("SplitterSplitterMovedDescr"), SRCategory("CatBehavior")]
        ////event SplitterEventHandler SplitterMoved;

        //////[SRDescription("SplitterSplitterMovingDescr"), SRCategory("CatBehavior")]
        ////event SplitterCancelEventHandler SplitterMoving;

        ////[SRCategory("CatLayout"), SRDescription("SplitContainerFixedPanelDescr")]
        //[DefaultValue(0)]
        //FixedPanel FixedPanel { get; set; }

        ////[SRCategory("CatLayout"), SRDescription("SplitContainerIsSplitterFixedDescr")]
        //[DefaultValue(false), Localizable(true)]
        //bool IsSplitterFixed { get; set; }

        ////[SRCategory("CatBehavior"), SRDescription("SplitContainerOrientationDescr")]
        //[DefaultValue(1), Localizable(true)]
        //Orientation Orientation { get; set; }

        ////[SRCategory("CatAppearance"), SRDescription("SplitContainerPanel1Descr")]
        //[Localizable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //IPanel Panel1 { get; }

        ////[SRDescription("SplitContainerPanel1CollapsedDescr"), SRCategory("CatLayout")]
        //[DefaultValue(false)]
        //bool Panel1Collapsed { get; set; }

        ////[SRCategory("CatLayout"), SRDescription("SplitContainerPanel1MinSizeDescr")]
        //[Localizable(true), DefaultValue(0x19), RefreshProperties(RefreshProperties.All)]
        //int Panel1MinSize { get; set; }

        ////[SRCategory("CatAppearance"), SRDescription("SplitContainerPanel2Descr")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Localizable(false)]
        //IPanel Panel2 { get; }

        ////[SRCategory("CatLayout"), SRDescription("SplitContainerPanel2CollapsedDescr")]
        //[DefaultValue(false)]
        //bool Panel2Collapsed { get; set; }

        ////[SRCategory("CatLayout"), SRDescription("SplitContainerPanel2MinSizeDescr")]
        //[RefreshProperties(RefreshProperties.All), DefaultValue(0x19), Localizable(true)]
        //int Panel2MinSize { get; set; }

        ////[SRCategory("CatLayout"), SRDescription("SplitContainerSplitterDistanceDescr")]
        //[DefaultValue(50), Localizable(true)]
        //[SettingsBindable(true)]
        //int SplitterDistance { get; set; }

        ////[SRDescription("SplitContainerSplitterIncrementDescr"), SRCategory("CatLayout")]
        //[DefaultValue(1), Localizable(true)]
        //int SplitterIncrement { get; set; }

        ////[SRDescription("SplitContainerSplitterRectangleDescr"), SRCategory("CatLayout")]
        //[Browsable(false)]
        //Rectangle SplitterRectangle { get; }

        ////[SRDescription("SplitContainerSplitterWidthDescr"), SRCategory("CatLayout")]
        //[Localizable(true), DefaultValue(4)]
        //int SplitterWidth { get; set; }
    }
}