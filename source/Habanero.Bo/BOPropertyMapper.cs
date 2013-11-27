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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// This is a mapper class that handles the mapping of a property name 
    /// to a specific property for a specified <see cref="IBusinessObject"/>.
    /// The property name can be specified as a path through single relationships 
    /// on the <see cref="IBusinessObject"/>
    /// and its' relationship tree.
    /// The property name can also be specified as a reflective Property i.e. a 
    /// property on the business object that is not mapped to a persistent prop of
    /// the business object.
    /// <remarks>For Related Prop Example:<br/>
    /// For the ContactPerson BusinessObject when the propertyName is "FirstName",
    ///  the returned <see cref="IBOProp"/> will be the "FirstName" property on ContactPerson.<br/>
    /// If the propertyName was "Organisation.Name" then the Organisation relationship on the contact person will 
    /// be traversed and monitored and return the corresponding "Name" <see cref="IBOProp"/> for the ContactPerson's current Organisation.</remarks>
    /// <remarks>For Reflective Prop Example:<br/>
    /// For the ContactPerson BusinessObject you may have a derived Property age
    ///  that is calculated based on date of birth.</remarks>
    /// </summary>
    public interface IBOPropertyMapper{
        ///<summary>
        /// The BusinessObject for which the Property is being mapped.
        ///</summary>
        ///<exception cref="HabaneroDeveloperException">This is thrown if the specified property does not exist on the <see cref="IBusinessObject"/> being set or if one of the Relationships within the Property Path is not a single relationship.</exception>
        IBusinessObject BusinessObject { get;}
        ///<summary>
        /// The name of the property to be mapped. 
        /// This could also be in the form of a path through single relationships on the BO.
        /// See <see cref="BOPropertyMapper"/> for more details.
        ///</summary>
        string PropertyName { get; }

        /// <summary>
        /// If the Property is invalid then returns the Invalid reason.
        /// </summary>
        string InvalidReason { get; }

        /// <summary>
        /// Sets the BOProp that this mapper is mapped to the associated propValue
        /// </summary>
        /// <param name="propValue"></param>
        void SetPropertyValue(object propValue);

        /// <summary>
        /// Return the Property Value for the Property being mapped.
        /// </summary>
        /// <returns></returns>
        object GetPropertyValue();
    }
    /// <summary>
    /// This is a mapper class that handles the mapping of a property name 
    /// to a specific property for a specified <see cref="IBusinessObject"/>.
    /// The property name can be specified as a path through single relationships on the <see cref="IBusinessObject"/>
    /// and its' relationship tree.
    /// <remarks>For Example:<br/>
    /// For the ContactPerson BusinessObject when the propertyName is "FirstName",
    ///  the returned <see cref="IBOProp"/> will be the "FirstName" property on ContactPerson.<br/>
    /// If the propertyName was "Organisation.Name" then the Organisation relationship on the contact person will 
    /// be traversed and monitored and return the corresponding "Name" <see cref="IBOProp"/> for the ContactPerson's current Organisation.</remarks>
    /// </summary>
    public class BOPropertyMapper : IBOPropertyMapper
    {
        private IBusinessObject _businessObject;
        /// <summary>
        /// The property used to map.
        /// </summary>
        protected IBOProp _property;
        private readonly BOPropertyMapper _childBoPropertyMapper;

        /// <summary>
        /// The BOPropertyMapper for the child of this mapper. 
        /// For a string of property names (eg Person.Address.FirstLine), the Person would 
        /// have a BOPropertyMapper and the Address would have a BOPropertyMapper
        /// that would be the child of the Person one.
        /// </summary>
        public BOPropertyMapper ChildBoPropertyMapper
        {
            get { return _childBoPropertyMapper; }
        }

        private readonly BORelationshipMapper _relationshipPathMapper;
        private ISingleRelationship _childRelationship;
        private const string RELATIONSHIP_SEPARATOR = ".";

        ///<summary>
        /// Creates a BOPropertyMapper for the specified property name/path.
        ///</summary>
        ///<param name="propertyName">The name of the property to be mapped (this could also be in the form of a path through single relationships on the BO).</param>
        ///<exception cref="ArgumentNullException">This is thrown if <paramref name="propertyName"/> is null or empty.</exception>
        public BOPropertyMapper(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");
            PropertyName = propertyName;
            if (PropertyName.Contains(RELATIONSHIP_SEPARATOR))
            {
                string[] parts = PropertyName.Split('.');
                string relationshipPath = String.Join(RELATIONSHIP_SEPARATOR, parts, 0, parts.Length - 1);
                _relationshipPathMapper = new BORelationshipMapper(relationshipPath);
                string childPropertyName = parts[parts.Length - 1];
                _childBoPropertyMapper = new BOPropertyMapper(childPropertyName);
                _childBoPropertyMapper.PropertyChanged += (sender, e) => FirePropertyChanged();
            }
        }

        /// <summary>
        /// This event is fired when the current <see cref="Property"/> object changes, either through 
        /// the current <see cref="BusinessObject"/> being changed, or one of the related BusinessObjects in the 
        /// mapped Property path has been changed.
        /// </summary>
        public event EventHandler PropertyChanged;

        ///<summary>
        /// The name of the property to be mapped. 
        /// This could also be in the form of a path through single relationships on the BO.
        /// See <see cref="BOPropertyMapper"/> for more details.
        ///</summary>
        public string PropertyName { get; private set; }

        ///<summary>
        /// The property for the current <see cref="BusinessObject"/> that is mapped by the given <see cref="PropertyName"/>.
        ///</summary>
        public IBOProp Property
        {
            get
            {
                if (_childBoPropertyMapper != null) return _childBoPropertyMapper.Property;
                return _property;
            }
            private set
            {
                _property = value;
                FirePropertyChanged();
            }
        }

        ///<summary>
        /// The BusinessObject for which the Property is being mapped.
        /// Once this property has been set, the <see cref="BOPropertyMapper"/>.<see cref="Property"/> property will be populated accordingly.
        ///</summary>
        ///<exception cref="InvalidPropertyException">This is thrown if the specified property does not exist on the <see cref="IBusinessObject"/> being set or if one of the Relationships within the Property Path is not a single relationship.</exception>
        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                IBusinessObject businessObject = value;
                IBOProp property = null;

                if (_childBoPropertyMapper != null)
                {
                    _relationshipPathMapper.BusinessObject = businessObject;
                    UpdateChildProperty();
                    _businessObject = businessObject;
                    return;
                }
                if (businessObject != null)
                {
                    if (businessObject.Props.Contains(PropertyName))
                        property = businessObject.Props[PropertyName];
                    else
                    {
                        IClassDef classDef = businessObject.ClassDef;
                        throw new InvalidPropertyException("The property '" + PropertyName + "' on '"
                                                             + classDef.ClassName + "' cannot be found. Please contact your system administrator.");
                    }
                }
                _businessObject = businessObject;
                Property = property;
            }
        }

        private void UpdateChildProperty()
        {
            DeRegisterForChildRelationshipEvents();
            IRelationship childRelationship = _relationshipPathMapper.Relationship;
            if (childRelationship != null && !(childRelationship is ISingleRelationship))
            {
                IClassDef classDef = childRelationship.OwningBO.ClassDef;
                throw new RelationshipNotFoundException("The relationship '" + _relationshipPathMapper.RelationshipName + "' on '"
                                                     + classDef.ClassName + "' is not a Single Relationship. Please contact your system administrator.");
            }
            _childRelationship = (ISingleRelationship)childRelationship;
            RegisterForChildRelationshipEvents();
            UpdateChildPropertyBO();
        }

        private void RegisterForChildRelationshipEvents()
        {
            if (_childRelationship != null)
                _childRelationship.RelatedBusinessObjectChanged += ChildProperty_OnRelatedBusinessObjectChanged;
        }

        private void DeRegisterForChildRelationshipEvents()
        {
            if (_childRelationship != null) _childRelationship.RelatedBusinessObjectChanged -= ChildProperty_OnRelatedBusinessObjectChanged;
        }

        private void ChildProperty_OnRelatedBusinessObjectChanged(object sender, EventArgs e)
        {
            UpdateChildPropertyBO();
        }

        private void UpdateChildPropertyBO()
        {
            IBusinessObject relatedObject = null;
            if (_childRelationship != null) relatedObject = _childRelationship.GetRelatedObject();
            _childBoPropertyMapper.BusinessObject = relatedObject;
        }

        private void FirePropertyChanged()
        {
            if (PropertyChanged != null) PropertyChanged(this, new EventArgs());
        }
        /// <summary>
        /// Sets the BOProp that this mapper is mapped to the associated propValue
        /// </summary>
        /// <param name="propValue"></param>
        public void SetPropertyValue(object propValue)
        {
            CheckBusinessObjectSet("Set Property Value");
            //Ideally we should raise an error when the Property is null 
            // but this is replacing code that currently just ignores
            // this. In the future as we refactor this to 
            // include reflective props then this should be changed.
            var boProp = this.Property;
            if(boProp != null) boProp.Value = propValue;
        }

        private void CheckBusinessObjectSet(string methodName)
        {
            if(this.BusinessObject == null)
            {
                throw new HabaneroApplicationException(string.Format("Tried to {1} the BOPropertyMapper for Property '{0}' when the BusinessObject is not set ", this.PropertyName, methodName));
            }
        }
        /// <summary>
        /// Return the Property Value for the Property being mapped.
        /// </summary>
        /// <returns></returns>
        public object GetPropertyValue()
        {
            CheckBusinessObjectSet("GetPropertyValue");
            //Ideally we should raise an error when the Property is null 
            // but this is replacing code that currently just ignores
            // this. In the future as we refactor this to 
            // include reflective props then this should be changed.
            var boProp = this.Property;
            return boProp != null ? this.Property.PropertyValueToDisplay : null;
        }
        /// <summary>
        /// If the Property is invalid then returns the Invalid reason.
        /// </summary>
        public string InvalidReason
        {
            get
            {
                var boProp = this.Property;
                if (boProp == null)
                {
                    return string.Format("The Property '{0}' is not available"
                                         , PropertyName);
                }
                return boProp.InvalidReason;
            }
        }
    }

}