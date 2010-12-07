// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.ComponentModel;
using System.Drawing;
using Habanero.Base;
using Habanero.Util;
using log4net;

namespace Habanero.BO.ClassDefinition
{
    ///<summary>
    /// Implements a data mapper for a Property of an unknown type
    /// The property data mapper conforms to the GOF strategy pattern <seealso cref="BOPropDataMapper"/>.
    ///</summary>
    public class BOPropGeneralDataMapper : BOPropDataMapper
    {
        private readonly PropDef _propDef;
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.BOPropGeneralDataMapper");

        ///<summary>
        /// Creates the Generalised Data Mapper with the appropriate propDef.
        ///</summary>
        ///<param name="propDef"></param>
        public BOPropGeneralDataMapper(IPropDef propDef)
        {
            if (propDef == null) throw new ArgumentNullException("propDef");
            _propDef = (PropDef) propDef;
        }

        /// <summary>
        /// This method provides a the functionality to convert any object to the appropriate
        ///   type for the particular BOProp Type. e.g it will convert a valid guid string to 
        ///   a valid Guid Object.
        /// </summary>
        /// <param name="valueToParse">The value to be converted</param>
        /// <param name="returnValue"></param>
        /// <returns>An object of the correct type.</returns>
        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;

            try
            {
                if (this._propDef.PropertyType.IsInstanceOfType(valueToParse))
                {
                    returnValue = valueToParse;
                    return true;
                }

                if (valueToParse.GetType().Name == "MySqlDateTime")
                {
                    if (valueToParse.ToString().Trim().Length > 0)
                    {
                        returnValue = DateTime.Parse(valueToParse.ToString());
                        return true;
                    }
                    returnValue = null;
                    return false;
                }
                if (_propDef.PropertyType == typeof(Image) && valueToParse is byte[])
                {
                    returnValue = SerialisationUtilities.ByteArrayToObject((byte[]) valueToParse);
                    return true;
                }
                if (_propDef.PropertyType.IsSubclassOf(typeof(CustomProperty)) && !CanConvertUsingTypeConverter(valueToParse))
                {
                    returnValue = _propDef.PropertyType.IsInstanceOfType(valueToParse) 
                                      ? valueToParse 
                                      : Activator.CreateInstance(_propDef.PropertyType, new[] {valueToParse, false});
                    return true;
                }
                if (_propDef.PropertyType == typeof (Object))
                {
                    //valueToParse = valueToParse;
                    return true;
                }
                if (_propDef.PropertyType == typeof (TimeSpan) && valueToParse.GetType() == typeof (DateTime))
                {
                    returnValue = ((DateTime) valueToParse).TimeOfDay;
                    return true;
                }
                if (_propDef.PropertyType.IsEnum && valueToParse is string)
                {
                    returnValue = Enum.Parse(_propDef.PropertyType, (string) valueToParse);
                    return true;
                }
                if (CanConvertUsingTypeConverter(valueToParse))
                {
                    var tc = GetTypeConverter();
                    returnValue = tc.ConvertFrom(valueToParse);
                    return true;
                }

                try
                {
                    var propDef = this._propDef;
                    returnValue = propDef.GetNewValue(valueToParse);
                }
                catch (InvalidCastException)
                {
                    log.Error
                        (string.Format
                             ("Problem in InitialiseProp(): Can't convert value of type {0} to {1}",
                              valueToParse.GetType().FullName, _propDef.PropertyType.FullName));
                    string tableName = this._propDef.ClassDef == null ? "" : this._propDef.ClassDef.GetTableName(_propDef);
                    log.Error
                        (string.Format
                             ("Value: {0}, Property: {1}, Field: {2}, Table: {3}", valueToParse,
                              this._propDef.PropertyName, this._propDef.DatabaseFieldName, tableName));
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPropertyValueException
                    (String.Format
                         ("An error occurred while attempting to convert "
                          + "the loaded property value of '{0}' to its specified "
                          + "type of '{1}'. The property value is '{2}'. See log for details", _propDef.PropertyName,
                          _propDef.PropertyType, valueToParse), ex);
            }
            return true;
        }
        private bool CanConvertUsingTypeConverter(object valueToParse)
        {
            var tc = GetTypeConverter();
            return tc != null && tc.CanConvertFrom(valueToParse.GetType());
        }

        private TypeConverter GetTypeConverter()
        {
            return TypeDescriptor.GetConverter(this._propDef.PropertyType);
        }
    }
}