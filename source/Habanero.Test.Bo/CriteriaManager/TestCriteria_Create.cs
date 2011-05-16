using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO.CriteriaManager
{
    [TestFixture]
    public class TestCriteria_Create
    {

        [Test]
        public void BinaryExpression_Equals_Const()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Equals;
            const string val = "hello";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => bo.TestProp == val);
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestProp == val);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
        // ReSharper restore ConvertToConstant.Local

        [Test]
        public void BinaryExpression_GreaterThan()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.GreaterThan;
            const int val = 5;
            var expectedCriteria = Criteria.Create<MyBO, int>(bo => bo.TestInt, op, val); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => bo.TestInt > val);
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestInt >= val);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        [Ignore("Peter working on this")]
        public void UnaryExpression_In()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.In;
            var values = new [] { 1, 5, 10 };
            var expectedCriteria = new Criteria("TestInt", Criteria.ComparisonOp.In, values); 
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => values.Contains(bo.TestInt));
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestProp == null);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        [Ignore("Peter working on this")]
        public void UnaryExpression_Is_HasValue()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.Is;
            var expectedCriteria = Criteria.Create<MyBO, int?>(bo => bo.TestIntNullable, op, null);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => bo.TestIntNullable.HasValue);
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestProp != null);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        [Ignore("Peter working on this")]
        public void UnaryExpression_IsNot_HasValue()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.IsNot;
            var expectedCriteria = Criteria.Create<MyBO, int?>(bo => bo.TestIntNullable, op, null);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => !bo.TestIntNullable.HasValue);
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestInt < val);
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestInt <= val);
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestProp.Contains(val));
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestProp.StartsWith(val));
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestProp.EndsWith(val));
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestInt != val);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        [Ignore("Peter working on this")]
        public void UnaryExpression_NotIn()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.NotIn;
            var values = new[] { 1, 5, 10 };
            var expectedCriteria = new Criteria("TestInt", op, values);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => !values.Contains(bo.TestInt));
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }


        [Test]
        [Ignore("Peter working on this")]
        public void UnaryExpression_NotLike_Contains()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.NotLike;
            const string val = "t";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, "%" + val + "%");
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => !bo.TestProp.Contains(val));
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        [Ignore("Peter working on this")]
        public void UnaryExpression_NotLike_StartsWith()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.NotLike;
            const string val = "t";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, val + "%");
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => !bo.TestProp.StartsWith(val));
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        [Ignore("Peter working on this")]
        public void UnaryExpression_NotLike_EndsWith()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.NotLike;
            const string val = "t";
            var expectedCriteria = Criteria.Create<MyBO, string>(bo => bo.TestProp, op, "%" + val);
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => !bo.TestProp.EndsWith(val));
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }
        
        [Test]
        public void CompoundBinaryExpression_And()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.GreaterThan;
            var expectedCriteria =
                new Criteria(
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.GreaterThan, 5),
                    Criteria.LogicalOp.And,
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.LessThan, 10)
                    );
                    
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => bo.TestInt > 5 && bo.TestInt < 10);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void CompoundBinaryExpression_Or()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.GreaterThan;
            var expectedCriteria =
                new Criteria(
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.LessThan, 5),
                    Criteria.LogicalOp.Or,
                    Criteria.Create<MyBO, int>(bo => bo.TestInt, Criteria.ComparisonOp.GreaterThan, 10)
                    );
                    
            //---------------Execute Test ----------------------
            var criteria = Criteria.Create<MyBO>(bo => bo.TestInt < 5 || bo.TestInt > 10);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }

        [Test]
        public void CompoundTernaryExpression()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            var op = Criteria.ComparisonOp.GreaterThan;
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
            var criteria = Criteria.Create<MyBO>(bo => bo.TestInt < 5 || bo.TestInt > 10 || bo.TestInt == 7);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedCriteria, criteria);
        }



    }
}
