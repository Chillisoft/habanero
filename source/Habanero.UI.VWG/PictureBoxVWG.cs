using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    internal class PictureBoxVWG : PictureBox,IPictureBox
    {
        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (Gizmox.WebGUI.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (Gizmox.WebGUI.Forms.DockStyle)value; }
        }

        #region Implementation of IPictureBox

        /// <summary>
        /// Indicates how the image is displayed.
        /// </summary>
        ///	<returns>One of the <see cref="Habanero.UI.Base.PictureBoxSizeMode"></see> values. The default is <see cref="Habanero.UI.Base.PictureBoxSizeMode.Normal"></see>.</returns>
        ///	<exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="Habanero.UI.Base.PictureBoxSizeMode"></see> values. </exception>
        //[DefaultValue(0), Localizable(true), SRDescription("PictureBoxSizeModeDescr"), SRCategory("CatBehavior"), RefreshProperties(RefreshProperties.Repaint)]
        Base.PictureBoxSizeMode IPictureBox.SizeMode
        {
            get { return (Base.PictureBoxSizeMode)base.SizeMode; }
            set { base.SizeMode = (Gizmox.WebGUI.Forms.PictureBoxSizeMode)value; }
        }

        /// <summary>
        /// Gets or sets the image that is displayed by <see cref="IPictureBox"></see>.
        /// </summary>
        /// <returns>The <see cref="T:System.Drawing.Image"></see> to display.</returns>
        //[SRDescription("PictureBoxImageDescr"), Localizable(true), Bindable(true), SRCategory("CatAppearance")]
        Image IPictureBox.Image
        {
            get { return this.Image.ToImage(); }
            set { this.Image = value; }
        }

        #endregion
    }
}