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
using System.IO;
using NUnit.Framework;
using Habanero.Base;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestProgressIndicators
    {
        private StringWriter standardOut;

        [SetUp]
        public void InitializeStringWriter()
        {
//            standardOut = new StreamWriter(Console.OpenStandardOutput());
//            standardOut.AutoFlush = true;
//            Console.SetOut(standardOut);

            standardOut = new StringWriter();
            Console.SetOut(standardOut);
        }

        [TearDown]
        public void CloseStringWriter()
        {
            //Closing this causes a TextWriter to fail elsewhere
            //standardOut.Close();
        }

        [Test]
        public void TestConsoleProgressIndicatorUpdate()
        {
            ConsoleProgressIndicator cp = new ConsoleProgressIndicator();
            cp.UpdateProgress(50,100,"description");
            string expected = String.Format("50 of 100 steps complete. description{0}", Environment.NewLine);
            Assert.AreEqual(expected, standardOut.ToString());
        }

        [Test]
        public void TestConsoleProgressIndicatorComplete()
        {
            ConsoleProgressIndicator cp = new ConsoleProgressIndicator();
            cp.Complete();
            string expected = String.Format("Complete.{0}", Environment.NewLine);
            Assert.AreEqual(expected, standardOut.ToString());
        }

        [Test]
        public void TestNullProgressIndicatorUpdate()
        {
            NullProgressIndicator np = new NullProgressIndicator();
            np.UpdateProgress(50, 100, "description");
            Assert.AreEqual("", standardOut.ToString());
        }

        [Test]
        public void TestNullProgressIndicatorComplete()
        {
            NullProgressIndicator np = new NullProgressIndicator();
            np.Complete();
            Assert.AreEqual("", standardOut.ToString());
        }
    }
}
