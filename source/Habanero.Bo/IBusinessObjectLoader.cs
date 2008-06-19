using Habanero.Base;

namespace Habanero.BO
{
    public interface IBusinessObjectLoader
    {
        T GetBusinessObject<T>(IPrimaryKey key) where T : class, IBusinessObject, new();
        T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new();
        BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria) where T : class, IBusinessObject, new();
        void Refresh<T>(BusinessObjectCollection<T> collection) where T : class, IBusinessObject, new();
        RelatedBusinessObjectCollection<T> GetRelatedBusinessObjectCollection<T>(IRelationship relationship) where T : class, IBusinessObject, new();
        BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, OrderCriteria orderCriteria) where T : BusinessObject, new();
    }
}