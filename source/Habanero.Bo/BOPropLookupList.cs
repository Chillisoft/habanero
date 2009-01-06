using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    ///<summary>
    ///</summary>
    public class BOPropLookupList : BOProp
    {
        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="propDef">The property definition</param>
        public BOPropLookupList(IPropDef propDef) : base(propDef)
        {
            CheckPropDefHasLookupList(propDef);
        }

        private static void CheckPropDefHasLookupList(IPropDef propDef)
        {
            if (!propDef.HasLookupList())
            {
                throw new HabaneroDeveloperException
                    ("There is a problem with the configuration of this application"
                     , string.Format("The application tried to configure a BOPropLookupList - with the propDef {0} that does not have a lookup list defined"
                                     , propDef.PropertyName));
            }
        }

        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="propDef">The property definition</param>
        /// <param name="propValue">the default value for this property</param>
        internal BOPropLookupList(IPropDef propDef, object propValue)
            : base(propDef, propValue)
        {
            CheckPropDefHasLookupList(propDef);
        }
        protected override void ParsePropValue(object valueToParse, out object returnValue)
        {
            if(this.PropDef.TryParsePropValue(valueToParse, out returnValue)) return;

            if (!_propDef.HasLookupList()) return;//TODO: must raise error
            Dictionary<string, object> lookupList = _propDef.LookupList.GetLookupList();
            try
            {
                returnValue = lookupList[Convert.ToString(valueToParse)];
            }
            catch (KeyNotFoundException ex)
            {
                throw new HabaneroApplicationException
                    (this.PropertyName + " cannot be set to '" + valueToParse
                     + "' this value does not exist in the lookup list", ex);
            }
//                if (newPropValue is IBusinessObject)
//                {
//                    newPropValue = ((BusinessObject)(newPropValue))._primaryKey.GetAsGuid();
//                }
        }

        protected internal override object PropertyValueToDisplay
        {
            get
            {
                string displayValue;
                Dictionary<object, string> keyLookupList = _propDef.LookupList.GetKeyLookupList();
                if (this.Value == null) return null;

                return keyLookupList.TryGetValue(this.Value, out displayValue) ? displayValue : null;
//                foreach (KeyValuePair<string, object> pair in this.PropDef.LookupList.GetLookupList())
//                {
//                    if (pair.Value.Equals(this.Value))
//                    {
//                        return pair.Key;
//                    }
//                }
//                return null;
            }
        }

//        public override string PersistedPropertyValueString
//        {
//            get {return ConvertValueToString(_persistedValue);}
//        }
//
//        public override string PropertyValueString
//        {
//            get {return ConvertValueToString(_currentValue);}
//        }
//
//        private string ConvertValueToString(object value)
//        {
//            if (value == null) return "";
//            object parsedPropValue = DoParsePropValue(value);
//            return (parsedPropValue is DateTime)
//                       ? ((DateTime)value).ToString(_standardDateTimeFormat)
//                       :value.ToString();
//        }

//        protected override object DoParsePropValue(object propValue)
//        {
//            if (!(propValue is DateTime))
//            {
//                DateTime dateTimeOut;
//                if (DateTime.TryParse(propValue.ToString(), out dateTimeOut))
//                {
//                    propValue = dateTimeOut;
//                }
//                else
//                {
//                    propValue = null;
//                }
//            }
//            return propValue;
//        }
    }
}