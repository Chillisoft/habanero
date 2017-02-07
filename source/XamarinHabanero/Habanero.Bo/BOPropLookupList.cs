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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;

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
        /// Used for caching to improve performance.
        /// </summary>
        private object _propValueWhenLookupListDisplayValueLastCalled;
        private string _displayValueWhenLookupListDisplayValueLastCalled;
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
            Loading = true;
            CheckPropDefHasLookupList(propDef);
            Loading = false;
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
        /// Initialises the property with the specified value, and indicates
        /// whether the object is new or not
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        /// <param name="isObjectNew">Whether the object is new or not</param>
        protected override bool InitialiseProp(object propValue, bool isObjectNew)
        {
            Loading = true;
            bool propValueHasChanged = base.InitialiseProp(propValue, isObjectNew);
            Loading = false;
            return propValueHasChanged;
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
            if (valueToParse == null)
            {
                returnValue = null;
                return;
            }
            CheckPropDefHasLookupList(_propDef);
            if (_propDef.LookupList is BusinessObjectLookupList && valueToParse is IBusinessObject)
            {
                Type expectedBOType = ((BusinessObjectLookupList) _propDef.LookupList).BoType;
                if (!expectedBOType.IsInstanceOfType(valueToParse))
                {
                    string message = string.Format
                        ("'{0}' cannot be set to a business object of type '{1}' since the lookup list is defined for type '{2}'",
                         this.PropertyName, valueToParse.GetType(), expectedBOType);
                    throw new HabaneroDeveloperException(message, message);
                }
                returnValue = ((IBusinessObject) valueToParse).ID.GetAsValue();
                return;
            }
            //The value will not be parsed from from a string value to a Guid Value 
            // when the object is loading. This is due to the performance issues.
            // This is particularly an issue for BusinessObjectLookupLists
            if (!((_propDef.LookupList is BusinessObjectLookupList) && this.Loading))
            {
                Dictionary<string, string> keyLookupList = _propDef.LookupList.GetIDValueLookupList();
                if (this.PropertyType.IsInstanceOfType(valueToParse)
                    && keyLookupList.ContainsKey(Convert.ToString(valueToParse)))
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
            }

            if (this.PropDef.TryParsePropValue(valueToParse, out returnValue))
            {
                return;
            }

            string className = this.PropDef.ClassDef == null ? "" : this.PropDef.ClassDef.ClassName;
            throw new HabaneroApplicationException
                (className + "." + this.PropertyName + " cannot be set to '" + valueToParse
                 + "' this value cannot be converted to a " + this.PropDef.PropertyTypeName);
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
                var propValue = this.Value;
                if (propValue == null) return null;
                //-----This has been added to improve the performance based on Profiling LGMIS.
                if (propValue.Equals(_propValueWhenLookupListDisplayValueLastCalled))
                {
                    return _displayValueWhenLookupListDisplayValueLastCalled;
                }
                _propValueWhenLookupListDisplayValueLastCalled = propValue;

                var lookupList = _propDef.LookupList;
                var keyLookupList = lookupList.GetIDValueLookupList();
                if (keyLookupList.TryGetValue(this.PropDef.ConvertValueToString(propValue), out _displayValueWhenLookupListDisplayValueLastCalled))
                    return _displayValueWhenLookupListDisplayValueLastCalled;
                if (lookupList is BusinessObjectLookupList)
                {
                    var businessObjectLookupList = lookupList as BusinessObjectLookupList;
                    var classDef = businessObjectLookupList.LookupBoClassDef;
                    var businessObject = GetBusinessObjectForProp(classDef);
                    _displayValueWhenLookupListDisplayValueLastCalled = businessObject == null ? null : businessObject.ToString();
                    return _displayValueWhenLookupListDisplayValueLastCalled;
                }
                return null;
            }
        }

        /// <summary>
        /// This is used to determine whether the BOPropLookup is loading or not.
        /// when the lookup is loading it does not validate entries against the lookup list
        ///  and it does not try to parse a value to a lookup value for database and business objects 
        ///  lookup items.
        /// </summary>
        protected bool Loading { get; set; }

        internal IBusinessObject GetBusinessObjectForProp(IClassDef classDef)
        {
            var businessObject = ((PropDef) this.PropDef).GetlookupBusinessObjectFromObjectManager(this.Value);
            if (businessObject != null) return businessObject;
            try
            {
                businessObject = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectByValue
                    (classDef, this.Value);
            }
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                _logger.Log(ex);
                return null;
            }
            return businessObject;
        }

        ///<summary>
        /// Returns the Business Object that is related to this property in the case 
        ///   where this property is related to a BusinessObjectLookupList
        ///</summary>
        ///<returns></returns>
        public IBusinessObject GetBusinessObjectForProp()
        {
            var propDef = this.PropDef;
            if (propDef.LookupList is BusinessObjectLookupList)
            {
                var businessObjectLookupList = propDef.LookupList as BusinessObjectLookupList;
                var classDef = businessObjectLookupList.LookupBoClassDef;
                var businessObject = GetBusinessObjectForProp(classDef);
                return businessObject;
            }

            return null;
        }
    }
}