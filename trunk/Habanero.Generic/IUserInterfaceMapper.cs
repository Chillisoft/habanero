namespace Habanero.Generic
{
    /// <summary>
    /// An interface to model a user interface mapper
    /// </summary>
    public interface IUserInterfaceMapper
    {
        /// <summary>
        /// Returns the UI form property definitions
        /// </summary>
        /// <returns>Returns a UIFormDef object</returns>
        UIFormDef GetUIFormProperties();

        /// <summary>
        /// Returns the UI grid property definitions
        /// </summary>
        /// <returns>Returns a UIGridDef object</returns>
        UIGridDef GetUIGridProperties();
    }
}