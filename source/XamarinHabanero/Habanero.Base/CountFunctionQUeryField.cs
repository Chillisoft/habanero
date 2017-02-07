namespace Habanero.Base
{
    /// <summary>
    /// QueryFunctionField which implements the COUNT function, on the mentioned field
    ///     or on * (which is the default)
    /// </summary>
    public class CountFunctionQueryField: FunctionQueryField
    {

        /// <summary>
        /// Creates a CountFunctionQueryField with the given field to count over. Input
        /// field may be anything that FunctionQueryField supports as a function argument
        /// (currently strings and QueryField instances). When not provided, the * field
        /// is used for you
        /// </summary>
        /// <param name="field">The Name of the field to use. Omit for default (*)</param>
        public CountFunctionQueryField(object field = null): base("count", FieldOrDefault(field))
        {
        }

        private static object FieldOrDefault(object input)
        {
            var queryField = input as QueryField;
            if (queryField != null) return queryField;
            return new QueryField("", "*", null);
        }

    }
}
