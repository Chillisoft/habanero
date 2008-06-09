using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.CriteriaManager
{
    ///<summary>
    /// This class returns SQL Parameter Information from a Property Definition and it's Class Definition.
    /// If a class definition is not specified, then a table name can explicitly be specified.
    ///</summary>
    public class PropDefParameterSQLInfo: IParameterSqlInfo
    {
        private IPropDef _propDef;
        private ClassDef _classDef;
        private readonly string _parameterName;
        private readonly string _tableName;

        #region Constructors

        ///<summary>
        /// Create SQL Parameter Information from a Property Definition.
        ///</summary>
        ///<param name="propDef">The property definition to use for the SQL Parameter Information.</param>
        public PropDefParameterSQLInfo(PropDef propDef) : this(propDef, null, null, null) { }

        ///<summary>
        /// Create SQL Parameter Information from a Property Definition and an explicit table name.
        ///</summary>
        ///<param name="propDef">The property definition to use for the SQL Parameter Information.</param>
        ///<param name="tableName">The table name to use for this field</param>
        public PropDefParameterSQLInfo(PropDef propDef, string tableName) : this(propDef, null, tableName, null) { }

        ///<summary>
        /// Create SQL Parameter Information from a Property Definition and an explicit table name.
        ///</summary>
        ///<param name="parameterName">The name of the parameter that this information is for.</param>
        ///<param name="propDef">The property definition to use for the SQL Parameter Information.</param>
        ///<param name="tableName">The table name to use for this field</param>
        public PropDefParameterSQLInfo(string parameterName, IPropDef propDef, string tableName) : this(propDef, null, tableName, parameterName) { }

        ///<summary>
        /// Create SQL Parameter Information from a Property Definition and it's Class Definition.
        ///</summary>
        ///<param name="propDef">The property definition to use for the SQL Parameter Information.</param>
        ///<param name="classDef">The class definition to use for the SQL Parameter Information.</param>
        public PropDefParameterSQLInfo(IPropDef propDef, ClassDef classDef) : this(propDef, classDef, null, null) { }

        ///<summary>
        /// Create SQL Parameter Information from a Property Definition and it's Class Definition.
        ///</summary>
        ///<param name="parameterName">The name of the parameter that this information is for.</param>
        ///<param name="propDef">The property definition to use for the SQL Parameter Information.</param>
        ///<param name="classDef">The class definition to use for the SQL Parameter Information.</param>
        public PropDefParameterSQLInfo(string parameterName, IPropDef propDef, ClassDef classDef) : this(propDef, classDef, null, parameterName) { }

        private PropDefParameterSQLInfo(IPropDef propDef, ClassDef classDef, string tableName, string parameterName)
        {
            _propDef = propDef;
            _classDef = classDef;
            _parameterName = parameterName;
            if (String.IsNullOrEmpty(_parameterName))
            {
                _parameterName = _propDef.PropertyName;
            }
            if (_classDef != null)
            {
                _tableName = classDef.TableName;
            } else
            {
                _tableName = tableName;
            }
        }

        #endregion //Constructors

        #region IParameterSqlInfo Members

        /// <summary>
        /// Returns the parameter type (typically either DateTime or String)
        /// </summary>
        public ParameterType ParameterType
        {
            get
            {
                if (_propDef.PropertyType == typeof(DateTime))
                {
                    return ParameterType.Date;
                }
                else
                {
                    return ParameterType.String;
                }
            }
        }

        /// <summary>
        /// Returns the database field name
        /// </summary>
        public string FieldName
        {
            get { return _propDef.DatabaseFieldName; }
        }

        /// <summary>
        /// Returns the parameter name
        /// </summary>
        public string ParameterName
        {
            get { return _parameterName; }
        }

        /// <summary>
        /// Returns an empty string
        /// </summary>
        public string TableName
        {
            get
            {
                if (_tableName != null)
                {
                    return _tableName;
                } else
                {
                    return "";
                }
            }
        }

        #endregion
    }
}
