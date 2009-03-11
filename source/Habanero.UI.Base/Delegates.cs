namespace Habanero.UI.Base
{
    ///<summary>
    /// A delegate for a function with the specified return type and one parameter of the specified parameter type.
    /// This is the equivalent of the System.Func<T,TReturn> in .Net 3.
    ///</summary>
    ///<param name="arg0">The first argument of the function.</param>
    ///<typeparam name="TArg0">The type of the first argument of the function.</typeparam>
    ///<typeparam name="TReturn">The return type of the function.</typeparam>
    public delegate TReturn Function<TArg0, TReturn>(TArg0 arg0);
}