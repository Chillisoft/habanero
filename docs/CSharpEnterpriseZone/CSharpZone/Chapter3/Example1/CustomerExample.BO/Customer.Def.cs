//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CustomerExample.BO
{
    using System;
    using Habanero.BO;
    
    
    public partial class Customer : BusinessObject
    {
        
        #region Properties
        public virtual String CustomerName
        {
            get
            {
                return ((String)(base.GetPropertyValue("CustomerName")));
            }
            set
            {
                base.SetPropertyValue("CustomerName", value);
            }
        }
        
        public virtual Guid? CustomerID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("CustomerID")));
            }
            set
            {
                base.SetPropertyValue("CustomerID", value);
            }
        }
        
        public virtual String CustomerCode
        {
            get
            {
                return ((String)(base.GetPropertyValue("CustomerCode")));
            }
            set
            {
                base.SetPropertyValue("CustomerCode", value);
            }
        }
        
        public virtual DateTime? DateCustomerApproved
        {
            get
            {
                return ((DateTime?)(base.GetPropertyValue("DateCustomerApproved")));
            }
            set
            {
                base.SetPropertyValue("DateCustomerApproved", value);
            }
        }
        #endregion
    }
}
