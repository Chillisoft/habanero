namespace Habanero.Base
{
    /// <summary>
    /// A super-class for a property that may be customised before
    /// persistance to the database
    /// </summary>
    public abstract class CustomProperty
    {
        /// <summary>
        /// Constructor to initialise the property
        /// </summary>
        /// <param name="value">The value to customise</param>
        /// <param name="isLoading">Whether the value is being loaded from
        /// the database, rather than being prepared to send to the database.
        /// This might determine whether the object is in its normal or
        /// customised form.
        /// </param>
        public CustomProperty(object value, bool isLoading)
        {
        }

        /// <summary>
        /// Returns the property in appropriate form to be persisted to the
        /// database
        /// </summary>
        /// <returns>Returns the property being held</returns>
        public abstract object GetPersistValue();
    }
}