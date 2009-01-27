using System.ComponentModel;
using System.Drawing;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Specifies how an image is positioned within a <see cref="IPictureBox"></see>.
    /// </summary>
    //[Serializable()]
    public enum PictureBoxSizeMode
    {
        /// <summary>
        /// The <see cref="IPictureBox"></see> is sized equal to the size of the image that it contains.
        /// </summary>
        AutoSize = 2,
        /// <summary>
        /// The image is displayed in the center if the <see cref="IPictureBox"></see> is larger than the image. If the image is larger than the <see cref="IPictureBox"></see>, the picture is placed in the center of the <see cref="IPictureBox"></see> and the outside edges are clipped.
        /// </summary>
        CenterImage = 3,
        /// <summary>
        /// The image is placed in the upper-left corner of the <see cref="IPictureBox"></see>. The image is clipped if it is larger than the <see cref="IPictureBox"></see> it is contained in.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// The image within the <see cref="IPictureBox"></see> is stretched or shrunk to fit the size of the <see cref="TIPictureBox"></see>.
        /// </summary>
        StretchImage = 1
    }

    /// <summary>
    /// Represents a PictureBox control
    /// </summary>
    public interface IPictureBox : IControlHabanero
    {
        /// <summary>
        /// Indicates how the image is displayed.
        /// </summary>
        ///	<returns>One of the <see cref="Habanero.UI.Base.PictureBoxSizeMode"></see> values. The default is <see cref="Habanero.UI.Base.PictureBoxSizeMode.Normal"></see>.</returns>
        ///	<exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="Habanero.UI.Base.PictureBoxSizeMode"></see> values. </exception>
        //[DefaultValue(0), Localizable(true), SRDescription("PictureBoxSizeModeDescr"), SRCategory("CatBehavior"), RefreshProperties(RefreshProperties.Repaint)]
        PictureBoxSizeMode SizeMode { get; set; }

        /// <summary>
        /// Gets or sets the image that is displayed by <see cref="IPictureBox"></see>.
		/// </summary>
        /// <returns>The <see cref="T:System.Drawing.Image"></see> to display.</returns>
        //[SRDescription("PictureBoxImageDescr"), Localizable(true), Bindable(true), SRCategory("CatAppearance")]
        Image Image { get; set; }
    }
}