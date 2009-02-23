
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

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
