//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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
        private readonly IBOProp _userLastUpdatedBoProp;
        private readonly IBOProp _dateLastUpdatedBoProp;

        #region Constructors

        ///<summary>
        /// This constructor initialises this update log with the UserLastUpdated and DateLastUpdated properties 
        /// that are to be updated when the BusinessObject is updated.
        ///</summary>
        ///<param name="userLastUpdatedBoProp">The UserLastUpdated property</param>
        ///<param name="dateLastUpdatedBoProp">The DateLastUpdated property</param>
        public BusinessObjectLastUpdatePropertiesLog(IBOProp userLastUpdatedBoProp, IBOProp dateLastUpdatedBoProp)
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
        public BusinessObjectLastUpdatePropertiesLog(IBOProp userLastUpdatedBoProp, IBOProp dateLastUpdatedBoProp, ISecurityController securityController)
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
        public BusinessObjectLastUpdatePropertiesLog(IBusinessObject businessObject)
        {
            IBOPropCol boPropCol = businessObject.Props;
            string propName = "UserLastUpdated";
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
        public BusinessObjectLastUpdatePropertiesLog(IBusinessObject businessObject, ISecurityController securityController)
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
