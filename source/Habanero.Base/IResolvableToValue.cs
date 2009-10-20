namespace Habanero.Base
{
    ///<summary>
    /// Defines a generalised type that can be resolved to a value
    ///</summary>
    public interface IResolvableToValue : IResolvableToValue<object> { }

    ///<summary>
    /// Defines a generalised type that can be resolved to a value of type <see cref="T"/>
    ///</summary>
    ///<typeparam name="T">The value type that this object can be resolved to.</typeparam>
    public interface IResolvableToValue<T>
    {
        ///<summary>
        /// Resolved the instance class to a value of type <see cref="T"/>.
        ///</summary>
        ///<returns>The value that the instance class is resolved to.</returns>
        T ResolveToValue();
    }
}