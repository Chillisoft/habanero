// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.IO;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    public class DataStoreInMemoryXmlReader
    {
        private readonly Stream _stream;

        public DataStoreInMemoryXmlReader(Stream stream)
        {
            _stream = stream;
        }

        public Dictionary<Guid, IBusinessObject> Read()
        {
            Dictionary<Guid, IBusinessObject> objects = new Dictionary<Guid, IBusinessObject>();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            XmlReader reader = XmlReader.Create(_stream, settings);
            reader.Read();
            reader.Read();
            while (reader.Name == "BusinessObjects") reader.Read();
            while (reader.Name == "bo")
            {
                string typeName = reader.GetAttribute("__tn");
                string assemblyName = reader.GetAttribute("__an");
                Type boType = ClassDef.ClassDefs[assemblyName, typeName].ClassType;
                IBusinessObject bo = (IBusinessObject)Activator.CreateInstance(boType);
                while (reader.MoveToNextAttribute())
                {
                    string propertyName = reader.Name;
                    if (reader.Name == "__tn" || reader.Name == "__an") continue;
                    string propertyValue = reader.Value;
                    try
                    {
                        bo.Props[propertyName].InitialiseProp(propertyValue);
                    }
                    catch (InvalidPropertyNameException)
                    {
                        //Do Nothing since the property no longer exists.
                        continue;
                    }
                }
                BusinessObjectLoaderBase.SetStatusAfterLoad(bo);
                BusinessObjectLoaderBase.CallAfterLoad(bo);
                objects.Add(bo.ID.GetAsGuid(), bo);
                reader.Read();
            }

            foreach (IBusinessObject businessObject in objects.Values)
            {
                businessObject.Props.BackupPropertyValues();
            }

            return objects;
        }
    }
}