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
using System.Xml.Serialization;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Util;

namespace Habanero.BO
{
    ///<summary>
    /// This is an in-memory data store designed to be used primarily for testing.
    ///</summary>
    public class DataStoreInMemory
    {
        private Dictionary<Guid, IBusinessObject> _objects = new Dictionary<Guid, IBusinessObject>();

        ///<summary>
        /// Returns the number of objects in the memory store.
        ///</summary>
        public int Count
        {
            get { return _objects.Count; }
        }

        ///<summary>
        /// Returns a Dictionary of all the objects in the memory store.
        ///</summary>
        public Dictionary<Guid, IBusinessObject> AllObjects
        {
            get { return _objects; }
            set { _objects = value;  }
        }

        ///<summary>
        /// Adds a new business object to the memory store.
        ///</summary>
        ///<param name="businessObject"></param>
        public void Add(IBusinessObject businessObject)
        {
            _objects.Add(businessObject.ID.ObjectID, businessObject);
        }

        ///<summary>
        /// Finds the object of type T that matches the criteria.
        ///</summary>
        ///<param name="criteria"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public T Find<T>(Criteria criteria) where T : class, IBusinessObject
        {
            return (T) Find(typeof (T), criteria);
        }

        ///<summary>
        /// Finds the object of type BOType that matches  and criteria.
        ///</summary>
        ///<param name="BOType"></param>
        ///<param name="criteria"></param>
        ///<returns></returns>
        ///<exception cref="HabaneroDeveloperException"></exception>
        public IBusinessObject Find(Type BOType, Criteria criteria)
        {
            IBusinessObject currentBO = null;
            foreach (IBusinessObject bo in _objects.Values)
            {
                if (!BOType.IsInstanceOfType(bo)) continue;
                if (!criteria.IsMatch(bo)) continue;
                if (currentBO == null)
                {
                    currentBO = bo;
                }
                else
                {
                    throw new HabaneroDeveloperException("There was an error with loading the class '"
                              + bo.ClassDef.ClassNameFull + "'", "Loading a '"
                              + bo.ClassDef.ClassNameFull + "' with criteria '" + criteria
                              + "' returned more than one record when only one was expected.");
                }
            }
            return currentBO;
        }        
        
        ///<summary>
        /// Finds
        ///</summary>
        ///<param name="classDef">ClassDef to match on.</param>
        ///<param name="criteria">Criteria being used to find the BusinessObject</param>
        ///<returns></returns>
        ///<exception cref="HabaneroDeveloperException"></exception>
        public IBusinessObject Find(IClassDef classDef, Criteria criteria)
        {
            IBusinessObject currentBO = null;
            foreach (IBusinessObject bo in _objects.Values)
            {
                if (!(bo.ClassDef==classDef)) continue;
                if (!criteria.IsMatch(bo)) continue;
                if (currentBO == null)
                {
                    currentBO = bo;
                }
                else
                {
                    throw new HabaneroDeveloperException("There was an error with loading the class '"
                              + bo.ClassDef.ClassNameFull + "'", "Loading a '"
                              + bo.ClassDef.ClassNameFull + "' with criteria '" + criteria
                              + "' returned more than one record when only one was expected.");
                }
            }
            return currentBO;
        }

        ///<summary>
        /// Finds an object of type T that has the primary key primaryKey.
        ///</summary>
        ///<param name="primaryKey"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public T Find<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject
        {
            foreach (IBusinessObject bo in _objects.Values)
            {
                if (bo.ID.Equals(primaryKey)) return bo as T;
            }
            return null;
        }

        ///<summary>
        /// Removes the object from the data store.
        ///</summary>
        ///<param name="businessObject"></param>
        public void Remove(IBusinessObject businessObject)
        {
            _objects.Remove(businessObject.ID.ObjectID);
        }

        ///<summary>
        /// Find all objects that match the criteria.
        ///</summary>
        ///<param name="criteria"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public BusinessObjectCollection<T> FindAll<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.Add(FindAllInternal<T>(criteria));
            col.SelectQuery.Criteria = criteria;
            return col;
        }

        ///<summary>
        /// Find all objects that match the criteria.
        ///</summary>
        ///<param name="criteria"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        internal List<T> FindAllInternal<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            List <T> col = new List<T>();
            foreach (IBusinessObject bo in _objects.Values)
            {
                T boAsT = bo as T;
                if (boAsT == null) continue;
                if (criteria == null || criteria.IsMatch(boAsT)) col.Add(boAsT);
            }
            return col;
        }
        ///<summary>
        /// Find all objects of type boType that match the criteria.
        ///</summary>
        ///<param name="BOType"></param>
        ///<param name="criteria"></param>
        ///<returns></returns>
        public IBusinessObjectCollection FindAll(Type BOType, Criteria criteria)
        {
            Type boColType = typeof (BusinessObjectCollection<>).MakeGenericType(BOType);
            IBusinessObjectCollection col = (IBusinessObjectCollection) Activator.CreateInstance(boColType);
            foreach (IBusinessObject bo in _objects.Values)
            {
                if (!BOType.IsInstanceOfType(bo)) continue;
                if (criteria == null || criteria.IsMatch(bo)) col.Add(bo);
            }
            col.SelectQuery.Criteria = criteria;
            return col;
        }
        ///<summary>
        /// Find all objects of type boType that match the criteria.
        ///</summary>
        /// <param name="classDef"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IBusinessObjectCollection FindAll(IClassDef classDef, Criteria criteria)
        {
            Type boType = classDef.ClassType;
            Type boColType = typeof (BusinessObjectCollection<>).MakeGenericType(boType);
            IBusinessObjectCollection col = (IBusinessObjectCollection) Activator.CreateInstance(boColType);
            col.ClassDef = classDef;           
            foreach (IBusinessObject bo in _objects.Values)
            {
                if (!classDef.ClassType.IsInstanceOfType(bo)) continue;
                if (classDef.TypeParameter != bo.ClassDef.TypeParameter) continue;
                if (criteria == null || criteria.IsMatch(bo)) col.Add(bo);
            }
            col.SelectQuery.Criteria = criteria;
            return col;
        }

        /// <summary>
        /// Clears all the objects in the memory datastore
        /// </summary>
        public void ClearAllBusinessObjects()
        {
            _objects.Clear();
        }

        /// <summary>
        /// Saves all the objects from the data store to the file defined in fullFileName
        /// </summary>
        /// <param name="fullFileName">The full file name to store including the file path e.g. C:\Systems\SomeFile.dat </param>
        public void SaveToXml(string fullFileName)
        {

            string directoryName = Path.GetDirectoryName(fullFileName);
            FileUtilities.CreateDirectory(directoryName);
           // XmlSerializer serializer = new XmlSerializer(typeof(BusinessObject));
            // Serialize the all BO's in _objectsCollection

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Auto;
            using (FileStream fs = new FileStream(fullFileName, FileMode.Create))
            {
                XmlWriter writer = XmlWriter.Create(fs, settings);
                writer.WriteStartDocument();
                writer.WriteStartElement("BusinessObjects");
                foreach (var o in _objects)
                {

                    ((BusinessObject) o.Value).WriteXml(writer);
                    //serializer.Serialize(fs, o.Value);                    
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();

            }
        }
        
        /// <summary>
        /// Loads all the objects to the data store from the file specified by fullFileName.
        /// The file specified to be loaded must be a serialised xml file.
        /// </summary>
        /// <param name="fullFileName">The full file name to store including the file path e.g. C:\Systems\SomeFile.xml </param>
        public void LoadFromXml(string fullFileName, Type typeToLoad)
        {
            if (!File.Exists(fullFileName)) return;
            XmlSerializer xs = new XmlSerializer(typeToLoad);
            using (Stream stream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                try
                {
                    IBusinessObject bo = (IBusinessObject) xs.Deserialize(stream);
                    this._objects.Add(bo.ID.GetAsGuid(), bo);
                }
                catch (Exception ex)
                {
                    string message = "The File " + fullFileName + " could not be deserialised because of the following error";
                    throw new Exception(message + ex.Message, ex);
                }
            }
        }



    }
}
