using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Remotion.Collections;
using Remotion.Data.Linq;
using Remotion.Data.Linq.Clauses;

namespace Habanero.BO.Linq
{
    public class HabQueryExecutor : IQueryExecutor
    {
        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            return ExecuteCollection<T>(queryModel).Single();
        }

        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            return returnDefaultWhenEmpty ? ExecuteCollection<T>(queryModel).SingleOrDefault() : ExecuteCollection<T>(queryModel).Single();
        }

        IEnumerable<T> IQueryExecutor.ExecuteCollection<T>(QueryModel queryModel)
        {
            return ExecuteCollection<T>(queryModel);
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var selectQuery = HabQueryModelVisitor.GenerateSelectQuery(queryModel);

            var businessObjectCollection = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(selectQuery.ClassDef, selectQuery);
            if (typeof(T) == selectQuery.ClassDef.ClassType) return (from object obj in businessObjectCollection select (T)obj).ToList();

            var expression = queryModel.SelectClause.Selector;
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                var member = (MemberExpression) expression;
                string name = member.Member.Name;
                return new PropertyEnumerator<T>(businessObjectCollection, name);
            } else if (expression.NodeType == ExpressionType.New)
            {
                var newExpression = (NewExpression) expression;
                var propertyNames = newExpression.Members.Select(info => info.Name);
                return new NewEnumerator<T>(businessObjectCollection, newExpression.Constructor, propertyNames);
            }
            
            return new List<T>();
        }


         
        private class NewEnumerator<T> : IEnumerable<T>
        {
            private readonly IBusinessObjectCollection _businessObjectCollection;
            private readonly ConstructorInfo _constructor;
            private readonly IEnumerable<string> _propertyNames;

            public NewEnumerator(IBusinessObjectCollection businessObjectCollection, ConstructorInfo constructor, IEnumerable<string> propertyNames)
            {
                _businessObjectCollection = businessObjectCollection;
                _constructor = constructor;
                _propertyNames = propertyNames;
            }
            public IEnumerator<T> GetEnumerator()
            {
                var enumerator = _businessObjectCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var element = (IBusinessObject)enumerator.Current;
                    var propertyValues = _propertyNames.Select(element.GetPropertyValue);
                    var obj = _constructor.Invoke(propertyValues.ToArray());
                    yield return (T)obj;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private class PropertyEnumerator<T> : IEnumerable<T>
        {
            private readonly IBusinessObjectCollection _businessObjectCollection;
            private readonly string _propertyName;

            public PropertyEnumerator(IBusinessObjectCollection businessObjectCollection, string propertyName)
            {
                _businessObjectCollection = businessObjectCollection;
                _propertyName = propertyName;
            }

            public IEnumerator<T> GetEnumerator()
            {
                var enumerator = _businessObjectCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var element = (IBusinessObject) enumerator.Current;
                    yield return (T) element.GetPropertyValue(_propertyName);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }

    public class HabQueryModelVisitor : QueryModelVisitorBase
    {
        private ISelectQuery _selectQuery;// = new SelectQuery();

        public static ISelectQuery GenerateSelectQuery(QueryModel queryModel)
        {
            var visitor = new HabQueryModelVisitor();
            visitor.VisitQueryModel(queryModel);
            return visitor.GetSelectQuery();
        }

        private ISelectQuery GetSelectQuery()
        {
            return _selectQuery;
        }

        private void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.SelectClause.Accept(this, queryModel);
            // queryModel.MainFromClause.Accept(this, queryModel);
            VisitBodyClauses(queryModel.BodyClauses, queryModel);
            // VisitResultOperators(queryModel.ResultOperators, queryModel);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            _selectQuery.Criteria = new CriteriaBuilder(whereClause.Predicate).Build();

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            //var source = _selectQuery.Source;
            var itemClassDef = ClassDef.ClassDefs[queryModel.MainFromClause.ItemType];
            _selectQuery = QueryBuilder.CreateSelectQuery(itemClassDef);
            //QueryBuilder.PrepareSource(itemClassDef, ref source);
            //_selectQuery.Source = source;

            base.VisitSelectClause(selectClause, queryModel);
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
        {
            var joinToSource = Source.FromString(joinClause.ItemType.Name);
            QueryBuilder.PrepareSource(ClassDef.ClassDefs[joinClause.ItemType], ref joinToSource);
            _selectQuery.Source.JoinToSource(joinToSource);
            base.VisitJoinClause(joinClause, queryModel, index);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            _selectQuery.OrderCriteria = GetOrderCriteria(orderByClause.Orderings);
            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        private IOrderCriteria GetOrderCriteria(ObservableCollection<Ordering> orderings)
        {
            var orderCriteria = new OrderCriteria();
            foreach (var ordering in orderings)
            {
                var expression = ordering.Expression;
                if (expression.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression) expression;
                    orderCriteria.Add(memberExpression.Member.Name, ordering.OrderingDirection == OrderingDirection.Asc ? SortDirection.Ascending : SortDirection.Descending);
                }
            }

            return orderCriteria;
        }

        //private Criteria GetCriteria(Expression expression)
        //{
        //    var criteriaString = CriteriaGeneratorExpressionTreeVisitor.GetCriteriaString(expression);
        //    return CriteriaParser.CreateCriteria(criteriaString);
        //}
    }


    //public class CriteriaGeneratorExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    //{
    //    readonly Dictionary<ExpressionType, string> operators = new Dictionary<ExpressionType, string>()
    //                                                                { 
    //                                                                    { ExpressionType.Equal, "=" },
    //                                                                    { ExpressionType.LessThan, "<" },
    //                                                                    { ExpressionType.LessThanOrEqual, "<=" },
    //                                                                    { ExpressionType.GreaterThan, ">" },
    //                                                                    { ExpressionType.GreaterThanOrEqual, ">=" },
    //                                                                    { ExpressionType.AndAlso, "AND" },
    //                                                                    { ExpressionType.And, "AND" },
    //                                                                    { ExpressionType.OrElse, "OR" },
    //                                                                    { ExpressionType.Or, "OR" },
    //                                                                    { ExpressionType.NotEqual, "<>" }
    //                                                                };
    //    private readonly StringBuilder _criteriaString = new StringBuilder();

    //    private CriteriaGeneratorExpressionTreeVisitor()
    //    {
    //    }

    //    public static string GetCriteriaString(Expression linqExpression)
    //    {
    //        var visitor = new CriteriaGeneratorExpressionTreeVisitor();
    //        visitor.VisitExpression(linqExpression);
    //        return visitor.GetCriteriaString();
    //    }

     
    //    public string GetCriteriaString()
    //    {
    //        return _criteriaString.ToString();
    //    }

    //    protected override Expression VisitBinaryExpression(BinaryExpression expression)
    //    {
    //        _criteriaString.Append("(");
    //        VisitExpression(expression.Left);
    //        _criteriaString.Append(" " + operators[expression.NodeType] + " ");
    //        VisitExpression(expression.Right);
    //        _criteriaString.Append(")");
    //        return expression;
    //    }

    //    protected override Expression VisitConstantExpression(ConstantExpression expression)
    //    {
    //        _criteriaString.Append(expression.Value);
    //        return expression;
    //    }

    //    protected override Expression VisitMemberExpression(MemberExpression expression)
    //    {
    //        VisitExpression(expression.Expression);
    //        //_criteriaString.AppendFormat(".{0}", expression.Member.Name);
    //        _criteriaString.AppendFormat("{0}", expression.Member.Name);

    //        return expression;
    //    }

    //    protected override Expression VisitQuerySourceReferenceExpression(Remotion.Data.Linq.Clauses.Expressions.QuerySourceReferenceExpression expression)
    //    {
    //        if (expression.ReferencedQuerySource is JoinClause)
    //        {
    //            var joinClause = (JoinClause) expression.ReferencedQuerySource;
    //            var outerKeySelector = joinClause.OuterKeySelector;
    //            if (outerKeySelector is MemberExpression)
    //            {
    //                var outerKeyMember = (MemberExpression) outerKeySelector;
    //                _criteriaString.Append(outerKeyMember.Member.Name + ".");
    //            }
    //        }
    //        return expression;
    //    }

    //    // Called when a LINQ expression type is not handled above.
    //    protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
    //    {
    //        string itemText = FormatUnhandledItem(unhandledItem);
    //        var message = string.Format("The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof(T));
    //        return new NotSupportedException(message);
    //    }

    //    private string FormatUnhandledItem<T>(T unhandledItem)
    //    {
    //        var itemAsExpression = unhandledItem as Expression;
    //        return itemAsExpression != null ? FormattingExpressionTreeVisitor.Format(itemAsExpression) : unhandledItem.ToString();
    //    }

    //}
}