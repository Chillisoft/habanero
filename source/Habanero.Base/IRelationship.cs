namespace Habanero.Base
{
    public interface IRelationship
    {
        /// <summary>
        /// Returns the set of business objects that relate to this one
        /// through the specific relationship
        /// </summary>
        /// <returns>Returns a collection of business objects</returns>
        IBusinessObjectCollection GetRelatedBusinessObjectCol();

        IRelKey RelKey
        {
            get;
        }

        ///<summary>
        /// 
        ///</summary>
        OrderCriteria OrderCriteria
        {
            get;
        }
    }
}