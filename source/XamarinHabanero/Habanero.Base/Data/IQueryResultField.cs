namespace Habanero.Base.Data
{
    /// <summary>
    /// A field for a <see cref="IQueryResult"/>.
    /// </summary>
    public interface IQueryResultField
    {
        /// <summary>
        /// The name of the field
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// The index of the field (position in the result set)
        /// </summary>
        int Index { get; set; }
    }
}