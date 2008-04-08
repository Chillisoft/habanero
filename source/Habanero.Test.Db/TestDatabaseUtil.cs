//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    /// <summary>
    /// Summary description for TestDatabaseUtil.
    /// </summary>
    [TestFixture]
    public class TestDatabaseUtil
    {
        public TestDatabaseUtil()
        {
        }

        [Test]
        public void TestFormatDatabaseDateTime()
        {
            DateTime dte = new DateTime(2004, 12, 15, 5, 10, 20);
            Assert.AreEqual("2004-12-15 05:10:20", DatabaseUtil.FormatDatabaseDateTime(dte));
        }
    }
}