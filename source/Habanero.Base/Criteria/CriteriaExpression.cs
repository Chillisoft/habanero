//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System;

namespace Habanero.Base
{
    /// <summary>
    /// This class is a binary tree that consists of a left expression, a right expression
    ///   and a join expression.
    /// The criteria Exrpession can be created by parsing a criteria string.
    /// </summary>
    public class CriteriaExpression
    {
        private static readonly String[] _defaultOperators = new[]
            {
                " OR ",
                " AND ",
                "<>",
                ">=",
                "<=",
                "=",
                ">",
                "<",
                " IS NOT",
                " IS",
                " NOT LIKE",
                " LIKE",
                " NOT IN ",
                " IN ",
            };

        private CriteriaExpression _left;
        private CriteriaExpression _right;
        private String _expression;
        private readonly String[] _operators;

        /// <summary>
        /// A constructor that takes a given expression string and parses it
        /// into a linked list of expressions
        /// </summary>
        /// <param name="expression">The expression string to parse</param>
        public CriteriaExpression(String expression)
        {
            if (string.IsNullOrEmpty(expression)) return;
            this._operators = _defaultOperators;
            this.parseExpression(new HabaneroStringBuilder(expression.Trim()));
        }

        /// <summary>
        /// A constructor as before, but allows a particular set of
        /// operators to be specified, unlike the default set usually used.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="operators">The set of operators</param>
        public CriteriaExpression(String expression, String[] operators)
        {
            if (string.IsNullOrEmpty(expression)) return;
            this._operators = operators;
            this.parseExpression(new HabaneroStringBuilder(expression.Trim()));
        }

        /// <summary>
        /// Parses the expression into a linked list of expression objects
        /// </summary>
        /// <param name="expression">The expression to parse</param>
        private void parseExpression(HabaneroStringBuilder expression)
        {
            HabaneroStringBuilder quotesRemovedExpression = expression;
            //Remove any sections sorounded by opening and closing ' (single quote)
            quotesRemovedExpression.RemoveQuotedSections();

            //Upper case such that the operator or Or and OR etc are identical.
            String expressionWithoutQuotes = quotesRemovedExpression.ToString().ToUpper();
            //Check if the first character is an opening bracket if it is then find the closing bracket
            // and set this as the left expression.
            if (expressionWithoutQuotes.IndexOf("(") == 0)
            {
                int bracketCount = 1;
                int bracketSearchPos = 0;
                while ((bracketCount > 0) && (bracketSearchPos < expressionWithoutQuotes.Length - 1))
                {
                    bracketSearchPos++;
                    switch (expressionWithoutQuotes[bracketSearchPos])
                    {
                        case '(':
                            bracketCount++;
                            break;
                        case ')':
                            bracketCount--;
                            break;
                    }
                }
                if (bracketSearchPos == expressionWithoutQuotes.Length - 1)
                {
                    parseExpression(
                        quotesRemovedExpression.Substring(1, expressionWithoutQuotes.Length - 2).PutBackQuotedSections());
                    return;
                }
                _left = new CriteriaExpression(quotesRemovedExpression.Substring(1, bracketSearchPos - 1)
                    .PutBackQuotedSections().ToString().Trim(), _operators);
                int pos = -1;
                string foundOperator = "";
                foreach (String op in _operators)
                {
                    int thisPos = expressionWithoutQuotes.IndexOf(op, bracketSearchPos);
                    if ((thisPos == -1 || thisPos >= pos) && pos != -1) continue;
                    pos = thisPos;
                    foundOperator = op;
                }
                if (pos != -1)
                {
                    _right = new CriteriaExpression(quotesRemovedExpression.Substring(pos + foundOperator.Length)
                        .PutBackQuotedSections().ToString().Trim(), _operators);
                    _expression = foundOperator;
                }
            }
            else
            {
                foreach (String op in _operators)
                {
                    int pos = expressionWithoutQuotes.IndexOf(op);
                    if (pos == -1 || IsPosInsideBrackets(expressionWithoutQuotes, pos)) continue;
                    _left = new CriteriaExpression(quotesRemovedExpression.Substring(0, pos)
                                                       .PutBackQuotedSections().ToString().Trim(), _operators);
                    if (op.Trim() == "IN" || op.Trim() == "NOT IN")
                    {
                        _right = CriteriaExpression.CreateInExpression(quotesRemovedExpression.Substring(pos + op.Length)
                                                           .PutBackQuotedSections().ToString().Trim());
                    }
                    else
                    {
                        _right = new CriteriaExpression(quotesRemovedExpression.Substring(pos + op.Length)
                                                            .PutBackQuotedSections().ToString().Trim(), _operators);
                    }
                    _expression = op;
                    break;
                }
            }
            //If this was a terminal criteria i.e. it has no more children then
            // this is the expression there will be no right and left expression.
            if (string.IsNullOrEmpty(_expression))
            {
                _expression = quotesRemovedExpression.PutBackQuotedSections().DropOuterQuotes().ToString();
            }
        }

        /// <summary>
        /// Checks whether the given position in a string is between opening
        /// and closing brackets
        /// </summary>
        /// <param name="quote">The string in question</param>
        /// <param name="pos">The position in the string</param>
        /// <returns>Returns true if between brackets, false if not</returns>
        private static bool IsPosInsideBrackets(string quote, int pos)
        {
            int bracketCount = 0;
            // ignore the first character because then the whole expression is inside brackets.
            for (int i = 1; i < pos; i++)
            {
                if (quote[i] == '(')
                {
                    bracketCount++;
                }
                else if (quote[i] == ')')
                {
                    bracketCount--;
                }
            }
            return bracketCount > 0;
        }

        /// <summary>
        /// Gets or sets a part of the full expression that may be to the left of this
        /// one, if you picture all the parts being laid out in a line.
        /// </summary>
        public CriteriaExpression Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// Gets or sets a part of the full expression that may be to the right of this
        /// one, if you picture all the parts being laid out in a line
        /// </summary>
        public CriteriaExpression Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// Gets the part of the expression held in this object
        /// </summary>
        public String Expression
        {
            get { return _expression; }
        }

        /// <summary>
        /// Gets the full expression by concatenating any parts that
        /// may be to the left or right of this expression with the part
        /// of the expression this object holds
        /// </summary>
        public String CompleteExpression
        {
            get
            {
                String exp = "";
                if (!IsLeaf())
                {
                    exp += "(";
                }
                if (this.Left != null)
                {
                    exp += this.Left.CompleteExpression + " ";
                }
                exp += _expression;
                if (this.Right != null)
                {
                    exp += " " + this.Right.CompleteExpression;
                }
                if (!IsLeaf())
                {
                    exp += ")";
                }
                return exp;
            }
        }

        /// <summary>
        /// Indicates whether this object contains the full expression
        /// </summary>
        /// <returns>Returns true if this is the full expression, or false if 
        /// there are other parts of the expression to the left or right</returns>
        public bool IsLeaf()
        {
            return ((this.Left == null) && (this.Right == null));
        }

        public static CriteriaExpression CreateInExpression(string expression)
        {
            CriteriaExpression criteriaExpression = new CriteriaExpression("");
            criteriaExpression._expression = expression;
            return criteriaExpression;
        }
    }
}