using System;
using System.Drawing;
using Habanero.Base;
using Habanero.Util;
using log4net;

namespace Habanero.BO.ClassDefinition
{
    ///<summary>
    /// Implements a data mapper for a Guid Property
    /// The property data mapper conforms to the GOF strategy pattern <seealso cref="BOPropDataMapper"/>.
    ///</summary>
    public class BOPropGeneralDataMapper : BOPropDataMapper
    {
        private readonly IPropDef _propDef;
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.BOPropGeneralDataMapper");

        ///<summary>
        /// Creates the Generalised Data Mapper with the appropriate propDef.
        ///</summary>
        ///<param name="propDef"></param>
        public BOPropGeneralDataMapper(IPropDef propDef)
        {
            _propDef = propDef;
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
                if (_propDef.PropertyType == typeof (Image))
                {
                    returnValue = SerialisationUtilities.ByteArrayToObject((byte[]) valueToParse);
                }
                else if (_propDef.PropertyType.IsSubclassOf(typeof (CustomProperty)))
                {
                    returnValue = _propDef.PropertyType.IsInstanceOfType(valueToParse) 
                        ? valueToParse 
                        : Activator.CreateInstance(_propDef.PropertyType, new[] {valueToParse, false});
                }
                else if (_propDef.PropertyType == typeof (Object))
                {
                    //valueToParse = valueToParse;
                }
                else if (_propDef.PropertyType == typeof (TimeSpan) && valueToParse.GetType() == typeof (DateTime))
                {
                    returnValue = ((DateTime) valueToParse).TimeOfDay;
                }
                else if (_propDef.PropertyType.IsEnum && valueToParse is string)
                {
                    returnValue = Enum.Parse(_propDef.PropertyType, (string) valueToParse);
                }
                else
                {
                    try
                    {
                        PropDef propDef = (PropDef) this._propDef;
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
    }
}