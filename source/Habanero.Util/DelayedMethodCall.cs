//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Timers;
using log4net;

namespace Habanero.Util
{
    public delegate void VoidMethodWithSender(object sender);

    /// <summary>
    /// Provides a facility for making a method call but having its actual
    /// execution delayed by a set period of time
    /// </summary>
    public class DelayedMethodCall
    {
        private static ILog log = LogManager.GetLogger("Habanero.Util.DelayedMethodCall");
        private readonly double _delayInMilliseconds;
        private readonly object _caller;
        private VoidMethodWithSender _methodToCall;
        private Timer _timer;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        /// <param name="delayInMilliseconds">The delay in milliseconds</param>
        /// <param name="caller">The caller</param>
        public DelayedMethodCall(double delayInMilliseconds, object caller)
        {
            _delayInMilliseconds = delayInMilliseconds;
            _caller = caller;
        }

        /// <summary>
        /// Handles the event of an elapsed timer, making the method call
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void TimerElapsedHandler(object sender, ElapsedEventArgs e)
        {
            _methodToCall(_caller);
        }

        /// <summary>
        /// Calls a method, but setting the delay timer going first, after
        /// which the method will be called
        /// </summary>
        /// <param name="methodToCall">The method to call once the timer has
        /// elapsed</param>
        public void Call(VoidMethodWithSender methodToCall)
        {
            //log.Debug("Delayed method call requested: " + methodToCall.Method.Name ) ;
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= TimerElapsedHandler;
            }
            _methodToCall = methodToCall;
            _timer = new Timer(_delayInMilliseconds);
            _timer.AutoReset = false;
            _timer.Elapsed += TimerElapsedHandler;
            _timer.Start();
        }
    }
}