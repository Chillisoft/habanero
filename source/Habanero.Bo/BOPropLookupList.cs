using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    ///<summary>
    /// Stores the values (current Value, DatabaseValue etc) and 
    ///  state (dirty, valid) of a property of a <see cref="IBusinessObject"/>.
    /// Inherits from <see cref="BOProp"/> for cases where the BOProp has a <see cref="ILookupList"/> as
    ///   defined in the <see cref="PropDef"/>.
    /// Has a reference to the Property Definition <see cref="PropDef"/> that was used to create it.
    /// The Property definition includes property rules and validation functionality.
    /// The property of a business object may represent a property such as FirstName, Surname.
    /// Typically a <see cref="IBusinessObject"/> will have a collection of Properties.
    /// 
    /// This allow the property value to be set to either the key or the display value of 
    ///   a lookup list the BOProp will always store the appropriate key value.
    /// The lookup list is also acts as a list for validation and the BOProp will
    ///  be placed in an invalid state if the key value set is not available in the list.
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

        /// <summary>
        /// Constructor to initialise a new property
        /// </summary>
        /// <param name="propDef">The property definition</param>
        /// <param name="propValue">the default value for this property</param>
        internal BOPropLookupList(IPropDef propDef, object propValue) : base(propDef, propValue)
        {
            CheckPropDefHasLookupList(propDef);
        }

        private static void CheckPropDefHasLookupList(IPropDef propDef)
        {
            if (!propDef.HasLookupList())
            {
                throw new HabaneroDeveloperException
                    ("There is a problem with the configuration of this application",
                     string.Format
                         ("The application tried to configure a BOPropLookupList - with the propDef {0} that does not have a lookup list defined",
                          propDef.PropertyName));
            }
        }

        /// <summary>
        /// This method provides a the functionality to convert any object to the appropriate
        ///   type for the particular BOProp Type. e.g it will convert a valid guid string to 
        ///   a valid Guid Object.
        /// This allow the property value to be set to either the key or the display value of 
        ///   a lookup list the BOProp will always store the appropriate key value.
        /// The lookup list is also acts as a list for validation and the BOProp will
        ///  be placed in an invalid state if the key value set is not available in the list.
        /// </summary>
        /// <param name="valueToParse">The value to be converted</param>
        /// <param name="returnValue">The value that has been parsed</param>
        public override void ParsePropValue(object valueToParse, out object returnValue)
        {
            // if type of value to parse is of prop type then try lookup in keys dict. if exists then
            //    use this as the value and return

            CheckPropDefHasLookupList(_propDef);
            Dictionary<string, string> keyLookupList = _propDef.LookupList.GetIDValueLookupList();
            if (this.PropertyType.IsInstanceOfType(valueToParse) && keyLookupList.ContainsKey(Convert.ToString(valueToParse)))
            {
                returnValue = valueToParse;
                return;
            }

            // if type of valueToParse is string then try lookup in value dict. If exists then
            //   use the key as the value and return.
            Dictionary<string, string> lookupList = _propDef.LookupList.GetLookupList();
            if (lookupList.ContainsKey(Convert.ToString(valueToParse)))
            {
                returnValue = lookupList[Convert.ToString(valueToParse)];
                this.PropDef.TryParsePropValue(returnValue, out returnValue);
                return;
            }

            if (!this.PropDef.TryParsePropValue(valueToParse, out returnValue))
            {
                throw new HabaneroApplicationException
                    (this.PropertyName + " cannot be set to '" + valueToParse
                     + "' this value does not exist in the lookup list");
            }
        }

        /// <summary>
        /// Returns the named property value that should be displayed
        ///   on a user interface e.g. a textbox or on a report.
        /// This is used primarily for Lookup lists where
        ///    the value stored for the object may be a guid but the value
        ///    to display may be a string.
        /// </summary>
        /// <returns>Returns the property value</returns>
        public override object PropertyValueToDisplay
        {
            get
            {
                string displayValue;
                Dictionary<string, string> keyLookupList = _propDef.LookupList.GetIDValueLookupList();
                if (this.Value == null) return null;
                if (keyLookupList.TryGetValue(this.PropDef.ConvertValueToString(this.Value), out displayValue))
                    return displayValue;
                if (_propDef.LookupList is BusinessObjectLookupList)
                {
                    BusinessObjectLookupList businessObjectLookupList = _propDef.LookupList as BusinessObjectLookupList;
                    ClassDef classDef = businessObjectLookupList.LookupBoClassDef;
                    IBusinessObject businessObject = GetBusinessObjectForProp(classDef);
                    return businessObject == null? null: businessObject.ToString();
                }
                return null;
            }
        }

        internal IBusinessObject GetBusinessObjectForProp(ClassDef classDef)
        {
            IBusinessObject businessObject = null;
            BOPrimaryKey boPrimaryKey = GetRelatedBOPrimaryKeyForProp(classDef);
            try
            {
                businessObject = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject
                    (classDef, boPrimaryKey);
            }
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                log.Error(ex.Message);
                return null;
            }
            return businessObject;
        }

        internal BOPrimaryKey GetRelatedBOPrimaryKeyForProp(ClassDef classDef)
        {
            PrimaryKeyDef primaryKeyDef = classDef.GetPrimaryKeyDef();
            if (primaryKeyDef.IsCompositeKey) return null;

            BOPropCol boPropCol = classDef.createBOPropertyCol(true);
            BOPrimaryKey boPrimaryKey = primaryKeyDef.CreateBOKey(boPropCol) as BOPrimaryKey;
            if (boPrimaryKey != null)
            {
                boPrimaryKey[0].Value = Value;
            }
            return boPrimaryKey;
        }
    }
}