// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Text;
using Habanero.Base.Util;

namespace Habanero.Util
{
    //TODO andrew 21 Dec 2010: CF : Change to use StringUtilitiesCE

    /// <summary>
    /// Provides a collection of utilities for strings
    /// </summary>
    public class FileUtilities
    {

        /// <summary>
        /// Searches for the last occurrence of a newline and indicates whether the
        /// text after this occurrence is only whitespace characters, tabs or an empty string.
        /// Returns false if there is no newline.
        /// </summary>
        public static bool StringHasOnlyWhitespaceSinceLastNewline(string result)
        {
            if (StringUtilitiesCE.Contains(StringUtilitiesCE.NewLine, result))
            {
                do // keeps eliminating everything left of and including the next newline until there are no newlines left
                {
                    result = StringUtilities.GetRightSection(result, StringUtilitiesCE.NewLine);
                } while (StringUtilitiesCE.Contains(StringUtilitiesCE.NewLine, result));
                result = result.Replace(" ", "");
                result = result.Replace("\t", "");
                return result.Length == 0;
            }
            return false;
        }

        ///<summary>
        /// Returns the relative path for a an absolute path and a partial path.
        ///</summary>
        ///<param name="absolutePath">The Full Path e.g C:\Systems\Habanero\Source\SomeDirectory\</param>
        ///<param name="relativeTo">The Path that you wan the relative path to e.g. C:\Systems\Habanero\</param>
        ///<returns>The Relative path e.g. in this case \Source\SomeDirectory\</returns>
        public static string GetRelativePath(string absolutePath, string relativeTo)
        {
            string rootPath = relativeTo;
            string directorySeperator = Path.DirectorySeparatorChar.ToString();
            if (!rootPath.EndsWith(directorySeperator))
            {
                rootPath = rootPath + directorySeperator;
            }
            //rootPath = GetRootedPath(basePath, rootPath);
            Uri outputDir = new Uri(rootPath);
            Uri fileDir = new Uri(absolutePath);
            return outputDir.MakeRelativeUri(fileDir).ToString().Replace("/", "\\");
        }

        /// <summary>
        /// Create a file in the specified location with the specified contents
        /// </summary>
        /// <param name="filePath">The full file path to be created including the file name e.g. C:\Systems\SomeFile.txt</param>
        /// <param name="fileContents">The contents to be created in the file</param>
        /// <returns>Returns the newly created full file name</returns>
        public static string CreateFile(string filePath, string fileContents)
        {
            return CreateFile(Path.GetDirectoryName(filePath), Path.GetFileName(filePath), fileContents, null);
        }
        /// <summary>
        /// Create a file in the specified location with the specified contents
        /// </summary>
        /// <param name="filePath">The full file path to be created e.g. C:\Systems\SomeFile.txt</param>
        /// <param name="fileContents">The contents to be created in the file</param>
        /// <param name="overwrite">Overrite an existing file or not</param>
        /// <returns>Returns the newly created full file name</returns>
        public static string CreateFile(string filePath, string fileContents, bool overwrite)
        {
            return CreateFile(Path.GetDirectoryName(filePath), Path.GetFileName(filePath), fileContents, overwrite);
        }

        /// <summary>
        /// Create a file in the specified location with the specified contents
        /// </summary>
        ///<param name="folderPath">The full folder path that the file must be created in e.g. C:\Systems\</param>
        ///<param name="fileName">The File name to be created e.g. SomeFile.txt</param>
        /// <param name="fileContents">The contents to be created in the file</param>
        ///<returns>Returns the newly created full file name</returns>
        public static string CreateFile(string folderPath, string fileName, string fileContents)
        {
            return CreateFile(folderPath, fileName, fileContents, null);
        }

        /// <summary>
        /// Create a file in the specified location with the specified contents
        /// </summary>
        ///<param name="folderPath">The full folder path that the file must be created in e.g. C:\Systems\</param>
        ///<param name="fileName">The File name to be created e.g. SomeFile.txt</param>
        /// <param name="fileContents">The contents to be created in the file</param>
        ///<param name="encoding">The encoding to be used when writing this file</param>
        ///<returns>Returns the newly created full file name</returns>
        public static string CreateFile(string folderPath, string fileName, string fileContents, Encoding encoding)
        {
            return CreateFile(folderPath, fileName, fileContents, false, encoding);
        }

        /// <summary>
        /// Create a file in the specified location with the specified contents
        /// </summary>
        ///<param name="folderPath">The full folder path that the file must be created in e.g. C:\Systems\</param>
        ///<param name="fileName">The File name to be created e.g. SomeFile.txt</param>
        /// <param name="fileContents">The contents to be created in the file</param>
        ///<param name="overwrite">Whether to overwrite the file or not</param>
        ///<returns>Returns the newly created full file name</returns>
        public static string CreateFile(string folderPath, string fileName, string fileContents, bool overwrite)
        {
            return CreateFile(folderPath, fileName, fileContents, overwrite, null);
        }

        /// <summary>
        /// Create a file in the specified location with the specified contents
        /// </summary>
        ///<param name="folderPath">The full folder path that the file must be created in e.g. C:\Systems\</param>
        ///<param name="fileName">The File name to be created e.g. SomeFile.txt</param>
        /// <param name="fileContents">The contents to be created in the file</param>
        ///<param name="overwrite">Whether to overwrite the file or not</param>
        ///<param name="encoding">Encoding to be used</param>
        ///<returns>Returns the newly created full file name</returns>
        public static string CreateFile(string folderPath, string fileName, string fileContents, bool overwrite, Encoding encoding)
        {
            string destFile = Path.Combine(folderPath, fileName);
            string path = Path.GetDirectoryName(destFile);
            if (!File.Exists(destFile) || overwrite)
            {
                CreateDirectory(path);
                if (encoding != null)
                {
                    File.WriteAllText(destFile, fileContents, encoding);
                }
                else
                {
                    File.WriteAllText(destFile, fileContents);
                }
            }
            return destFile;
        }

        ///<summary>
        /// If the Directory Does note exists then it creates one else does nothing
        ///</summary>
        ///<param name="path">The path of the Directory</param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
    }
}