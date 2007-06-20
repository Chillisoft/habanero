namespace Habanero.Bo.CriteriaManager
{
    /// <summary>
    /// This interface is created so that any component that is using the 
    /// expression and needs to build up valid sql syntax, will be able to 
    /// replace the parameter name (BO property name) with 
    /// a table name and field name.
    /// </summary>
    public interface IParameterSqlInfo
    {
        /// <summary>
        /// The name in the expression tree to be updated
        /// </summary>
        string ParameterName { get; }

        /// <summary>
        /// The table name to be added to the parameter
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// The field name to be added to the parameter
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// The parameter type to be added to the parameter
        /// </summary>
        ParameterType ParameterType { get; }
    }
}