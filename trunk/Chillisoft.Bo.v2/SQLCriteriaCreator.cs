using System;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using BusinessObject=Chillisoft.Bo.v2.BusinessObject;

namespace Chillisoft.Bo.CriteriaManager.v2
{
    /// <summary>
    /// Creates sql criteria clauses
    /// </summary>
    /// TODO: Needs to be parametrized.
    /// TODO ERIC - needs serious revision
    public class SQLCriteriaCreator
    {
        private IExpression _expression;

        /// <summary>
        /// Constructor to initialise a new criteria creator
        /// </summary>
        /// <param name="exp">The expression</param>
        /// <param name="bo">The business object</param>
        public SQLCriteriaCreator(IExpression exp, BusinessObject bo)
        {
            ClassDef lClassDef = bo.ClassDef;
            ConstructSQLCriteriaCreator(exp, lClassDef);
        }

        /// <summary>
        /// Constructor as before, but with a class definition specified
        /// </summary>
        public SQLCriteriaCreator(IExpression exp, ClassDef classDef)
        {
            ConstructSQLCriteriaCreator(exp, classDef);
        }

        /// <summary>
        /// Sets up the sql criteria creator
        /// </summary>
        /// <param name="exp">The expression to create sql for</param>
        /// <param name="classDef">The class definition</param>
        private void ConstructSQLCriteriaCreator(IExpression exp, ClassDef classDef)
        {
            _expression = exp;
            ClassDef lClassDef = classDef;
            String tableName = lClassDef.TableName;
            foreach (PropDef def in lClassDef.PropDefcol.Values)
            {
                _expression.SetParameterSqlInfo(def, tableName);
            }

            //		//TODO:Use DB Connection to get field/date separators - use parametrized sql
            //		mSQLExpression = exp.SqlExpressionString("", "", "", "");
        }

        //	public ISqlStatement SQLExpression {
        //		get { return mSQLExpression; }
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