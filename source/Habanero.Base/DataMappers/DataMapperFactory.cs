using System;
using System.Collections.Generic;
using System.Drawing;

namespace Habanero.Base.DataMappers
{
    /// <summary>
    /// Factory for returning the appropriate Datamapper depending on the target type (typically a <see cref="IBOProp"/> type.)
    /// </summary>
    public class DataMapperFactory : IDataMapperFactory
    {
        private Dictionary<Type, IDataMapper> _dataMappers;
        /// <summary>
        /// Constructs the Factory and sets up the default datamappers for each type
        /// </summary>
        public DataMapperFactory()
        {
            InitialiseDataMappers();
        }

        private void InitialiseDataMappers()
        {
            _dataMappers = new Dictionary<Type, IDataMapper>();
            _dataMappers.Add(typeof(Guid), new GuidDataMapper());
            _dataMappers.Add(typeof(DateTime), new DateTimeDataMapper());
            _dataMappers.Add(typeof(string), new StringDataMapper());
            _dataMappers.Add(typeof(bool), new BoolDataMapper());
            _dataMappers.Add(typeof(int), new IntDataMapper());
            _dataMappers.Add(typeof(long), new LongDataMapper());
            _dataMappers.Add(typeof(Image), new ImageDataMapper());
            _dataMappers.Add(typeof(byte[]), new ByteArrayDataMapper());
            _dataMappers.Add(typeof(TimeSpan), new TimeSpanDataMapper());
        }
        /// <summary>
        /// Returns the DataMapper for the <paramref name="targetType"/>.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public IDataMapper GetDataMapper(Type targetType)
        {
            try
            {
                return _dataMappers[targetType];
            } catch (KeyNotFoundException)
            {
                var generalDataMapper = new GeneralDataMapper(targetType);
                _dataMappers.Add(targetType, generalDataMapper);
                return generalDataMapper;
            }
        }
        /// <summary>
        /// sets the <paramref name="dataMapper"/> for the <paramref name="targetType"/>
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="dataMapper"></param>
        public void SetDataMapper(Type targetType, IDataMapper dataMapper)
        {
            _dataMappers[targetType] = dataMapper;
        }
    }
}