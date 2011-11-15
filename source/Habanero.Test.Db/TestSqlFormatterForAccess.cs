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
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestSqlFormatterForAccess
    {
        [Test]
        public void Test_PrepareValue_WithBool_WhenTrue_ShouldReturnNeg1()
        {
            //---------------Set up test pack-------------------
            SqlFormatterForAccess sqlFormatter = new SqlFormatterForAccess("", "", "", "");
            const bool value = true;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object preparedValue = sqlFormatter.PrepareValue(value);
            //---------------Test Result -----------------------
            Assert.AreEqual(-1, preparedValue, "PrepareValue is not preparing bools correctly for Access.");
        }

        [Test]
        public void Test_PrepareValue_WithBool_WhenFalse_ShouldreturnZero()
        {
            //---------------Set up test pack-------------------
            SqlFormatterForAccess sqlFormatter = new SqlFormatterForAccess("", "", "", "");
            const bool value = false;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object preparedValue = sqlFormatter.PrepareValue(value);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, preparedValue, "PrepareValue is not preparing bools correctly for Access.");
        }
    }
}