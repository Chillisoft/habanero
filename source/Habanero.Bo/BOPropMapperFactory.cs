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
        private const string RELATIONSHIP_SEPARATOR = ".";

        /// <summary>
        /// Creates the appropriate PropertyMapper based on the BusinessObject and propertyName.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IBOPropertyMapper CreateMapper(IBusinessObject businessObject, string propertyName)
        {
            if (IsReflectiveProp(propertyName))
            {
                if (propertyName.Contains(RELATIONSHIP_SEPARATOR))
                {
                    IBusinessObject relatedBo = businessObject;
                    //Get the first property name
                    string relationshipName = propertyName.Substring(0, propertyName.IndexOf("."));
                    propertyName = propertyName.Remove(0, propertyName.IndexOf(".") + 1);
                    relatedBo = relatedBo.Relationships.GetRelatedObject(relationshipName);
                    if (relatedBo == null)
                    {
                        return null;
                        //throw new HabaneroApplicationException("Unable to retrieve property " + propertyName + " from a business object of type " + this._businessObject.GetType().Name);
                    }
                    return new ReflectionPropertyMapper(propertyName) { BusinessObject = relatedBo };
                }
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