
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class LegalEntity : Entity
    {
        
        #region Properties
        public virtual Guid? LegalEntityID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("LegalEntityID")));
            }
            set
            {
                base.SetPropertyValue("LegalEntityID", value);
            }
        }
        
        public virtual String LegalEntityType
        {
            get
            {
                return ((String)(base.GetPropertyValue("LegalEntityType")));
            }
            set
            {
                base.SetPropertyValue("LegalEntityType", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<Vehicle> VehiclesOwned
        {
            get
            {
                return Relationships.GetRelatedCollection<Vehicle>("VehiclesOwned");
            }
        }
        #endregion
    }
}
