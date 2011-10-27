// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Converts the content of an <see cref="XmlReader"/> into a set of <see cref="IBusinessObject"/>.
    /// The default implementation, <see cref="BusinessObjectXmlReader"/> converts xml written by the
    /// <see cref="BusinessObjectXmlWriter"/> into business objects.
    /// It's easiest to use this interface via <see cref="ObjectTreeXmlReader"/> 
    /// or <see cref="DataStoreInMemoryXmlReader"/>. However, if you need to change how
    /// objects are read/written from xml, implement your own <see cref="IBusinessObjectXmlReader"/>
    /// and <see cref="IBusinessObjectXmlWriter"/> and use them with those classes.
    /// </summary>
    public interface IBusinessObjectXmlReader
    {
        /// <summary>
        /// Contains a list of errors encountered when setting properties on objects.
        /// If, for example, you have removed properties from you objects since you saved your
        /// xml, then the reader will encounter an error when trying to set that property.
        /// That error is added to this list.  
        /// </summary>
        IEnumerable<string> PropertyReadExceptions { get; }

        /// <summary>
        /// Uses the XmlReader given to read an xml stream and instantiates IBusinessObjects for
        /// each bo node in the stream. The resultant IEnumerable can only be iterated once,
        /// so convert it to a list or array (using ToList() or ToArray()) if you want to 
        /// iterate through it more than once (this includes using Count() etc). The XmlReader
        /// is closed once you have iterated.
        /// </summary>
        /// <param name="xmlReader">The reader to read from</param>
        /// <returns>An enumerator. As you iterate through the enumerator the xml will be read</returns>
        IEnumerable<IBusinessObject> Read(XmlReader xmlReader);
    }

    /// <summary>
    /// Converts the content of an <see cref="XmlReader"/> into a set of <see cref="IBusinessObject"/>.
    /// This is the default implementation, and converts xml written by the
    /// <see cref="BusinessObjectXmlWriter"/> into business objects.
    /// It's easiest to use this class via <see cref="ObjectTreeXmlReader"/> 
    /// or <see cref="DataStoreInMemoryXmlReader"/>. However, if you need to change how
    /// objects are read/written from xml, implement your own <see cref="IBusinessObjectXmlReader"/>
    /// and <see cref="IBusinessObjectXmlWriter"/> and use them with those classes.
    /// </summary>
    public class BusinessObjectXmlReader : IBusinessObjectXmlReader
    {
        private readonly List<string> _propertyReadExceptions = new List<string>();

        /// <summary>
        /// Contains a list of errors encountered when setting properties on objects.
        /// If, for example, you have removed properties from you objects since you saved your
        /// xml, then the reader will encounter an error when trying to set that property.
        /// That error is added to this list.  
        /// </summary>
        public IEnumerable<string> PropertyReadExceptions { get { return _propertyReadExceptions; } }

        /// <summary>
        /// Uses the XmlReader given to read an xml stream and instantiates IBusinessObjects for
        /// each bo node in the stream. The resultant IEnumerable can only be iterated once,
        /// so convert it to a list or array (using ToList() or ToArray()) if you want to 
        /// iterate through it more than once (this includes using Count() etc). The XmlReader
        /// is closed once you have iterated.
        /// </summary>
        /// <param name="xmlReader">The reader to read from</param>
        /// <returns>An enumerator. As you iterate through the enumerator the xml will be read</returns>
        public IEnumerable<IBusinessObject> Read(XmlReader xmlReader)
        {
            xmlReader.Read();
            xmlReader.Read();
            while (xmlReader.Name == "BusinessObjects") xmlReader.Read();
            while (xmlReader.Name == "bo")
            {
                var typeName = xmlReader.GetAttribute("__tn");
                var assemblyName = xmlReader.GetAttribute("__an");
                var classDef = ClassDef.ClassDefs[assemblyName, typeName];
                var boType = classDef.ClassType;
                var bo = (IBusinessObject) Activator.CreateInstance(boType);

                while (xmlReader.MoveToNextAttribute())
                {
                    var propertyName = xmlReader.Name;
                    if (xmlReader.Name == "__tn" || xmlReader.Name == "__an") continue;
                    var propertyValue = xmlReader.Value;
                    try
                    {
                        SetupProperty(bo, propertyName, propertyValue);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and continue
                        _propertyReadExceptions.Add(
                            string.Format("An error occured when attempting to set property '{0}.{1}'. {2}",
                                          bo.ClassDef.ClassName, propertyName, ex.Message));
                        continue;
                    }
                }
                yield return bo;
                xmlReader.Read();
            }
            xmlReader.Close();
        }

        protected virtual void SetupProperty(IBusinessObject bo, string propertyName, string propertyValue)
        {
            bo.Props[propertyName].InitialiseProp(propertyValue);
        }

    }
}
