// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
namespace Habanero.Base
{
    ///<summary>
    /// Provides a predifined list of Actions that can be performed on any Business Object element 
    ///</summary>
    public enum BusinessObjectActions
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
    }

    /// <summary>
    /// Provides an interface for the Security Authorisation Policy (Strategy) 
    ///  for checking the users permissions to access <see cref="IBusinessObject"/> Functionality.
    /// If you would like to implement your own Authorisation Strategy then
    ///   implement this Interface and set it as the strategy for the Business Object.
    /// Security Authorisation Policy for the <see cref="IBusinessObject"/> is implemented
    ///  using the GOF Strategy Pattern.
    /// The Business Object will have a Reference to an object of this type.
    /// Since most objects of the same type (e.g. ContactPerson) implement the Same authorisation policy the 
    /// IsAuthorised accepts the businessObject as an argument this allows you to have only one 
    /// instance of the policy for all loaded Business object of the ContactPerson Type.
    /// This results in significant performance improvements.
    /// </summary>
    public interface IBusinessObjectAuthorisation
    {
        ///<summary>
        /// Adds a role to the list of roles that can perform the specified action.
        ///</summary>
        ///<param name="actionToPerform"></param>
        ///<param name="authorisedRole"></param>
        void AddAuthorisedRole(string authorisedRole, BusinessObjectActions actionToPerform);
        /// <summary>
        /// Method returns true if the user has permission to use the element specified according to the actionToPerform.
        /// the dictionary key will be Permission e.g. CanCreate and the value will be true or false. E.g. CanCreate, True.
        /// The permissions are determined by the least restrictive permission that the user has to the element via all
        /// profiles e.g. if a user is a member of readonly profile but also of CreateNewMember profile then the user
        /// will have permissions to CanCreate = true for member.
        /// 
        /// NNB: It is assumed that the user is logged on. This method will raise an error if the currentUser is not logged on.
        /// </summary>
        /// <param name="businessObject">The Business Object being authorised</param>
        /// <param name="actionToPerform">The action that the user is being required to perform e.g. Delete a ContactPerson.</param>
        bool IsAuthorised(IBusinessObject businessObject, BusinessObjectActions actionToPerform);
    }

    ///<summary>
    /// Provides a predifined list of Actions that can be performed on any BOProp.
    ///</summary>
    public enum BOPropActions
    {
        /// <summary>
        /// Can view the element
        /// </summary>
        CanRead,
        /// <summary>
        /// Can update an existing element
        /// </summary>
        CanUpdate,
    }

    /// <summary>
    /// Provides an interface for the Security Authorisation Policy (Strategy) 
    ///  for checking the users permissions to access <see cref="IBOProp"/> Functionality.
    /// If you would like to implement your own Authorisation Strategy then
    ///   implement this Interface and set it as the strategy for the BOProp.
    /// Security Authorisation Policy for the <see cref="IBOProp"/> is implemented
    ///  using the GOF Strategy Pattern.
    /// The BOProp will have a Reference to an object of this type.
    /// In most cases only one Strategy is required for all BOProps Instances i.e. 
    /// The BOProp for Surname for all the loaded ContactPeople all use the same Authorisation strategy.
    /// For this reason the BOProp Authorisation Strategy IsAuthorised accepts the prop as an argument.
    /// The BOPropAuthorisation strategy can then be set for the property definition <see cref="IPropDef"/>.
    /// For the <see cref="IBOProp"/>
    /// </summary>
    public interface IBOPropAuthorisation
    {
        ///<summary>
        /// Adds a authorisedRole to the list of roles that can perform the specified action.
        ///</summary>
        ///<param name="actionToPerform">The Action to perform e.g. Delete that is being added to the authorisation role</param>
        ///<param name="authorisedRole">The role that the actionToPerform is being added to</param>
        void AddAuthorisedRole(string authorisedRole, BOPropActions actionToPerform);
        /// <summary>
        /// Method returns true if the user has permission to use the element specified according to the actionToPerform.
        /// the dictionary key will be Permission e.g. CanCreate and the value will be true or false. E.g. CanCreate, True.
        /// The permissions are determined by the least restrictive permission that the user has to the element via all
        /// profiles e.g. if a user is a member of readonly profile but also of CreateNewMember profile then the user
        /// will have permissions to CanCreate = true for member.
        /// 
        /// NNB: It is assumed that the user is logged on. This method will raise an error if the currentUser is not logged on.
        /// </summary>
        /// <param name="prop">The Property that is being authorised.</param>
        /// <param name="actionToPerform">The action that the user is being required to perform e.g. Delete a Contact.</param>
        bool IsAuthorised(IBOProp prop, BOPropActions actionToPerform);
    }

//    /// <summary>
//    /// Provides an interface for the Security Authorisation Policy (Strategy) 
//    ///  for checking the users permissions to access <see cref="IBOProp"/> Functionality.
//    /// If you would like to implement your own Authorisation Strategy then
//    ///   implement this Interface and set it as the strategy for the BOProp.
//    /// Security Authorisation Policy for the <see cref="IBOProp"/> is implemented
//    ///  using the GOF Strategy Pattern.
//    /// The BOProp will have a Reference to an object of this type.
//    /// </summary>
//    public interface IBOMethodAuthorisation
//    {
//        ///<summary>
//        /// Adds a authorisedRole to the list of roles that can execute the method.
//        ///</summary>
//        ///<param name="authorisedRole"></param>
//        void AddAuthorisedRole(string authorisedRole);
//        /// <summary>
//        /// Method returns true if the user has permission to execute the method.
//        /// The permissions are determined by the least restrictive permission that the user has to the element via all
//        /// profiles e.g. if a user is a member of readonly profile but also of CalculateVat profile then the user
//        /// will have permissions to CanExecute = true for the method.
//        /// 
//        /// NNB: It is assumed that the user is logged on. This method will raise an error if the currentUser is not logged on.
//        /// </summary>
//        /// <param name="businessObject">The Business Object whose method is being authorised.</param>
//        bool IsAuthorised(IBusinessObject businessObject);
//    }
}