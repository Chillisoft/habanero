using System;

namespace Habanero.Base.DataMappers
{
    public interface IDataMapperFactory
    {
        IDataMapper GetDataMapper(Type targetType);
        void SetDataMapper(Type targetType, IDataMapper dataMapper);
    }
}