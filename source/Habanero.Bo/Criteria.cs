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

        ///<summary>
        /// When converting the Criteria expression to a string there are contexts that must be taken into account. The default implementation
        /// (<see cref="Criteria.ToString()"/>) will not do any conversions of property names or values given other than to convert
        /// DateTimes and Guids to sensible, universal string equivalents (see <see cref="Criteria.DATE_FORMAT"/>).
        /// 
        /// This delegate is designed to map a property name to a datasource name (such as to a field name on table).
        ///</summary>
        ///<param name="propName">The property name to map to the datasource equivalent</param>
        /// <returns>The mapped property name ie the datasource equivalent of the property name (perhaps the field name on a database table)</returns>
        public delegate string PropNameConverterDelegate(string propName);

        /// <summary>
        /// For details of what this delegate is used for, see <see cref="PropNameConverterDelegate"/>
        /// 
        /// This delegate is designed to format a value of a criteria into a datasource equivalent. For example, in a database context,
        /// this delegate might be used to return a parameter name and store the parameter value so that the criteria can be used as part
        /// of a where clause in a parametrized SQL statement.
        /// </summary>
        /// <param name="parameterValue">The value of the criteria given, as an object. </param>
        /// <returns>The string format of this criteria to append to the string.</returns>
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

        /// <summary>
        /// Converts this Criteria object to a string, using the two delegates to convert the field name to a mapped field name
        /// and the value in object form to a value in string form.
        /// 
        /// The <see cref="ToString()"/> method uses this method with a simple set of delegates that don't change the field name
        /// and simply convert DateTimes and Guids to sensible string representations.
        /// </summary>
        /// <param name="convertToFieldName">The delegate to use to convert the property name to a field name. 
        /// See <see cref="PropNameConverterDelegate"/></param>
        /// <param name="addParameter">The delegate to use to convert the value in object form to a value in string form. 
        /// See <see cref="AddParameterDelegate"/></param>
        /// <returns>The Criteria in string form.</returns>
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
            return ToString().GetHashCode();
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