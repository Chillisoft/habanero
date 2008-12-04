
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
    
    
    public partial class MachineProperty : BusinessObject
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
        
        public virtual Guid? MachinePropertyDefID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("MachinePropertyDefID")));
            }
            set
            {
                base.SetPropertyValue("MachinePropertyDefID", value);
            }
        }
        
        public virtual String Value
        {
            get
            {
                return ((String)(base.GetPropertyValue("Value")));
            }
            set
            {
                base.SetPropertyValue("Value", value);
            }
        }
        
        public virtual Guid? MachinePropertyID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("MachinePropertyID")));
            }
            set
            {
                base.SetPropertyValue("MachinePropertyID", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual Machine Machine
        {
            get
            {
                return Relationships.GetRelatedObject<Machine>("Machine");
            }
            set
            {
                Relationships.SetRelatedObject("Machine", value);
            }
        }
        
        public virtual MachinePropertyDef MachinePropertyDef
        {
            get
            {
                return Relationships.GetRelatedObject<MachinePropertyDef>("MachinePropertyDef");
            }
            set
            {
                Relationships.SetRelatedObject("MachinePropertyDef", value);
            }
        }
        #endregion
    }
}
