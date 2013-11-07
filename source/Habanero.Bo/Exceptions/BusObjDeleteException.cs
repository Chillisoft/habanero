using System;
using Habanero.Base;

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when the object cannot be deleted due to either the 
    /// custom rules being broken for a deletion or the IsDeletable flag being set to false.
    /// </summary>
    [Serializable]
    public class BusObjDeleteException : BusinessObjectException
    {
        /// <summary>
        /// Constructor to initialise the exception with details regarding the
        /// object whose record was deleted
        /// </summary>
        /// <param name="bo">The business object in question</param>
        /// <param name="message">Additional err message</param>
        public BusObjDeleteException(IBusinessObject bo, string message):
            base(String.IsNullOrEmpty(message) ? string.Format(
                "You cannot delete the '{0}', as the IsDeletable is set to false for the object. " +
                "ObjectID: {1}, also identified as {2}",
                bo.ClassDef.ClassName, bo.ID, bo) : message, 
                !String.IsNullOrEmpty(message) ? new Exception(
                    string.Format(
                        "You cannot delete the '{0}', as the IsDeletable is set to false for the object. " +
                        "ObjectID: {1}, also identified as {2}",
                        bo.ClassDef.ClassName, bo.ID, bo)) : null)
        {
        }
    }
}