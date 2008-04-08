//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.IO;
using System.Xml;
using System.Xml.Schema;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.Loaders
{
	/// <summary>
	/// Provides a super-class for different types of xml loaders.
	/// A sub-class must implement LoadFromReader(), which loads the data
	/// from the reader, and Create(), which creates the object that the
	/// xml represents.
	/// </summary>
	/// (from before) Using Form Template Method (Refactoring p345) to make 
	/// sure all XmlLoaders implement dtd validation. 
	public abstract class XmlLoader
	{
		protected IDefClassFactory _defClassFactory;
		protected XmlReader  _reader;
		private bool _documentValid = true;
		private ValidationEventArgs _invalidDocumentArgs;
		private XmlElement _element;
	    private DtdLoader _dtdLoader;

	    /// <summary>
		/// Constructor to initialise a new loader with a dtd path
		/// </summary>
		/// <param name="dtdLoader">The loader for the dtds (pass through a DtdLoader object using the standard constructor for the default behaviour)</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
		public XmlLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
		{
			_dtdLoader = dtdLoader;
			if (defClassFactory != null)
			{
				_defClassFactory = defClassFactory;
			} else
			{
				_defClassFactory = new DefClassFactory();
			}
		}

		/// <summary>
		/// Constructor to initialise a new loader
		/// </summary>
		public XmlLoader() : this(new DtdLoader(), null)
		{
		}

		/// <summary>
		/// Loads the specified xml element
		/// </summary>
		/// <param name="el">The xml element</param>
		/// <returns>Returns the loaded object</returns>
		protected object Load(XmlElement el)
		{
			_element = el;
			CreateValidatingReader(el);
			LoadFromReader();
			CheckDocumentValidity();
			return Create();
		}

		/// <summary>
		/// Creates the object using the data that has been read in using
		/// LoadFromReader(). This method needs to be implemented by the
		/// sub-class.
		/// </summary>
		/// <returns>Returns the object created</returns>
		protected abstract object Create();

		/// <summary>
		/// Loads all the data out of the reader, assuming the document is 
		/// well-formed, otherwise the error must be caught and thrown.
		/// By the end of this method the reader must be finished reading.
		/// This method needs to be implemented by the sub-class.
		/// </summary>
		protected abstract void LoadFromReader();

		/// <summary>
		/// Checks that the xml document is valid and well-formed
		/// </summary>
		/// <exception cref="InvalidXmlDefinitionException">Thrown if the
		/// xml document is not valid</exception>
		private void CheckDocumentValidity()
		{
			if (!_documentValid)
			{
				throw new InvalidXmlDefinitionException("The '" + _element.Name + "' " +
				                                        "node does not conform to its Document Type Definition (DTD). " +
				                                        _invalidDocumentArgs.Message);
			}
		}

		/// <summary>
		/// Creates a reader to read and validate the xml data for the
		/// property element specified
		/// </summary>
		/// <param name="propertyElement">The xml property element</param>
		private void CreateValidatingReader(XmlElement propertyElement)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(propertyElement.OuterXml);
			doc.InsertBefore(
				doc.CreateDocumentType(doc.DocumentElement.Name, null, null, GetDTD(doc.DocumentElement.Name)),
				doc.DocumentElement);
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.CheckCharacters = true;
			settings.ConformanceLevel = ConformanceLevel.Auto;
			settings.IgnoreComments = true;
			settings.IgnoreWhitespace = true;
			settings.ValidationType = ValidationType.DTD;
			settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
			_reader = XmlReader.Create(new XmlTextReader(new StringReader(doc.OuterXml)), settings);

            
			_reader.Read();
		}

		/// <summary>
		/// Returns the dtd for the root element name provided
		/// </summary>
		/// <param name="rootElementName">The root element name</param>
		/// <returns>Returns a string</returns>
		private string GetDTD(string rootElementName)
		{
		    return _dtdLoader.LoadDtd(rootElementName);

		}

		/// <summary>
		/// Handles the event of an xml document being invalid
		/// </summary>
		/// <param name="sender">The object that notified of the event</param>
		/// <param name="e">Attached arguments regarding the event</param>
		private void ValidationHandler(object sender, ValidationEventArgs args)
		{
			_documentValid = false;
			_invalidDocumentArgs = args;
		}

		/// <summary>
		/// Creates and returns an xml element for the element name provided
		/// </summary>
		/// <param name="element">The element</param>
		/// <returns>Returns an XmlElement object</returns>
		protected XmlElement CreateXmlElement(string element)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(element);
			return doc.DocumentElement;
		}

		/// <summary>
		/// Carries out the next read, but makes provision to ignore the end
		/// tag if there is one (and not treat it as an element itself)
		/// </summary>
		protected void ReadAndIgnoreEndTag()
		{
			if (_reader.IsEmptyElement)
				_reader.Read();
			else
			{
				_reader.Read();
				_reader.Read();
			}
		}

        /// <summary>
        /// Retrieves the dtd loader this XmlLoader was initialised with.
        /// </summary>
	    protected DtdLoader DtdLoader {
	         get {
	             return _dtdLoader;
	         }
	    }
	}
}