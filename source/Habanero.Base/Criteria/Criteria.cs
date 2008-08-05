//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

namespace Habanero.Base
{
    /// <summary>
    /// A criteria expression that can be used to build up a criteria tree. The IsMatch method can then be
    /// used to check whether a BusinessObject matches the criteria.
    /// </summary>
    public class Criteria
    {


        #region LogicalOp enum

        ///<summary>
        /// A logical operator used to conjoin two criteria trees.
        ///</summary>
        public enum LogicalOp
        {
            ///<summary>
            /// The logical And (&&) operator
            ///</summary>
            And,
            /// <summary>
            /// The logical Or (||) operator
            /// </summary>
            Or
        }

        #endregion

        #region Op enum

        /// <summary>
        /// An operator used on a leaf criteria - ie to check a property against a value
        /// </summary>
        public enum Op
        {
            ///<summary>
            /// The equals (=) operator
            ///</summary>
            Equals,
            ///<summary>
            /// The Greater than (&gt;) operator.
            ///</summary>
            GreaterThan,
            ///<summary>
            /// The less than (&lt;) operator
            ///</summary>
            LessThan
        }

        #endregion
        /// <summary>
        /// The default date format to be used.
        /// </summary>
        public const string DATE_FORMAT = "yyyy/MM/dd HH:mm:ss";

        private readonly Criteria _leftCriteria;
        private readonly LogicalOp _logicalOp;
        private readonly Op _op;
        private readonly Criteria _rightCriteria;
        private object _fieldValue;
        protected readonly string[] LogicalOps = {"AND", "OR"};
        protected readonly string[] Ops = {"=", ">", "<"};
        private QueryField _field;

        protected Criteria()
        {
        }

        /// <summary>
        /// Creates a leaf criteria (meaning it has no children in the tree structure).
        /// </summary>
        /// <param name="propName">The property whose value to check</param>
        /// <param name="op">The operator to use to compare the property value to the given value</param>
        /// <param name="value">The value to compare to</param>
        public Criteria(string propName, Op op, object value)
        {
            _field = new QueryField(propName, propName, null);
            _op = op;
            _fieldValue = value;
        }

        /// <summary>
        /// Creates a composite criteria by logically joining two other criteria.
        /// </summary>
        /// <param name="leftCriteria">The left criteria (can be a whole tree structure)</param>
        /// <param name="logicalOp">The logical operator to use to join the left criteria tree with the right</param>
        /// <param name="rightCriteria">The right criteria (can be a whole tree structure)</param>
        public Criteria(Criteria leftCriteria, LogicalOp logicalOp, Criteria rightCriteria)
        {
            _leftCriteria = leftCriteria;
            _logicalOp = logicalOp;
            _rightCriteria = rightCriteria;
        }

        public virtual QueryField Field
        {
            get { return _field; }
        }

        public virtual Criteria LeftCriteria
        {
            get { return _leftCriteria; }
        }

        public virtual Criteria RightCriteria
        {
            get { return _rightCriteria; }
        }

        public virtual LogicalOp LogicalOperator
        {
            get { return _logicalOp; }
        }

        public virtual object FieldValue
        {
            get { return _fieldValue; }
            set { _fieldValue = value; }
        }

        public virtual Op ComparisonOperator
        {
            get { return _op; }
        }


        /// <summary>
        /// Evaluates the businessObject passed in to see if it matches the criteria that have been set up
        /// </summary>
        /// <typeparam name="T">The type of BusinessObject</typeparam>
        /// <param name="businessObject">The businessobject to check for a match against the criteria</param>
        /// <returns>True if the businessobject matches the criteria, false if it does not</returns>
        public bool IsMatch<T>(T businessObject) where T : IBusinessObject
        {
            if (IsComposite())
            {
                switch (_logicalOp)
                {
                    case LogicalOp.And:
                        return _leftCriteria.IsMatch(businessObject) && _rightCriteria.IsMatch(businessObject);
                    case LogicalOp.Or:
                        return _leftCriteria.IsMatch(businessObject) || _rightCriteria.IsMatch(businessObject);
                }
            }

            //todo: criterias with relationships - this will pass the source through to the GetPropertyValue
            object leftValue = businessObject.GetPropertyValue(_field.PropertyName);
            if (leftValue == null)
            {
                switch (_op)
                {
                    case Op.Equals:
                        return _fieldValue == null;
                    case Op.GreaterThan:
                        return false;
                    case Op.LessThan:
                        return _fieldValue != null;
                    default:
                        return false;
                }
            }

            IComparable x = businessObject.GetPropertyValue(_field.PropertyName) as IComparable;
            if (x == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Property '{0}' on class '{1}' does not implement IComparable and cannot be matched.", _field.PropertyName,
                        businessObject.GetType().FullName));
            }
            IComparable y = _fieldValue as IComparable;
            if (y is DateTimeToday)
            {
                y = DateTimeToday.Value;
            }
            switch (_op)
            {
                case Op.Equals:
                    return x.Equals(y);
                case Op.GreaterThan:
                    return x.CompareTo(y) > 0;
                case Op.LessThan:
                    return x.CompareTo(y) < 0;
                default:
                    return false;
            }
        }

        ///<summary>
        ///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override string ToString()
        {
            if (IsComposite())
            {
                string leftCriteriaAsString =  LeftCriteria.ToString();
                string rightCriteriaAsString = RightCriteria.ToString();
                return string.Format("({0}) {1} ({2})", leftCriteriaAsString, LogicalOps[(int)LogicalOperator],
                                     rightCriteriaAsString);
            }
            string sourceName = Convert.ToString(_field.Source);
            if (!String.IsNullOrEmpty(sourceName)) sourceName += ".";
            return string.Format("{0}{1} {2} {3}", sourceName, Field.PropertyName, Ops[(int)ComparisonOperator], GetValueAsString());
            //return ToString(delegate(string propName) { return propName; }, delegate(object value)
            //    {
            //        string valueString;
            //        if (value is DateTime)
            //        {
            //            valueString = ((DateTime) value).ToString(DATE_FORMAT);
            //        }
            //        else if (value is Guid)
            //        {
            //            valueString = ((Guid) value).ToString("B");
            //        }
            //        else
            //        {
            //            valueString = value.ToString();
            //        }
            //        return "'" + valueString + "'";
            //    });
        }

        private string GetValueAsString()
        {
            string valueString;
            if (FieldValue is DateTime)
            {
                valueString = ((DateTime)FieldValue).ToString(DATE_FORMAT);
            }
            else if (FieldValue is Guid)
            {
                valueString = ((Guid)FieldValue).ToString("B");
            }
            else
            {
                valueString = FieldValue.ToString();
            }
            return "'" + valueString + "'";
        }


        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            Criteria otherCriteria = obj as Criteria;
            if (otherCriteria == null) return false;
            if (IsComposite())
            {
                if (!otherCriteria.IsComposite()) return false;
                if (!LeftCriteria.Equals(otherCriteria.LeftCriteria)) return false;
                return LogicalOperator == otherCriteria.LogicalOperator && _rightCriteria.Equals(otherCriteria.RightCriteria);
            }
            if (ComparisonOperator != otherCriteria.ComparisonOperator) return false;
            if (String.Compare(Field.PropertyName, otherCriteria.Field.PropertyName) != 0) return false;
            return _fieldValue.Equals(otherCriteria.FieldValue);
        }

        ///<summary>
        ///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Returns whether this is a leaf node (not composite, ie false) or a composite node (true).  This is determined
        /// by whether it has child nodes or not.
        /// </summary>
        /// <returns></returns>
        public bool IsComposite()
        {
            return LeftCriteria != null;
        }

        /// <summary>
        /// Creates a Criteria from an IPrimaryKey object. For each property in the primary key an Equals criteria is created, and these
        /// are all chained together with AND clauses to build up a composite Criteria representing the IPrimaryKey
        /// </summary>
        /// <param name="key">The IPrimaryKey to create Criteria for</param>
        /// <returns>The Criteria this IPrimaryKey instance is defined by</returns>
        public static Criteria FromPrimaryKey(IPrimaryKey key)
        {
            if (key.Count == 1)
            {
                return new Criteria(key[0].PropertyName, Op.Equals, key[0].Value);
            }
            Criteria lastCriteria = null;
            foreach (IBOProp prop in key)
            {
                Criteria propCriteria = new Criteria(prop.PropertyName, Op.Equals, prop.Value);
                lastCriteria = lastCriteria == null
                                   ? propCriteria
                                   : new Criteria(lastCriteria, LogicalOp.And, propCriteria);
            }
            return lastCriteria;
        }

        /// <summary>
        /// Creates a Criteria from an IRelationship object. For each relationship property in the relationship's key a simple 
        /// Equals Criteria object is created, and these are chained together with AND clauses to build up a composite criteria
        /// representing the relationship.
        /// </summary>
        /// <param name="relationship">The IRelationship to create Criteria for</param>
        /// <returns>The Criteria this IRelationship instance is defined by.</returns>
        public static Criteria FromRelationship(IRelationship relationship)
        {
            if (relationship.RelKey.Count == 1)
            {
                IRelProp relProp = relationship.RelKey[0];
                return new Criteria(relProp.RelatedClassPropName, Op.Equals, relProp.BOProp.Value);
            }
            Criteria lastCriteria = null;
            foreach (IRelProp relProp in relationship.RelKey)
            {
                Criteria propCriteria = new Criteria(relProp.RelatedClassPropName, Op.Equals, relProp.BOProp.Value);
                lastCriteria = lastCriteria == null 
                        ? propCriteria 
                        : new Criteria(lastCriteria, LogicalOp.And, propCriteria);
            }
            return lastCriteria;
        }
    }
}