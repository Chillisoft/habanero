using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// This is a mapper class that handles a null mapping of a property for a specified <see cref="IBusinessObject"/>.
    /// </summary>
    public class NullBOPropertyMapper : IBOPropertyMapper
    {
        /// <summary>
        /// Constructor. Nothing is done with the values except for storing them.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="invalidReason"></param>
        public NullBOPropertyMapper(string propertyName, string invalidReason)
        {
            PropertyName = propertyName;
            InvalidReason = invalidReason;
        }

        ///<summary>
        /// The BusinessObject for which the Property is being mapped.
        ///</summary>
        ///<exception cref="HabaneroDeveloperException">This is thrown if the specified property does not exist on the <see cref="IBusinessObject"/> being set or if one of the Relationships within the Property Path is not a single relationship.</exception>
        public IBusinessObject BusinessObject { get; set; }

        ///<summary>
        /// The name of the property to be mapped. 
        /// This could also be in the form of a path through single relationships on the BO.
        /// See <see cref="BOPropertyMapper"/> for more details.
        ///</summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// If the Property is invalid then returns the Invalid reason.
        /// </summary>
        public string InvalidReason { get; private set; }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="propValue"></param>
        public void SetPropertyValue(object propValue)
        {
            //Do Nothing
        }

        /// <summary>
        /// Returns null
        /// </summary>
        /// <returns></returns>
        public object GetPropertyValue()
        {
            return null;
        }
    }
}