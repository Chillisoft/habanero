using System.Drawing;

namespace Habanero.UI.Base
{
    public interface ICheckBox:IControlChilli
    {
        /// <summary>
        /// Is the check box checked
        /// </summary>
        bool Checked { get; set; }
        /// <summary>
        /// alignment of the check box left or right
        /// </summary>
        ContentAlignment CheckAlign { get; set; }
    }
}
