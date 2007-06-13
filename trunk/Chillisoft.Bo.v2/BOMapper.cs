using System;
using System.Collections;
using System.Reflection;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;
using log4net;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Manages the mapper for the business object
    /// </summary>
    /// TODO ERIC - review above
    public class BOMapper
    {
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.v2.BoMapper");
        private BusinessObjectBase _businessObject;

        /// <summary>
        /// Constructor to initialise a new mapper
        /// </summary>
        /// <param name="bo">The business object to map</param>
        public BOMapper(BusinessObjectBase bo)
        {
            _businessObject = bo;
        }

        /// <summary>
        /// Returns a property of the name specified
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the BOProp object by that name</returns>
        public BOProp GetProperty(string propertyName)
        {
            return _businessObject.GetBOProp(propertyName);
        }

        /// <summary>
        /// Returns the lookup list contents for the property specified
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the lookup list or an empty collection if
        /// not available</returns>
        public StringGuidPairCollection GetLookupList(string propertyName)
        {
            PropDef def = _businessObject.ClassDef.GetPropDef(propertyName);
            //return def.GetLookupList(_businessObject.GetDatabaseConnection());
            if (def.LookupListSource != null)
            {
                return def.LookupListSource.GetLookupList(_businessObject.GetDatabaseConnection());
            }
            else
            {
                return new StringGuidPairCollection();
            }
        }

        /// <summary>
        /// Returns the business object's interface mapper
        /// </summary>
        /// <returns>Returns the interface mapper</returns>
        public IUserInterfaceMapper GetUserInterfaceMapper()
        {
            return _businessObject.GetUserInterfaceMapper();
        }

        /// <summary>
        /// Returns the business object's interface mapper with the UIDefName
        /// specified
        /// </summary>
        /// <param name="uiDefName">The UIDefName</param>
        /// <returns>Returns the interface mapper</returns>
        public IUserInterfaceMapper GetUserInterfaceMapper(string uiDefName)
        {
            return _businessObject.GetUserInterfaceMapper(uiDefName);
        }

        /// <summary>
        /// Returns a property value in the form it will be displayed in a
        /// user interface (sometimes the underlying value may be stored
        /// differently, as in a lookup-list)
        /// </summary>
        /// <param name="thePropName">The property name</param>
        /// <returns>Returns the formatted object to display</returns>
        /// TODO ERIC P - change parameter to propName and remove 1st line of code
        /// - come back to this (mind is foggy now)
        public object GetPropertyValueForUser(string thePropName)
        {
            string propName = thePropName;
            if (propName.IndexOf(".") != -1)
            {
                //log.Debug("Prop with . found : " + propName);
                BusinessObjectBase relatedBo = this._businessObject;
                string relationshipName = propName.Substring(0, propName.IndexOf("."));
                propName = propName.Remove(0, propName.IndexOf(".") + 1);
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
                    BusinessObjectBase oldBo = relatedBo;
                    int i = 0;
                    do
                    {
                        relatedBo = oldBo.Relationships.GetRelatedBusinessObject((String) relNames[i++]);
                    } while (relatedBo == null && i < relNames.Count);
                }
                else
                {
                    relatedBo = relatedBo.Relationships.GetRelatedBusinessObject(relationshipName);
                }
                if (relatedBo == null)
                {
                    return null;
                    //throw new HabaneroApplicationException("Unable to retrieve property " + thePropName + " from a business object of type " + this._businessObject.GetType().Name);
                }
                BOMapper relatedBoMapper = new BOMapper(relatedBo);
                return relatedBoMapper.GetPropertyValueForUser(propName);
            }
            else if (propName.IndexOf("-") != -1)
            {
                string virtualPropName = propName.Substring(1, propName.Length - 2);
                try
                {
                    PropertyInfo propInfo =
                        this._businessObject.GetType().GetProperty(virtualPropName,
                                                                     BindingFlags.Public | BindingFlags.Instance);
                    object propValue = propInfo.GetValue(this._businessObject, new object[] {});
                    return propValue;
                }

                catch (TargetInvocationException ex)
                {
                    log.Error("Error retrieving virtual property " + virtualPropName + " from object of type " +
                              this._businessObject.GetType().Name + Environment.NewLine +
                              ExceptionUtil.GetExceptionString(ex.InnerException, 8));
                    throw ex.InnerException;
                }
            }
            else
            {
                return _businessObject.GetPropertyValueForUser(propName);
            }
        }

        /// <summary>
        /// Returns the lookup list related to the specified property in the
        /// class definition
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the lookup list or null if not available</returns>
        public ClassDef GetLookupListClassDef(string propertyName)
        {
            PropDef def = _businessObject.ClassDef.GetPropDef(propertyName);
            if (def.LookupListSource != null && def.LookupListSource.GetType() == typeof (DatabaseLookupListSource))
            {
                return ((DatabaseLookupListSource) def.LookupListSource).ClassDef;
            }
            return null;
        }
    }
}