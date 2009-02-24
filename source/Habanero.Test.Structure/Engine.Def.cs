
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Engine : Part
    {
        
        #region Properties
        public virtual Guid? EngineID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("EngineID")));
            }
            set
            {
                base.SetPropertyValue("EngineID", value);
            }
        }
        
        public virtual String EngineNo
        {
            get
            {
                return ((String)(base.GetPropertyValue("EngineNo")));
            }
            set
            {
                base.SetPropertyValue("EngineNo", value);
            }
        }
        
        public virtual DateTime? DateManufactured
        {
            get
            {
                return ((DateTime?)(base.GetPropertyValue("DateManufactured")));
            }
            set
            {
                base.SetPropertyValue("DateManufactured", value);
            }
        }
        
        public virtual Int32? HorsePower
        {
            get
            {
                return ((Int32?)(base.GetPropertyValue("HorsePower")));
            }
            set
            {
                base.SetPropertyValue("HorsePower", value);
            }
        }
        
        public virtual Boolean? FuelInjected
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("FuelInjected")));
            }
            set
            {
                base.SetPropertyValue("FuelInjected", value);
            }
        }
        
        public virtual Guid? CarID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("CarID")));
            }
            set
            {
                base.SetPropertyValue("CarID", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual Car Car
        {
            get
            {
                return Relationships.GetRelatedObject<Car>("Car");
            }
            set
            {
                Relationships.SetRelatedObject("Car", value);
            }
        }
        #endregion
    }
}
