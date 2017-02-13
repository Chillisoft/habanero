using System;
using System.Collections.Generic;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test
{
    [TestFixture]
    public class TestFunctionQueryField
    {
        private class ArbFunction : FunctionQueryField
        {
            public ArbFunction(string functionName, params object[] parameters) : base(functionName, parameters)
            {
            }
        }
        [Test]
        public void Construct_GivenNullFunctionName_ShouldThrowArgumentException()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => new ArbFunction(null));

            //---------------Test Result -----------------------
            Assert.AreEqual("functionName", ex.ParamName);
        }

        [Test]
        public void Construct_GivenEmptyFunctionName_ShouldThrowArgumentException()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => new ArbFunction(string.Empty));

            //---------------Test Result -----------------------
            Assert.AreEqual("functionName", ex.ParamName);
        }

        [Test]
        public void PropertyName_ShouldReturnFunctionNameWithBrackets()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.PropertyName;

            //---------------Test Result -----------------------
            Assert.AreEqual("ARB()", result);
        }

        [Test]
        public void FieldName_ShouldReturnStringRepresentationOfParameters()
        {
            //---------------Set up test pack-------------------
            var sut = Create("foo", "bar");
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.FieldName;

            //---------------Test Result -----------------------
            Assert.AreEqual("foo,bar", result);
        }

        [Test]
        public void Source_ShouldReturnNullByDefault()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.Source;

            //---------------Test Result -----------------------
            Assert.IsNull(result);
        }

        [Test]
        public void GetFormattedStringWith_GivenOneStringParameter_ShouldProduceExpectedString()
        {
            //---------------Set up test pack-------------------
            var sut = Create("foo");
            var sqlFormatter = new SqlFormatter("", "", "", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.GetFormattedStringWith(sqlFormatter, new Dictionary<string, string>());

            //---------------Test Result -----------------------
            Assert.AreEqual("arb('foo')", result);
        }

        [Test]
        public void GetFormattedStringWith_GivenTwoStringParameters_ShouldProduceExpectedString()
        {
            //---------------Set up test pack-------------------
            var sut = Create("foo", "bar");
            var sqlFormatter = new SqlFormatter("", "", "", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.GetFormattedStringWith(sqlFormatter, new Dictionary<string, string>());

            //---------------Test Result -----------------------
            Assert.AreEqual("arb('foo','bar')", result);
        }

        private ArbFunction Create(params object[] parameters)
        {
            return new ArbFunction("arb", parameters);
        }
    }
}
