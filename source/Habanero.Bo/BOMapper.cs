// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections.Generic;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

//using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Provides mapping to a business object
    /// </summary>
    public class BOMapper
    {
        //private static readonly ILog log = LogManager.GetLogger("Habanero.BO.BoMapper");
        private readonly IBusinessObject _businessObject;

        /// <summary>
        /// Constructor to initialise a new mapper
        /// </summary>
        /// <param name="bo">The business object to map</param>
        public BOMapper(IBusinessObject bo)
        {
            _businessObject = bo;
        }

        /// <summary>
        /// Returns the lookup list contents for the property specified
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the lookup list or an empty collection if
        /// not available</returns>
        public Dictionary<string, string> GetLookupList(string propertyName)
        {
            if (_businessObject != null)
            {
                IPropDef propDef = _businessObject.ClassDef.GetPropDef(propertyName, false);
                if (propDef != null && propDef.LookupList != null && propDef.HasLookupList())
                {
                    return propDef.LookupList.GetLookupList();
                }
            } else
            {
                string message = string.Format("The lookup list for '{0}' could not be returned since the business object is null", propertyName);
                throw new HabaneroDeveloperException(message, message);
            }
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns the business object's interface mapper
        /// </summary>
        /// <returns>Returns the interface mapper</returns>
        public IUIDef GetUIDef()
        {
            return GetUIDef("default");
        }

        /// <summary>
        /// Returns the business object's interface mapper with the UIDefName
        /// specified
        /// </summary>
        /// <param name="uiDefName">The UIDefName</param>
        /// <returns>Returns the interface mapper</returns>
        public IUIDef GetUIDef(string uiDefName)
        {
            return ((ClassDef) _businessObject.ClassDef).GetUIDef(uiDefName);
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
            if (IsAlternateRelationshipProp(propertyName))
            {
                return GetAlternateRelationshipValue(propertyName);
            }
            var propertyMapper = BOPropMapperFactory.CreateMapper(this._businessObject, propertyName);

            return propertyMapper.GetPropertyValue();
        }
        /// <summary>
        /// This is a bit of a hack_ was used on a specific project some time ago.
        /// This is not generally supported throughout Habanero so has been isolated here.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private object GetAlternateRelationshipValue(string propertyName)
        {
            string relationshipName = propertyName.Substring(0, propertyName.IndexOf("."));
            propertyName = propertyName.Remove(0, propertyName.IndexOf(".") + 1);
            string[] parts = relationshipName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> relNames = new List<string>(parts);
            IBusinessObject relatedBo = this._businessObject;
            IBusinessObject oldBo = relatedBo;
            int i = 0;
            do
            { 
                relatedBo = oldBo.Relationships.GetRelatedObject(relNames[i++]);
            } while (relatedBo == null && i < relNames.Count);
            if (relatedBo == null)
            {
                return null;
               
            }
            BOMapper relatedBoMapper = new BOMapper(relatedBo);
            return relatedBoMapper.GetPropertyValueToDisplay(propertyName);
        }

        private bool IsAlternateRelationshipProp(string propertyName)
        {
            return propertyName.IndexOf("|") != -1;
        }


        ///<summary>
        /// Sets a property of a Business Object given the property name 
        /// (or the virtual property name delimited by dashes) and the value.
        ///</summary>
        ///<param name="propertyName">The name of the property to set.</param>
        ///<param name="value">The value to set.</param>
        public void SetDisplayPropertyValue(string propertyName, object value)
        {
            if (_businessObject == null) return;
            var propertyMapper = BOPropMapperFactory.CreateMapper(this._businessObject, propertyName);
             propertyMapper.SetPropertyValue(value);
        }


        // ReSharper restore MemberCanBePrivate.Global
        /// <summary>
        /// Returns the class definition related to the
        /// lookup list for the specified property.
        /// If the property does not have a LookupList returns null.
        /// If the LookupList is not of Type <see cref="ILookupListWithClassDef"/>
        /// then returns null.
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the class definition or null if not available</returns>
        public IClassDef GetLookupListClassDef(string propertyName)
        {
            IClassDef classDef = _businessObject.ClassDef;
            IPropDef propDef = classDef.GetPropDef(propertyName, false);

            if (propDef != null && propDef.LookupList != null && propDef.HasLookupList())
            {
                if (propDef.LookupList is ILookupListWithClassDef)
                {
                    ILookupListWithClassDef lookupList = (ILookupListWithClassDef)propDef.LookupList;
                    return lookupList.ClassDef;
                }
                
            }
            return null;
        }
    }
}