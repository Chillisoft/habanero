
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Part : Entity
    {
        
        #region Properties
        public virtual Guid? PartID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("PartID")));
            }
            set
            {
                base.SetPropertyValue("PartID", value);
            }
        }
        
        public virtual String ModelNo
        {
            get
            {
                return ((String)(base.GetPropertyValue("ModelNo")));
            }
            set
            {
                base.SetPropertyValue("ModelNo", value);
            }
        }
        
        public virtual String PartType
        {
            get
            {
                return ((String)(base.GetPropertyValue("PartType")));
            }
            set
            {
                base.SetPropertyValue("PartType", value);
            }
        }
        #endregion
    }
}
