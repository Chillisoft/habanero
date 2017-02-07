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
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// A super-class for xml loaders that load lookup list data
    /// </summary>
    public abstract class XmlLookupListLoader : XmlLoader
    {
        /// <summary>
        /// The name of the rule
        /// </summary>
        protected string _ruleName;
        /// <summary>
        /// whether this is compulsory or not
        /// </summary>
        protected bool _isCompulsory;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        protected XmlLookupListLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads a lookup list using the specified source element
        /// </summary>
        /// <param name="sourceElement">The source element as a string</param>
        /// <returns>Returns an ILookupList object</returns>
        public ILookupList LoadLookupList(string sourceElement)
        {
            return this.LoadLookupList(this.CreateXmlElement(sourceElement));
        }

        /// <summary>
        /// Loads a lookup list using the specified source element
        /// </summary>
        /// <param name="sourceElement">The source element as an XmlElement
        /// object</param>
        /// <returns>Returns an ILookupList object</returns>
        public ILookupList LoadLookupList(XmlElement sourceElement)
        {
            return (ILookupList) this.Load(sourceElement);
        }

        /// <summary>
        /// Loads the data from the reader
        /// </summary>
        protected override sealed void LoadFromReader()
        {
            _reader.Read();
            LoadLookupListFromReader();
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected abstract void LoadLookupListFromReader();

        /// <summary>
        /// Loads the lookup list data into the specified property definition
        /// </summary>
        /// <param name="sourceElement">The source element</param>
        /// <param name="def">The property definition to load into</param>
        /// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public static void LoadLookupListIntoProperty(string sourceElement, IPropDef def, DtdLoader dtdLoader, IDefClassFactory defClassFactory)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sourceElement);
            if (doc.DocumentElement == null)
            {
                throw new HabaneroDeveloperException
                    ("There was a problem loading the class definitions pleaser refer to the system administrator",
                     "The load lookup list property could not be loaded since the source element does not contain a document name");
            }
            string loaderClassName = "Xml" + doc.DocumentElement.Name + "Loader";
            Type loaderType = Type.GetType
                (typeof (XmlLookupListLoader).Namespace + "." + loaderClassName, true, true);
            XmlLookupListLoader loader =
                (XmlLookupListLoader)
                Activator.CreateInstance(loaderType, new object[] {dtdLoader, defClassFactory});
            def.LookupList = loader.LoadLookupList(doc.DocumentElement);
        }
    }
}