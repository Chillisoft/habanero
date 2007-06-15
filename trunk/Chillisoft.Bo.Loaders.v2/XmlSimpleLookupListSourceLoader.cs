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
                string stringPart = _reader.GetAttribute("string");
                string guidPart = _reader.GetAttribute("guid");
                if (stringPart == null || stringPart.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("A 'stringGuidPair' " +
                        "is missing a 'string' attribute that specifies the " +
                        "string to show to the user in a display.");
                }
                if (guidPart == null || guidPart.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("A 'stringGuidPair' " +
                        "is missing a 'guid' attribute that specifies the " +
                        "guid to store for the given property.");
                }

                try
                {
                    Guid newGuid = new Guid(guidPart);
                    _stringGuidPairCollection.Add(
                        new StringGuidPair(stringPart, newGuid));
                }
                catch (Exception ex)
                {
                    throw new InvalidXmlDefinitionException(String.Format(
                        "In a 'stringGuidPair', a 'guid' attribute provides '{0}', " +
                        "which is not a valid Guid.", guidPart), ex);
                }
                
                ReadAndIgnoreEndTag();
            }

            if (_stringGuidPairCollection.Count == 0)
            {
                throw new InvalidXmlDefinitionException("A 'simpleLookupListSource' " +
                    "element does not contain any 'stringGuidPair' elements.  It " +
                    "should contain one or more 'stringGuidPair' elements that " +
                    "specify each of the available options in the lookup list.");
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