using System.Drawing;

namespace Habanero.UI.Base
{
    public interface ILabel:IControlChilli
    {
        int PreferredWidth { get; }

        bool AutoSize { get; set; }

        Font Font { get; set; }

        ContentAlignment TextAlign { get; set; }
    }
}