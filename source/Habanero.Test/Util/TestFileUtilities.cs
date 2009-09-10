// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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