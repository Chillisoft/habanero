//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Reflection;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using Habanero.Util.File;
using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Provides mapping to a business object
    /// </summary>
    public class BOMapper
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.BoMapper");
        private BusinessObject _businessObject;

        /// <summary>
        /// Constructor to initialise a new mapper
        /// </summary>
        /// <param name="bo">The business object to map</param>
        public BOMapper(BusinessObject bo)
        {
            _businessObject = bo;
        }

        /// <summary>
        /// Returns the lookup list contents for the property specified
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the lookup list or an empty collection if
        /// not available</returns>
        public Dictionary<string, object> GetLookupList(string propertyName)
        {
            IPropDef propDef = _businessObject.ClassDef.GetPropDef(propertyName, false);
            //return def.GetLookupList(_businessObject.GetDatabaseConnection());
            if (propDef != null && propDef.LookupList != null)
            {
                return propDef.LookupList.GetLookupList(_businessObject.GetDatabaseConnection());
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// Returns the business object's interface mapper
        /// </summary>
        /// <returns>Returns the interface mapper</returns>
        public UIDef GetUIDef()
        {
            return GetUIDef("default");
        }

        /// <summary>
        /// Returns the business object's interface mapper with the UIDefName
        /// specified
        /// </summary>
        /// <param name="uiDefName">The UIDefName</param>
        /// <returns>Returns the interface mapper</returns>
        public UIDef GetUIDef(string uiDefName)
        {
            return _businessObject.ClassDef.GetUIDef(uiDefName);
        }

        /// <summary>
        /// Returns a property value in the form it will be displayed in a
        /// user interface (sometimes the underlying value may be stored
        /// differently, as in a lookup-list)
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the formatted object to display</returns>
        public object GetPropertyValueToDisplay(string propertyName)
        {
            if (propertyName.IndexOf(".") != -1)
            {
                //log.Debug("Prop with . found : " + propertyName);
                BusinessObject relatedBo = this._businessObject;
                string relationshipName = propertyName.Substring(0, propertyName.IndexOf("."));
                propertyName = propertyName.Remove(0, propertyName.IndexOf(".") + 1);
                if (relationshipName.IndexOf("|") != -1)
                {
                    //log.Debug("| found in relationship name :" + relationshipName);
                    ArrayList relNames = new ArrayList();
                    while (relationshipName.IndexOf("|") != -1)
                    {
                        string relName = relationshipName.Substring(0, relationshipName.IndexOf("|"));
                        relNames.Add(relName);
                        //log.Debug("Relationship name found : " + relName);
                        relationshipName = relationshipName.Remove(0, relationshipName.IndexOf("|") + 1);
                    }
                    relNames.Add(relationshipName);
                    BusinessObject oldBo = relatedBo;
                    int i = 0;
                    do
                    {
                        relatedBo = oldBo.Relationships.GetRelatedObject((String) relNames[i++]);
                    } while (relatedBo == null && i < relNames.Count);
                }
                else
                {
                    relatedBo = relatedBo.Relationships.GetRelatedObject(relationshipName);
                }
                if (relatedBo == null)
                {
                    return null;
                    //throw new HabaneroApplicationException("Unable to retrieve property " + propertyName + " from a business object of type " + this._businessObject.GetType().Name);
                }
                BOMapper relatedBoMapper = new BOMapper(relatedBo);
                return relatedBoMapper.GetPropertyValueToDisplay(propertyName);
            }
            else if (propertyName.IndexOf("-") != -1)
            {
                return GetVirtualPropertyValue(propertyName);
            }
            else
            {
                return _businessObject.GetPropertyValueToDisplay(propertyName);
            }
        }

        private object GetVirtualPropertyValue(string propertyName)
        {
            string virtualPropName = propertyName.Substring(1, propertyName.Length - 2);
            return ReflectionUtilities.GetPropertyValue(_businessObject, virtualPropName);
        }

        ///<summary>
        /// Sets a property of a Business Object given the property name 
        /// (or the virtual property name delimited by dashes) and the value.
        ///</summary>
        ///<param name="propertyName">The name of the property to set.</param>
        ///<param name="value">The value to set.</param>
        public void SetDisplayPropertyValue(string propertyName, object value)
        {
            if (propertyName.IndexOf(".") != -1)
            {
                //Do Nothing
            }
            else if (propertyName.IndexOf("-") != -1)
            {
                SetVirtualPropertyValue(propertyName, value);
            }
            else
            {
                _businessObject.SetPropertyValue(propertyName, value);
            }
        }

        internal void SetVirtualPropertyValue(string propertyName, object value)
        {
            if (_businessObject == null) return;
            string virtualPropName = propertyName.Substring(1, propertyName.Length - 2);
            PropertyInfo propertyInfo = ReflectionUtilities.GetPropertyInfo(_businessObject.GetType(), virtualPropName);
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                ReflectionUtilities.SetPropertyValue(_businessObject, virtualPropName, value);
            }
        }

        /// <summary>
        /// Returns the class definition related to the specified database 
        /// lookup list for the specified property in the class definition
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the class definition or null if not available</returns>
        public ClassDef GetLookupListClassDef(string propertyName)
        {
            ClassDef classDef = _businessObject.ClassDef;
            IPropDef propDef = classDef.GetPropDef(propertyName, false);

            if (propDef != null && propDef.LookupList != null) {
                if (propDef.LookupList is DatabaseLookupList)
                {
                    DatabaseLookupList databaseLookupList = (DatabaseLookupList) propDef.LookupList;
                    return databaseLookupList.ClassDef;
                }
                else if (propDef.LookupList is BusinessObjectLookupList) 
                {
                    BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
                    return businessObjectLookupList.LookupBoClassDef;
                    //Type lookupListType = null;
                    //TypeLoader.LoadClassType(ref lookupListType, businessObjectLookupList.AssemblyName,
                    //                         businessObjectLookupList.ClassName, "Class", "Lookup List");
                    //return ClassDef.ClassDefs[lookupListType];
                }
            }
            return null;
        }
    }
}