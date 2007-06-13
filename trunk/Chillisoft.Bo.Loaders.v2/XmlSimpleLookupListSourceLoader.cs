using System;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads xml data for a lookup list
    /// </summary>
    public class XmlSimpleLookupListSourceLoader : XmlLookupListSourceLoader
    {
        private StringGuidPairCollection _stringGuidPairCollection;

        /// <summary>
        /// Constructor to initialise a loader
        /// </summary>
        public XmlSimpleLookupListSourceLoader() : this("")
        {
        }

        /// <summary>
        /// Constructor to initialise a loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlSimpleLookupListSourceLoader(string dtdPath) : base(dtdPath)
        {
            _stringGuidPairCollection = new StringGuidPairCollection();
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected override void LoadLookupListSourceFromReader()
        {
            _reader.Read();
            while (_reader.Name == "stringGuidPair")
            {
                _stringGuidPairCollection.Add(
                    new StringGuidPair(_reader.GetAttribute("string"), new Guid(_reader.GetAttribute("guid"))));
                _reader.Read();
            }
        }

        /// <summary>
        /// Creates a lookup list data source from the data already read in
        /// </summary>
        /// <returns>Returns a SimpleLookupListSource object</returns>
        protected override object Create()
        {
            return new SimpleLookupListSource(_stringGuidPairCollection);
        }
    }
}