using System;
using System.IO;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestFileUtilities
    {
        private string _testFolderName;

        public string GetTestPath(string folderName)
        {
            return Path.Combine(_testFolderName, folderName);
        }

        [SetUp]
        public void Setup()
        {
            _testFolderName = Path.Combine(Environment.CurrentDirectory, "TestFolder");
            if (!Directory.Exists(_testFolderName)) Directory.CreateDirectory(_testFolderName);
        }

        [TearDown]
        public void TearDownTest()
        {
            if (Directory.Exists(_testFolderName)) Directory.Delete(_testFolderName, true);
        }

        [Test]
        public void Test_CreateDirectory_DoesNotExist()
        {
            //---------------Set up test pack-------------------
            string path = GetTestPath("NonExistent");
            if (Directory.Exists(path)) Directory.Delete(path, true);
            //---------------Assert Precondition----------------
            AssertDirectoryDoesNotExist(path);
            //---------------Execute Test ----------------------
            FileUtilities.CreateDirectory(path);
            //---------------Test Result -----------------------
            AssertDirectoryExists(path);
        }

        [Test]
        public void Test_CreateDirectory_AlreadyExists()
        {
            //---------------Set up test pack-------------------
            string path = GetTestPath("Existing");
            if (Directory.Exists(path)) Directory.Delete(path, true);
            FileUtilities.CreateDirectory(path);
            //---------------Assert Precondition----------------
            AssertDirectoryExists(path);
            //---------------Execute Test ----------------------
            FileUtilities.CreateDirectory(path);
            //---------------Test Result -----------------------
            AssertDirectoryExists(path);
        }

        [Test]
        public void Test_CreateDirectory_AlreadyExists_WithFiles()
        {
            //---------------Set up test pack-------------------
            string path = GetTestPath("Existing");
            if (Directory.Exists(path)) Directory.Delete(path, true);
            FileUtilities.CreateDirectory(path);
            string filePath = Path.Combine(path, "TestFile.txt");
            System.IO.File.AppendAllText(filePath, "File Contents");
            //---------------Assert Precondition----------------
            AssertDirectoryExists(path);
            AssertFileExists(filePath);
            //---------------Execute Test ----------------------
            FileUtilities.CreateDirectory(path);
            //---------------Test Result -----------------------
            AssertDirectoryExists(path);
            AssertFileExists(filePath);
        }

        private static void AssertDirectoryDoesNotExist(string fullFileName)
        {
            Assert.IsFalse(Directory.Exists(fullFileName));
        }
        private static void AssertDirectoryExists(string fullFileName)
        {
            Assert.IsTrue(Directory.Exists(fullFileName));
        }
        private static void AssertFileExists(string fullFileName)
        {
            Assert.IsTrue(System.IO.File.Exists(fullFileName), "The file : " + fullFileName + " should have been created");
        }
        private static void AssertFileDoesNotExist(string fullFileName)
        {
            Assert.IsFalse(System.IO.File.Exists(fullFileName), "The file : " + fullFileName + " should not exist");
        }
    }
}