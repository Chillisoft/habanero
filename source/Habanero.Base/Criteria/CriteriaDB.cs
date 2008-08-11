using System;

namespace Habanero.Base
{
    public class CriteriaDB : Criteria
    {
        #region Delegates

        /// <summary>
        /// For details of what this delegate is used for, see <see cref="PropNameConverterDelegate"/>
        /// 
        /// This delegate is designed to format a value of a criteria into a datasource equivalent. For example, in a database context,
        /// this delegate might be used to return a parameter name and store the parameter value so that the criteria can be used as part
        /// of a where clause in a parametrized SQL statement.
        /// </summary>
        /// <param name="parameterValue">The value of the criteria given, as an object. </param>
        /// <returns>The string format of this criteria to append to the string.</returns>
        public delegate string AddParameterDelegate(object parameterValue);

        ///<summary>
        /// When converting the Criteria expression to a string there are contexts that must be taken into account. The default implementation
        /// (<see cref="Criteria.ToString()"/>) will not do any conversions of property names or values given other than to convert
        /// DateTimes and Guids to sensible, universal string equivalents (see <see cref="Criteria.DATE_FORMAT"/>).
        /// 
        /// This delegate is designed to map a property name to a datasource name (such as to a field name on table).
        ///</summary>
        ///<param name="propName">The property name to map to the datasource equivalent</param>
        /// <returns>The mapped property name ie the datasource equivalent of the property name (perhaps the field name on a database table)</returns>
        public delegate string PropNameConverterDelegate(string propName);

        #endregion

        private readonly Criteria _criteria;

        public CriteriaDB(Criteria criteria)
        {
            _criteria = criteria;
        }

        public override string ToString()
        {
            return _criteria.ToString();
        }

        public override QueryField Field
        {
            get { return _criteria.Field; }
        }

        public override Criteria LeftCriteria
        {
            get { return _criteria.LeftCriteria; }
        }

        public override LogicalOp LogicalOperator
        {
            get { return _criteria.LogicalOperator; }
        }

        public override Criteria RightCriteria
        {
            get { return _criteria.RightCriteria; }
        }

        public override object FieldValue
        {
            get { return _criteria.FieldValue; }
        }

        public override Op ComparisonOperator
        {
            get { return _criteria.ComparisonOperator; }
        }

        /// <summary>
        /// Converts this Criteria object to a string, using field names instead of property names and entity names instead of
        /// source names. The <see cref="AddParameterDelegate"/> allows a database query builder to create a parameter value
        /// when adding the value to the string for use with parametrized SQL.  Also see <see cref="ISqlStatement"/>.
        /// 
        /// The <see cref="ToString()"/> method uses this method with a simple delegate that converts DateTimes and Guids 
        /// to sensible string representations.
        /// </summary>
        /// See <see cref="PropNameConverterDelegate"/></param>
        /// <param name="addParameter">The delegate to use to convert the value in object form to a value in string form. 
        /// See <see cref="AddParameterDelegate"/></param>
        /// <returns>The Criteria in string form.</returns>
        public string ToString(SqlFormatter formatter, AddParameterDelegate addParameter)
        {
            if (IsComposite())
            {
                string leftCriteriaAsString = new CriteriaDB(LeftCriteria).ToString(formatter, addParameter);
                string rightCriteriaAsString = new CriteriaDB(RightCriteria).ToString(formatter, addParameter);
                return string.Format("({0}) {1} ({2})", leftCriteriaAsString, LogicalOps[(int)LogicalOperator],
                                     rightCriteriaAsString);
            }
            string valueString = addParameter(FieldValue);
            string sourceEntityName = ""; if (Field.Source != null) sourceEntityName = Field.Source.EntityName;
            string separator = "";
            if (!String.IsNullOrEmpty(sourceEntityName))
            {
                sourceEntityName = formatter.DelimitTable(sourceEntityName);
                separator = ".";
            }
            return string.Format("{0}{1}{2} {3} {4}", sourceEntityName, separator, formatter.DelimitField(Field.FieldName), ComparisonOps[(int)ComparisonOperator], valueString);
        }

    
    }
}