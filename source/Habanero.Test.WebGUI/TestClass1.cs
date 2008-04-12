using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Habanero.Test.WebGUI
{
    [TestFixture]
    public class TestClass1 : TestBase
    {
        [SetUp]
        public void TestSetup()
        {
            //Code that is run before every single test
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }
        [Test]
        public void TestMethod1()
        {
        }
    }
}
