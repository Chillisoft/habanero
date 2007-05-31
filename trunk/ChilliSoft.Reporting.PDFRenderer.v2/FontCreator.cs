using Chillisoft.Reporting.v2;
using com.lowagie.text;

namespace Chillisoft.Reporting.PDFRenderer.v2
{
    /// <summary>
    /// Summary description for FontCreator.
    /// </summary>
    public class FontCreator
    {
        public static Font CreateFontForLayout(Layout layout)
        {
            int fontStyle;
            if (layout.FontBold && layout.FontItalic)
            {
                fontStyle = Font.BOLDITALIC;
            }
            else if (layout.FontBold)
            {
                fontStyle = Font.BOLD;
            }
            else if (layout.FontItalic)
            {
                fontStyle = Font.ITALIC;
            }
            else
            {
                fontStyle = Font.NORMAL;
            }
            return FontFactory.getFont(layout.FontName, layout.FontSize, fontStyle, layout.ForeColor);
        }
    }
}