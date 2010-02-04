
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

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
