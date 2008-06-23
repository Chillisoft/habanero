using System;
using System.Collections.Generic;
using System.Globalization;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// A criteria expression that can be used to build up a criteria tree. The IsMatch method can then be
    /// used to check whether a BusinessObject matches the criteria.
    /// </summary>
    public class Criteria
    {
        public const string DATE_FORMAT = "yyyy/MM/dd HH:mm:ss";
    
        private readonly Criteria _leftCriteria;
        private readonly LogicalOp _logicalOp;
        private readonly Criteria _rightCriteria;
        private readonly string _propName;
        private readonly Op _op;
        private readonly object _value;
        private readonly string[] Ops = { "=", ">", "<" } ;
        private readonly string[] LogicalOps = {"AND", "OR"};

        public delegate string PropNameConverterDelegate(string propName);
        public delegate string AddParameterDelegate(object parameterValue);

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

        /// <summary>
        /// Creates a leaf criteria (meaning it has no children in the tree structure).
        /// </summary>
        /// <param name="propName">The property whose value to check</param>
        /// <param name="op">The operator to use to compare the property value to the given value</param>
        /// <param name="value">The value to compare to</param>
        public Criteria(string propName, Op op, object value)
        {
            _propName = propName;
            _op = op;
            _value = value;
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
                    case LogicalOp.And: return _leftCriteria.IsMatch(businessObject) && _rightCriteria.IsMatch(businessObject);
                    case LogicalOp.Or: return _leftCriteria.IsMatch(businessObject) || _rightCriteria.IsMatch(businessObject);
                }

            }

            object leftValue = businessObject.GetPropertyValue(_propName);
            if (leftValue == null)
            {
                switch (_op)
                {
                    case Op.Equals:
                        return _value == null;
                    case Op.GreaterThan:
                        return false;
                    case Op.LessThan:
                        return _value != null;
                    default:
                        return false;
                } 
            }

            IComparable x = businessObject.GetPropertyValue(_propName) as IComparable;
            if (x == null)
            {
                throw new InvalidOperationException(string.Format("Property '{0}' on class '{1}' does not implement IComparable and cannot be matched.", _propName, businessObject.GetType().FullName));
            }
            IComparable y = _value as IComparable;
            switch (_op)
            {
                case Op.Equals:
                    return (businessObject.GetPropertyValue(_propName).Equals(_value));
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
            return ToString(delegate(string propName) { return propName; }, delegate(object value)
            {
                string valueString;
                if (value is DateTime)
                {
                    valueString = ((DateTime)value).ToString(DATE_FORMAT);
                }
                else if (value is Guid)
                {
                    valueString = ((Guid)value).ToString("B");
                }
                else
                {
                    valueString = value.ToString();
                }
                return "'" + valueString + "'";
            });
        }


        public string ToString(PropNameConverterDelegate convertToFieldName, AddParameterDelegate addParameter)
        {
            if (IsComposite())
            {
                return string.Format("({0}) {1} ({2})", _leftCriteria, LogicalOps[(int)_logicalOp], _rightCriteria);
            }
            string valueString = addParameter(_value);
            return string.Format("{0} {1} {2}", convertToFieldName(_propName), Ops[(int)_op], valueString);

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
                if (!_leftCriteria.Equals(otherCriteria._leftCriteria)) return false;
                if (_logicalOp != otherCriteria._logicalOp) return false;
                if (!_rightCriteria.Equals(otherCriteria._rightCriteria)) return false;
                return true;
            }
            if (_op != otherCriteria._op) return false;
            if (String.Compare(_propName, otherCriteria._propName) != 0) return false;
            if (! _value.Equals(otherCriteria._value)) return false;
            return true;
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
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns whether this is a leaf node (not composite, ie false) or a composite node (true).  This is determined
        /// by whether it has child nodes or not.
        /// </summary>
        /// <returns></returns>
        public bool IsComposite()
        {
            return _leftCriteria != null;
        }

        public static Criteria FromPrimaryKey(IPrimaryKey key)
        {
            if (key.Count == 1)
            {
                return new Criteria(key[0].PropertyName, Op.Equals, key[0].Value);
            } else
            {
                Criteria lastCriteria = null;
                foreach (IBOProp prop in key)
                {
                    Criteria propCriteria = new Criteria(prop.PropertyName, Op.Equals, prop.Value);
                    if (lastCriteria == null) lastCriteria = propCriteria;
                    else lastCriteria = new Criteria(lastCriteria, LogicalOp.And, propCriteria);
                }
                return lastCriteria;
            }
        }

        public static Criteria FromRelationship(IRelationship relationship)
        {
            if (relationship.RelKey.Count == 1)
            {
                IRelProp relProp = relationship.RelKey[0];
                return new Criteria(relProp.RelatedClassPropName, Op.Equals, relProp.BOProp.Value);
            } else
            {
                Criteria lastCriteria = null;
                foreach (IRelProp relProp in relationship.RelKey)
                {
                    Criteria propCriteria = new Criteria(relProp.RelatedClassPropName, Op.Equals, relProp.BOProp.Value);
                    if (lastCriteria == null) lastCriteria = propCriteria;
                    else lastCriteria = new Criteria(lastCriteria, LogicalOp.And, propCriteria);
                }
                return lastCriteria;
            }
        }
    }
}