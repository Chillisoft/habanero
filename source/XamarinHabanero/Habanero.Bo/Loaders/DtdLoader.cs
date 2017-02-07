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
using System;
using System.Collections;
using System.IO;
using System.Resources;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads document type definitions (dtd's)
    /// </summary>
    public class DtdLoader
    {
        private readonly ITextFileLoader _textFileLoader;
        private readonly string _dtdPath;
        private readonly ResourceManager _resourceManager;

        ///<summary>
        /// Constructs the DataTypeLoader.
        ///</summary>
        public DtdLoader() {
            _resourceManager = new ResourceManager("Habanero.BO.Loaders.Dtds", typeof(DtdLoader).Assembly);
        }

        ///// <summary>
        ///// Constructor to initialise a new loader with a specified dtd path
        ///// </summary>
        ///// <param name="dtdPath">The dtd path</param>
        //public DtdLoader(string dtdPath) : this(new TextFileLoader(), dtdPath)
        //{
        //}

        /// <summary>
        /// Constructor to initialise a new loader with a specific text file 
        /// loader and a dtd path
        /// </summary>
        /// <param name="textFileLoader">The text file loader</param>
        /// <param name="dtdPath">The dtd path</param>
        public DtdLoader(ITextFileLoader textFileLoader, string dtdPath)
        {
            this._textFileLoader = textFileLoader;
            _dtdPath = dtdPath;
        }

        /// <summary>
        /// Loads a dtd with the name provided.  If the dtd loader is loading from files it will append .dtd to the end of the name provided and
        /// attempt to open that file.
        /// </summary>
        /// <param name="dtdName">The dtd name</param>
        /// <returns>Returns a string containing the dtd</returns>
        public string LoadDtd(string dtdName)
        {
            if (_resourceManager != null)
            {
                return LoadDtd(dtdName, new ArrayList());
            }
            string dtdFileName = _dtdPath + dtdName + ".dtd";
            if (!File.Exists(dtdFileName))
            {
                string errorMessage = "The Document Type Definition (DTD) for " +
                                      "the XML element '" + dtdName + "' was not found in the ";
                if (string.IsNullOrEmpty(_dtdPath))
                {
                    errorMessage += "application's output/execution directory (eg. bin/debug). ";
                }
                else
                {
                    errorMessage += "path: '" + _dtdPath + "'. ";
                }
                errorMessage += "Ensure that you have a .DTD file for each of the XML class " +
                                "definition elements you will be using, and that they are being copied to the " +
                                "application's output directory (eg. bin/debug).  Alternatively, check that " +
                                "the element name was spelt correctly and has the correct capitalisation.";
                throw new FileNotFoundException(errorMessage);
            }
            return LoadDtd(dtdFileName, new ArrayList());
        }

        /// <summary>
        /// Loads a dtd from the file name provided
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="alreadyIncludedFiles">A list of files already
        /// included</param>
        /// <returns>Returns a string containing the dtd</returns>
        internal string LoadDtd(string fileName, IList alreadyIncludedFiles)
        {
            TextReader reader;
            string dtd = "";
            if (_resourceManager == null)
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException("The Document Type Definition " +
                                                    "(DTD) file, '" + fileName + "', was not found.  Please ensure " +
                                                    "that you have a DTD for each type of XML element you are " +
                                                    "using, and that these files are being copied to your application's " +
                                                    "output folder (eg. bin/debug).  Alternatively, check that " +
                                                    "the element name was spelt correctly and has the correct capitalisation.");
                }
                reader = _textFileLoader.LoadTextFile(fileName);
            }
            else
            {
                string dtdName = fileName;
                if (fileName.EndsWith(".dtd"))
                {
                    dtdName = fileName.Substring(0, fileName.Length - 4);
                }
                object o = _resourceManager.GetObject(dtdName);
                if (o == null) o = _resourceManager.GetObject("_" + dtdName);
                if (o == null) o = _resourceManager.GetObject(dtdName.Substring(0, 1).ToUpper() +
                                                   dtdName.Substring(1, dtdName.Length - 1));
                if (o == null) o = _resourceManager.GetObject(dtdName.Substring(0, 2).ToUpper() +
                                                   dtdName.Substring(2, dtdName.Length - 2));
                if (o == null)
                {
                    throw new InvalidXmlDefinitionException("An invalid node '" + dtdName +
                                                            "' was encountered when loading the class definitions.");
                }
                reader = new StringReader((string) o);
            }
            do
            {
                string line = reader.ReadLine().Trim();
                if (line.StartsWith("#include"))
                {
                    string fileToInclude = line.Substring(9);
                    if (!alreadyIncludedFiles.Contains(fileToInclude))
                    {
                        alreadyIncludedFiles.Add(fileToInclude);
                        dtd += LoadDtd(_dtdPath + fileToInclude, alreadyIncludedFiles);
                    }
                }
                else
                {
                    dtd += line;
                    dtd += Environment.NewLine;
                }
            } while (reader.Peek() != -1);
            return dtd;
        }
    }
}