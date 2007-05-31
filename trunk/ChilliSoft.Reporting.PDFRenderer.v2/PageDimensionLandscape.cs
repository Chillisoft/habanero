namespace Chillisoft.Reporting.PDFRenderer.v2
{
    /// <summary>
    /// Summary description for PageDimensionLandscape.
    /// </summary>
    public class PageDimensionLandscape : PageDimension
    {
        public PageDimensionLandscape()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override double Left
        {
            get { return 24; }
        }

        public override double Right
        {
            get { return 278; }
        }

        public override double Top
        {
            get { return 20; }
        }

        public override double Bottom
        {
            get { return 195; }
        }
    }
}