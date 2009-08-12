using System;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
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
    public class BOPropertyMapper
    {
        private IBusinessObject _businessObject;
        private IBOProp _property;
        private readonly BOPropertyMapper _childBoPropertyMapper;
        private readonly BORelationshipMapper _relationshipPathMapper;
        private ISingleRelationship _childRelationship;

        ///<summary>
        /// Creates a BOPropertyMapper for the specified property name/path.
        ///</summary>
        ///<param name="propertyName">The name of the property to be mapped (this could also be in the form of a path through single relationships on the BO).</param>
        ///<exception cref="ArgumentNullException">This is thrown if <paramref name="propertyName"/> is null or empty.</exception>
        public BOPropertyMapper(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");
            PropertyName = propertyName;
            if (PropertyName.Contains("."))
            {
                string[] parts = PropertyName.Split('.');
                string relationshipPath = String.Join(".", parts, 0, parts.Length - 1);
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
        ///<exception cref="HabaneroDeveloperException">This is thrown if the specified property does not exist on the <see cref="IBusinessObject"/> being set or if one of the Relationships within the Property Path is not a single relationship.</exception>
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
                        throw new HabaneroDeveloperException("The property '" + PropertyName + "' on '"
                                                             + classDef.ClassName + "' cannot be found. Please contact your system administrator.",
                                                             "The property '" + PropertyName + "' does not exist on the BusinessObject '"
                                                             + classDef.ClassNameFull + "'.");
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
                throw new HabaneroDeveloperException("The relationship '" + _relationshipPathMapper.RelationshipName + "' on '"
                                                     + classDef.ClassName + "' is not a Single Relationship. Please contact your system administrator.",
                                                     "The relationship '" + _relationshipPathMapper.RelationshipName + "' on the BusinessObject '"
                                                     + classDef.ClassNameFull + "' is not a Single Relationship therefore cannot be traversed.");
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
    }
}