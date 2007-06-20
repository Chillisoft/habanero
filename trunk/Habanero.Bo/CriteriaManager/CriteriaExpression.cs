using System;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Bo.CriteriaManager
{
    /// <summary>
    /// This class manages expressions that force certain criteria upon data
    /// or equations.  An instance of this class could hold the entire
    /// expression, or the expression may be broken up into manageable parts,
    /// with each part in a different object, to form a linked list of
    /// objects.
    /// </summary>
    public class CriteriaExpression
    {
        private static String[] _defaultOperators = new String[]
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
                " LIKE",
                " IN"
            };

        private CriteriaExpression _left;
        private CriteriaExpression _right;
        private String _expression;
        private String[] _operators;

        /// <summary>
        /// A constructor that takes a given expression string and parses it
        /// into a linked list of expressions
        /// </summary>
        /// <param name="expression">The expression string to parse</param>
        public CriteriaExpression(String expression)
        {
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
                    if (expressionWithoutQuotes[bracketSearchPos] == '(')
                    {
                        bracketCount++;
                    }
                    else if (expressionWithoutQuotes[bracketSearchPos] == ')')
                    {
                        bracketCount--;
                    }
                }
                if (bracketSearchPos == expressionWithoutQuotes.Length - 1)
                {
                    parseExpression(
                        quotesRemovedExpression.Substring(1, expressionWithoutQuotes.Length - 2).PutBackQuotedSections());
                    return;
                }
                _left =
                    new CriteriaExpression(
                        quotesRemovedExpression.Substring(1, bracketSearchPos - 1).PutBackQuotedSections().ToString().
                            Trim(), _operators);
                foreach (String op in _operators)
                {
                    int pos = expressionWithoutQuotes.IndexOf(op, bracketSearchPos);
                    if (pos != -1)
                    {
                        _right =
                            new CriteriaExpression(
                                quotesRemovedExpression.Substring(pos + op.Length).PutBackQuotedSections().ToString().
                                    Trim(), _operators);
                        _expression = op;
                        break;
                    }
                }
            }
            else
            {
                foreach (String op in _operators)
                {
                    int pos = expressionWithoutQuotes.IndexOf(op);
                    if (pos != -1 && !IsPosInsideBrackets(expressionWithoutQuotes, pos))
                    {
                        _left =
                            new CriteriaExpression(
                                quotesRemovedExpression.Substring(0, pos).PutBackQuotedSections().ToString().Trim(),
                                _operators);
                        _right =
                            new CriteriaExpression(
                                quotesRemovedExpression.Substring(pos + op.Length).PutBackQuotedSections().ToString().
                                    Trim(), _operators);
                        _expression = op;
                        break;
                    }
                }
            }
            //If this was a terminal criteria i.e. it has no more children then
            // this is the expression there will be no right and left expression.
            if ((_expression == null) || (_expression.Length == 0))
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
        private bool IsPosInsideBrackets(string quote, int pos)
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
        /// A part of the full expression that may be to the left of this
        /// one, if you picture all the parts being laid out in a line.
        /// </summary>
        public CriteriaExpression Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// A part of the full expression that may be to the right of this
        /// one, if you picture all the parts being laid out in a line
        /// </summary>
        public CriteriaExpression Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// Returns the part of the expression held in this object
        /// </summary>
        public String Expression
        {
            get { return _expression; }
        }

        /// <summary>
        /// Returns the full expression by concatenating any parts that
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

        /// <summary>
        /// Runs some associated tests for this class.  Not for ordinary use.
        /// </summary>
        public static void RunTests()
        {
            CriteriaParserTester tester = new CriteriaParserTester();
            tester.TestWithBrackets();
        }
    }


    #region Testing

    [TestFixture]
    public class CriteriaParserTester
    {
        [Test]
        public void TestSimpleExpression()
        {
            CriteriaExpression tree = new CriteriaExpression("Name = 'Peter'");
            Assert.AreEqual("=", tree.Expression);
            Assert.AreEqual("Name", tree.Left.Expression);
            Assert.AreEqual("Peter", tree.Right.Expression);
            tree = new CriteriaExpression("Amount >= 0");
            Assert.AreEqual(">=", tree.Expression);
            Assert.AreEqual("Amount", tree.Left.Expression);
            Assert.AreEqual("0", tree.Right.Expression);
        }

        [Test]
        public void TestSimpleExpressionWithOperatorInQuotes()
        {
            CriteriaExpression tree = new CriteriaExpression("Name = 'Peter = is not cool'");
            Assert.AreEqual("=", tree.Expression);
            Assert.AreEqual("Name", tree.Left.Expression);
            Assert.AreEqual("Peter = is not cool", tree.Right.Expression);
        }

        [Test]
        public void TestCompleteExpression()
        {
            CriteriaExpression tree = new CriteriaExpression("Name = 'Peter'");
            Assert.AreEqual("(Name = Peter)", tree.CompleteExpression);
        }

        [Test]
        public void TestComplexExpression()
        {
            CriteriaExpression tree = new CriteriaExpression("Name = 'Peter' AND Age < 30");
            Assert.AreEqual(" AND ", tree.Expression);
            Assert.AreEqual("=", tree.Left.Expression);
            Assert.AreEqual("Name", tree.Left.Left.Expression);
            Assert.AreEqual("(Name = Peter)", tree.Left.CompleteExpression);
            Assert.AreEqual("Peter", tree.Left.Right.Expression);
            Assert.AreEqual("<", tree.Right.Expression);
            Assert.AreEqual("Age", tree.Right.Left.Expression);
            Assert.AreEqual("30", tree.Right.Right.Expression);
        }

        [Test]
        public void TestSettingOperators()
        {
            String[] operators = new String[]
                {
                    "OR",
                    "AND"
                };
            CriteriaExpression tree = new CriteriaExpression("Name = 'Test'", operators);
            Assert.AreEqual("Name = 'Test'", tree.CompleteExpression);
            tree = new CriteriaExpression("Name = 'Test' and Field1 >= 1", operators);
            Assert.AreEqual("Name = 'Test'", tree.Left.CompleteExpression);
            Assert.AreEqual("Field1 >= 1", tree.Right.CompleteExpression);
            tree = new CriteriaExpression("A = 1 and B = 2 or C = 3", operators);
            Assert.AreEqual("((A = 1 AND B = 2) OR C = 3)", tree.CompleteExpression);
        }

        [Test]
        public void TestWithBrackets()
        {
            String[] operators = new String[]
                {
                    "OR",
                    "AND"
                };
            CriteriaExpression tree =
                new CriteriaExpression("(Name = 'Test' AND Field1 >= 1) or Field2 <= 2", operators);
            Assert.AreEqual("(Name = 'Test' AND Field1 >= 1)", tree.Left.CompleteExpression);
            Assert.AreEqual("((Name = 'Test' AND Field1 >= 1) OR Field2 <= 2)", tree.CompleteExpression);
            tree = new CriteriaExpression("((Name = 'Te' '(st' and Field1 >= 1) or Field2 <= 2)", operators);
            Assert.AreEqual("(Name = 'Te' '(st' AND Field1 >= 1)", tree.Left.CompleteExpression);
            Assert.AreEqual("((Name = 'Te' '(st' AND Field1 >= 1) OR Field2 <= 2)", tree.CompleteExpression);
        }

        [Test]
        public void TestWithQuotes()
        {
            String[] operators = new String[]
                {
                    "OR",
                    "AND"
                };
            CriteriaExpression tree =
                new CriteriaExpression("((Name = 'Te' '(st' and Field1 >= 1) or Field2 <= 2)", operators);
            Assert.AreEqual("(Name = 'Te' '(st' AND Field1 >= 1)", tree.Left.CompleteExpression);
            Assert.AreEqual("((Name = 'Te' '(st' AND Field1 >= 1) OR Field2 <= 2)", tree.CompleteExpression);

            tree = new CriteriaExpression(" ((Name = 'Te' '(st' and Field1 >= 1) or Field2 <= 2) ", operators);
            Assert.AreEqual("(Name = 'Te' '(st' AND Field1 >= 1)", tree.Left.CompleteExpression);
            Assert.AreEqual("((Name = 'Te' '(st' AND Field1 >= 1) OR Field2 <= 2)", tree.CompleteExpression);

            tree = new CriteriaExpression(" ((Name = 'Te' '(st' and Field1 >= 1) or Field2 <= 2)", operators);
            Assert.AreEqual("(Name = 'Te' '(st' AND Field1 >= 1)", tree.Left.CompleteExpression);
            Assert.AreEqual("((Name = 'Te' '(st' AND Field1 >= 1) OR Field2 <= 2)", tree.CompleteExpression);

            tree = new CriteriaExpression(" (  (Name = 'Te' '(st' and Field1 >= 1) or Field2 <= 2)", operators);
            Assert.AreEqual("(Name = 'Te' '(st' AND Field1 >= 1)", tree.Left.CompleteExpression);

            Assert.AreEqual("((Name = 'Te' '(st' AND Field1 >= 1) OR Field2 <= 2)", tree.CompleteExpression);
            Assert.AreEqual("Name = 'Te' '(st'", tree.Left.Left.CompleteExpression);
        }

        [Test]
        public void TestWithAndOR()
        {
            String[] operators = new String[]
                {
                    "OR",
                    "AND"
                };
            CriteriaExpression tree =
                new CriteriaExpression("((Name = 'Te' '(st' and Field1 >= 1) OR Field2 <= 2)", operators);
            Assert.AreEqual("(Name = 'Te' '(st' AND Field1 >= 1)", tree.Left.CompleteExpression);
            Assert.AreEqual("((Name = 'Te' '(st' AND Field1 >= 1) OR Field2 <= 2)", tree.CompleteExpression);
        }
    }

    #endregion //Testing

}