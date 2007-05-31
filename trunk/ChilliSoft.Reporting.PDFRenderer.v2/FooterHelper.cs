using System;
using com.lowagie.text;
using com.lowagie.text.pdf;

namespace Chillisoft.Reporting.PDFRenderer.v2
{
    /// <summary>
    /// Summary description for FooterHelper.
    /// </summary>
    public class FooterHelper : PdfPageEventHelper
    {
        /** The headertable. */
        public PdfPTable table;
        /** A template that will hold the total number of pages. */
        public PdfTemplate tpl;
        /** The font that will be used. */
        public BaseFont helv;

        public FooterHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public override void onOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                // initialization of the header table
                table = new PdfPTable(2);

                table.getDefaultCell().setBorderWidth(0);
                table.getDefaultCell().setHorizontalAlignment(ElementConst.ALIGN_RIGHT);
                // initialization of the template
                tpl = writer.getDirectContent().createTemplate(100, 100);
                tpl.setBoundingBox(new Rectangle(-20, -20, 100, 100));
                // initialization of the font
                helv = BaseFont.createFont("Helvetica", BaseFont.WINANSI, false);
            }
            catch (java.lang.Exception e)
            {
                throw new ExceptionConverter(e);
            }
        }


        public override void onEndPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.getDirectContent();
            cb.saveState();
            // write the headertable
            table.setTotalWidth(document.right() - document.left());
            table.writeSelectedRows(0, -1, document.left(), document.getPageSize().height() - 50, cb);
            // compose the footer
            String text = "Page " + writer.getPageNumber() + " of ";
            float textSize = helv.getWidthPoint(text, 8);
            float textBase = document.bottom() - 20;
            cb.beginText();
            cb.setFontAndSize(helv, 8);
            // for odd pagenumbers, show the footer at the left
            if ((writer.getPageNumber() & 1) == 1)
            {
                cb.setTextMatrix(document.left(), textBase);
                cb.showText(text);
                cb.endText();
                cb.addTemplate(tpl, document.left() + textSize, textBase);
            }
                // for even numbers, show the footer at the right
            else
            {
                float adjust = helv.getWidthPoint("0", 8);
                cb.setTextMatrix(document.right() - textSize - adjust, textBase);
                cb.showText(text);
                cb.endText();
                cb.addTemplate(tpl, document.right() - adjust, textBase);
            }
            cb.restoreState();
        }

        public override void onCloseDocument(PdfWriter writer, Document document)
        {
            tpl.beginText();
            tpl.setFontAndSize(helv, 8);
            tpl.setTextMatrix(0, 0);
            tpl.showText("" + (writer.getPageNumber() - 1));
            tpl.endText();
        }
    }
}