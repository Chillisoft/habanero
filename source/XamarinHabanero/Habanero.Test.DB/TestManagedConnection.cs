using System;
using System.Data;
using Habanero.DB;
using Habanero.Test.Structure;
using NSubstitute;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestManagedConnection
    {
        [Test]
        public void Construct_GivenNullConnection_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new ManagedConnection(null));

            //---------------Test Result -----------------------
            Assert.AreEqual("connection", ex.ParamName);
        }

        [Test]
        public void Construct_GivenAllParameters_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new ManagedConnection(Substitute.For<IDbConnection>()));

            //---------------Test Result -----------------------
        }

        [Test]
        public void Dispose_ShouldCallUnderlying_Dispose()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var sut = Create(connection);
            //---------------Assert Precondition----------------
            connection.DidNotReceive().Dispose();
            //---------------Execute Test ----------------------
            sut.Dispose();
            //---------------Test Result -----------------------
            connection.Received(1).Dispose();
        }

        [Test]
        public void BeginTransaction_ShouldCallUnderlying_BeginTransaction()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var sut = Create(connection);
            //---------------Assert Precondition----------------
            connection.DidNotReceive().BeginTransaction();

            //---------------Execute Test ----------------------
            sut.BeginTransaction();

            //---------------Test Result -----------------------
            connection.Received(1).BeginTransaction();
        }

        [Test]
        public void BeginTransaction_ShouldReturnTheResultFromUnderlyingMethod()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = Substitute.For<IDbTransaction>();
            connection.BeginTransaction().Returns(expected);
            var sut = Create(connection);
            //---------------Assert Precondition----------------
            connection.DidNotReceive().BeginTransaction();

            //---------------Execute Test ----------------------
            var result = sut.BeginTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void BeginTransaction_GivenIsolationLevel_ShouldPassIsolationlevelToUnderlyingMethod()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = RandomValueGen.GetRandomEnum<IsolationLevel>();
            var sut = Create(connection);
            //---------------Assert Precondition----------------
            connection.DidNotReceive().BeginTransaction(expected);

            //---------------Execute Test ----------------------
            sut.BeginTransaction(expected);

            //---------------Test Result -----------------------
            connection.Received(1).BeginTransaction(expected);
        }

        [Test]
        public void BeginTransaction_GivenIsolationLevel_ShouldReturnUnderlyingResult()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = Substitute.For<IDbTransaction>();
            var isolationLevel = RandomValueGen.GetRandomEnum<IsolationLevel>();
            connection.BeginTransaction(isolationLevel).Returns(expected);
            var sut = Create(connection);
            //---------------Assert Precondition----------------
            connection.DidNotReceive().BeginTransaction(Arg.Any<IsolationLevel>());

            //---------------Execute Test ----------------------
            var result = sut.BeginTransaction(isolationLevel);

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Close_ShouldCallUnderlyingClose()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------
            connection.DidNotReceive().Close();

            //---------------Execute Test ----------------------
            sut.Close();

            //---------------Test Result -----------------------
            connection.Received(1).Close();
        }

        [Test]
        public void Close_ShouldSetAvailableToTrue()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------
            Assert.IsFalse(sut.Available);

            //---------------Execute Test ----------------------
            sut.Close();

            //---------------Test Result -----------------------
            Assert.IsTrue(sut.Available);
        }

        [Test]
        public void ChangeDatabase_ShouldCallUnderlyingChangeDatabaseWithParameter()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = RandomValueGen.GetRandomString(1, 10);
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------
            connection.DidNotReceive().ChangeDatabase(Arg.Any<string>());

            //---------------Execute Test ----------------------
            sut.ChangeDatabase(expected);

            //---------------Test Result -----------------------
            connection.Received(1).ChangeDatabase(expected);
        }

        [Test]
        public void CreateCommand_ShouldCallUnderlyingMethod()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------
            connection.DidNotReceive().CreateCommand();

            //---------------Execute Test ----------------------
            sut.CreateCommand();

            //---------------Test Result -----------------------
            connection.Received(1).CreateCommand();
        }

        [Test]
        public void CreateCommand_ShouldReturnResultFromUnderlyingCall()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = Substitute.For<IDbCommand>();
            connection.CreateCommand().Returns(expected);
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------
            connection.DidNotReceive().CreateCommand();

            //---------------Execute Test ----------------------
            var result = sut.CreateCommand();

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Open_ShouldCallUnderlyingMethod()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var sut = Create(connection);
            //---------------Assert Precondition----------------
            connection.DidNotReceive().Open();

            //---------------Execute Test ----------------------
            sut.Open();

            //---------------Test Result -----------------------
            connection.Received(1).Open();
        }

        [Test]
        public void Open_ShouldSetAvailableToFalse()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------
            Assert.IsFalse(sut.Available);
            sut.Open();
            Assert.IsFalse(sut.Available);
            sut.Close();
            Assert.IsTrue(sut.Available);

            //---------------Execute Test ----------------------
            sut.Open();

            //---------------Test Result -----------------------
            Assert.IsFalse(sut.Available);
        }

        [Test]
        public void ConnectionString_ShouldReturnValueOfUnderlyingProperty()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = RandomValueGen.GetRandomString();
            connection.ConnectionString.Returns(expected);
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.ConnectionString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionString_ShouldSetValueOfUnderlyingProperty()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = RandomValueGen.GetRandomString();
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ConnectionString = expected;
            var result = sut.ConnectionString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionTimeout_ShouldReturnValueOfUnderlyingProperty()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = RandomValueGen.GetRandomInt(1, 100);
            connection.ConnectionTimeout.Returns(expected);
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.ConnectionTimeout;

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Database_ShouldReturnValueOfUnderlyingProperty()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = RandomValueGen.GetRandomString();
            connection.ConnectionString.Returns(expected);
            var sut = Create(connection);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.ConnectionString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ConnectionState_ShouldReturnValueOfUnderlyingProperty()
        {
            //---------------Set up test pack-------------------
            var connection = Substitute.For<IDbConnection>();
            var expected = RandomValueGen.GetRandomEnum<ConnectionState>();
            connection.State.Returns(expected);
            var sut = Create(connection);
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.State;

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [TestCase("ConnectionTimeout")]
        [TestCase("Database")]
        [TestCase("State")]
        public void Property_ShouldBeReadOnly_(string propertyName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var type = typeof(ManagedConnection);
            var property = type.GetProperty(propertyName);
            var method = property.GetSetMethod();
            Assert.IsNull(method);

            //---------------Test Result -----------------------
        }


        private ManagedConnection Create(IDbConnection connection)
        {
            return new ManagedConnection(connection);
        }
    }
}
