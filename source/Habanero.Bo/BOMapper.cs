//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
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
            PropDef propDef = _businessObject.ClassDef.GetPropDef(propertyName);
            //return def.GetLookupList(_businessObject.GetDatabaseConnection());
            if (propDef.LookupList != null)
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
            return _businessObject.ClassDef.UIDefCol[uiDefName];
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
            Type type = this._businessObject.GetType();
            string className = type.Name;
            try
            {
                PropertyInfo propInfo = type.GetProperty( virtualPropName, BindingFlags.Public | BindingFlags.Instance);
                if (propInfo == null)
                {
                    throw new TargetInvocationException(new Exception(
                                                            String.Format("Virtual property '{0}' does not exist for object of type '{1}'.", virtualPropName, className)));
                }
                object propValue = propInfo.GetValue(this._businessObject, new object[] {});
                return propValue;
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error retrieving virtual property '{0}' from object of type '{1}'" +
                                        Environment.NewLine + "{2}", virtualPropName, className,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
            }
        }

        internal void SetVirtualPropertyValue(string propertyName, object value)
        {
            string virtualPropName = propertyName.Substring(1, propertyName.Length - 2);
            Type type = this._businessObject.GetType();
            string className = type.Name;
            try
            {
                PropertyInfo propInfo = type.GetProperty(virtualPropName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
                if (propInfo == null)
                {
                    throw new TargetInvocationException(new Exception(
                                                            String.Format("Virtual property set for '{0}' does not exist for object of type '{1}'.", virtualPropName, className)));
                }
                propInfo.SetValue(this._businessObject, value, new object[] { });
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error setting virtual property '{0}' for object of type '{1}'" +
                                        Environment.NewLine + "{2}", virtualPropName, className,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
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
            PropDef propDef = _businessObject.ClassDef.GetPropDef(propertyName);
            if (propDef.LookupList != null) {
                if (propDef.LookupList.GetType() == typeof (DatabaseLookupList)) {
                    return ((DatabaseLookupList) propDef.LookupList).ClassDef;
                }
                else if (propDef.LookupList.GetType() == typeof (BusinessObjectLookupList)) {
                    Type lookupListType = null;
                    TypeLoader.LoadClassType(ref lookupListType, ((BusinessObjectLookupList) propDef.LookupList).AssemblyName,
                                             ((BusinessObjectLookupList) propDef.LookupList).ClassName, "Class", "Lookup List");
                    return ClassDef.ClassDefs[lookupListType];
                }
            }
            return null;
        }
    }
}