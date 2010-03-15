using System;
using System.Linq.Expressions;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO.Rules
{
#pragma warning disable 1591
    ///<summary>
    /// These operators are used for Comparing Properties
    /// using the <see cref="InterPropRule"/>
    ///</summary>
    public enum ComparisonOperator
    {
        GreaterThan,
        GreaterThanOrEqual,
        EqualTo,
        LessThanOrEqual,
        LessThan
    }
#pragma warning restore 1591

    /// <summary>
    /// This Rule is used for the Common Scenario where
    /// the One properties Validity is based on another Properties Value.
    /// E.g. DisposalDate must be Greater Than AcquisitionDate
    /// 
    /// In the above case the Rule will be on the Disposal Date.
    /// The other Prop will be the AcquisitionDate Prop and
    ///   the ComparisonOperator Will be GreaterThan
    /// 
    /// </summary>
    public class InterPropRule : IBusinessObjectRule
    {
        private IComparable _prop1Value;
        private IComparable _prop2Value;
        /// <summary>
        /// The Left Prop Being Compared e.g. LeftProp LessThan RightProp
        /// </summary>
        public IPropDef LeftProp { get; protected set; }
        ///<summary>
        /// The ComparisonOperator e.g. e.g. LeftProp LessThan RightProp
        ///</summary>
        public ComparisonOperator ComparisonOp { get; private set; }
        /// <summary>
        /// The Right Prop being compared e.g. LeftProp LessThan RightProp
        /// </summary>
        public IPropDef RightProp { get; protected set; }

        ///<summary>
        ///</summary>
        ///<param name="propLeft"></param>
        ///<param name="comparisonOperator"></param>
        ///<param name="propRight"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public InterPropRule(IPropDef propLeft, ComparisonOperator comparisonOperator, IPropDef propRight)
        {
            if (propLeft == null) throw new ArgumentNullException("propLeft");
            if (propRight == null) throw new ArgumentNullException("propRight");
            LeftProp = propLeft;
            ComparisonOp = comparisonOperator;
            RightProp = propRight;
        }
        protected InterPropRule( ComparisonOperator comparisonOperator)
        {
            ComparisonOp = comparisonOperator;
        }
        /// <summary>
        /// This is not implemented for InterPropRules since it is obsolete.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Determines whether the Expression LeftProp ComparisonOperator RightProp is true or false.
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool IsValid(IBusinessObject bo)
        {
            _prop1Value = (IComparable) bo.GetPropertyValue(this.LeftProp.PropertyName);
            _prop2Value = (IComparable) bo.GetPropertyValue(this.RightProp.PropertyName);
            if(_prop1Value == null || _prop2Value == null) return true;
            int compareTo = _prop1Value.CompareTo(_prop2Value);
            if (this.ComparisonOp == ComparisonOperator.LessThan) return compareTo <= -1;
            if (this.ComparisonOp == ComparisonOperator.LessThanOrEqual) return compareTo <= 0;
            if (this.ComparisonOp == ComparisonOperator.EqualTo) return compareTo == 0;
            if (this.ComparisonOp == ComparisonOperator.GreaterThan) return compareTo > 0;
            return compareTo > -1;
        }

        /// <summary>
        /// Returns the rule name
        /// </summary>
        public string Name
        {
            get { return this.LeftProp.PropertyName + " Is " + this.ComparisonOp + " " + this.RightProp.PropertyName; }
        }

        /// <summary>
        /// Returns the error message for if the rule fails.
        /// </summary>
        public string Message
        {
            get
            {
                return string.Format("Property '{0}' with value '{1}'  should "
                                     + "be {4} property '{2}' with value '{3}'"
                                     , LeftProp.PropertyName, _prop1Value, RightProp.PropertyName, _prop2Value,
                                     this.ComparisonOp);
            }
        }

        /// <summary>
        /// The <see cref="IBusinessObjectRule.ErrorLevel"/> for this BusinessObjectRule e.g. Warning, Error. 
        /// </summary>
        public ErrorLevel ErrorLevel
        {
            get { return Base.ErrorLevel.Error; }
        }
    }
    /// <summary>
    /// This is a Generic version of <see cref="InterPropRule"/>
    /// </summary>
    public class InterPropRule<T> : InterPropRule where T: IBusinessObject
    {
        ///<summary>
        /// Basic Constructor for the Generic InterPropRule
        ///</summary>
        ///<param name="propLeft"></param>
        ///<param name="comparisonOperator"></param>
        ///<param name="propRight"></param>
        ///<exception cref="ArgumentNullException">If either propLeft or propRight are null</exception>
        public InterPropRule(IPropDef propLeft, ComparisonOperator comparisonOperator, IPropDef propRight)
            : base(propLeft, comparisonOperator, propRight)
        {
        }

        ///<summary>
        /// Overloaded Constructor for the Generic InterPropRule
        ///</summary>
        ///<param name="propExpressionLeft"></param>
        ///<param name="comparisonOperator"></param>
        ///<param name="propExpressionRight"></param>
        public InterPropRule(Expression<Func<T, object>> propExpressionLeft, ComparisonOperator comparisonOperator, Expression<Func<T, object>> propExpressionRight)
            : base( comparisonOperator)
        {
            if (propExpressionLeft == null) throw new ArgumentNullException("propExpressionLeft");
            if (propExpressionRight == null) throw new ArgumentNullException("propExpressionRight");
            string propertyNameLeft = ReflectionUtilities.GetPropertyName(propExpressionLeft);
            string propertyNameRight = ReflectionUtilities.GetPropertyName(propExpressionRight);
            IClassDef classDef = ClassDef.ClassDefs[typeof (T)];

            this.LeftProp = classDef.PropDefcol[propertyNameLeft];
            this.RightProp = classDef.PropDefcol[propertyNameRight];
        }
    }
}