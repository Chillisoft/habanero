using System;

namespace Habanero.Base
{
    public interface IPrimaryKey : IBOKey
    {
        /// <summary>
        /// Returns the object's ID
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetObjectId();

        /// <summary>
        /// Returns the object ID as if the object had been persisted 
        /// regardless of whether the object is new or not
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetObjectNewID();

        /// <summary>
        /// Get the original ObjectID
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetOrigObjectID();

        /// <summary>
        /// Returns the ID as a Guid
        /// </summary>
        /// <returns>Returns a Guid</returns>
        Guid GetAsGuid();

        /// <summary>
        /// Sets the object's ID
        /// </summary>
        /// <param name="id">The ID to set to</param>
        void SetObjectID(Guid id);
    }
}