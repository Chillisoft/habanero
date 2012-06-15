#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
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
                IBusinessObject relatedBo = businessObject;
                while (propertyName.Contains(RELATIONSHIP_SEPARATOR))
                {
                    //Get the first property name
                    string relationshipName = propertyName.Substring(0, propertyName.IndexOf("."));
                    propertyName = propertyName.Remove(0, propertyName.IndexOf(".") + 1);
                    relatedBo = relatedBo.Relationships.GetRelatedObject(relationshipName);
                    if (relatedBo == null)
                    {
                        return null;
                        //throw new HabaneroApplicationException("Unable to retrieve property " + propertyName + " from a business object of type " + this._businessObject.GetType().Name);
                    }
                }
                return new ReflectionPropertyMapper(propertyName) { BusinessObject = relatedBo };
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