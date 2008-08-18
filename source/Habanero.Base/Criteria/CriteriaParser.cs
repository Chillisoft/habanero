using Habanero.Base.Exceptions;

namespace Habanero.Base
{
    ///<summary>
    /// Parses a criteria string and creates a criteria object.
    ///</summary>
    public static class CriteriaParser
    {
        ///<summary>
        /// Creates a criteria object by parsing the criteriaString into a criteria
        /// expression object.
        ///</summary>
        ///<param name="criteriaString">The Criteria string that is being parsed.</param>
        public static Criteria CreateCriteria(string criteriaString)
        {
            if (string.IsNullOrEmpty(criteriaString)) return null;
            CriteriaExpression criteriaExpression = new CriteriaExpression(criteriaString);

            if (criteriaExpression == null || criteriaExpression.Left == null || criteriaExpression.Right == null || criteriaExpression.Expression == null)
            {
                throw new HabaneroDeveloperException("There is an application error please contact your system administrator", 
                        "The criteria string " + criteriaString + " is not a valid criteria string");
            }
            Criteria criteria = GetCriteria(criteriaExpression);
            return criteria;
        }

        private static Criteria GetCriteria(CriteriaExpression criteriaExpression)
        {

            Criteria criteria;
            if (criteriaExpression.Left.IsLeaf())//I.e. the left is a prop name.
            {
                criteria = GetCriteriaLeaf(criteriaExpression);
            }
            else
            {
                Criteria leftCriteria = GetCriteria(criteriaExpression.Left);
                Criteria rightCriteria = GetCriteria(criteriaExpression.Right);
                Criteria.LogicalOp logicalOp = CreateLogicalOperator(criteriaExpression.Expression);
                criteria = new Criteria(leftCriteria, logicalOp, rightCriteria);
            }
            return criteria;
        }

        private static Criteria GetCriteriaLeaf(CriteriaExpression criteriaExpression)
        {
            string propName = criteriaExpression.Left.Expression;
            string operatorString = criteriaExpression.Expression;
            object value = criteriaExpression.Right.Expression;
            Criteria.ComparisonOp comparisonOp = CreateComparisonOperator(operatorString);
            return new Criteria(propName, comparisonOp, value);
        }

        ///<summary>
        /// Converts string comparison operators into <see cref="Criteria.ComparisonOp"></see> Enums.
        ///</summary>
        ///<param name="operatorString">string operator</param>
        ///<returns></returns>
        public static Criteria.ComparisonOp CreateComparisonOperator(string operatorString)
        {
            switch (operatorString.Trim().ToUpper())
            {
                case "=":
                    return Criteria.ComparisonOp.Equals;
                case ">":
                    return Criteria.ComparisonOp.GreaterThan;
                case "<":
                    return Criteria.ComparisonOp.LessThan;
                case "<=":
                    return Criteria.ComparisonOp.LessThanEqual;
                case ">=":
                    return Criteria.ComparisonOp.GreaterThanEqual;
                case "<>":
                    return Criteria.ComparisonOp.NotEquals;
                case "LIKE":
                    return Criteria.ComparisonOp.Like;
                case "NOT LIKE":
                    return Criteria.ComparisonOp.NotLike;
                case "IS":
                    return Criteria.ComparisonOp.Is;
                case "IS NOT":
                    return Criteria.ComparisonOp.IsNot;
                default:
                    throw new HabaneroDeveloperException("An error has occured in the application, please contact your system administrator.","Invalid operator used in a criteria string: "+operatorString);
            }
        }

        ///<summary>
        /// Converts Logic operators strings into logical operator enumerated type <see cref="Criteria.LogicalOp"/>
        ///</summary>
        ///<param name="operatorString"></param>
        ///<returns></returns>
        public static Criteria.LogicalOp CreateLogicalOperator(string operatorString)
        {
            switch (operatorString.ToUpper().Trim())
            {
                case "AND":
                    return Criteria.LogicalOp.And;
                case "OR":
                    return Criteria.LogicalOp.Or;
                default:
                    throw new HabaneroDeveloperException("An error has occured in the application, please contact your system administrator.", "Invalid operator used in a criteria string: " + operatorString);

            }
        }
    }
}