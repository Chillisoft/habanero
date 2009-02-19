using System;
using System.Collections.Specialized;
using System.IO;
using Habanero.Base.Exceptions;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestFileUtilities
    {
        [Test]
        public void Test_CreateDirectory_IfDoesNotExist()
        {
            //---------------Set up test pack-------------------
            const string path = @"C:\NonExistent\";
            if (Directory.Exists(path)) Directory.Delete(path);
            //---------------Assert Precondition----------------
            AssertDirectoryDoesNotExist(path);
            //---------------Execute Test ----------------------
            FileUtilities.CreateDirectory(path);
            //---------------Test Result -----------------------
            AssertDirectoryExists(path);
        }
        [Test]
        public void Test_CreateDirectory_IfAlreadyExists()
        {
            //---------------Set up test pack-------------------
            const string path = @"C:\NonExistent\";
            if (Directory.Exists(path)) Directory.Delete(path);
            FileUtilities.CreateDirectory(path);
            //---------------Assert Precondition----------------
            AssertDirectoryExists(path);
            //---------------Execute Test ----------------------
            FileUtilities.CreateDirectory(path);
            //---------------Test Result -----------------------
            AssertDirectoryExists(path);
        }
        private static void AssertDirectoryDoesNotExist(string fullFileName)
        {
            Assert.IsFalse(Directory.Exists(Path.GetDirectoryName(fullFileName)));
        }
        private static void AssertDirectoryExists(string fullFileName)
        {
            Assert.IsTrue(Directory.Exists(Path.GetDirectoryName(fullFileName)));
        }
        private static void AssertFileHasBeenCreated(string fullFileName)
        {
            Assert.IsTrue(System.IO.File.Exists(fullFileName), "The file : " + fullFileName + " should have been created");
        }
        private static void AssertFileDoesNotExist(string fullFileName)
        {
            Assert.IsFalse(System.IO.File.Exists(fullFileName), "The file : " + fullFileName + " should not exist");
        }
    }
}