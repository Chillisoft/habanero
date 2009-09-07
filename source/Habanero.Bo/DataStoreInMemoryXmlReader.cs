using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Habanero.Base;
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
                    bo.SetPropertyValue(propertyName, propertyValue);
                }
                bo.Props.BackupPropertyValues();
                objects.Add(bo.ID.GetAsGuid(), bo);
                reader.Read();
            }

            return objects;
        }
    }
}