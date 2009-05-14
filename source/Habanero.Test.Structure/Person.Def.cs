
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

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
