using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO
{
    public class ObjectTreeXmlReader
    {
        private readonly Stream _stream;

        private List<string> _propertyReadExceptions = new List<string>();
        public Result ReadResult { get; private set; }

        public ObjectTreeXmlReader( )
        {
        }
        public ObjectTreeXmlReader(Stream stream)
        {
            _stream = stream;
        }

        public IEnumerable<IBusinessObject> ReadFromString(string xml)
        {
            var reader = XmlReader.Create(new StringReader(xml), GetSettings());
            return ReadFromReader(reader);
        }

        public IEnumerable<IBusinessObject> Read()
        {
            if (_stream == null) throw new ArgumentException("'stream' cannot be null");
            var reader = XmlReader.Create(_stream, GetSettings());
            return ReadFromReader(reader);
        }

        private IEnumerable<IBusinessObject> ReadFromReader(XmlReader reader)
        {
            BOSequenceNumber.LoadNumberGenClassDef();
            var objects = new List<IBusinessObject>();
            reader.Read();
            reader.Read();
            while (reader.Name == "BusinessObjects") reader.Read();
            while (reader.Name == "bo")
            {
                var typeName = reader.GetAttribute("__tn");
                var assemblyName = reader.GetAttribute("__an");
                var classDef = ClassDef.ClassDefs[assemblyName, typeName];
                var boType = classDef.ClassType;
                var bo = (IBusinessObject)Activator.CreateInstance(boType);
                
                while (reader.MoveToNextAttribute())
                {
                    var propertyName = reader.Name;
                    if (reader.Name == "__tn" || reader.Name == "__an") continue;
                    var propertyValue = reader.Value;
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
                BusinessObjectManager.Instance.Remove(bo);
                var existingBo = GetExistingBo(classDef, bo);
                if (existingBo != null)
                {
                    foreach (var prop in existingBo.Props)
                    {
                        existingBo.SetPropertyValue(prop.PropertyName, bo.GetPropertyValue(prop.PropertyName));
                    }
                    objects.Add(existingBo);
                }
                else
                {
                    //BusinessObjectLoaderBase.SetStatusAfterLoad(bo);
                    //((BusinessObject)bo).SetStatus(BOStatus.Statuses.isNew, true);
                    //ReflectionUtilities.SetPropertyValue(bo.Status, "IsNew", true);
                    BusinessObjectLoaderBase.CallAfterLoad(bo);
                    objects.Add(bo);
                    try
                    {
                        BusinessObjectManager.Instance.Add(bo);
                    } catch (HabaneroDeveloperException ex)
                    {
                        // object already exists - this is a possible circumstance so we can let it go
                        if (!ex.DeveloperMessage.Contains("Two copies of the business object"))
                            throw;
                        
                    }
                    bo.Props.BackupPropertyValues();
                }
                
                reader.Read();
            }

            //foreach (IBusinessObject businessObject in objects.Values)
            //{
            //    businessObject.Props.BackupPropertyValues();
            //}

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

        private IBusinessObject GetExistingBo(IClassDef classDef, IBusinessObject bo)
        {
            if (BORegistry.DataAccessor != null)
            {
                try
                {
                    return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, bo.ID);
                } catch (BusObjDeleteConcurrencyControlException ex)
                {
                    return null;
                }
            }
            return null;
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