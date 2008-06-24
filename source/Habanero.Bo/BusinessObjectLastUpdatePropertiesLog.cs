using System;
using Habanero.Base;

namespace Habanero.BO
{
    ///<summary>
    /// This business object update log class updates the necessary logging fields on a business object.
    /// These logging fields are either passed in explicitly or when the class is initialised with the 
    /// business object the default fields are found and used.
    /// These fields are then updated when the update method is executed.
    /// There is also an optional parameter of the constructor for the ISecurityController for getting 
    /// the current user name. If no Security Controller is passed in then 
    /// the Global Registry's ISecurityController is used.
    ///</summary>
    public class BusinessObjectLastUpdatePropertiesLog : IBusinessObjectUpdateLog
    {
        private readonly ISecurityController _securityController;
        private readonly BOProp _userLastUpdatedBoProp;
        private readonly BOProp _dateLastUpdatedBoProp;

        #region Constructors

        ///<summary>
        /// This constructor initialises this update log with the UserLastUpdated and DateLastUpdated properties 
        /// that are to be updated when the BusinessObject is updated.
        ///</summary>
        ///<param name="userLastUpdatedBoProp">The UserLastUpdated property</param>
        ///<param name="dateLastUpdatedBoProp">The DateLastUpdated property</param>
        public BusinessObjectLastUpdatePropertiesLog(BOProp userLastUpdatedBoProp, BOProp dateLastUpdatedBoProp)
        {
            _userLastUpdatedBoProp = userLastUpdatedBoProp;
            _dateLastUpdatedBoProp = dateLastUpdatedBoProp;
        }

        ///<summary>
        /// This constructor initialises this update log with the UserLastUpdated and DateLastUpdated properties 
        /// that are to be updated when the BusinessObject is updated, and the ISecurityController to be used to 
        /// retrieve the current user name.
        ///</summary>
        ///<param name="userLastUpdatedBoProp">The UserLastUpdated property</param>
        ///<param name="dateLastUpdatedBoProp">The DateLastUpdated property</param>
        ///<param name="securityController">The ISecurityController class</param>
        public BusinessObjectLastUpdatePropertiesLog(BOProp userLastUpdatedBoProp, BOProp dateLastUpdatedBoProp, ISecurityController securityController)
            : this(userLastUpdatedBoProp, dateLastUpdatedBoProp)
        {
            _securityController = securityController;
        }

        ///<summary>
        /// This constructor initialises this update log with the BusinessObject to be updated.
        /// This businessobject is then searched for the default UserLastUpdated and DateLastUpdated properties 
        /// that are to be updated when the BusinessObject is updated.
        ///</summary>
        ///<param name="businessObject">The BusinessObject to be updated</param>
        public BusinessObjectLastUpdatePropertiesLog(BusinessObject businessObject)
        {
            BOPropCol boPropCol = businessObject.Props;
            string propName;
            propName = "UserLastUpdated";
            if (boPropCol.Contains(propName))
            {
                _userLastUpdatedBoProp = boPropCol[propName];
            }
            propName = "DateLastUpdated";
            if (boPropCol.Contains(propName))
            {
                _dateLastUpdatedBoProp = boPropCol[propName];
            }
        }

        ///<summary>
        /// This constructor initialises this update log with the BusinessObject to be updated.
        /// This businessobject is then searched for the default UserLastUpdated and DateLastUpdated properties 
        /// that are to be updated when the BusinessObject is updated. The ISecurityController to be used to 
        /// retrieve the current user name is also passed in.
        ///</summary>
        ///<param name="businessObject">The BusinessObject to be updated</param>
        ///<param name="securityController">The ISecurityController class</param>
        public BusinessObjectLastUpdatePropertiesLog(BusinessObject businessObject, ISecurityController securityController)
            : this(businessObject)
        {
            _securityController = securityController;
        }

        #endregion //Constructors

        #region IBusinessObjectUpdateLog Members

        ///<summary>
        /// Perform the log action for this Update Log class.
        ///</summary>
        public void Update()
        {
            if (_userLastUpdatedBoProp != null) _userLastUpdatedBoProp.Value = GetCurrentUserName();
            if (_dateLastUpdatedBoProp != null) _dateLastUpdatedBoProp.Value = DateTime.Now;
        }

        #endregion

        private string GetCurrentUserName()
        {
            if (_securityController != null) return _securityController.CurrentUserName;
            if (GlobalRegistry.SecurityController != null) return GlobalRegistry.SecurityController.CurrentUserName;
            return "";
        }

    }
}
