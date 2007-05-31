namespace Chillisoft.Reporting.PDFRenderer.v2
{
    /// <summary>
    /// Summary description for PageDimensionPortrait.
    /// </summary>
    public class PageDimensionPortrait : PageDimension
    {
        public PageDimensionPortrait()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override double Left
        {
            get { return 20; }
        }

        public override double Right
        {
            get { return 195; }
        }

        public override double Top
        {
            get { return 24; }
        }

        public override double Bottom
        {
            get { return 278; }
        }
    }
}