using System.Drawing;

namespace Habanero.UI.Base
{
    public interface ICheckBox:IControlChilli
    {
        bool Checked { get; set; }

        ContentAlignment CheckAlign { get; set; }
    }
}
