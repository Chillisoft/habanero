//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Holds a collection of properties on which two classes in a relationship
    /// are matching
    /// </summary>
    public class RelKey : IRelKey
    {
        private readonly RelKeyDef _relKeyDef;
        private readonly Dictionary<string, IRelProp> _relProps;

        /// <summary>
        /// Event raised when the value for one of the Properties (<see cref="IRelProp"/>) for this <see cref="IRelKey"/> is changed
        /// </summary>
        public event EventHandler RelatedPropValueChanged;


        /// <summary>
        /// Constructor to initialise a new instance. This initialises the RelKey and sets all its
        /// relationship properties (IRelProp).
        /// </summary>
        /// <param name="lRelKeyDef">The relationship key definition</param>
        /// <param name="lBoPropCol">The properties of the business object that this relationship key
        /// is being created form</param>
        public RelKey(RelKeyDef lRelKeyDef, IBOPropCol lBoPropCol)
        {
            _relKeyDef = lRelKeyDef;
            _relProps = new Dictionary<string, IRelProp>();
            foreach (RelPropDef relPropDef in _relKeyDef)
            {
                IRelProp relProp = relPropDef.CreateRelProp(lBoPropCol);
                relProp.PropValueUpdated += (sender, e) => FireRelatedPropValueChangedEvent();
                this.Add(relProp);
            }
        }

        private void FireRelatedPropValueChangedEvent()
        {
            if (this.RelatedPropValueChanged ==null) return;
            this.RelatedPropValueChanged(this, new EventArgs());
        }

        /// <summary>
        /// Provides an indexing facility so that the properties can be
        /// accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the RelProp object found with that name</returns>
        public IRelProp this[string propName]
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
        private void Add(IRelProp relProp)
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
        public bool Contains(string propName)
        {
            return (_relProps.ContainsKey(propName));
        }

        /// <summary>
        /// Returns a copy of the key's Criteria (ie the search string matching this key). 
        /// </summary>
        /// <returns>Returns a Criteria object</returns>
        public Criteria Criteria
        {
            get
            {
                if (_relProps.Count == 0) return null;
                Criteria criteria = null;
                foreach (RelProp relProp in this)
                {
                    criteria = criteria == null ? relProp.Criteria : new Criteria(criteria, Criteria.LogicalOp.And, relProp.Criteria);
                }
                return criteria;
 
                //if (_relProps.Count >= 1)
                //{
                //    IExpression exp = null;
                //    foreach (RelProp relProp in this)
                //    {
                //        exp = exp == null
                //            ? relProp.RelatedPropExpression()
                //            : new Expression(exp, new SqlOperator("AND"), relProp.RelatedPropExpression());
                //    }
                //    return exp;
                //}
                //return null;
            }
        }
//        /// <summary>
//        /// Returns a copy of the key's Criteria (ie the search string matching this key). 
//        /// </summary>
//        /// <returns>Returns a Criteria object</returns>
//        public Criteria RelatedCriteria
//        {
//            get
//            {
//                if (_relProps.Count == 0) return null;
//                Criteria criteria = null;
//                foreach (RelProp relProp in this)
//                {
//                    if (criteria == null)
//                        criteria = relProp.Criteria;
//                    else
//                    {
//                        criteria = new Criteria(criteria, Criteria.LogicalOp.And, relProp.Criteria);
//                    }
//                }
//                return criteria;
// 
//                //if (_relProps.Count >= 1)
//                //{
//                //    IExpression exp = null;
//                //    foreach (RelProp relProp in this)
//                //    {
//                //        exp = exp == null
//                //            ? relProp.RelatedPropExpression()
//                //            : new Expression(exp, new SqlOperator("AND"), relProp.RelatedPropExpression());
//                //    }
//                //    return exp;
//                //}
//                //return null;
//            }
//        }


        /// <summary>
        /// Indicates if there is a related object.
        /// If all relationship properties are null then it is assumed that 
        /// there is no related object.
        /// </summary>
        /// <returns>Returns true if there is a valid relationship</returns>
        public bool HasRelatedObject()
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

        ///// <summary>
        ///// Returns the relationship expression. This is a copy of the expression as stored in the <see cref="RelKey"/>
        ///// </summary>
        ///// <returns>Returns an IExpression object</returns>
        //public IExpression RelationshipExpression()
        //{
        //    if (_relProps.Count >= 1)
        //    {
        //        IExpression exp = null;
        //        foreach (RelProp relProp in this)
        //        {
        //            exp = exp == null 
        //                ? relProp.RelatedPropExpression() 
        //                : new Expression(exp, new SqlOperator("AND"), relProp.RelatedPropExpression());
        //        }
        //        return exp;
        //    }
        //    return null;
        //}

        /// <summary>
        /// Returns an enumrated for theis RelKey to iterate through its RelProps
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IRelProp> GetEnumerator()
        {
            return _relProps.Values.GetEnumerator();
        }

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _relProps.Values.GetEnumerator();
        }

        ///<summary>
        /// Gets the number of properties in this relationship key.
        ///</summary>
        public int Count
        {
            get { return _relProps.Count; }
        }
    }
}