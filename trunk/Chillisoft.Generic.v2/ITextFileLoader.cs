using System.IO;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// An interface to model a text file loader
    /// </summary>
    public interface ITextFileLoader
    {
        /// <summary>
        /// Returns a TextReader object for the given file name
        /// </summary>
        /// <param name="fileName">The text file name</param>
        /// <returns>Returns a TextReader object</returns>
        TextReader LoadTextFile(string fileName);
    }
}