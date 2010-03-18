namespace Habanero.BO
{
    /// <summary>
    /// This is an interface for Comparable Prop Rule typically Comparable prop rule are Prop Rules that are implemented
    /// for certain types in this case we have implemented them for Decimal, Double, DateTime and Integer.
    /// The IPropRuleComparable has only a MinValue and a MaxValue to Type T.
    /// </summary>
    public interface IPropRuleComparable<T> where T:struct
    {
        /// <summary>
        /// Gets and sets the minimum value that the Double can be assigned
        /// </summary>
        T MinValue { get; }

        /// <summary>
        /// Gets and sets the maximum value that the Double can be assigned
        /// </summary>
        T MaxValue { get; }
    }
}