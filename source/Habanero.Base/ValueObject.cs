namespace Habanero.Base
{
    /// <summary>
    /// A super-class for a property that may be customised before
    /// persistance to the database
    /// </summary>
    public abstract class ValueObject: CustomProperty
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
        public ValueObject(object value, bool isLoading):base(value, isLoading)
        {
        }

        /// <summary>
        /// Provides a base method for implementing rules to determine whether this object is valid or not.
        /// </summary>
        /// <returns></returns>
        public abstract Result IsValid();


    }
}