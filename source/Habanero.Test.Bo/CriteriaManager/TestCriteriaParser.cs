using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.CriteriaManager;
using NUnit.Framework;

namespace Habanero.Test.BO.CriteriaManager
{

    [TestFixture]
    public class TestCriteriaParser
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

}
