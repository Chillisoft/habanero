using System;
using Habanero.Base;
using Habanero.BO.CriteriaManager;

namespace Habanero.Test.BO
{
    public static class CriteriaParser
    {
        private static readonly string[] _operators = { "=", ">", "<" };

        ///<summary>
        /// Creates a criteria object by parsing the criteriaString into a criteria
        /// expression object.
        ///</summary>
        ///<param name="criteriaString">The Criteria string that is being parsed.</param>
        public static Criteria CreateCriteria(string criteriaString)
        {
            CriteriaExpression criteriaExpression = new CriteriaExpression(criteriaString);
            Criteria criteria = GetCriteria(criteriaExpression);
            return criteria;

            //string[] parts = criteriaString.Split(_operators,StringSplitOptions.None);
            //string propName = parts[0];
            //string operatorString = criteriaString.Substring(propName.Length, 1);
            //propName = propName.Trim();
            //Criteria.Op op = CreateOperator(operatorString);
            //object value = parts[1].Trim();
            //Criteria criteria = new Criteria(propName, op, value);
            //return criteria;
            ////Criteria.NewCriteria(criteriaString);
            ////Criteria crit = Criteria.CreateCriteria(string);
            ////Guid guid = new Guid();
        }

        private static Criteria GetCriteria(CriteriaExpression criteriaExpression)
        {
            Criteria criteria;
            if (criteriaExpression.Left.IsLeaf())
            {
                criteria = GetCriteriaLeaf(criteriaExpression);
                
            } else
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
            Criteria.Op op = CreateOperator(operatorString);
            return new Criteria(propName, op, value);
        }

        public static Criteria.Op CreateOperator(string operatorString)
        {
            switch (operatorString)
            {
                case "=":
                    return Criteria.Op.Equals;
                case ">": 
                    return Criteria.Op.GreaterThan;
                case "<":
                    return Criteria.Op.LessThan;
                default:
                    return Criteria.Op.Equals;
            }
        }

        public static Criteria.LogicalOp CreateLogicalOperator(string operatorString)
        {
            switch (operatorString.ToUpper().Trim())
            {
                case "AND":
                    return Criteria.LogicalOp.And;
                case "OR":
                    return Criteria.LogicalOp.Or;
                default:
                    return Criteria.LogicalOp.And;
            }
        }
    }
}