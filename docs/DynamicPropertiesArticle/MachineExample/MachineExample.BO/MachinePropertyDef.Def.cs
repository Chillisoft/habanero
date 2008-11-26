
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
    
    
    public partial class MachinePropertyDef : BusinessObject
    {
        
        #region Properties
        public virtual String PropertyName
        {
            get
            {
                return ((String)(base.GetPropertyValue("PropertyName")));
            }
            set
            {
                base.SetPropertyValue("PropertyName", value);
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
        
        public virtual String PropertyType
        {
            get
            {
                return ((String)(base.GetPropertyValue("PropertyType")));
            }
            set
            {
                base.SetPropertyValue("PropertyType", value);
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
        
        public virtual Boolean? IsCompulsory
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("IsCompulsory")));
            }
            set
            {
                base.SetPropertyValue("IsCompulsory", value);
            }
        }
        #endregion
        
        #region Relationships
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
