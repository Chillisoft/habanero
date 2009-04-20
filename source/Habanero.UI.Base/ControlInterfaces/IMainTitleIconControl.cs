namespace Habanero.UI.Base
{
    /// <summary>
    /// This control is the Title Control that can be used in many ways
    /// It is typically used at the top of a form or a Panel where the Title is set to 
    /// be the Business Objects Tostring and the Icon is set to show the current 
    /// state of the Business object e.g. Valid/Warning, or to show the Unique Icon
    /// for the business object.
    /// </summary>
    public interface IMainTitleIconControl : IControlHabanero
    {
        /// <summary>
        /// Gets the Panel that the Title Lable and Icon Label are added to.
        /// </summary>
        IPanel Panel { get; }

        /// <summary>
        /// The <see cref="ILabel"/> that contains the Icon being displayed.
        /// </summary>
        ILabel Icon { get; }

        /// <summary>
        /// The <see cref="ILabel"/> that contains the Title being displayed.
        /// </summary>
        ILabel Title { get; }
        /// <summary>
        /// Gets the control factory that is used by this control to create its Icon and Panel
        /// </summary>
        IControlFactory ControlFactory { get; }

        /// <summary>
        /// Sets the Image that is shown on the <see cref="IMainTitleIconControl.Icon"/> label.
        /// </summary>
        /// <param name="image"></param>
        void SetIconImage(string image);

        /// <summary>
        /// Removes any Image shown on the <see cref="IMainTitleIconControl.Icon"/> Label
        /// </summary>
        void RemoveIconImage();

        /// <summary>
        /// Sets the Image to a standard valid image.
        /// </summary>
        void SetValidImage();

        /// <summary>
        /// Sets the Image to a standard invalid image.
        /// </summary>
        void SetInvalidImage();
    }
}