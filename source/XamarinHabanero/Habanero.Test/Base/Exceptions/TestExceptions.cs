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
using Habanero.Base.Exceptions;
using NUnit.Framework;

namespace Habanero.Test.Base.Exceptions
{
    /// <summary>
    /// This is an extensible exceptions tester.
    /// If your exception is standard, with four overloaded constructors:
    /// (), (message), (message, inner), (ser,stream), then you only
    /// need to add the new exception to the appropriate list in the setup.
    /// If the assembly is new, add the assembly to the namespaces array
    /// and copy the structure of the other namespaces in the setup method.
    /// 
    /// If your exception has extra constructors or special formatting of
    /// the message, then you'll need to add some extra test methods for
    /// your exception.
    /// </summary>
    [TestFixture]
    public class TestExceptions
    {
        private static readonly object[] ExceptionTestCaseSource =
        {
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.HabaneroApplicationException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.InvalidObjectIdException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.InvalidXmlDefinitionException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.UserException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.CannotSaveException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.UnknownTypeNameException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.InvalidPropertyException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.InvalidDefinitionException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.RecordedExceptionsException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.InvalidPropertyNameException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.InvalidRelationshipAccessException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.ReflectionException"},
            new [] {"Habanero.Base", "Habanero.Base.Exceptions.InvalidKeyException"},
                
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.InvalidPropertyValueException"},
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.RelationshipNotFoundException"},
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.BusinessObjectNotFoundException"},
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.BusObjectInAnInvalidStateException"},
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.BusObjectConcurrencyControlException"}, 
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.BusObjOptimisticConcurrencyControlException"}, 
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.BusObjPessimisticConcurrencyControlException"}, 
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.BusObjDeleteConcurrencyControlException"}, 
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.BusObjBeginEditConcurrencyControlException"}, 
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.EditingException"}, 
            new [] {"Habanero.BO", "Habanero.BO.Exceptions.BusObjDuplicateConcurrencyControlException"},
 
            new [] {"Habanero.DB", "Habanero.DB.DatabaseConnectionException"},
            new [] {"Habanero.DB", "Habanero.DB.DatabaseReadException"},
            new [] {"Habanero.DB", "Habanero.DB.DatabaseWriteException"},
            new [] {"Habanero.DB", "Habanero.DB.SqlStatementException"},
        };

        [Test]
        public void Test_Construct_HabaneroApplicationException_WithDeveloperMessage_ShouldHaveDevMessage()
        {
            //---------------Set up test pack-------------------
            const string expectedDevMessage = "DeveloperMessage";
            const string expectedUserMessage = "IserMessage";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var habaneroApplicationException = new HabaneroApplicationException(expectedUserMessage, expectedDevMessage);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedUserMessage, habaneroApplicationException.Message);
            Assert.AreEqual(expectedDevMessage, habaneroApplicationException.DeveloperMessage);
        }

        [Test]
        public void Test_Construct_HabaneroApplicationException_WithDeveloperMessage_WithInnerException_ShouldHaveDevMessage()
        {
            //---------------Set up test pack-------------------
            const string expectedDevMessage = "DeveloperMessage";
            const string expectedUserMessage = "IserMessage";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var habaneroApplicationException = new HabaneroApplicationException(expectedUserMessage, expectedDevMessage, new Exception());
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedUserMessage, habaneroApplicationException.Message);
            Assert.AreEqual(expectedDevMessage, habaneroApplicationException.DeveloperMessage);
        }

        [TestCaseSource("ExceptionTestCaseSource")]
        public void Construct_WithParameterlessConstructor(string assembly, string className)
        {
            //---------------Set up test pack-------------------
            var expectedMessage = String.Format("Exception of type '{0}' was thrown.", className);
            //---------------Execute Test ----------------------
            var ex = (Exception)Activator.CreateInstance(GetType(assembly, className), new object[] { });
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [TestCaseSource("ExceptionTestCaseSource")]
        public void Construct_WithMessageConstructor(string assembly, string className)
        {
            //---------------Set up test pack-------------------
            const string expectedMessage = "special message";
            //---------------Execute Test ----------------------
            var ex = (Exception)Activator.CreateInstance(GetType(assembly, className), new object[] { expectedMessage });
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [TestCaseSource("ExceptionTestCaseSource")]
        public void Construct_WithMessageAndInnerExceptionConstructor(string assembly, string className)
        {
            //---------------Set up test pack-------------------
            var expectedInnerException = new Exception();
            const string expectedMessage = "special message";
            //---------------Execute Test ----------------------
            var ex = (Exception)Activator.CreateInstance(GetType(assembly, className), new object[] { expectedMessage, expectedInnerException });
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedMessage, ex.Message);
            Assert.AreSame(expectedInnerException, ex.InnerException);
        }

        private Type GetType(string assembly, string className)
        {
            var exType = Type.GetType(className + ", " + assembly);
            if (exType == null)
            {
                throw new TypeLoadException("Could not find: " + className + ", " + assembly);
            }
            return exType;
        }

        [Test]
        public void TestHabaneroArgumentException()
        {
            var inner = new UserException();

            var hae = new HabaneroArgumentException("param");
            Assert.AreEqual("The argument 'param' is not valid. ", hae.Message);
            hae = new HabaneroArgumentException("param", "message");
            Assert.AreEqual("The argument 'param' is not valid. message", hae.Message);
            hae = new HabaneroArgumentException("param", "message", inner);
            Assert.AreEqual("The argument 'param' is not valid. message", hae.Message);
            Assert.AreEqual(inner, hae.InnerException);
            hae = new HabaneroArgumentException("param", inner);
            Assert.AreEqual("The argument 'param' is not valid. ", hae.Message);
            Assert.AreEqual(inner, hae.InnerException);
            hae = new HabaneroArgumentException();
            Assert.AreEqual("Exception of type 'Habanero.Base.Exceptions.HabaneroArgumentException' was thrown.", hae.Message);
        }
    }
}