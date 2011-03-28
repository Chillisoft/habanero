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
using System.IO;
using System.Text;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    public class DataStoreInMemoryXmlReader
    {
        private readonly Stream _stream;

        private List<string> _propertyReadExceptions = new List<string>();
        public Result ReadResult { get; private set; }

        public DataStoreInMemoryXmlReader( )
        {
        }
        public DataStoreInMemoryXmlReader(Stream stream)
        {
            _stream = stream;
        }

        public Dictionary<Guid, IBusinessObject> ReadFromString(string xml)
        {
            var reader = XmlReader.Create(new StringReader(xml), GetSettings());
            return ReadFromReader(reader);
        }

        public Dictionary<Guid, IBusinessObject> Read()
        {
            if (_stream == null) throw new ArgumentException("'stream' cannot be null");
            var reader = XmlReader.Create(_stream, GetSettings());
            return ReadFromReader(reader);
        }

        private Dictionary<Guid, IBusinessObject> ReadFromReader(XmlReader reader)
        {
            BOSequenceNumber.LoadNumberGenClassDef();
            Dictionary<Guid, IBusinessObject> objects = new Dictionary<Guid, IBusinessObject>();
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
                        SetupProperty(bo, propertyName, propertyValue);
                    }
                    catch (Exception ex)
                    {

                        // Log the exception and continue
                        _propertyReadExceptions.Add(string.Format("An error occured when attempting to set property '{0}.{1}'. {2}", bo.ClassDef.ClassName , propertyName, ex.Message));
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

            if (_propertyReadExceptions.Count == 0)
            {
                ReadResult = new Result(true);
            }
            else
            {
                ReadResult = new Result(false, BuildExceptionMessage(_propertyReadExceptions));
            }
            return objects;
        }

        private XmlReaderSettings GetSettings()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            return settings;
        }

        private static string BuildExceptionMessage(IEnumerable<string> propertyReadExceptions)
        {
            const string crlf = @"\r\n";
            var exceptionMessage = new StringBuilder("");
            foreach (var propertyReadException in propertyReadExceptions)
            {
                exceptionMessage.Append(propertyReadException);
                exceptionMessage.Append(crlf);
            }
            return exceptionMessage.ToString();
        }

        protected  virtual void SetupProperty(IBusinessObject bo, string propertyName, string propertyValue)
        {
            bo.Props[propertyName].InitialiseProp(propertyValue);
        }

     
    }


}