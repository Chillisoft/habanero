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
namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Person : LegalEntity
    {
        
        #region Properties
        public virtual String IDNumber
        {
            get
            {
                return ((String)(base.GetPropertyValue("IDNumber")));
            }
            set
            {
                base.SetPropertyValue("IDNumber", value);
            }
        }
        
        public virtual String FirstName
        {
            get
            {
                return ((String)(base.GetPropertyValue("FirstName")));
            }
            set
            {
                base.SetPropertyValue("FirstName", value);
            }
        }
        
        public virtual String LastName
        {
            get
            {
                return ((String)(base.GetPropertyValue("LastName")));
            }
            set
            {
                base.SetPropertyValue("LastName", value);
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
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<Car> CarsDriven
        {
            get
            {
                return Relationships.GetRelatedCollection<Car>("CarsDriven");
            }
        }
        
        public virtual BusinessObjectCollection<OrganisationPerson> OrganisationPerson
        {
            get
            {
                return Relationships.GetRelatedCollection<OrganisationPerson>("OrganisationPerson");
            }
        }
        #endregion
    }
}
