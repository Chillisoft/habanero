using System.IO;

namespace Habanero.Base
{
    /// <summary>
    /// Provides a StreamReader to load a text file
    /// </summary>
    public class TextFileLoader : ITextFileLoader
    {
        /// <summary>
        /// Returns a StreamReader object to load the specified file name
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>Returns a StreamReader object</returns>
        public TextReader LoadTextFile(string fileName)
        {
            return new StreamReader(fileName);
        }
    }
}