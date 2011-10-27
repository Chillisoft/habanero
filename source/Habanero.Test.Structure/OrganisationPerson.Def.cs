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
namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class OrganisationPerson : BusinessObject
    {
        
        #region Properties
        public virtual Guid? OrganisatiionID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("OrganisatiionID")));
            }
            set
            {
                base.SetPropertyValue("OrganisatiionID", value);
            }
        }
        
        public virtual Guid? PersonID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("PersonID")));
            }
            set
            {
                base.SetPropertyValue("PersonID", value);
            }
        }
        
        public virtual String Relationship
        {
            get
            {
                return ((String)(base.GetPropertyValue("Relationship")));
            }
            set
            {
                base.SetPropertyValue("Relationship", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual Organisation Organisation
        {
            get
            {
                return Relationships.GetRelatedObject<Organisation>("Organisation");
            }
            set
            {
                Relationships.SetRelatedObject("Organisation", value);
            }
        }
        
        public virtual Person Person
        {
            get
            {
                return Relationships.GetRelatedObject<Person>("Person");
            }
            set
            {
                Relationships.SetRelatedObject("Person", value);
            }
        }
        #endregion
    }
}
