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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.CriteriaManager
{
    [TestFixture]
    public class TestCriteriaBuilder
    {
        private CriteriaBuilder Builder { get; set; }

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp] 
        public void SetupFixture()
        {
            ClassDefCol.GetColClassDef().Clear();
        }

        [Test]
        public void BinaryExpression_Equals_Const()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Equals;
            const string val = "hello";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestProp == val).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        // ReSharper disable ConvertToConstant.Local
        // testing specifically with a variable, not a constant.
        [Test]
        public void BinaryExpression_Equals_Var()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Equals;
            var val = "hello";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestProp == val).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
        // ReSharper restore ConvertToConstant.Local

        [Test]
        public void BinaryExpression_NullableProperty()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadClassDefWithNullableDateTime();
            var op = Criteria.ComparisonOp.GreaterThan;
            var dateTime = DateTime.Now;
            var expectedCriteria = Criteria.Create<MyBO, DateTime?>(bo => bo.TestDateTimeNullable, op, dateTime);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestDateTimeNullable > dateTime).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void BinaryExpression_GreaterThan()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.GreaterThan;
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>(bo => bo.TestInt, op, val); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt > val).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void BinaryExpression_GreaterThanEqual()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.GreaterThanEqual;
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>(bo => bo.TestInt, op, val); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt >= val).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void UnaryExpression_In()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var values = new [] { 1, 5, 10 };
            var expectedCriteria = new Criteria("TestInt", Criteria.ComparisonOp.In, values); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => values.Contains(bo.TestInt)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void BinaryExpression_Is_EqualsNull()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Is;
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, null);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestProp == null).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void UnaryExpression_Is_NotHasValue()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria = Criteria.Create<MyBO, int?>
                (bo => bo.TestIntNullable, Criteria.ComparisonOp.Is, null);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !bo.TestIntNullable.HasValue).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
   
        [Test]
        public void BinaryExpression_IsNot_Null()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.IsNot;
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, null);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestProp != null).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void UnaryExpression_IsNot_HasValue()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria = Criteria.Create<MyBO, int?>
                (bo => bo.TestIntNullable, Criteria.ComparisonOp.IsNot, null);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestIntNullable.HasValue).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
   
        [Test]
        public void BinaryExpression_LessThan()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.LessThan;
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>(bo => bo.TestInt, op, val); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt < val).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
   
        [Test]
        public void BinaryExpression_LessThanEqual()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.LessThanEqual;
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>(bo => bo.TestInt, op, val); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt <= val).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
   
        [Test]
        public void UnaryExpression_Like_Contains()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Like;
            const string val = "t";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, "%" + val + "%"); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestProp.Contains(val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
    
        [Test]
        public void UnaryExpression_Like_Contains_WithVar()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Like;
            string val = BOTestUtils.RandomString;
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, "%" + val + "%"); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestProp.Contains(val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
   
        [Test]
        public void UnaryExpression_Like_StartsWith()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Like;
            const string val = "t";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, val + "%"); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestProp.StartsWith(val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
   
        [Test]
        public void UnaryExpression_Like_EndsWith()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Like;
            const string val = "t";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, "%" + val); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestProp.EndsWith(val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void BinaryExpression_NotEquals()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.NotEquals;
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>(bo => bo.TestInt, op, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt != val).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void UnaryExpression_NotIn()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.NotIn;
            var values = new[] { 1, 5, 10 };
            var expectedCriteria = new Criteria("TestInt", op, values);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !values.Contains(bo.TestInt)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }


        [Test]
        public void UnaryExpression_NotLike_Contains()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.NotLike;
            const string val = "t";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, "%" + val + "%");
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !bo.TestProp.Contains(val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void UnaryExpression_NotLike_StartsWith()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.NotLike;
            const string val = "t";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, val + "%");
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !bo.TestProp.StartsWith(val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void UnaryExpression_NotLike_EndsWith()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.NotLike;
            const string val = "t";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, "%" + val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !bo.TestProp.EndsWith(val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
        
        [Test]
        public void CompoundBinaryExpression_And()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria =
                new Criteria(
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.GreaterThan, 5),
                    Criteria.LogicalOp.And,
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.LessThan, 10)
                    );
                    
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt > 5 && bo.TestInt < 10).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
        
        [Test]
        public void CompoundBinaryExpression_BitwiseAnd()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria =
                new Criteria(
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.GreaterThan, 5),
                    Criteria.LogicalOp.And,
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.LessThan, 10)
                    );
                    
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt > 5 & bo.TestInt < 10).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void CompoundBinaryExpression_Or()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria =
                new Criteria(
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.LessThan, 5),
                    Criteria.LogicalOp.Or,
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.GreaterThan, 10)
                    );
                    
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt < 5 || bo.TestInt > 10).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void CompoundBinaryExpression_BitwiseOr()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria =
                new Criteria(
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.LessThan, 5),
                    Criteria.LogicalOp.Or,
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.GreaterThan, 10)
                    );
                    
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt < 5 | bo.TestInt > 10).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void CompoundTernaryExpression()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria =
                new Criteria(
                    new Criteria(
                        Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.LessThan, 5),
                        Criteria.LogicalOp.Or,
                        Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.GreaterThan, 10)
                        ), Criteria.LogicalOp.Or,
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.Equals, 7)
                    );

            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => bo.TestInt < 5 || bo.TestInt > 10 || bo.TestInt == 7).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        // ReSharper disable NegativeEqualityExpression
        [Test]
        public void NotExpression_Equals()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            const string val = "hello";
            var expectedCriteria = Criteria.Create<MyBO, string>
                (bo => bo.TestProp, Criteria.ComparisonOp.NotEquals, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !(bo.TestProp == val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
        // ReSharper restore NegativeEqualityExpression

        [Test]
        public void NotExpression_LessThan()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>
                (bo => bo.TestInt, Criteria.ComparisonOp.GreaterThanEqual, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !(bo.TestInt < val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void NotExpression_LessThanEqualTo()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>
                (bo => bo.TestInt, Criteria.ComparisonOp.GreaterThan, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !(bo.TestInt <= val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
        
        [Test]
        public void NotExpression_GreaterThan()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>
                (bo => bo.TestInt, Criteria.ComparisonOp.LessThanEqual, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !(bo.TestInt > val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
      
        [Test]
        public void NotExpression_GreaterThanOrEqualTo()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>
                (bo => bo.TestInt, Criteria.ComparisonOp.LessThan, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !(bo.TestInt >= val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        // ReSharper disable NegativeEqualityExpression
        [Test]
        public void NotExpression_NotEquals()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>
                (bo => bo.TestInt, Criteria.ComparisonOp.Equals, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !(bo.TestInt != val)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
        // ReSharper restore NegativeEqualityExpression


        [Test]
        public void CompoundBinaryExpression_Not_And()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria =
                new Criteria(
                    null, Criteria.LogicalOp.Not, new Criteria(
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.GreaterThan, 5),
                    Criteria.LogicalOp.And,
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.LessThan, 10)
                    ));

            //---------------Execute Test ----------------------
            var criteria = Criteria.Expr<MyBO>(bo => !(bo.TestInt > 5 && bo.TestInt < 10)).Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void Expr_And()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria = Criteria.Expr<MyBO>(bo => bo.TestInt > 5 && bo.TestInt < 10).Build();
            //---------------Execute Test ----------------------
            var criteria = Criteria
                .Expr<MyBO>(bo => bo.TestInt > 5)
                .And
                .Expr<MyBO>(bo => bo.TestInt < 10)
                .Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void Expr_Or()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria = Criteria.Expr<MyBO>(bo => bo.TestInt < 5 || bo.TestInt > 10).Build();
            //---------------Execute Test ----------------------
            var criteria = Criteria
                .Expr<MyBO>(bo => bo.TestInt < 5)
                .Or
                .Expr<MyBO>(bo => bo.TestInt > 10)
                .Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void Expr_Not()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria = Criteria.Expr<MyBO>(bo => !(bo.TestInt < 5)).Build();
            //---------------Execute Test ----------------------
            var criteria = Criteria
                .Not<MyBO>(bo => bo.TestInt < 5)
                .Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void Expr_And_Not()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria = Criteria.Expr<MyBO>(bo => bo.TestInt > 5 && !(bo.TestInt < 2)).Build();
            //---------------Execute Test ----------------------
            var criteria = Criteria
                .Expr<MyBO>(bo => bo.TestInt > 5)
                .And
                .Not<MyBO>(bo => bo.TestInt < 2)
                .Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void Expr_Or_Not()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var expectedCriteria = Criteria.Expr<MyBO>(bo => bo.TestInt > 5 || !(bo.TestInt > 2)).Build();
            //---------------Execute Test ----------------------
            var criteria = Criteria
                .Expr<MyBO>(bo => bo.TestInt > 5)
                .Or
                .Not<MyBO>(bo => bo.TestInt > 2)
                .Build();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
    }
}
