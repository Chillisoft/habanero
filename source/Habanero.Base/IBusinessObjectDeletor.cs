namespace Habanero.Base
{
    ///<summary>
    /// This provides an interface that is used by standard habanero control such as read only grid for deleting business objects.
    /// By default all these controls will use the DefaultBODeletor but a developer can set up any other custom deletor 
    /// as required.
    ///</summary>
    public interface IBusinessObjectDeletor
    {
        ///<summary>
        /// Deletes the business object.
        ///</summary>
        ///<param name="businessObject">The business object to delete</param>
        void DeleteBusinessObject(IBusinessObject businessObject);
    }
}