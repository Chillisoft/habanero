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
    
    
    public partial class Organisation : LegalEntity
    {
        
        #region Properties
        public virtual String Name
        {
            get
            {
                return ((String)(base.GetPropertyValue("Name")));
            }
            set
            {
                base.SetPropertyValue("Name", value);
            }
        }
        
        public virtual String DateFormed
        {
            get
            {
                return ((String)(base.GetPropertyValue("DateFormed")));
            }
            set
            {
                base.SetPropertyValue("DateFormed", value);
            }
        }
        
        public virtual Guid? OrganisationID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("OrganisationID")));
            }
            set
            {
                base.SetPropertyValue("OrganisationID", value);
            }
        }
        #endregion
        
        #region Relationships
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
