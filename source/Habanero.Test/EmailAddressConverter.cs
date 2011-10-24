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