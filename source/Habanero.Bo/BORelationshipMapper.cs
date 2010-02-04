// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// This is a mapper class that handles the mapping of a relationship name 
    /// to a specific relationship for a specified <see cref="IBusinessObject"/>.
    /// The relationship name can be specified as a path through single relationships on the <see cref="IBusinessObject"/>
    /// and its' relationship tree.
    /// <remarks>For Example:<br/>
    /// For the ContactPerson BusinessObject when the relationshipName is "Organisation",
    ///  the returned <see cref="IRelationship"/> will be the "Organisation" relationship on ContactPerson.<br/>
    /// If the relationshipName was "Organisation.Address" then the Organisation relationship on the contact person will 
    /// be traversed and monitored and return the corresponding "Address" <see cref="IRelationship"/> for the ContactPerson's current Organisation.</remarks>
    /// </summary>
    public class BORelationshipMapper
    {
        private IBusinessObject _businessObject;
        private IRelationship _relationship;
        private readonly BORelationshipMapper _childBoRelationshipMapper;
        private readonly BORelationshipMapper _localBoRelationshipMapper;
        private ISingleRelationship _childRelationship;

        ///<summary>
        /// Creates a BORelationshipMapper for the specified relationship name/path.
        ///</summary>
        ///<param name="relationshipName">The name of the relationship to be mapped (this could also be in the form of a path through single relationships on the BO).</param>
        ///<exception cref="ArgumentNullException">This is thrown if <paramref name="relationshipName"/> is null or empty.</exception>
        public BORelationshipMapper(string relationshipName)
        {
            if (String.IsNullOrEmpty(relationshipName)) throw new ArgumentNullException("relationshipName");
            RelationshipName = relationshipName;
            if (RelationshipName.Contains("."))
            {
                string[] parts = RelationshipName.Split('.');
                string localRelationshipName = parts[0];
                _localBoRelationshipMapper = new BORelationshipMapper(localRelationshipName);
                string remainingPath = String.Join(".", parts, 1, parts.Length - 1);
                _childBoRelationshipMapper = new BORelationshipMapper(remainingPath);
                _childBoRelationshipMapper.RelationshipChanged += (sender, e) => FireRelationshipChanged();
            }
        }

        /// <summary>
        /// This event is fired when the current <see cref="Relationship"/> object changes, either through 
        /// the current <see cref="BusinessObject"/> being changed, or one of the related BusinessObjects in the 
        /// mapped Relationship Path has been changed.
        /// </summary>
        public event EventHandler RelationshipChanged;

        ///<summary>
        /// The name of the relationship to be mapped. 
        /// This could also be in the form of a path through single relationships on the BO.
        /// See <see cref="BORelationshipMapper"/> for more details.
        ///</summary>
        public string RelationshipName { get; private set; }

        ///<summary>
        /// The relationship for the current <see cref="BusinessObject"/> that is mapped by the given <see cref="RelationshipName"/>.
        ///</summary>
        public IRelationship Relationship
        {
            get
            {
                if (_childBoRelationshipMapper != null) return _childBoRelationshipMapper.Relationship;
                return _relationship;
            }
            private set
            {
                _relationship = value;
                FireRelationshipChanged();
            }
        }

        ///<summary>
        /// The BusinessObject for which the Relationship is being mapped.
        /// Once this property has been set, the <see cref="BORelationshipMapper"/>.<see cref="Relationship"/> property will be populated accordingly.
        ///</summary>
        ///<exception cref="HabaneroDeveloperException">This is thrown if the specified relationship does not exist on the <see cref="IBusinessObject"/> being set or if one of the Relationships within the Relationship Path is not a single Relationship.</exception>
        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                IBusinessObject businessObject = value;
                IRelationship relationship = null;

                if (_childBoRelationshipMapper != null)
                {
                    _localBoRelationshipMapper.BusinessObject = businessObject;
                    UpdateChildRelationship();
                    _businessObject = businessObject;
                    return;
                }
                if (businessObject != null)
                {
                    if (businessObject.Relationships.Contains(RelationshipName))
                        relationship = businessObject.Relationships[RelationshipName];
                    else
                    {
                        IClassDef classDef = businessObject.ClassDef;
                        throw new HabaneroDeveloperException("The relationship '" + RelationshipName + "' on '"
                                                             + classDef.ClassName + "' cannot be found. Please contact your system administrator.",
                                                             "The relationship '" + RelationshipName + "' does not exist on the BusinessObject '"
                                                             + classDef.ClassNameFull + "'.");
                    }
                }
                _businessObject = businessObject;
                Relationship = relationship;
            }
        }

        private void UpdateChildRelationship()
        {
            DeRegisterForChildRelationshipEvents();
            IRelationship childRelationship = _localBoRelationshipMapper.Relationship;
            if (childRelationship != null && !(childRelationship is ISingleRelationship))
            {
                IClassDef classDef = childRelationship.OwningBO.ClassDef;
                throw new HabaneroDeveloperException("The relationship '" + _localBoRelationshipMapper.RelationshipName + "' on '"
                                                     + classDef.ClassName + "' is not a Single Relationship. Please contact your system administrator.",
                                                     "The relationship '" + _localBoRelationshipMapper.RelationshipName + "' on the BusinessObject '"
                                                     + classDef.ClassNameFull + "' is not a Single Relationship therefore cannot be traversed.");
            }
            _childRelationship = (ISingleRelationship)childRelationship;
            RegisterForChildRelationshipEvents();
            UpdateChildRelationshipBO();
        }

        private void RegisterForChildRelationshipEvents()
        {
            if (_childRelationship != null)
                _childRelationship.RelatedBusinessObjectChanged += ChildRelationship_OnRelatedBusinessObjectChanged;
        }

        private void DeRegisterForChildRelationshipEvents()
        {
            if (_childRelationship != null) _childRelationship.RelatedBusinessObjectChanged -= ChildRelationship_OnRelatedBusinessObjectChanged;
        }

        private void ChildRelationship_OnRelatedBusinessObjectChanged(object sender, EventArgs e)
        {
            UpdateChildRelationshipBO();
        }

        private void UpdateChildRelationshipBO()
        {
            IBusinessObject relatedObject = null;
            if (_childRelationship != null) relatedObject = _childRelationship.GetRelatedObject();
            _childBoRelationshipMapper.BusinessObject = relatedObject;
        }

        private void FireRelationshipChanged()
        {
            if (RelationshipChanged != null) RelationshipChanged(this, new EventArgs());
        }
    }
}