using System;
using System.IO;
using NUnit.Framework;
using Habanero.Base;

namespace Habanero.Test
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
