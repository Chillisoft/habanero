using System.Collections.Generic;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test
{
    [TestFixture]
    public class TestCountFunctionQueryField
    {
        private CountFunctionQueryField Create(object field = null)
        {
            return new CountFunctionQueryField(field);
        }

        [Test]
        public void PropertyName_ShouldReturn_COUNT()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.PropertyName;

            //---------------Test Result -----------------------
            Assert.AreEqual("COUNT()", result);
        }

        [Test]
        public void GetFormattedStringWith_WhenConstructedWithStarField_ReturnsFormattedSQLString()
        {
            //---------------Set up test pack-------------------
            var sut = Create("*");
            var formatter = new SqlFormatter("[", "]", "", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.GetFormattedStringWith(formatter, EmptyAliases());

            //---------------Test Result -----------------------
            Assert.AreEqual("count(*)", result);
        }

        private static Dictionary<string, string> EmptyAliases()
        {
            return new Dictionary<string, string>();
        }

        [Test]
        public void GetFormattedStringWith_WhenConstructedWithNoField_ReturnsFormattedSQLString()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            var formatter = new SqlFormatter("[", "]", "", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.GetFormattedStringWith(formatter, EmptyAliases());

            //---------------Test Result -----------------------
            Assert.AreEqual("count(*)", result);
        }

        [TestCase("[", "]")]
        [TestCase("\"", "\"")]
        [TestCase("", "")]
        public void GetFormattedStringWith_ParameterIsString_ShouldQuoteParameter(string left, string right)
        {
            //---------------Set up test pack-------------------
            var sut = Create(CreateQueryFieldFor("SomeColumn"));
            var formatter = new SqlFormatter(left, right, "", "");
            var expected = string.Join("", new[] { "count(", left, "SomeColumn", right, ")" });
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.GetFormattedStringWith(formatter, EmptyAliases());

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        private QueryField CreateQueryFieldFor(string someColumn)
        {
            return new QueryField("", someColumn, null);
        }

        [TestCase("[", "]")]
        [TestCase("\"", "\"")]
        [TestCase("", "")]
        public void GetFormattedStringWith_WhenConstructedWithQueryFieldWithNoTableName_ShouldReturnCorrectString(string left, string right)
        {
            //---------------Set up test pack-------------------
            var queryField = new QueryField("property", "field", null);
            var sut = Create(queryField);
            var formatter = new SqlFormatter(left, right, "", "");
            var expected = string.Join("", new[] { "count(", left, "field", right, ")" });
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.GetFormattedStringWith(formatter, EmptyAliases());

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [TestCase("[", "]")]
        [TestCase("\"", "\"")]
        [TestCase("", "")]
        public void GetFormattedStringWith_WhenConstructedWithQueryFieldWithTableName_ShouldReturnCorrectString(string left, string right)
        {
            //---------------Set up test pack-------------------
            var queryField = new QueryField("property", "field", new Source("table"));
            var sut = Create(queryField);
            var formatter = new SqlFormatter(left, right, "", "");
            var expected = string.Join("", new[] { "count(", left, "table", right, ".", left, "field", right, ")" });
            var aliases = new Dictionary<string, string>();
            aliases["table"] = "table";
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.GetFormattedStringWith(formatter, aliases);

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }
    }
}