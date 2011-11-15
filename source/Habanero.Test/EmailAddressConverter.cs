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
using System;
using System.ComponentModel;
using System.Globalization;

namespace Habanero.Test
{
    ///<summary>
    /// A test type converter class.
    ///</summary>
    public class EmailAddressConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string) return new EmailAddressWithTypeConverter(Convert.ToString(value));

            return base.ConvertFrom(context, culture, value);
        }
    }

    [TypeConverter(typeof(EmailAddressConverter))]
    public class EmailAddressWithTypeConverter
    {
        public EmailAddressWithTypeConverter(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
        public override string ToString()
        {
            return this.EmailAddress;
        }

        public string EmailAddress { get; private set; }
    }

}