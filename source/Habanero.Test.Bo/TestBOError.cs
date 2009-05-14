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
            string message = "message";
            ErrorLevel errorLevel = ErrorLevel.Error;
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
