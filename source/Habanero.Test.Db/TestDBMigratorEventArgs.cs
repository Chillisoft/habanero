using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using NSubstitute;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDBMigratorEventArgs
    {
        [TestCase(1, 1, 2, 50)]
        [TestCase(1, 1, 4, 25)]
        [TestCase(1, 2, 10, 20)]
        public void Constructor_SetsPercentageCompleteToCorrectValue(int start, int step, int total, decimal expectedPercentage)
        {
            //---------------Set up test pack-------------------
            var conn = Substitute.For<IDatabaseConnection>();
            var migrator = new DBMigrator(conn);
            
            //---------------Assert Precondition----------------
            Assert.IsNotNull(migrator);

            //---------------Execute Test ----------------------
            var e = new DBMigratorEventArgs((uint)start, (uint)step, (uint)total);
            var propVal = e.PercentageComplete;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedPercentage, propVal);
        }

        [Test]
        public void Constructor_SetsTotalStepsToSpecifiedValue()
        {
            //---------------Set up test pack-------------------
            var conn = Substitute.For<IDatabaseConnection>();
            var migrator = new DBMigrator(conn);
            var step = (uint)TestUtil.GetRandomInt(1, 50);
            var totalSteps = (uint)TestUtil.GetRandomInt((int)step, (int)step + 50);
            
            //---------------Assert Precondition----------------
            Assert.AreNotEqual(0, step);
            Assert.That(totalSteps, Is.GreaterThan(step));
            Assert.IsNotNull(migrator);

            //---------------Execute Test ----------------------
            var e = new DBMigratorEventArgs(1, step, totalSteps);
            var propVal = e.TotalSteps;
            //---------------Test Result -----------------------
            Assert.AreEqual(totalSteps, propVal);
        }

        [Test]
        public void Constructor_SetsCurrentStepToSpecifiedStep()
        {
            //---------------Set up test pack-------------------
            var conn = Substitute.For<IDatabaseConnection>();
            var migrator = new DBMigrator(conn);
            var step = (uint)TestUtil.GetRandomInt(1, 50);
            var totalSteps = (uint)TestUtil.GetRandomInt((int)step, (int)step + 50);
            
            //---------------Assert Precondition----------------
            Assert.AreNotEqual(0, step);
            Assert.IsNotNull(migrator);

            //---------------Execute Test ----------------------
            var e = new DBMigratorEventArgs(1, step, totalSteps);
            var propVal = e.CurrentStep;
            //---------------Test Result -----------------------
            Assert.AreEqual(step, propVal);
        }

        [Test]
        public void Constructor_SetsStartingStepToSpecifiedStep()
        {
            //---------------Set up test pack-------------------
            var conn = Substitute.For<IDatabaseConnection>();
            var migrator = new DBMigrator(conn);
            var startingStep = (uint)TestUtil.GetRandomInt(1, 50);
            var step = (uint)TestUtil.GetRandomInt((int)startingStep, (int)startingStep + 50);
            var totalSteps = (uint)TestUtil.GetRandomInt((int)step, (int)step + 50);
            
            //---------------Assert Precondition----------------
            Assert.AreNotEqual(0, step);
            Assert.IsNotNull(migrator);

            //---------------Execute Test ----------------------
            var e = new DBMigratorEventArgs(startingStep, step, totalSteps);
            var propVal = e.StartingStep;
            //---------------Test Result -----------------------
            Assert.AreEqual(startingStep, propVal);
        }

    }
}
