using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Manages a composite filter clause, which is composed of individual
    /// filter clauses that filter which data to display in a DataView, 
    /// according to some criteria
    /// </summary>
    public class DataViewCompositeFilterClause : FilterClause
    {
        private readonly FilterClause itsRightClause;
        private readonly FilterClauseCompositeOperator itsCompositeOperator;
        private readonly FilterClause itsLeftClause;

        /// <summary>
        /// Constructor to initialise a new composite filter clause
        /// </summary>
        /// <param name="leftClause">The left filter clause</param>
        /// <param name="compositeOperator">The operator to connect the
        /// clauses</param>
        /// <param name="rightClause">The right filter clause</param>
        public DataViewCompositeFilterClause(FilterClause leftClause, FilterClauseCompositeOperator compositeOperator,
                                             FilterClause rightClause)
        {
            itsLeftClause = leftClause;
            itsCompositeOperator = compositeOperator;
            itsRightClause = rightClause;
        }

        /// <summary>
        /// Adds the clauses together to return a complete filter clause string
        /// </summary>
        /// <returns>The completed string</returns>
        public string GetFilterClauseString()
        {
            if (itsLeftClause.GetFilterClauseString().Length > 0 && itsRightClause.GetFilterClauseString().Length > 0)
            {
                return GetLeftClause() + GetOperatorClause() + GetRightClause();
            }
            else if (itsLeftClause.GetFilterClauseString().Length > 0)
            {
                return itsLeftClause.GetFilterClauseString();
            }
            else if (itsRightClause.GetFilterClauseString().Length > 0)
            {
                return itsRightClause.GetFilterClauseString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the right filter clause surrounded by round brackets
        /// </summary>
        /// <returns>Returns the clause as a string</returns>
        private string GetRightClause()
        {
            return "(" + itsRightClause.GetFilterClauseString() + ")";
        }

        /// <summary>
        /// Returns the operator
        /// </summary>
        /// <returns>Returns the operator</returns>
        private string GetOperatorClause()
        {
            switch (this.itsCompositeOperator)
            {
                case FilterClauseCompositeOperator.OpAnd:
                    return " and ";
                case FilterClauseCompositeOperator.OpOr:
                    return " or ";
                default:
                    return " <unsupported composite operator> ";
            }
        }

        /// <summary>
        /// Returns the left filter clause surrounded by round brackets
        /// </summary>
        /// <returns>Returns the clause as a string</returns>
        private string GetLeftClause()
        {
            return "(" + itsLeftClause.GetFilterClauseString() + ")";
        }
    }
}
