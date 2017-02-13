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
