using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOError
    {
        [Test]
        public void Test_Constructor()
        {
            //--------------- Set up test pack ------------------
            const string message = "message";
            const ErrorLevel errorLevel = ErrorLevel.Error;
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            IBusinessObject bo = new MyBO();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            BOError boError = new BOError(message, errorLevel);
            //--------------- Test Result -----------------------

            Assert.AreEqual(message, boError.Message);
            Assert.AreEqual(errorLevel, boError.Level);

        }
    }
}
