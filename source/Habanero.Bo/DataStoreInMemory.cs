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
//TODO_ brett 08 Jun 2010: For DotNet 2_0  using System.Linq;
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
        private Dictionary<IClassDef, INumberGenerator> _autoIncrementNumberGenerators = new Dictionary<IClassDef, INumberGenerator>();

        ///<summary>
        /// Returns the number of objects in the memory store.
        ///</summary>
        public int Count
        {
            get { return AllObjects.Count; }
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
        public virtual void Add(IBusinessObject businessObject)
        {
            AllObjects.Add(businessObject.ID.ObjectID, businessObject);
        }

        ///<summary>
        /// Finds the object of type T that matches the criteria.
        ///</summary>
        ///<param name="criteria"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public virtual T Find<T>(Criteria criteria) where T : class, IBusinessObject
        {
            return (T) Find(typeof (T), criteria);
        }

        ///<summary>
        /// Finds the object of type BOType that matches  and criteria.
        ///</summary>
        ///<param name="boType"></param>
        ///<param name="criteria"></param>
        ///<returns></returns>
        ///<exception cref="HabaneroDeveloperException"></exception>
        public virtual IBusinessObject Find(Type boType, Criteria criteria)
        {
            IBusinessObject currentBO = null;
            foreach (IBusinessObject bo in AllObjects.Values)
            {
                if (!boType.IsInstanceOfType(bo)) continue;
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
        public virtual IBusinessObject Find(IClassDef classDef, Criteria criteria)
        {
            IBusinessObject currentBO = null;
            foreach (IBusinessObject bo in AllObjects.Values)
            {
                if (bo.ClassDef != classDef && !classDef.ClassType.IsInstanceOfType(bo))
                {
                    continue;
                }
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
        public virtual T Find<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject
        {
            Type boType = typeof(T);
            foreach (IBusinessObject bo in AllObjects.Values)
            {
                if (!boType.IsInstanceOfType(bo)) continue;
                if (bo.ID.Equals(primaryKey)) return bo as T;
            }
            return null;
            //TODO_ brett 08 Jun 2010: For DotNet 2_0
/*
            return (from bo in AllObjects.Values where bo.ID.Equals(primaryKey) select bo as T).FirstOrDefault();*/
        }

        ///<summary>
        /// Removes the object from the data store.
        ///</summary>
        ///<param name="businessObject"></param>
        public virtual void Remove(IBusinessObject businessObject)
        {
            AllObjects.Remove(businessObject.ID.ObjectID);
        }

        ///<summary>
        /// Find all objects that match the criteria.
        ///</summary>
        ///<param name="criteria"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public virtual BusinessObjectCollection<T> FindAll<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T> {FindAllInternal<T>(criteria)};
            col.SelectQuery.Criteria = criteria;
            return col;
        }

        ///<summary>
        /// Find all objects that match the criteria.
        ///</summary>
        ///<param name="criteria"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        internal virtual List<T> FindAllInternal<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            //return AllObjects.Values.OfType<T>().Where(boAsT => criteria == null || criteria.IsMatch(boAsT)).ToList();
            List<T> list = new List<T>();
            Type boType = typeof (T);
            foreach (IBusinessObject bo in AllObjects.Values)
            {
                if (!boType.IsInstanceOfType(bo)) continue;
                if (criteria == null || criteria.IsMatch(bo)) list.Add((T)bo);
            }

            return list;
        }
        ///<summary>
        /// Find all objects of type boType that match the criteria.
        ///</summary>
        ///<param name="boType"></param>
        ///<param name="criteria"></param>
        ///<returns></returns>
        public virtual IBusinessObjectCollection FindAll(Type boType, Criteria criteria)
        {
            IBusinessObjectCollection col = CreateGenericCollection(boType);

/*            IEnumerable<IBusinessObject> allMatchineBOs = AllObjects.Values.Where(boType.IsInstanceOfType).Where(bo => criteria == null || criteria.IsMatch(bo));
            foreach (IBusinessObject bo in allMatchineBOs)
            {
                col.Add(bo);
            }*/

            foreach (IBusinessObject bo in AllObjects.Values)
            {
                if (!boType.IsInstanceOfType(bo)) continue;
                if (criteria == null || criteria.IsMatch(bo)) col.Add(bo);
            }
            col.SelectQuery.Criteria = criteria;
            return col;

        }

        private static IBusinessObjectCollection CreateGenericCollection(Type boType)
        {
            Type boColType = typeof (BusinessObjectCollection<>).MakeGenericType(boType);
            return (IBusinessObjectCollection) Activator.CreateInstance(boColType);
        }

        ///<summary>
        /// Find all objects of type boType that match the criteria.
        ///</summary>
        /// <param name="classDef"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual IBusinessObjectCollection FindAll(IClassDef classDef, Criteria criteria)
        {
            Type boType = classDef.ClassType;
            IBusinessObjectCollection col = CreateGenericCollection(boType);
            col.ClassDef = classDef;
            foreach (IBusinessObject bo in AllObjects.Values)
            {
                if (!boType.IsInstanceOfType(bo)) continue;
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
            AllObjects.Clear();
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
                foreach (var o in AllObjects)
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
        /// <param name="typeToLoad">The type to load that is the top of the heirarchy in the xml file</param>
        public void LoadFromXml(string fullFileName, Type typeToLoad)
        {
            if (!File.Exists(fullFileName)) return;
            XmlSerializer xs = new XmlSerializer(typeToLoad);
            using (Stream stream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                try
                {
                    IBusinessObject bo = (IBusinessObject) xs.Deserialize(stream);
                    this.AllObjects.Add(bo.ID.GetAsGuid(), bo);
                }
                catch (Exception ex)
                {
                    string message = "The File " + fullFileName + " could not be deserialised because of the following error";
                    throw new Exception(message + ex.Message, ex);
                }
            }
        }

        ///<summary>
        /// The <see cref="INumberGenerator"/>s used to produce the AutoIncremented values for Classes in this DataStore.
        ///</summary>
        public Dictionary<IClassDef, INumberGenerator> AutoIncrementNumberGenerators
        {
            get { return _autoIncrementNumberGenerators; }
        }
        
        ///<summary>
        /// Returns the next value for the AutoIncrement value for the specified <see cref="IClassDef"/>.
        /// If a number generator does not exist for the specified <see cref="IClassDef"/> then one is created.
        ///</summary>
        ///<param name="classDef">The ClassDef that the AutoIncremented value is being generated for.</param>
        ///<returns>The next AutoIncrement value for the ClassDef specified.</returns>
        public virtual long GetNextAutoIncrementingNumber(IClassDef classDef)
        {
            if(!AutoIncrementNumberGenerators.ContainsKey(classDef))
                AutoIncrementNumberGenerators.Add(classDef, new NumberGenerator(string.Format("AutoIncrement({0})", classDef.ClassName)));
            return AutoIncrementNumberGenerators[classDef].NextNumber();
        }
    }
}
