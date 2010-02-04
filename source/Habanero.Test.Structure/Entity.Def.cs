
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Entity : BusinessObject
    {
        
        #region Properties
        public virtual Guid? EntityID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("EntityID")));
            }
            set
            {
                base.SetPropertyValue("EntityID", value);
            }
        }
        
        public virtual String EntityType
        {
            get
            {
                return ((String)(base.GetPropertyValue("EntityType")));
            }
            set
            {
                base.SetPropertyValue("EntityType", value);
            }
        }
        #endregion
    }
}
