using System;
using System.Data;
using System.Security;
using System.Security.Principal;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides functionality to check if another user or process has 
    /// updated the object in the database since it was last read. 
    /// If there has been a conflict then the 
    /// BusObjUpdateConcurrencyControlException is thrown.
    /// A version number is used to check if the object has been edited. 
    /// To use this class effectively, the update properties method 
    /// must be called before the database is updated (i.e. in  
    /// <see cref="Chillisoft.Bo.v2.BusinessObjectBase.ApplyEdit"/> and 
    /// <see cref= "Chillisoft.Bo.v2.OptimisticLockingVersionNumber.UpdatePropertiesWithLatestConcurrencyInfo"/>)
    /// </summary>
    /// TODO ERIC - could knock all the "prop"s off these fields and parameters
    public class OptimisticLockingVersionNumber : IConcurrencyControl
    {
        private BOProp mDateLastUpdatedProp;
        private BOProp mUserLastUpdatedProp;
        private BOProp mMachineLastUpdatedProp;
        private BOProp mVersionNumberProp;
        private BOProp mOperatingSystemUserProp;

        /// <summary>
        /// Constructor to initialise a new instance with details of the last
        /// update of the object in the database
        /// </summary>
        /// <param name="dateLastUpdatedProp">The date that the object was
        /// last updated</param>
        /// <param name="userLastUpdatedProp">The user that last updated the
        /// object</param>
        /// <param name="machineLastUpdatedProp">The machine name on which the
        /// object was last updated</param>
        /// <param name="versionNumberProp">The version number</param>
        public OptimisticLockingVersionNumber(BOProp dateLastUpdatedProp,
                                              BOProp userLastUpdatedProp,
                                              BOProp machineLastUpdatedProp,
                                              BOProp versionNumberProp)
        {
            mDateLastUpdatedProp = dateLastUpdatedProp;
            mUserLastUpdatedProp = userLastUpdatedProp;
            mMachineLastUpdatedProp = machineLastUpdatedProp;
            mVersionNumberProp = versionNumberProp;
        }

        /// <summary>
        /// Constructor as before, but allows the operating system on which
        /// the update was done to be specified
        /// </summary>
        public OptimisticLockingVersionNumber(BOProp dateLastUpdatedProp,
                                              BOProp userLastUpdatedProp,
                                              BOProp machineLastUpdatedProp,
                                              BOProp versionNumberProp,
                                              BOProp operatingSystemUserProp)
            : this(dateLastUpdatedProp, userLastUpdatedProp,
                   machineLastUpdatedProp, versionNumberProp)
        {
            mOperatingSystemUserProp = operatingSystemUserProp;
        }

        /// <summary>
        /// Checks concurrency before persisting an object to the database
        /// in order to prevent one of two conflicting copies from being lost.
        /// Throws an exception if the object has been edited by another
        /// process/user, as determined by a version number.
        /// The object is persisted by calling 
        /// <see cref= "Chillisoft.Bo.v2.OptimisticLockingVersionNumber.UpdatePropertiesWithLatestConcurrencyInfo"/>.
        /// </summary>
        /// <param name="busObj">The business object to be persisted</param>
        /// <exception cref="BusObjDeleteConcurrencyControlException">Thrown if 
        /// the object has been deleted by another process/user</exception>
        /// <exception cref="BusObjOptimisticConcurrencyControlException">Thrown 
        /// if the object has been edited by another process/user</exception>
        public void CheckConcurrencyBeforePersisting(BusinessObjectBase busObj)
        {
            if (!busObj.IsNew) //you cannot have concurrency control issues on a new object 
                // all you can have is duplicate data issues.
            {
                using (IDataReader dr = busObj.LoadDataReader(busObj.GetDatabaseConnection(), null))
                {
                    try
                    {
                        // If this object no longer exists in the database
                        // then we have a concurrency conflict since it has been deleted by another process.
                        // If our objective was to delete it as well then no worries else throw error.
                        bool drHasData = dr.Read();
                        if (! (drHasData) && !busObj.IsDeleted)
                        {
                            //The object you are trying to save has been deleted by another user.
                            throw new BusObjDeleteConcurrencyControlException(busObj.ClassName, busObj.ID.ToString(),
                                                                              busObj);
                        }
                        else
                        {
                            int versionNumber = (int) dr[mVersionNumberProp.DataBaseFieldName];
                            //Compare the value that the property has as its versionNumber
                            // to the value that the database currently has.
                            // if these two are not equal then the objects data has been 
                            // updated to the database since this object read it,
                            // we thus have a concurrency conflict.
                            if (versionNumber != (int) mVersionNumberProp.PropertyValue)
                            {
                                String dateLastUpdatedInDB = dr[mDateLastUpdatedProp.DataBaseFieldName].ToString();
                                string userNameLastUpdated = (string) dr[mUserLastUpdatedProp.DataBaseFieldName];
                                string machineLastUpdated = (string) dr[mMachineLastUpdatedProp.DataBaseFieldName];
                                throw new BusObjOptimisticConcurrencyControlException(busObj.ClassName,
                                                                                      userNameLastUpdated,
                                                                                      machineLastUpdated,
                                                                                      DateTime.Parse(dateLastUpdatedInDB),
                                                                                      busObj.ID.ToString(), busObj);
                            }
                        }
                    }
                    finally
                    {
                        if (dr != null & !(dr.IsClosed))
                        {
                            dr.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks concurrency before the user begins editing an object, in
        /// order to avoid the user making changes to an object and then losing
        /// those changes when the committal process shows a concurrency
        /// failure
        /// </summary>
        /// <param name="busObj">The business object to be edited</param>
        /// <exception cref="BusObjBeginEditConcurrencyControlException">Thrown
        /// if the object has been edited in the database since it was last
        /// loaded by the object manager</exception>
        public void CheckConcurrencyBeforeBeginEditing(BusinessObjectBase busObj)
        {
            try
            {
                CheckConcurrencyBeforePersisting(busObj);
            }
            catch (BusObjOptimisticConcurrencyControlException ex)
            {
                throw new BusObjBeginEditConcurrencyControlException(ex);
            }
        }

        /// <summary>
        /// Checks concurrency when retrieving an object from the object
        /// manager, in order to ensure that up-to-date information is
        /// displayed to the user.  If there have been changes to the object
        /// in the database, then the object manager simply reloads the
        /// updated copy.
        /// </summary>
        /// <param name="busObj">The business object to be loaded</param>
        public void CheckConcurrencyOnGettingObjectFromObjectManager(BusinessObjectBase busObj)
        {
            try
            {
                CheckConcurrencyBeforePersisting(busObj);
            }
            catch (BusObjectConcurrencyControlException)
            {
                busObj.Refresh();
            }
        }

        /// <summary>
        /// Updates the version number, machine name, username and time edited.
        /// The version number is used to determine whether there is a 
        /// concurrency conflict, and the other properties are used for 
        /// reporting of concurrency conflicts if they occur.
        /// </summary>
        public void UpdatePropertiesWithLatestConcurrencyInfo()
        {
            mDateLastUpdatedProp.PropertyValue = DateTime.Now;

            try
            {
                //TODO Temp code possibly integrate with a system to get the user as
                // per custom code for now use the OS user.
                mUserLastUpdatedProp.PropertyValue = WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
            }

            try
            {
                mMachineLastUpdatedProp.PropertyValue = Environment.MachineName;
            }
            catch (InvalidOperationException)
            {
            }

            mVersionNumberProp.PropertyValue = (int) mVersionNumberProp.PropertyValue + 1;
            if (!(mOperatingSystemUserProp == null))
            {
                try
                {
                    mOperatingSystemUserProp.PropertyValue = WindowsIdentity.GetCurrent().Name;
                }
                catch (SecurityException)
                {
                }
            }
            //	TODO: maybe add thread identity on later.			threadIdentity = Thread.CurrentPrincipal.Identity.Name;
        }

        /// <summary>
        /// Does nothing since optimistic locking is used and no locks are applied
        /// </summary>
        public void ReleaseReadLocks()
        {
        }

        /// <summary>
        /// Does nothing since optimistic locking is used and no locks are applied
        /// </summary>
        public void ReleaseWriteLocks()
        {
        }
    }
}