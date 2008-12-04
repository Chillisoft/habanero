
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace MachineExample.BO
{
    using System;
    using Habanero.BO;
    using MachineExample.BO;
    
    
    public partial class Machine : BusinessObject
    {
        
        #region Properties
        public virtual Guid? MachineID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("MachineID")));
            }
            set
            {
                base.SetPropertyValue("MachineID", value);
            }
        }
        
        public virtual Guid? MachineTypeID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("MachineTypeID")));
            }
            set
            {
                base.SetPropertyValue("MachineTypeID", value);
            }
        }
        
        public virtual String MachineNo
        {
            get
            {
                return ((String)(base.GetPropertyValue("MachineNo")));
            }
            set
            {
                base.SetPropertyValue("MachineNo", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<MachineProperty> MachineProperties
        {
            get
            {
                return Relationships.GetRelatedCollection<MachineProperty>("MachineProperties");
            }
        }
        
        public virtual MachineType MachineType
        {
            get
            {
                return Relationships.GetRelatedObject<MachineType>("MachineType");
            }
            set
            {
                Relationships.SetRelatedObject("MachineType", value);
            }
        }
        #endregion
    }
}
