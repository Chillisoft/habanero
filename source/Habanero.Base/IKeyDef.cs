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
using System.Collections;
using Habanero.Base.Exceptions;

namespace Habanero.Base
{
    /// <summary>
    /// The IKeyDef is a definition of an <see cref="IBusinessObject"/> key.
    /// It is essentially a key name and a collection of property 
    /// definitions that place certain limitations on the data
    /// that the key can hold.  The property definitions can also relate
    /// together in some way (e.g. for a composite alternate 
    /// key, the combination of properties is required to be unique).
    /// </summary>
    public interface IKeyDef: IEnumerable
    {
        /// <summary>
        /// A method used by BOKey to determine whether to check for
        /// duplicate keys.  If true, then the uniqueness check will be ignored
        /// if any of the properties making up the key are null.<br/>
        /// NB: If the BOKey is a primary key, then this cannot be
        /// set to true.
        /// </summary>
        bool IgnoreIfNull { get; set; }

        /// <summary>
        /// Returns the key name for this key definition - this key name is built
        /// up through a combination of the key name and the property names
        /// </summary>
        string KeyName { get;  set; }

        /// <summary>
        /// Gets and sets the message to show to the user if a key validation
        /// fails.  A default message will be provided if this is nto set.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Adds a property definition to the collection of definitions.
        /// The object to be added cannot be null.  If a key name was not
        /// originally provided at instantiation of the key definition, then
        /// the new property definition's name will be appended to the key name.
        /// </summary>
        /// <param name="propDef">The PropDef object to add</param>
        /// <exeption cref="HabaneroArgumentException">Will throw an exception
        /// if the argument is null</exeption>
        void Add(IPropDef propDef);

        /// <summary>
        /// Provides an indexing facility for the collection of property
        /// definitions that belong to the key, so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the property stored under that property name</returns>
        IPropDef this[string propName] { get; }

        /// <summary>
        /// Provides an indexing facility for the collection of property
        /// definitions that belong to the key, so that items
        /// in the collection can be accessed like an array. The order is
        /// always the same, but not determinable
        /// </summary>
        /// <param name="index">The index of the property</param>
        /// <returns>Returns the property stored under that index</returns>
        IPropDef this[int index] { get; }

        /// <summary>
        /// Returns a count of the number of property definitions held
        /// in this key definition
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Creates a new business object key (BOKey) using this key
        /// definition and its property definitions
        /// </summary>
        /// <param name="lBOPropCol">The master property collection</param>
        /// <returns>Returns a new BOKey object that mirrors this
        /// key definition</returns>
        IBOKey CreateBOKey(IBOPropCol lBOPropCol);

        /// <summary>
        /// Clears all <see cref="IPropDef"/> objects.
        /// </summary>
        void Clear();
    }
}