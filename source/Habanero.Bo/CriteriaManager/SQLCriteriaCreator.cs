//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.CriteriaManager
{
    /// <summary>
    /// Creates sql criteria clauses
    /// </summary>
    /// TODO: Needs to be parametrized.
    public class SqlCriteriaCreator
    {
        private IExpression _expression;
        private IDatabaseConnection _connection;

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
        /// Constructor as before, but with a class definition specified
        /// </summary>
        public SqlCriteriaCreator(IExpression exp, ClassDef classDef, IDatabaseConnection connection)
        {
            _connection = connection;
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
            //ClassDef lClassDef = classDef;
        	ClassDef thisClassDef = classDef;
			while (thisClassDef != null)
			{
				//String tableName = thisClassDef.TableName;
				foreach (PropDef propDef in thisClassDef.PropDefcol)
				{
                    PropDefParameterSQLInfo propDefParameterSQLInfo = new PropDefParameterSQLInfo(propDef, thisClassDef);
                    _expression.SetParameterSqlInfo(propDefParameterSQLInfo);
				}
				thisClassDef = thisClassDef.SuperClassClassDef;
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
            string leftDelimiter = "";
            string rightDelimiter = "";
            if (_connection != null)
            {
                leftDelimiter = _connection.LeftFieldDelimiter;
                rightDelimiter = _connection.RightFieldDelimiter;
            }
            _expression.SqlExpressionString(sqlStatement, leftDelimiter, rightDelimiter);
        }
    }
}