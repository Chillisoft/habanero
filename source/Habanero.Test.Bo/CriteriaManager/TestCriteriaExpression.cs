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
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO
{

    [TestFixture]
    public class TestCriteriaExpression
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
        public void TestSimpleExpression_Like()
        {
            CriteriaExpression tree = new CriteriaExpression("Name like 'Pet%'");
            Assert.AreEqual(" LIKE", tree.Expression);
            Assert.AreEqual("Name", tree.Left.Expression);
            Assert.AreEqual("Pet%", tree.Right.Expression);
        }

        [Test]
        public void TestSimpleExpression_NotLike()
        {
            CriteriaExpression tree = new CriteriaExpression("Name not like 'Pet%'");
            Assert.AreEqual(" NOT LIKE", tree.Expression);
            Assert.AreEqual("Name", tree.Left.Expression);
            Assert.AreEqual("Pet%", tree.Right.Expression);
        }

        [Test]
        public void TestSimpleExpression_In()
        {
            CriteriaExpression tree = new CriteriaExpression("Name in ('Peter', 'Mark')");
            Assert.AreEqual(" IN ", tree.Expression);
            Assert.AreEqual("Name", tree.Left.Expression);
            Assert.AreEqual("('Peter', 'Mark')", tree.Right.Expression);
        }

        [Test]
        public void TestSimpleExpression_NotIn()
        {
            CriteriaExpression tree = new CriteriaExpression("Name not in ('Peter', 'Mark')");
            Assert.AreEqual(" NOT IN ", tree.Expression);
            Assert.AreEqual("Name", tree.Left.Expression);
            Assert.AreEqual("('Peter', 'Mark')", tree.Right.Expression);
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

        [Test]
        public void TestSetLeftAndRight()
        {
            CriteriaExpression tree = new CriteriaExpression("Name = 'Peter' AND Age < 30");
            tree.Left = new CriteriaExpression("Height > 20");
            tree.Right = new CriteriaExpression("Town = 'Durban'");
            
            Assert.AreEqual("(Height > 20)", tree.Left.CompleteExpression);
            Assert.AreEqual("(Town = Durban)", tree.Right.CompleteExpression);
        }

        [Test]
        public void TestWithDoubleBrackets()
        {
            String[] operators = new String[]
                {
                    "OR",
                    "AND"
                };
            CriteriaExpression tree =
                new CriteriaExpression("(Name = 'Test' OR Field1 >= 1) AND (Field2 <= 2 OR Name = 'Test2')", operators);
            //Test left side
            CriteriaExpression leftExpression = tree.Left;
            Assert.AreEqual("(Name = 'Test' OR Field1 >= 1)", leftExpression.CompleteExpression);
            Assert.AreEqual("Name = 'Test'", leftExpression.Left.CompleteExpression);
            Assert.AreEqual("OR", leftExpression.Expression);
            Assert.AreEqual("Field1 >= 1", leftExpression.Right.CompleteExpression);
            //Tes operator
            Assert.AreEqual("AND", tree.Expression);
            //Test right side
            CriteriaExpression rightExpression = tree.Right;
            Assert.AreEqual("(Field2 <= 2 OR Name = 'Test2')", rightExpression.CompleteExpression);
            Assert.AreEqual("Field2 <= 2", rightExpression.Left.CompleteExpression);
            Assert.AreEqual("OR", rightExpression.Expression);
            Assert.AreEqual("Name = 'Test2'", rightExpression.Right.CompleteExpression);
            //Test complete
            Assert.AreEqual("((Name = 'Test' OR Field1 >= 1) AND (Field2 <= 2 OR Name = 'Test2'))", tree.CompleteExpression);
        }

        [Test]
        public void Test_CreateInExpression()
        {
            //---------------Set up test pack-------------------
            string expression = "'Item1', 'Item2'";

            //---------------Execute Test ----------------------
            CriteriaExpression inExpression = CriteriaExpression.CreateInExpression(expression);

            //---------------Test Result -----------------------
            Assert.AreEqual(expression, inExpression.CompleteExpression);
            Assert.IsNull(inExpression.Left);
            Assert.IsNull(inExpression.Right);
        }


    }

}
