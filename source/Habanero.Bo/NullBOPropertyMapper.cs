using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// This is a mapper class that handles a null mapping of a property for a specified <see cref="IBusinessObject"/>.
    /// </summary>
    public class NullBOPropertyMapper : IBOPropertyMapper
    {
        public NullBOPropertyMapper(string propertyName, string invalidReason)
        {
            PropertyName = propertyName;
            InvalidReason = invalidReason;
        }

        public IBusinessObject BusinessObject { get; set; }
        public string PropertyName { get; private set; }
        public string InvalidReason { get; private set; }

        public void SetPropertyValue(object propValue)
        {
            //Do Nothing
        }

        public object GetPropertyValue()
        {
            return null;
        }
    }
}