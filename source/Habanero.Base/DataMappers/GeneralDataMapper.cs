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
using Habanero.Base.DataMappers;
using Habanero.Base.Logging;
using Habanero.Util;
using log4net;

namespace Habanero.Base.DataMappers
{
    ///<summary>
    /// Implements a data mapper for an unknown type. Tries to convert the value given to that type
    /// The data mapper conforms to the GOF strategy pattern <seealso cref="DataMapper"/>.
    ///</summary>
    public class GeneralDataMapper : DataMapper
    {
        private readonly Type _targetType;
        protected static readonly IHabaneroLogger Logger = GlobalRegistry.LoggerFactory.GetLogger(typeof(GeneralDataMapper));

        ///<summary>
        /// Creates the Generalised Data Mapper with the appropriate propDef.
        ///</summary>
        ///<param name="propDef"></param>
        public GeneralDataMapper(Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException("targetType");
            _targetType = targetType;
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

            if (_targetType.IsInstanceOfType(valueToParse))
            {
                returnValue = valueToParse;
                return true;
            }

            if (_targetType.IsSubclassOf(typeof(CustomProperty)) && !CanConvertUsingTypeConverter(valueToParse))
            {
                returnValue = _targetType.IsInstanceOfType(valueToParse)
                                    ? valueToParse
                                    : Activator.CreateInstance(_targetType, new[] { valueToParse, false });
                return true;
            }

            if (_targetType.IsEnum && valueToParse is string)
            {
                returnValue = Enum.Parse(_targetType, (string)valueToParse);
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
                returnValue = Convert.ChangeType(valueToParse, _targetType);
            }
            catch (InvalidCastException)
            {
                Logger.Log(string.Format
                            ("Problem in InitialiseProp(): Can't convert value of type {0} to {1}",
                            valueToParse.GetType().FullName, _targetType.FullName), LogCategory.Exception);
                throw;
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
            return TypeDescriptor.GetConverter(_targetType);
        }

    }
}