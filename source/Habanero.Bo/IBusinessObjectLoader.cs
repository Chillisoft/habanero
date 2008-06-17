using Habanero.Base;

namespace Habanero.BO
{
    public interface IBusinessObjectLoader
    {
        T GetBusinessObject<T>(IPrimaryKey key) where T : class, IBusinessObject;
        T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject;
        BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria) where T : class, IBusinessObject;
        void Refresh<T>(BusinessObjectCollection<T> collection) where T : class, IBusinessObject, new();
    }
}