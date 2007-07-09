using System;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Base;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Bo.CriteriaManager
{
    /// <summary>
    /// Creates sql criteria clauses
    /// </summary>
    /// TODO: Needs to be parametrized.
    public class SqlCriteriaCreator
    {
        private IExpression _expression;

        /// <summary>
        /// Constructor to initialise a new criteria creator
        /// </summary>
        /// <param name="exp">The expression</param>
        /// <param name="bo">The business object</param>
        public SqlCriteriaCreator(IExpression exp, BusinessObject bo)
        {
            ClassDef lClassDef = bo.ClassDef;
            ConstructSqlCriteriaCreator(exp, lClassDef);
        }

        /// <summary>
        /// Constructor as before, but with a class definition specified
        /// </summary>
        public SqlCriteriaCreator(IExpression exp, ClassDef classDef)
        {
            ConstructSqlCriteriaCreator(exp, classDef);
        }

        /// <summary>
        /// Sets up the sql criteria creator
        /// </summary>
        /// <param name="exp">The expression to create sql for</param>
        /// <param name="classDef">The class definition</param>
        private void ConstructSqlCriteriaCreator(IExpression exp, ClassDef classDef)
        {
            _expression = exp;
            ClassDef lClassDef = classDef;
            String tableName = lClassDef.TableName;
            foreach (PropDef def in lClassDef.PropDefcol.Values)
            {
                _expression.SetParameterSqlInfo(def, tableName);
            }

            //		//TODO:Use DB Connection to get field/date separators - use parametrized sql
            //		mSqlExpression = exp.SqlExpressionString("", "", "", "");
        }

        //	public ISqlStatement SqlExpression {
        //		get { return mSqlExpression; }
        //	}

        /// <summary>
        /// Appends the criteria to a sql statement
        /// </summary>
        /// <param name="sqlStatement">The sql statement to append to</param>
        public void AppendCriteriaToStatement(ISqlStatement sqlStatement)
        {
            //TODO:Use DB Connection to get field/date separators
            _expression.SqlExpressionString(sqlStatement, "", "");
        }
    }
}