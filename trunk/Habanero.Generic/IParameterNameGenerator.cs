namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a generator for parameter names for 
    /// parameterised sql statements
    /// </summary>
    public interface IParameterNameGenerator
    {
        /// <summary>
        /// Generates a parameter name with the current seed value
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetNextParameterName();

        /// <summary>
        /// Sets the parameter count back to zero
        /// </summary>
        void Reset();
    }
}