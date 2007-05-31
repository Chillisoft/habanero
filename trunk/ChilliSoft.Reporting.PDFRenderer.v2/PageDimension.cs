namespace Chillisoft.Reporting.PDFRenderer.v2
{
    /// <summary>
    /// Summary description for PageDimension.
    /// </summary>
    public abstract class PageDimension
    {
        protected PageDimension()
        {
        }

        public abstract double Left { get; }
        public abstract double Right { get; }
        public abstract double Top { get; }
        public abstract double Bottom { get; }

        public double Width
        {
            get { return Right - Left; }
        }

        public double Height
        {
            get { return Bottom - Top; }
        }
    }
}