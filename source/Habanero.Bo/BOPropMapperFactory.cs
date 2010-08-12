using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// A Factory for creating the appropriate <see cref="BOPropertyMapper"/>
    /// depending on the propertyName.
    /// </summary>
    public static class BOPropMapperFactory
    {
        /// <summary>
        /// Creates the appropriate PropertyMapper based on the BusinessObject and propertyName.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IBOPropertyMapper CreateMapper(IBusinessObject businessObject, string propertyName)
        {
            if(IsReflectiveProp(propertyName))
            {
                return new ReflectionPropertyMapper(propertyName) { BusinessObject = businessObject };
            }
            try
            {
                return new BOPropertyMapper(propertyName) { BusinessObject = businessObject };
            }
            catch (InvalidPropertyException)
            {
                //If the BOProp is not found then try a ReflectiveProperty.
                return new ReflectionPropertyMapper(propertyName) { BusinessObject = businessObject };
            }
        }

        private static bool IsReflectiveProp(string propertyName)
        {
            bool isDefinedAsReflective = propertyName.IndexOf("-") != -1;
            return isDefinedAsReflective;
        }
    }
}