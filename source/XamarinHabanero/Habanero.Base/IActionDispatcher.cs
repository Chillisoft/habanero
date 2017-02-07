using System;

namespace Habanero.Base
{
    /// <summary>
    /// An interface that allows you to execute a method (Action in a specified way e.g. on a background thread.
    /// </summary>
    public interface IActionDispatcher
    {
        /// <summary>
        /// executed the method in a specified manner e.g. on a seperate thread
        /// </summary>
        /// <param name="method"></param>
        void Dispatch(Action method);
    }
}