using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestSource_Join
    {
        [Test]
        public void Test_Join_Constructor()
        {
            //---------------Set up test pack-------------------
            Source fromSource = new Source("From");
            Source toSource = new Source("To");
            //---------------Execute Test ----------------------
            Source.Join join = new Source.Join(fromSource, toSource);

            //---------------Test Result -----------------------
            Assert.AreSame(fromSource, join.FromSource);
            Assert.AreSame(toSource, join.ToSource);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestJoinStructure()
        {
            //-------------Setup Test Pack ------------------
            string tableName = "MY_SOURCE";
            Source source = new Source("MySource", tableName);
            string joinTableName = "MY_JOINED_TABLE";
            Source joinSource = new Source("JoinSource", joinTableName);

            Source.Join join = new Source.Join(source, joinSource);
            QueryField fromField = new QueryField("FromField", "FROM_FIELD", source);
            QueryField toField = new QueryField("ToField", "TO_FIELD", joinSource);

            //-------------Execute test ---------------------
            join.JoinFields.Add(new Source.Join.JoinField(fromField, toField));
            source.Joins.Add(join);
            //-------------Test Result ----------------------

            Assert.AreEqual(1, source.Joins.Count);
            Assert.AreSame(join, source.Joins[0]);
            Assert.AreSame(fromField, join.JoinFields[0].FromField);
            Assert.AreSame(toField, join.JoinFields[0].ToField);
        }

        [Test]
        public void TestJoinField_Constructor()
        {
            //-------------Setup Test Pack ------------------
            string tableName = "MY_SOURCE";
            Source source = new Source("MySource", tableName);
            string joinTableName = "MY_JOINED_TABLE";
            Source joinSource = new Source("JoinSource", joinTableName);
            QueryField fromField = new QueryField("FromField", "FROM_FIELD", source);
            QueryField toField = new QueryField("ToField", "TO_FIELD", joinSource);

            //-------------Execute test ---------------------
            Source.Join.JoinField joinField = new Source.Join.JoinField(fromField, toField);
            //-------------Test Result ----------------------
            Assert.AreSame(fromField, joinField.FromField);
            Assert.AreSame(toField, joinField.ToField);
        }
    }
}
