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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;

namespace Habanero.BO
{
    /// <summary>
    /// Holds a collection of properties on which two classes in a relationship
    /// are matching
    /// </summary>
    public class RelKey : IRelKey
    {
        private RelKeyDef _relKeyDef;
        private Dictionary<string, IRelProp> _relProps;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        /// <param name="lRelKeyDef">The relationship key definition</param>
        public RelKey(RelKeyDef lRelKeyDef)
        {
            _relKeyDef = lRelKeyDef;
            _relProps = new Dictionary<string, IRelProp>();
        }

        /// <summary>
        /// Provides an indexing facility so that the properties can be
        /// accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the RelProp object found with that name</returns>
        internal IRelProp this[string propName]
        {
            get
            {
                if (!_relProps.ContainsKey(propName))
                {
                    throw new InvalidPropertyNameException(String.Format(
                        "A related property with the name '{0}' is being " +
                        "accessed, but no property with that name exists in " +
                        "the relationship's collection.", propName));
                }
                return (_relProps[propName]);
            }
        }

        /// <summary>
        /// Indexes the array of relprops this relkey contains.
        /// </summary>
        /// <param name="index">The position of the relprop to get</param>
        /// <returns>Returns the RelProp object found with that name</returns>
        public IRelProp this[int index]
        {
            get
            {
                int i = 0;
                foreach (KeyValuePair<string, IRelProp> prop in _relProps)
                {
                    if (i++ == index) return prop.Value;
                }
                throw new IndexOutOfRangeException("This RelKey does not contain a RelProp at index " + index);
            }
        }

        /// <summary>
        /// Adds the given RelProp to the key
        /// </summary>
        /// <param name="relProp">The RelProp object to add</param>
        internal virtual void Add(RelProp relProp)
        {
            if (_relProps.ContainsKey(relProp.OwnerPropertyName))
            {
                throw new InvalidPropertyException(String.Format(
                    "A related property with the name '{0}' is being added " +
                    "to a collection, but already exists in the collection.",
                    relProp.OwnerPropertyName));
            }
            _relProps.Add(relProp.OwnerPropertyName, relProp);
        }

        /// <summary>
        /// Indicates whether a property with the given name is part of the key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if a property with this name is held</returns>
        internal bool Contains(string propName)
        {
            return (_relProps.ContainsKey(propName));
        }

        /// <summary>
        /// Indicates if there is a related object.
        /// If all relationship properties are null then it is assumed that 
        /// there is no related object.
        /// </summary>
        /// <returns>Returns true if there is a valid relationship</returns>
        internal bool HasRelatedObject()
        {
            foreach (RelProp relProp in this)
            {
                if (! (relProp.IsNull))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the relationship expression
        /// </summary>
        /// <returns>Returns an IExpression object</returns>
        internal IExpression RelationshipExpression()
        {
            if (_relProps.Count >= 1)
            {
                IExpression exp = null;
                foreach (RelProp relProp in this)
                {
                    if (exp == null)
                    {
                        exp = relProp.RelatedPropExpression();
                    }
                    else
                    {
                        exp = new Expression(exp, new SqlOperator("AND"), relProp.RelatedPropExpression());
                    }
                }
                return exp;
            }
            return null;
        }

        /// <summary>
        /// Returns an enumrated for theis RelKey to iterate through its RelProps
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IRelProp> GetEnumerator()
        {
            return _relProps.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _relProps.Values.GetEnumerator();
        }

        public int Count
        {
            get { return _relProps.Count; }
        }
    }
}