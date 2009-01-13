//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a lookup-list sourced from business object collections.
    /// A lookup-list is typically used to populate features like a ComboBox,
    /// where the string would be displayed, but the Guid would be the
    /// value stored (for reasons of data integrity).
    /// The string-Guid pair collection will be created using each object's
    /// ToString() function and the object's Guid ID.<br/>
    /// NB: this class does not provide criteria, so the entire collection
    /// will be loaded.
    /// </summary>
    public class BusinessObjectLookupList : ILookupList
    {
        private readonly int _timeout;
        private Type _boType;
        private string _assemblyName;
        private string _className;

        /// <summary>
        /// Provides a key value pair where the persisted value can be returned for 
        ///   any displayed value. E.g. the persisted value may be a GUID but the
        ///   displayed value may be a related string.
        /// </summary>
        private Dictionary<string, string> _displayValueDictionary;

        /// <summary>
        /// Provides a key value pair where the persisted value can be returned for 
        ///   any displayed value. E.g. the persisted value may be a GUID but the
        ///   displayed value may be a related string.
        /// </summary>
        private Dictionary<string, string> _keyValueDictionary = new Dictionary<string, string>();

        private DateTime _lastCallTime;
        private OrderCriteria _orderCriteria;
        private readonly string _criteriaString;
        private readonly string _sortString;

        #region Constructors

        /// <summary>
        /// Constructor to initialise a new lookup-list
        /// </summary>
        /// <param name="boType">The business object type</param>
        public BusinessObjectLookupList(Type boType) : this(boType, 10000)
        {
        }

        /// <summary>
        /// Constructor to initialise a new lookup-list
        /// </summary>
        /// <param name="boType">The business object type</param>
        /// <param name="timeout">The period after which the cache expires</param>
        public BusinessObjectLookupList(Type boType, int timeout)
        {
            BoType = boType;
            _timeout = timeout;
            _lastCallTime = DateTime.MinValue;
        }

        /// <summary>
        /// Constructor to initialise a new lookup-list
        /// </summary>
        /// <param name="assemblyName">The assembly containing the class</param>
        /// <param name="className">The class from which to load the values</param>
        public BusinessObjectLookupList(string assemblyName, string className) : this(assemblyName, className, 10000)
        {
        }

        /// <summary>
        /// Constructor to initialise a new lookup-list
        /// </summary>
        /// <param name="assemblyName">The assembly containing the class</param>
        /// <param name="className">The class from which to load the values</param>
        /// <param name="timeout">The period after which the cache expires</param>
        public BusinessObjectLookupList(string assemblyName, string className, int timeout)
        {
            _assemblyName = assemblyName;
            _className = className;
            _boType = null;
            _timeout = timeout;
            _lastCallTime = DateTime.MinValue;
        }

        /// <summary>
        /// Constructor to initialise a new lookup-list
        /// </summary>
        /// <param name="assemblyName">The assembly containing the class</param>
        /// <param name="className">The class from which to load the values</param>
        /// <param name="criteria">Sql criteria to apply on loading of the 
        /// collection</param>
        /// <param name="sort">The property to sort on.
        /// The possible formats are: "property", "property asc",
        /// "property desc" and "property des".</param>
        public BusinessObjectLookupList(string assemblyName, string className, string criteria, string sort)
            : this(assemblyName, className)
        {
            _criteriaString = criteria;
            _sortString = sort;
        }

        /// <summary>
        /// Constructor to initialise a new lookup-list
        /// </summary>
        /// <param name="type">The type of business object that the lookup list is being loaded for</param>
        /// <param name="criteria">Sql criteria to apply on loading of the 
        /// collection</param>
        /// <param name="sort">The property to sort on.
        /// The possible formats are: "property", "property asc",
        /// "property desc" and "property des".</param>
        public BusinessObjectLookupList(Type type, string criteria, string sort):this(type)
        {
            _criteriaString = criteria;
            _sortString = sort;
        }
        #endregion Constructors

        #region Properties

        /// <summary>
        /// The assembly containing the class from which values are loaded
        /// </summary>
        public string AssemblyName
        {
            get { return _assemblyName; }
            protected set
            {
                if (_assemblyName != value)
                {
                    _className = null;
                    _boType = null;
                }
                _assemblyName = value;
            }
        }

        /// <summary>
        /// The class from which values are loaded
        /// </summary>
        public string ClassName
        {
            get { return _className; }
            protected set
            {
                if (_className != value)
                {
                    _boType = null;
                }
                _className = value;
            }
        }

//        /// <summary>
//        /// Gets and sets the sql criteria used to limit which objects
//        /// are loaded in the BO collection
//        /// </summary>
//        public string Criteria
//        {
//            get { return _criteria; }
//            private set { _criteria = value;
//            Criteria criteria = BusinessObjectLoaderBase.GetCriteriaObject(classDef, criteriaString);
//            }
//        }
        private Criteria _criteria;


        /// <summary>
        /// Gets and sets the sql criteria used to limit which objects
        /// are loaded in the BO collection
        /// </summary>
        public Criteria Criteria
        {
            get
            {
                if (_criteria == null && !string.IsNullOrEmpty(_criteriaString))
                {
                    _criteria = CriteriaParser.CreateCriteria(_criteriaString);
                }
                return _criteria;
            } //            private set { _criteria = value; }
        }

        /// <summary>
        /// Gets and sets the sort string used to sort the lookup
        /// list.  This string must contain the name of a property
        /// belonging to the business object used to construct the list.
        /// The possible formats are: "property", "property asc",
        /// "property desc" and "property des".
        /// </summary>
        public OrderCriteria OrderCriteria
        {
            get { if(_orderCriteria == null && !string.IsNullOrEmpty(_sortString))
            {
                ClassDef classDef = this.LookupBoClassDef;
                _orderCriteria = QueryBuilder.CreateOrderCriteria(classDef, _sortString);
            }
                return _orderCriteria;
            }
            //set { _sort = FormatSortAttribute(value); }
        }

        #endregion Properties

        #region ILookupList Implementation

        /// <summary>
        /// Returns a lookup-list for all the business objects stored under
        /// the class definition held in this instance
        /// </summary>
        /// <returns>Returns a collection of string-value pairs</returns>
        public Dictionary<string, string> GetLookupList()
        {
            return GetLookupList(null);
        }

        /// <summary>
        /// Returns a lookup-list for all the business objects stored under
        /// the class definition held in this instance, using the database
        /// connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <returns>Returns a collection of string-value pairs</returns>
        public Dictionary<string, string> GetLookupList(IDatabaseConnection connection)
        {
            return GetLookupList(false);
        }

        /// <summary>
        /// Returns the Property definition that this lookup list is for.
        /// </summary>
        public IPropDef PropDef { get; set; }

        /// <summary>
        /// Returns a lookup-list for all the business objects stored under
        /// the class definition held in this instance.  An option is included to ignore the default
        /// timeout, which causes use of a cached version within the timeout
        /// period.
        /// </summary>
        /// <param name="ignoreTimeout">Whether to ignore the timeout and reload
        /// from the database regardless of when the lookup list was last loaded.</param>
        /// <returns>Returns a collection of string-value pairs</returns>
        public Dictionary<string, string> GetLookupList(bool ignoreTimeout)
        {
            if (!ignoreTimeout && DateTime.Now.Subtract(_lastCallTime).TotalMilliseconds < _timeout)
            {
                _lastCallTime = DateTime.Now;
                return _displayValueDictionary;
            }
            ClassDef classDef = LookupBoClassDef;
            if (classDef.PrimaryKeyDef.Count > 1)
            {
                throw new HabaneroDeveloperException
                    ("There is an application setup error. Please contact your system administrator",
                     "The lookup list cannot contain business objects '" + classDef.ClassNameFull
                     + "' with a composite primary key.");
            }

            IBusinessObjectCollection col = GetBusinessObjectCollection();
            _displayValueDictionary = CreateDisplayValueDictionary(col, OrderCriteria == null);
            FillKeyValueDictionary();
            _lastCallTime = DateTime.Now;
            return _displayValueDictionary;
        }

        ///<summary>
        /// Returns a collection of related business objects for a particular property based on the definition of the business object lookup list.
        /// This returns the same set of data as returned by the <see cref="GetLookupList()"/> method.
        ///</summary>
        ///<returns></returns>
        public virtual IBusinessObjectCollection GetBusinessObjectCollection()
        {
            ClassDef classDef = LookupBoClassDef;
            return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (classDef, this.Criteria, this.OrderCriteria);
        }

        ///<summary>
        /// Returns the class definition for the business object type that is the source of the list for this lookup
        ///</summary>
        public ClassDef LookupBoClassDef
        {
            get { return ClassDef.ClassDefs[BoType]; }
        }

        /// <summary>
        /// Returns a collection of string Guid pairs from the business object
        /// collection provided. Each pair consists of a string version of a
        /// business object and the object's ID.
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <param name="sortByDisplayValue">Must the collection be sorted by the display value or not</param>
        /// <returns>Returns a collection of display-value pairs</returns>
        public Dictionary<string, string> CreateDisplayValueDictionary
            (IBusinessObjectCollection col, bool sortByDisplayValue)
        {
            if (col == null)
            {
                return new Dictionary<string, string>();
            }
            if (this.PropDef == null)
            {
                throw new HabaneroDeveloperException
                    ("There is an application setup error. There is no propdef set for the business object lookup list. Please contact your system administrator",
                     "There is no propdef set for the business object lookup list.");
            }

            if (sortByDisplayValue)
            {
                SortedDictionary<string, string> sortedLookupList = new SortedDictionary<string, string>();
                foreach (BusinessObject bo in col)
                {
                    string stringValue = GetAvailableDisplayValue(sortedLookupList, bo.ToString());
                    sortedLookupList.Add(stringValue, this.PropDef.ConvertValueToString(bo.ID.GetAsValue()));
                }

                Dictionary<string, string> lookupList = new Dictionary<string, string>();
                foreach (string key in sortedLookupList.Keys)
                {
                    AddBusinessObjectToLookupList(lookupList, sortedLookupList[key], key);
                }
                return lookupList;
            }
            else
            {
                Dictionary<string, string> lookupList = new Dictionary<string, string>();
                foreach (BusinessObject bo in col)
                {
                    string stringValue = GetAvailableDisplayValue(lookupList, bo.ToString());
                    string objectID = this.PropDef.ConvertValueToString(bo.ID.GetAsValue());
                    AddBusinessObjectToLookupList(lookupList, objectID, stringValue);
                }
                return lookupList;
            }
        }

        private static void AddBusinessObjectToLookupList
            (IDictionary<string, string> lookupList, string objectID, string stringValue)
        {
            lookupList.Add(stringValue, objectID);
        }

        ///<summary>
        /// Returns a unique display value for an item of the given name, so that it can be added to the list without the risk of having duplicate entries.
        ///</summary>
        ///<param name="sortedLookupList"></param>
        ///<param name="stringValue">The new value to determine a display value for</param>
        ///<returns>Returns a unique display value for an item of the given name.</returns>
        private static string GetAvailableDisplayValue(IDictionary<string, string> sortedLookupList, string stringValue)
        {
            string originalValue = null;
            int count = 1;
            while (sortedLookupList.ContainsKey(stringValue))
            {
                if (originalValue == null) originalValue = stringValue;
                stringValue = originalValue + "(" + ++count + ")";
            }
            return stringValue;
        }

        #endregion ILookupList Implementation

        #region Type Initialisation

        ///<summary>
        /// Returns the BOType for this Business Object.
        ///</summary>
        public Type BoType
        {
            get
            {
                TypeLoader.LoadClassType(ref _boType, _assemblyName, _className, "property", "property definition");
                return _boType;
            }
            private set
            {
                _boType = value;
                TypeLoader.ClassTypeInfo(_boType, out _assemblyName, out _className);
            }
        }

        #endregion Type Initialisation

        /// <summary>
        /// Returns a collection of all the business objects stored under
        /// the class definition held in this instance.  The collection contains
        /// a string version of each of the business objects.
        /// </summary>
        /// <returns>Returns an ICollection object</returns>
        public ICollection GetValueCollection()
        {
            ClassDef classDef = LookupBoClassDef;
            IBusinessObjectCollection col = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (classDef, "", "");
            return CreateValueList(col);
        }

        /// <summary>
        /// Populates a collection with a string version of each business
        /// object in the collection provided
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <returns>Returns an ICollection object</returns>
        private ICollection CreateValueList(IBusinessObjectCollection col)
        {
            if (this.OrderCriteria == null)
            {
                SortedStringCollection valueList = new SortedStringCollection();
                foreach (IBusinessObject bo in col)
                {
                    valueList.Add(bo.ToString());
                }
                return valueList;
            }
            else
            {
                ArrayList valueList = new ArrayList();
                foreach (IBusinessObject bo in col)
                {
                    valueList.Add(bo.ToString());
                }
                return valueList;
            }
        }
//
//        /// <summary>
//        /// Indicates whether the given sort attribute is valid
//        /// </summary>
//        private string FormatSortAttribute(string sortAttribute)
//        {
//            string modifiedString = sortAttribute;
//            if (!String.IsNullOrEmpty(sortAttribute))
//            {
//                string propertyName = sortAttribute;
//                if (sortAttribute.Contains(" "))
//                {
//                    propertyName = StringUtilities.GetLeftSection(sortAttribute, " ");
//                }
//
//                ClassDef classDef = null;
//                if (ClassDef.ClassDefs.Contains(_assemblyName, _className))
//                {
//                    classDef = LookupBoClassDef;
////                    classDef = ClassDef.ClassDefs[_assemblyName, _className];
//                }
//                if (classDef == null)
//                {
//                    return sortAttribute;
//                    // Throwing this error is problematic during loading of classDefs when not
//                    //    all the defs have loaded yet.  Rather let the missing column be
//                    //    exposed by a database error.
////                    throw new InvalidXmlDefinitionException(String.Format(
////                        "In a 'businessLookupList' element, " +
////                        "the class definitions for class '{0}' and assembly '{1}' could " +
////                        "not be found.  Check that the class definitions for that type have " +
////                        "been loaded.", _className, _assemblyName));
//                }
//
//                bool propertyNameExists = false;
//                foreach (PropDef propDef in classDef.PropDefColIncludingInheritance)
//                {
//                    if (propDef.PropertyName.ToLower() == propertyName.ToLower())
//                    {
//                        propertyNameExists = true;
//                    }
//                }
//                if (!propertyNameExists)
//                {
//                    throw new InvalidXmlDefinitionException
//                        (String.Format
//                             ("In a 'sort' attribute on a 'businessLookupList' element, the "
//                              + "property name '{0}' does not exist.", propertyName));
//                }
//
//                if (sortAttribute.Contains(" "))
//                {
//                    string sortOrder = StringUtilities.GetRightSection(sortAttribute, propertyName + " ");
//                    if (sortOrder.ToLower() == "des")
//                    {
//                        modifiedString = propertyName + " desc";
//                    }
//                    else if (sortOrder.ToLower() != "asc" && sortOrder.ToLower() != "desc")
//                    {
//                        throw new InvalidXmlDefinitionException
//                            (String.Format
//                                 ("In a 'sort' attribute on a 'businessLookupList' element, the "
//                                  + "attribute given as '{0}' was not valid.  The correct "
//                                  + "definition has the form of 'property' or " + "'property asc' or 'property desc'.",
//                                  sortAttribute));
//                    }
//                }
//            }
//            return modifiedString;
//        }

        /// <summary>
        /// Returns the lookup list contents being held where the list is keyed on the list key 
        ///  either a Guid, int or Business object i.e. the value being stored for the property.
        /// The display value can be looked up.
        /// </summary>
        ///<returns>The Key Value Lookup List</returns>
        public Dictionary<string, string> GetIDValueLookupList()
        {
            GetLookupList(true);
            return _keyValueDictionary;
        }

        private void FillKeyValueDictionary()
        {
            if (this.PropDef == null)
            {
                throw new HabaneroDeveloperException
                    ("There is an application setup error. There is no propdef set for the business object lookup list. Please contact your system administrator",
                     "There is no propdef set for the business object lookup list.");
            }
            _keyValueDictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in _displayValueDictionary)
            {
                if (string.IsNullOrEmpty(Convert.ToString(pair.Value)))
                {
                    string developerMessage = string.Format
                        ("A business object of '{0}' is being added to a lookup list for {1} it does not have a value for its primary key set",
                         this.PropDef.PropertyTypeName, this.PropDef.PropertyName);
                    throw new HabaneroDeveloperException(developerMessage, developerMessage);
                }
                if (_keyValueDictionary.ContainsKey(pair.Value)) continue;
//                object parsedKey;

//                if (!this.PropDef.TryParsePropValue(pair.Value, out parsedKey))
//                {
//                    throw new HabaneroDeveloperException
//                        ("There is an application setup error Please contact your system administrator",
//                         "There is a class definition setup error the business object lookup list has lookup value items that are not of type "
//                         + this.PropDef.PropertyTypeName);
//                }
                _keyValueDictionary.Add(pair.Value, pair.Key);
            }
        }
    }
}