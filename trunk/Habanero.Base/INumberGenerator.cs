namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a generator of unique numbers. It keeps
    /// record of the last number retrieved and can provide a new
    /// unique number on request.  An example usage might be a till
    /// receipt number for a retail business.  Incrementing the numbers
    /// as they are dispensed is one means of achieving uniqueness.
    /// </summary>
    /// TODO:
    /// - need to apply synchronisation to ensure new number not retrieved
    /// by another user before first user saves and updates number (could
    /// update immediately)
    public interface INumberGenerator
    {
        /// <summary>
        /// Returns the next available unique number. One possible means
        /// of providing unique numbers is simply to increment the last one
        /// dispensed.
        /// </summary>
        /// <returns>Returns an integer</returns>
        int GetNextNumberInt();

        /// <summary>
        /// Creates a database transaction that updates the database to the
        /// last number dispensed, so the next number dispensed will be a
        /// fresh increment
        /// </summary>
        /// <returns>Returns an ITransaction object</returns>
        ITransaction GetUpdateTransaction();
    }
}