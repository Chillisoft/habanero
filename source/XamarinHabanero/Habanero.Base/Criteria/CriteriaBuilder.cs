#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Habanero.Base
{
    /// <summary>
    /// Assistant class to generate criteria for database queries
    /// </summary>
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
                    { ExpressionType.And, Criteria.LogicalOp.And },
                    { ExpressionType.OrElse, Criteria.LogicalOp.Or },
                    { ExpressionType.Or, Criteria.LogicalOp.Or }
                };
        
        private static readonly Dictionary<ExpressionType, Criteria.ComparisonOp> 
            Ops = new Dictionary<ExpressionType, Criteria.ComparisonOp>
                { { ExpressionType.GreaterThan, Criteria.ComparisonOp.GreaterThan},
                  { ExpressionType.GreaterThanOrEqual, Criteria.ComparisonOp.GreaterThanEqual},
                  { ExpressionType.LessThan, Criteria.ComparisonOp.LessThan},
                  { ExpressionType.LessThanOrEqual, Criteria.ComparisonOp.LessThanEqual},
                  { ExpressionType.NotEqual, Criteria.ComparisonOp.NotEquals},
                  {ExpressionType.Equal, Criteria.ComparisonOp.Equals} };

        /// <summary>
        /// Creates a CriteriaBuilder. Use Build() to create a Criteria.
        /// </summary>
        /// <param name="expression">The expression to parse into a Criteria.</param>
        public CriteriaBuilder(Expression expression)
        {
            _expression = expression;
        }

       
        /// <summary>
        /// Creates the final Criteria object.
        /// </summary>
        /// <returns>A Criteria object created from the expression</returns>
        public Criteria Build()
        {
            return Create(_expression);
        }

        /// <summary>
        /// Returns a new CriteriaBuilder linked to the current one with an And clause.
        /// </summary>
        public CriteriaBuilderAnd And
        {
            get
            {
                return new CriteriaBuilderAnd(_expression);
            }
        }

        /// <summary>
        /// Returns a new CriteriaBuilder linked to the current one with an Or clause.
        /// </summary>
        public CriteriaBuilderOr Or
        {
            get
            {
                return new CriteriaBuilderOr(_expression);
            }
        }

        /// <summary>
        /// Appends a Not expression on to the end of this builder.
        /// </summary>
        /// <param name="expression">The expression to append (with a Not)</param>
        /// <typeparam name="T">The type the criteria is for.</typeparam>
        /// <returns>A new CriteriaBuilder that incorporates the new expression negated</returns>
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
                throw new ArgumentException(expression + " is not a valid expression for a Criteria, the left must a MemberExpression or a BinaryExpression");
            }
            var memberExpression = (MemberExpression)binaryExpression.Left;

            var comparisonOp = Ops[binaryExpression.NodeType];

            object finalValue = null;

            if (binaryExpression.Right is ConstantExpression)
            {
                var valueExpression = (ConstantExpression)binaryExpression.Right;
                finalValue = valueExpression.Value;
            }
            else if (binaryExpression.Right is MemberExpression)
            {
                var fieldExpression = (MemberExpression)binaryExpression.Right;
                finalValue = GetValueFromMemberExpression(fieldExpression);
            }
            else if (binaryExpression.Right is MethodCallExpression)
            {
                var methodCallExpression = (MethodCallExpression)binaryExpression.Right;
                finalValue = Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
            }
            else if (binaryExpression.Right is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression) binaryExpression.Right;
                var operand = unaryExpression.Operand as MemberExpression;
                if (operand != null)
                {
                    var fieldExpression = operand;
                    finalValue = GetValueFromMemberExpression(fieldExpression);
                }
            }

            if (finalValue == null && comparisonOp == Criteria.ComparisonOp.Equals) comparisonOp = Criteria.ComparisonOp.Is;
            if (finalValue == null && comparisonOp == Criteria.ComparisonOp.NotEquals) comparisonOp = Criteria.ComparisonOp.IsNot;

            var memberInfo = memberExpression.Member;
            var propInfo = memberInfo as PropertyInfo;
            var type = propInfo.PropertyType;
            if (typeof(IBusinessObject).IsAssignableFrom(type))
            {
                return CreateCriteriaForRelationship(memberInfo, comparisonOp, finalValue);
            }
            
            return new Criteria(memberExpression.Member.Name, comparisonOp, finalValue);
        }

        private Criteria CreateCriteriaForRelationship(MemberInfo memberInfo, Criteria.ComparisonOp comparisonOp, object finalValue)
        {
            var relationshipName = memberInfo.Name;
            var classDef = ClassDefCol.GetColClassDef()[memberInfo.DeclaringType];
            var relationshipDef = classDef.GetRelationship(relationshipName);
            var relatedObject = finalValue as IBusinessObject;
            Criteria criteria = null;
            var logicalOp = comparisonOp == Criteria.ComparisonOp.Equals
                        ? Criteria.LogicalOp.And
                        : Criteria.LogicalOp.Or;
            foreach (var relPropDef in relationshipDef.RelKeyDef)
            {
                var propertyCriteria = new Criteria(
                        relationshipName + "." + relPropDef.OwnerPropertyName,
                        comparisonOp,
                        relatedObject.GetPropertyValue(relPropDef.RelatedClassPropName)
                    );

                if (criteria == null)
                    criteria = propertyCriteria;
                else
                {
                    criteria = new Criteria(criteria, logicalOp, propertyCriteria);
                }
            }
            return criteria;
        }

        private object GetValueFromMemberExpression(MemberExpression fieldExpression)
        {
            return Expression.Lambda(fieldExpression).Compile().DynamicInvoke();
        }

        /// <summary>
        /// Used to append another set of criteria (<see cref="CriteriaBuilder.And"/>
        /// </summary>
        public class CriteriaBuilderAnd : CriteriaBuilder
        {
            /// <summary>
            /// <see cref="CriteriaBuilder"/>
            /// </summary>
            /// <param name="expression"></param>
            public CriteriaBuilderAnd(Expression expression) : base(expression)  { }


            /// <summary>
            /// <see cref="Criteria.Expr{T}"/>
            /// </summary>
            /// <param name="expression">The expression</param>
            /// <typeparam name="T">The type the criteria is for</typeparam>
            /// <returns>A new CriteriaBuilder that links this one to that one with an And</returns>
            public CriteriaBuilder Expr<T>(Expression<Func<T, bool>> expression)
            {
                return new CriteriaBuilder(Expression.AndAlso(_expression, expression.Body));
            }

            /// <summary>
            /// <see cref="Criteria.Not{T}"/>
            /// </summary>
            /// <param name="expression">The expression</param>
            /// <typeparam name="T">The type the criteria is for</typeparam>
            /// <returns>A new CriteriaBuilder that links this one to that one with an "And Not"</returns>
            public override CriteriaBuilder Not<T>(Expression<Func<T, bool>> expression)
            {
                return new CriteriaBuilder(Expression.AndAlso(_expression, Expression.Not(expression.Body)));
            }
        }

        /// <summary>
        /// Used to append another set of criteria (<see cref="CriteriaBuilder.Or"/>
        /// </summary>
        public class CriteriaBuilderOr : CriteriaBuilder
        {  
            /// <summary>
            /// <see cref="CriteriaBuilder"/>
            /// </summary>
            /// <param name="expression"></param>
            public CriteriaBuilderOr(Expression expression) : base(expression)  { }

            /// <summary>
            /// <see cref="Criteria.Expr{T}"/>
            /// </summary>
            /// <param name="expression">The expression</param>
            /// <typeparam name="T">The type the criteria is for</typeparam>
            /// <returns>A new CriteriaBuilder that links this one to that one with an Or</returns>
            public CriteriaBuilder Expr<T>(Expression<Func<T, bool>> expression)
            {
                return new CriteriaBuilder(Expression.OrElse(_expression, expression.Body));
            }

            /// <summary>
            /// <see cref="Criteria.Not{T}"/>
            /// </summary>
            /// <param name="expression">The expression</param>
            /// <typeparam name="T">The type the criteria is for</typeparam>
            /// <returns>A new CriteriaBuilder that links this one to that one with an "Or Not"</returns>
            public override CriteriaBuilder Not<T>(Expression<Func<T, bool>> expression)
            {
                return new CriteriaBuilder(Expression.OrElse(_expression, Expression.Not(expression.Body)));
            }
        }

    }


}
