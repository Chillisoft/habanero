using System;
using System.Collections.Generic;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads xml data for a lookup list
    /// </summary>
    public class XmlSimpleLookupListLoader : XmlLookupListSourceLoader
    {
        private Dictionary<string, object> _displayValueDictionary;

        ///// <summary>
        ///// Constructor to initialise a loader
        ///// </summary>
        //public XmlSimpleLookupListLoader() : this("", null)
        //{
        //}

        /// <summary>
        /// Constructor to initialise a loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlSimpleLookupListLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
            _displayValueDictionary = new Dictionary<string, object>();
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected override void LoadLookupListSourceFromReader()
        {
            string options = _reader.GetAttribute("options");
            if (options != null && options.Length > 0) {
                string[] optionsArr = options.Split(new char[] {'|'});
                foreach (string s in optionsArr) {
                    _displayValueDictionary.Add(s, s);
                }
            }
            _reader.Read();
            while (_reader.Name == "item")
            {
                string stringPart = _reader.GetAttribute("display");
                string valuePart = _reader.GetAttribute("value");
                if (stringPart == null || stringPart.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("An 'item' " +
                        "is missing a 'display' attribute that specifies the " +
                        "string to show to the user in a display.");
                }
                if (valuePart == null || valuePart.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("An 'item' " +
                        "is missing a 'value' attribute that specifies the " +
                        "value to store for the given property.");
                }

                    try {
                        Guid newGuid = new Guid(valuePart);
                        _displayValueDictionary.Add(stringPart, newGuid );
                    } catch (FormatException) {
                        _displayValueDictionary.Add(stringPart, valuePart);
                    }
                
                ReadAndIgnoreEndTag();
            }

            if (_displayValueDictionary.Count == 0)
            {
                throw new InvalidXmlDefinitionException("A 'simpleLookupList' " +
                    "element does not contain any 'item' elements or any items in the 'options' attribute.  It " +
                    "should contain one or more 'item' elements or one or more | separated options in the 'options' attribute that " +
                    "specify each of the available options in the lookup list.");
            }
        }

        /// <summary>
        /// Creates a lookup list data source from the data already read in
        /// </summary>
        /// <returns>Returns a SimpleLookupListSource object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateSimpleLookupListSource(_displayValueDictionary);
			//return new SimpleLookupListSource(_displayValueDictionary);
        }
    }
}