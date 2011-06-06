using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Habanero.Base
{
    public class CriteriaBuilder
    {
        private readonly Expression _expression;

        private static readonly Dictionary<Criteria.ComparisonOp, Criteria.ComparisonOp> 
            NegatedOps = new Dictionary<Criteria.ComparisonOp, Criteria.ComparisonOp>
                { { Criteria.ComparisonOp.In, Criteria.ComparisonOp.NotIn},
                  { Criteria.ComparisonOp.IsNot, Criteria.ComparisonOp.Is},
                  { Criteria.ComparisonOp.Like, Criteria.ComparisonOp.NotLike},
                  { Criteria.ComparisonOp.Equals, Criteria.ComparisonOp.NotEquals},
                  { Criteria.ComparisonOp.GreaterThan, Criteria.ComparisonOp.LessThanEqual},
                  { Criteria.ComparisonOp.GreaterThanEqual, Criteria.ComparisonOp.LessThan},
                  { Criteria.ComparisonOp.LessThan, Criteria.ComparisonOp.GreaterThanEqual},
                  { Criteria.ComparisonOp.LessThanEqual, Criteria.ComparisonOp.GreaterThan},
                  {Criteria.ComparisonOp.NotEquals, Criteria.ComparisonOp.Equals} 
                };

        private static readonly Dictionary<ExpressionType, Criteria.LogicalOp> 
            LogicalOps = new Dictionary<ExpressionType, Criteria.LogicalOp>
                {
                    { ExpressionType.AndAlso, Criteria.LogicalOp.And },
                    { ExpressionType.OrElse, Criteria.LogicalOp.Or }
                };
        
        private static readonly Dictionary<ExpressionType, Criteria.ComparisonOp> 
            Ops = new Dictionary<ExpressionType, Criteria.ComparisonOp>
                { { ExpressionType.GreaterThan, Criteria.ComparisonOp.GreaterThan},
                  { ExpressionType.GreaterThanOrEqual, Criteria.ComparisonOp.GreaterThanEqual},
                  { ExpressionType.LessThan, Criteria.ComparisonOp.LessThan},
                  { ExpressionType.LessThanOrEqual, Criteria.ComparisonOp.LessThanEqual},
                  { ExpressionType.NotEqual, Criteria.ComparisonOp.NotEquals},
                  {ExpressionType.Equal, Criteria.ComparisonOp.Equals} };

        private CriteriaBuilderAnd _and;

        public CriteriaBuilder(Expression expression)
        {
            _expression = expression;
        }

       
        public Criteria Build()
        {
            return Create(_expression);
        }

        public CriteriaBuilderAnd And
        {
            get
            {
                return new CriteriaBuilderAnd(_expression);
            }
        }

        public CriteriaBuilderOr Or
        {
            get
            {
                return new CriteriaBuilderOr(_expression);
            }
        }

        public virtual CriteriaBuilder Not<T>(Expression<Func<T, bool>> expression)
        {
            return new CriteriaBuilder(Expression.Not(expression.Body));
        }
      
        private Criteria Create(Expression expression)
        {
            if (expression is BinaryExpression)
                return CreateFromBinaryExpression(expression);
            if (expression is MethodCallExpression)
                return CreateFromMethodCallExpression(expression);
            if (expression is MemberExpression)
                return CreateFromMemberExpression((MemberExpression)expression);
            if (expression is UnaryExpression)
                return CreateFromUnaryExpression((UnaryExpression)expression);
            throw new ArgumentException("Sorry, don't know how to handle a " + expression.ToString());
        }

        private Criteria CreateFromMemberExpression(MemberExpression memberExpression)
        {
            var propInfo = (PropertyInfo)memberExpression.Member;
            if (propInfo.Name.Equals("HasValue"))
            {
                var fieldExpression = (MemberExpression)memberExpression.Expression;
                return new Criteria(fieldExpression.Member.Name, Criteria.ComparisonOp.IsNot, null);
            }
            throw new ArgumentException("Sorry, don't know how to handle a MemberExpression: " + memberExpression.ToString());
        }

        private Criteria CreateFromUnaryExpression(UnaryExpression unaryExpression)
        {
            if (unaryExpression.NodeType == ExpressionType.Not)
            {
                var criteria = Create(unaryExpression.Operand);
                if (criteria.IsComposite())
                {
                    return new Criteria(null, Criteria.LogicalOp.Not, criteria);
                }
                criteria.ComparisonOperator = NegatedOps[criteria.ComparisonOperator];
                return criteria;
            }
            throw new ArgumentException("Sorry, don't know how to handle a UnaryExpression: " + unaryExpression.ToString());
        }

        private Criteria CreateFromMethodCallExpression(Expression expression)
        {
            var methodCallExpression = (MethodCallExpression)expression;
            var argument = methodCallExpression.Arguments[0];

            object argValue;
            if (argument is MemberExpression)
            {
                var memberExpression = (MemberExpression)argument;
                var field = (FieldInfo)memberExpression.Member;
                var constExpression = (ConstantExpression)memberExpression.Expression;
                argValue = field.GetValue(constExpression.Value);
            }
            else
            {
                var constExpression = ((ConstantExpression)argument);
                argValue = constExpression.Value;
            }

            if (methodCallExpression.Method.Name == "Contains")
            {
                if (methodCallExpression.Object != null)
                {
                    var memberExpression = (MemberExpression)methodCallExpression.Object;
                    var propInfo = (PropertyInfo)memberExpression.Member;
                    if (propInfo.PropertyType == typeof(string))
                    {
                        return new Criteria(propInfo.Name, Criteria.ComparisonOp.Like, "%" + argValue + "%");
                    }
                }
                else
                {
                    var valuesExpression = (MemberExpression)methodCallExpression.Arguments[0];
                    var field = (FieldInfo)valuesExpression.Member;
                    var constExpression = (ConstantExpression)valuesExpression.Expression;
                    var values = field.GetValue(constExpression.Value);
                    if (values is IEnumerable)
                    {
                        var memberExpression = (MemberExpression)methodCallExpression.Arguments[1];
                        var propInfo = (PropertyInfo)memberExpression.Member;
                        return new Criteria(propInfo.Name, Criteria.ComparisonOp.In, values);
                    }
                }
            }
            else if (methodCallExpression.Method.Name == "StartsWith")
            {
                var memberExpression = (MemberExpression)methodCallExpression.Object;
                var propInfo = (PropertyInfo)memberExpression.Member;
                if (propInfo.PropertyType == typeof(string))
                {
                    return new Criteria(propInfo.Name, Criteria.ComparisonOp.Like, argValue + "%");
                }
            }
            else if (methodCallExpression.Method.Name == "EndsWith")
            {
                var memberExpression = (MemberExpression)methodCallExpression.Object;
                var propInfo = (PropertyInfo)memberExpression.Member;
                if (propInfo.PropertyType == typeof(string))
                {
                    return new Criteria(propInfo.Name, Criteria.ComparisonOp.Like, "%" + argValue);
                }
            }
            throw new ArgumentException("Sorry, don't know how to handle a MethodCallExpression: " +
                                        expression.ToString());
        }

        private Criteria CreateFromBinaryExpression(Expression expression)
        {
            var binaryExpression = (BinaryExpression)expression;

            if (binaryExpression.Left is BinaryExpression)
            {
                var leftCriteria = Create(binaryExpression.Left);
                var logicalOp = LogicalOps[binaryExpression.NodeType];
                var rightCriteria = Create(binaryExpression.Right);
                return new Criteria(leftCriteria, logicalOp, rightCriteria);
            }

            if (!(binaryExpression.Left is MemberExpression))
            {
                throw new ArgumentException(expression + " is not a valid expression for a Criteria, the left must a MemberExpression");
            }
            var memberExpression = (MemberExpression)binaryExpression.Left;

            var comparisonOp = Ops[binaryExpression.NodeType];

            ConstantExpression valueExpression;
            object finalValue = null;

            if (binaryExpression.Right is ConstantExpression)
            {
                valueExpression = (ConstantExpression)binaryExpression.Right;
                finalValue = valueExpression.Value;
            }
            else if (binaryExpression.Right is MemberExpression)
            {
                var fieldExpression = (MemberExpression)binaryExpression.Right;
                if (fieldExpression.Expression is ConstantExpression)
                {
                    valueExpression = (ConstantExpression)fieldExpression.Expression;
                    var fieldInfo = (FieldInfo)fieldExpression.Member;
                    finalValue = fieldInfo.GetValue(valueExpression.Value);
                }
            }

            if (finalValue == null && comparisonOp == Criteria.ComparisonOp.Equals) comparisonOp = Criteria.ComparisonOp.Is;
            if (finalValue == null && comparisonOp == Criteria.ComparisonOp.NotEquals) comparisonOp = Criteria.ComparisonOp.IsNot;
            return new Criteria(memberExpression.Member.Name, comparisonOp, finalValue);
        }

        public class CriteriaBuilderAnd : CriteriaBuilder
        {
            public CriteriaBuilderAnd(Expression expression) : base(expression)  { }

            public CriteriaBuilder Expr<T>(Expression<Func<T, bool>> expression)
            {
                return new CriteriaBuilder(Expression.AndAlso(_expression, expression.Body));
            }

            public override CriteriaBuilder Not<T>(Expression<Func<T, bool>> expression)
            {
                return new CriteriaBuilder(Expression.AndAlso(_expression, Expression.Not(expression.Body)));
            }
        }

        public class CriteriaBuilderOr : CriteriaBuilder
        {
            public CriteriaBuilderOr(Expression expression) : base(expression)  { }

            public CriteriaBuilder Expr<T>(Expression<Func<T, bool>> expression)
            {
                return new CriteriaBuilder(Expression.OrElse(_expression, expression.Body));
            }

            public override CriteriaBuilder Not<T>(Expression<Func<T, bool>> expression)
            {
                return new CriteriaBuilder(Expression.OrElse(_expression, Expression.Not(expression.Body)));
            }
        }

    }


}
