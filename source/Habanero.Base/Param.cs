using System.Data;

namespace Habanero.Base
{
    public class Param
    {
        private readonly DbType _dbType;
        private readonly string _paramName;
        private readonly string _paramValue;

        public Param(DbType dbType, string paramName, string paramValue)
        {
            _dbType = dbType;
            _paramName = paramName;
            _paramValue = paramValue;
        }

        public DbType DbType
        {
            get { return _dbType; }
        }

        public string ParamName
        {
            get { return _paramName; }
        }

        public string ParamValue
        {
            get { return _paramValue; }
        }
    }
}