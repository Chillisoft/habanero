using System;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// An interface that models a date provider
    /// </summary>
    public interface IDateProvider
    {
        /// <summary>
        /// Returns a date
        /// </summary>
        /// <returns>Returns a DateTime object</returns>
        DateTime GetDate();
    }
}