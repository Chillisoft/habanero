//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Data;
using System.Security;
using System.Security.Principal;
using Habanero.Base;

namespace Habanero.BO.ConcurrencyControl
{
    /// <summary>
    /// Provides functionality to check if another user or process has 
    /// updated the object in the database since it was last read. 
    /// If there has been a conflict then the 
    /// BusObjUpdateConcurrencyControlException is thrown.
    /// A version number is used to check if the object has been edited. 
    /// To use this class effectively, the update properties method 
    /// must be called before the database is updated (i.e. in  
    /// <see cref="BusinessObject.Save"/> ) 
    /// <see cref= "UpdatePropertiesWithLatestConcurrencyInfoBeforePersisting"/>)
    /// This class implements the followign concurrency control philosophy.
    /// Every time a non dirty object is retrieved from the database the 
    /// </summary>
    public class OptimisticLockingVersionNumberDB : IConcurrencyControl
    {
        private enum VerificationStage
        {
            BeforeBeginEdit,
            BeforePersist
        }

        private readonly BusinessObject _busObj;
        private readonly IBOProp _dateLastUpdated;
        private readonly IBOProp _userLastUpdated;
        private readonly IBOProp _machineLastUpdated;
        private readonly IBOProp _versionNumber;
        private readonly IBOProp _operatingSystemUser;

        /// <summary>
        /// Constructor to initialise a new instance with details of the last
        /// update of the object in the database
        /// </summary>
        /// <param name="busObj">The business object on which to perform concurrency control</param>
        /// <param name="dateLastUpdated">The date that the object was
        /// last updated</param>
        /// <param name="userLastUpdated">The user that last updated the
        /// object</param>
        /// <param name="machineLastUpdated">The machine name on which the
        /// object was last updated</param>
        /// <param name="versionNumber">The version number</param>
        public OptimisticLockingVersionNumberDB(BusinessObject busObj,
                                                IBOProp dateLastUpdated,
                                                IBOProp userLastUpdated,
                                                IBOProp machineLastUpdated,
                                                IBOProp versionNumber)
        {
            _busObj = busObj;
            _dateLastUpdated = dateLastUpdated;
            _userLastUpdated = userLastUpdated;
            _machineLastUpdated = machineLastUpdated;
            _versionNumber = versionNumber;
            _operatingSystemUser = null;
        }

        /// <summary>
        /// Constructor as before, but allows the operating system on which
        /// the update was done to be specified
        /// </summary>
        public OptimisticLockingVersionNumberDB(BusinessObject busObj,
                                                BOProp dateLastUpdated,
                                                BOProp userLastUpdated,
                                                BOProp machineLastUpdated,
                                                BOProp versionNumber,
                                                BOProp operatingSystemUser)
            : this(busObj, dateLastUpdated, userLastUpdated,
                   machineLastUpdated, versionNumber)
        {
            _operatingSystemUser = operatingSystemUser;
        }

        /// <summary>
        /// Checks concurrency before persisting an object to the database
        /// in order to prevent one of two conflicting copies from being lost.
        /// Throws an exception if the object has been edited by another
        /// process/user, as determined by a version number.
        /// The object is persisted by calling 
        /// <see cref= "UpdatePropertiesWithLatestConcurrencyInfoBeforePersisting"/>.
        /// </summary>
        /// <exception cref="BusObjDeleteConcurrencyControlException">Thrown if 
        /// the object has been deleted by another process/user</exception>
        /// <exception cref="BusObjOptimisticConcurrencyControlException">Thrown 
        /// if the object has been edited by another process/user</exception>
        public void CheckConcurrencyBeforePersisting()
        {
            CheckConcurrencyControl(VerificationStage.BeforePersist);
        }

        private void CheckConcurrencyControl(VerificationStage verificationStage)
        {
            if (!_busObj.State.IsNew) //you cannot have concurrency control issues on a new object 
                // all you can have is duplicate data issues.
            {
                using (IDataReader dr = BOLoader.Instance.LoadDataReader(_busObj, _busObj.GetDatabaseConnection(), null)
                    )
                {
                    // If this object no longer exists in the database
                    // then we have a concurrency conflict since it has been deleted by another process.
                    // If our objective was to delete it as well then no worries else throw error.
                    bool drHasData = dr.Read();
                    if (! (drHasData) && !_busObj.State.IsDeleted)
                    {
                        //The object you are trying to save has been deleted by another user.
                        throw new BusObjDeleteConcurrencyControlException(_busObj.ClassName, _busObj.ID.ToString(),
                                                                          _busObj);
                    }
                    else
                    {
                        int versionNumberBusinessObject = (int) _versionNumber.Value;
                        int versionNumberDB = (int) dr[_versionNumber.DatabaseFieldName];

                        if (versionNumberDB != versionNumberBusinessObject)
                        {
                            string dateLastUpdatedInDB = dr[_dateLastUpdated.DatabaseFieldName].ToString();
                            string userNameLastUpdated = (string) dr[_userLastUpdated.DatabaseFieldName];
                            string machineLastUpdated = (string) dr[_machineLastUpdated.DatabaseFieldName];
                            ThrowConcurrencyException(verificationStage, userNameLastUpdated, machineLastUpdated,
                                                      dateLastUpdatedInDB);
                        }
                    }
                }
            }
        }

        private void ThrowConcurrencyException(VerificationStage verificationStage, string userNameLastUpdated,
                                               string machineLastUpdated, string dateLastUpdatedInDB)
        {
            if (verificationStage == VerificationStage.BeforeBeginEdit)
            {
                throw new BusObjBeginEditConcurrencyControlException(_busObj.ClassName,
                                                                     userNameLastUpdated,
                                                                     machineLastUpdated,
                                                                     DateTime.Parse(dateLastUpdatedInDB),
                                                                     _busObj.ID.ToString(), _busObj);
            }
            throw new BusObjOptimisticConcurrencyControlException(_busObj.ClassName,
                                                                  userNameLastUpdated,
                                                                  machineLastUpdated,
                                                                  DateTime.Parse(dateLastUpdatedInDB),
                                                                  _busObj.ID.ToString(), _busObj);
        }

        /// <summary>
        /// Checks concurrency before the user begins editing an object, in
        /// order to avoid the user making changes to an object and then losing
        /// those changes when the committal process shows a concurrency
        /// failure
        /// </summary>
        /// <exception cref="BusObjBeginEditConcurrencyControlException">Thrown
        /// if the object has been edited in the database since it was last
        /// loaded by the object manager</exception>
        public void CheckConcurrencyBeforeBeginEditing()
        {
            CheckConcurrencyControl(VerificationStage.BeforeBeginEdit);
        }

        /// <summary>
        /// Updates the version number, machine name, username and time edited.
        /// The version number is used to determine whether there is a 
        /// concurrency conflict, and the other properties are used for 
        /// reporting of concurrency conflicts if they occur.
        /// </summary>
        public void UpdatePropertiesWithLatestConcurrencyInfoBeforePersisting()
        {
            _dateLastUpdated.Value = DateTime.Now;

            GetUserName();

            GetMachineName();
            if (_versionNumber.Value == null) _versionNumber.Value = 0;
            _versionNumber.Value = (int) _versionNumber.Value + 1;
            GetOperatingSystemUser();
            //	TODO: maybe add thread identity on later.			threadIdentity = Thread.CurrentPrincipal.Identity.Name;
        }

        private void GetOperatingSystemUser()
        {
            if ((_operatingSystemUser == null)) return;

            try
            {
                _operatingSystemUser.Value = WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
            }
        }

        private void GetMachineName()
        {
            if ((_machineLastUpdated == null)) return;
            try
            {
                _machineLastUpdated.Value = Environment.MachineName;
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void GetUserName()
        {
            if ((_userLastUpdated == null)) return;
            try
            {
                //TODO Temp code possibly integrate with a system to get the user as
                // per custom code for now use the OS user.
                _userLastUpdated.Value = WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
            }
        }

        /// <summary>
        /// Does nothing since optimistic locking is used and no locks are applied
        /// </summary>
        public void ReleaseWriteLocks()
        {
        }

        #region IConcurrencyControl Members

        ///<summary>
        /// Makes any changes required to the concurrency control mechanism 
        /// to assert that the transaction has failed and thus been rolled back.
        ///</summary>
        public void UpdateAsTransactionRolledBack()
        {
            if (_versionNumber.Value == null)
            {
                _versionNumber.Value = 0;
                return;
            }
            _versionNumber.Value = (int)_versionNumber.Value - 1;
        }

        #endregion
    }
}