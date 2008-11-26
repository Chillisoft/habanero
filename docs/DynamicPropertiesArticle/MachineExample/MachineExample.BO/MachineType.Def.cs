
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
    
    
    public partial class MachineType : BusinessObject
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
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<MachinePropertyDef> MachinePropertyDefs
        {
            get
            {
                return Relationships.GetRelatedCollection<MachinePropertyDef>("MachinePropertyDefs");
            }
        }
        
        public virtual BusinessObjectCollection<Machine> Machines
        {
            get
            {
                return Relationships.GetRelatedCollection<Machine>("Machines");
            }
        }
        #endregion
    }
}
