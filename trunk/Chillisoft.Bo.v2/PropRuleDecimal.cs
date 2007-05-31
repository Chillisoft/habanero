namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Checks decimal values against property rules that test for validity
    /// </summary>
    /// TODO ERIC - where is validity checked?
    public class PropRuleDecimal : PropRuleBase
    {
        private decimal itsMinValue;
        private decimal itsMaxValue;

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="isCompulsory">Whether a value is compulsory and
        /// null values are invalid</param>
        /// <param name="minValue">The minimum value allowed for the decimal</param>
        /// <param name="maxValue">The maximum value allowed for the decimal</param>
        public PropRuleDecimal(string ruleName, bool isCompulsory, decimal minValue, decimal maxValue)
            : base(ruleName, isCompulsory, typeof(decimal))
        {
            itsMinValue = minValue;
            itsMaxValue = maxValue;
        }

        /// <summary>
        /// Gets and sets the minimum value that the decimal can be assigned
        /// </summary>
        public decimal MinValue
        {
            get { return itsMinValue; }
            set { itsMinValue = value; }
        }

        /// <summary>
        /// Gets and sets the maximum value that the decimal can be assigned
        /// </summary>
        public decimal MaxValue
        {
            get { return itsMaxValue; }
            set { itsMaxValue = value; }
        }
    }
}