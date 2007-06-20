using System;
using System.Collections;
using System.IO;

namespace Habanero.Generic
{
    /// <summary>
    /// Loads document type definitions (dtd's)
    /// </summary>
    public class DtdLoader
    {
        private ITextFileLoader _textFileLoader;
        private readonly string _dtdPath;

        /// <summary>
        /// Constructor to initialise a new loader with a specified dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public DtdLoader(string dtdPath) : this(new TextFileLoader(), dtdPath)
        {
        }

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
        /// Loads a dtd from the file name provided
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>Returns a string containing the dtd</returns>
        public string LoadDtd(string fileName)
        {
            return LoadDtd(fileName, new ArrayList());
        }

        /// <summary>
        /// Loads a dtd from the file name provided
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="alreadyIncludedFiles">A list of files already
        /// included</param>
        /// <returns>Returns a string containing the dtd</returns>
        private string LoadDtd(string fileName, IList alreadyIncludedFiles)
        {
            string dtd = "";
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("The Document Type Definition " +
                    "(DTD) file, '" + fileName + "', was not found.  Please ensure " +
                    "that you have a DTD for each type of XML element you are " +
                    "using, and that these files are being copied to your application's " +
                    "output folder (eg. bin/debug).  Alternatively, check that " +
                    "the element name was spelt correctly and has the correct capitalisation.");
            }
            TextReader reader = _textFileLoader.LoadTextFile(fileName);
            string line;
            do
            {
                line = reader.ReadLine().Trim();
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