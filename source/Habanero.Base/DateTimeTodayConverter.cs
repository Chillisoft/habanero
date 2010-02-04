using System;
using System.ComponentModel;
using System.Globalization;

namespace Habanero.Base
{
    ///<summary>
    /// The Type Converter class for conversion of a DateTimeNow value to a DateTime.
    ///</summary>
    public class DateTimeTodayConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. 
        ///                 </param><param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to convert to. 
        ///                 </param>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(DateTime)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. 
        ///                 </param><param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current culture is assumed. 
        ///                 </param><param name="value">The <see cref="T:System.Object"/> to convert. 
        ///                 </param><param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to. 
        ///                 </param><exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter is null. 
        ///                 </exception><exception cref="T:System.NotSupportedException">The conversion cannot be performed. 
        ///                 </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(DateTime)) return DateTimeToday.Value;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}