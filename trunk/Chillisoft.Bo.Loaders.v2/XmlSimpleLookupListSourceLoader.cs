using System;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads xml data for a lookup list
    /// </summary>
    public class XmlSimpleLookupListSourceLoader : XmlLookupListSourceLoader
    {
        private StringGuidPairCollection itsStringGuidPairCollection;

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
            itsStringGuidPairCollection = new StringGuidPairCollection();
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected override void LoadLookupListSourceFromReader()
        {
            itsReader.Read();
            while (itsReader.Name == "stringGuidPair")
            {
                itsStringGuidPairCollection.Add(
                    new StringGuidPair(itsReader.GetAttribute("string"), new Guid(itsReader.GetAttribute("guid"))));
                itsReader.Read();
            }
        }

        /// <summary>
        /// Creates a lookup list data source from the data already read in
        /// </summary>
        /// <returns>Returns a SimpleLookupListSource object</returns>
        protected override object Create()
        {
            return new SimpleLookupListSource(itsStringGuidPairCollection);
        }
    }
}