using System.Timers;
using log4net;

namespace Chillisoft.Util.v2
{
    public delegate void VoidMethodWithSender(object sender);

    /// <summary>
    /// Provides a facility for making a method call but having its actual
    /// execution delayed by a set period of time
    /// </summary>
    public class DelayedMethodCall
    {
        private static ILog log = LogManager.GetLogger("CorChillisoftil.DelayedMethodCall");
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
                _timer.Elapsed -= new ElapsedEventHandler(TimerElapsedHandler);
            }
            _methodToCall = methodToCall;
            _timer = new Timer(_delayInMilliseconds);
            _timer.AutoReset = false;
            _timer.Elapsed += new ElapsedEventHandler(TimerElapsedHandler);
            _timer.Start();
        }
    }
}