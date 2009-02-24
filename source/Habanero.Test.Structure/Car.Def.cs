
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Car : Vehicle
    {
        
        #region Properties
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
        
        public virtual String RegistrationNo
        {
            get
            {
                return ((String)(base.GetPropertyValue("RegistrationNo")));
            }
            set
            {
                base.SetPropertyValue("RegistrationNo", value);
            }
        }
        
        public virtual Double? Length
        {
            get
            {
                return ((Double?)(base.GetPropertyValue("Length")));
            }
            set
            {
                base.SetPropertyValue("Length", value);
            }
        }
        
        public virtual Boolean? IsConvertible
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("IsConvertible")));
            }
            set
            {
                base.SetPropertyValue("IsConvertible", value);
            }
        }
        
        public virtual Guid? DriverID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("DriverID")));
            }
            set
            {
                base.SetPropertyValue("DriverID", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual Person Driver
        {
            get
            {
                return Relationships.GetRelatedObject<Person>("Driver");
            }
            set
            {
                Relationships.SetRelatedObject("Driver", value);
            }
        }
        #endregion
    }
}
