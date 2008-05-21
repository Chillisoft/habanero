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


using System;
using System.Collections;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a collection of objects that are sub-types of ControlMapper
    /// </summary>
    public class ControlMapperCollection : IControlMapperCollection, ICollection
    {
        private IList _collection;
        private BusinessObject _businessObject;

        /// <summary>
        /// Contructor to initialise an empty collection
        /// </summary>
        public ControlMapperCollection()
        {
            _collection = new ArrayList();
        }

        /// <summary>
        /// Copies the elements of the collection to an Array, 
        /// starting at a particular Array index
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The zero-based index position to start
        /// copying from</param>
        public void CopyTo(Array array, int index)
        {
            _collection.CopyTo(array, index);
        }

        /// <summary>
        /// Returns the number of objects in the collection
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Returns the collection's synchronisation root
        /// </summary>
        public object SyncRoot
        {
            get { return _collection.SyncRoot; }
        }

        /// <summary>
        /// Indicates whether the collection is synchronised
        /// </summary>
        public bool IsSynchronized
        {
            get { return _collection.IsSynchronized; }
        }

        /// <summary>
        /// Provides an enumerator of the collection
        /// </summary>
        /// <returns>Returns an enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Provides an indexing facility so that the collection can be
        /// accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to read</param>
        /// <returns>Returns the mapper at the position specified</returns>
        public IControlMapper this[int index]
        {
            get { return (IControlMapper) _collection[index]; }
        }

        /// <summary>
        /// Provides an indexing facility as before, but allows the objects
        /// to be referenced using their property names instead of their
        /// numerical position
        /// </summary>
        /// <param name="propertyName">The property name of the object</param>
        /// <returns>Returns the mapper if found, or null if not</returns>
        public IControlMapper this[string propertyName]
        {
            get
            {
                foreach (IControlMapper controlMapper in _collection)
                {
                    if (controlMapper.PropertyName == propertyName)
                    {
                        return controlMapper;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Adds a mapper object to the collection
        /// </summary>
        /// <param name="mapper">The object to add, which must be a type or
        /// sub-type of ControlMapper</param>
        public void Add(IControlMapper mapper)
        {
            this._collection.Add(mapper);
        }

        /// <summary>
        /// Gets and sets the business object being represented by
        /// the mapper collection.  Changes are applied to the business
        /// object represented by this collection and to each BO within the
        /// collection.
        /// </summary>
        public BusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                foreach (IControlMapper controlMapper in _collection)
                {
                    controlMapper.BusinessObject = value;
                }
                _businessObject = value;
            }
        }

        /// <summary>
        /// Enables or Disables all the controls managed in this control mapper collection.
        /// </summary>
        public bool ControlsEnabled
        {
            set
            {
                foreach (IControlMapper mapper in _collection)
                {
                    mapper.Control.Enabled = value;
                }
            }
        }


        /// <summary>
        /// Sets the property of the business object that the mapper is mapped to
        /// to the value set in the control.
        /// </summary>
        public void ApplyChangesToBusinessObject()
        {
            foreach (IControlMapper mapper in _collection)
            {
                mapper.ApplyChangesToBusinessObject();
            }
        }
    }
}