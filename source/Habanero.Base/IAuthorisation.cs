namespace Habanero.Base
{
    ///<summary>
    /// Provides a predifined list of Actions that can be executed on any element 
    /// Defined in the system an Element could be a Business Object, a BusinessObject Property
    ///  a Method of any object, a form, a menu item a report or any other element 
    ///  in the system.
    ///</summary>
    public enum PermissionActions
    {
        /// <summary>
        /// Can create the element
        /// </summary>
        CanCreate,
        /// <summary>
        /// Can view the element
        /// </summary>
        CanRead,
        /// <summary>
        /// Can update an existing element
        /// </summary>
        CanUpdate,
        /// <summary>
        /// Can delete the element
        /// </summary>
        CanDelete,
        /// <summary>
        /// Can execute the element (usually used for methods and menu items)
        /// </summary>
        CanExecute
    }

    /// <summary>
    /// Provides an interface for the Security Authorisation Policy (Strategy) 
    ///  for checking the users permissions to access certain Functionality.
    /// If you would like to implement your own property
    /// rule checker, implement this interface or inherit from PropRuleBase.
    /// In the class definitions, in the 'rule'
    /// element under the relevant 'property', specify the class and assembly
    /// of your newly implemented class.
    /// The Property rules for the <see cref="IBusinessObject"/> are implemented
    ///  using the GOF Strategy Pattern.
    /// </summary>
    public interface IAuthorisation
    {
        /// <summary>
        /// Method returns a collection of permissions that the user has to the element for the application.
        /// the dictionary key will be Permission e.g. CanCreate and the value will be true or false. E.g. CanCreate, True.
        /// The permissions are determined by the least restrictive permission that the user has to the element via all
        /// profiles e.g. if a user is a member of readonly profile but also of CreateNewMember profile then the user
        /// will have permissions to CanCreate = true for member.
        /// 
        /// NNB: It is assumed that the user is logged on. This method does not check logon. It is expected that the UI developer.
        /// Has ensured that the user is already logged on.  In the case of calling this method the
        /// current user (the logged in user) will be used.
        /// </summary>
        /// <param name="applicationName">Name of the application that the element is part of</param>
        /// <param name="elementName">Element that the permissions are required for E.g. Member</param>
        /// <param name="actionToPerform">The action that the user is being required to perform e.g. Delete a User.</param>
        /// <param name="errMessage">Message if any error occurs e.g. element, user or app not found.</param>
        /// <returns>returns a collection of permissions. Returns null if an error occured</returns>
        bool IsAuthorised(string applicationName, string elementName, PermissionActions actionToPerform , out string errMessage);
    }
}