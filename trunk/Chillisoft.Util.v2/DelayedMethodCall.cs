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
        private readonly double itsDelayInMilliseconds;
        private readonly object itsCaller;
        private VoidMethodWithSender itsMethodToCall;
        private Timer itsTimer;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        /// <param name="delayInMilliseconds">The delay in milliseconds</param>
        /// <param name="caller">The caller</param>
        public DelayedMethodCall(double delayInMilliseconds, object caller)
        {
            itsDelayInMilliseconds = delayInMilliseconds;
            itsCaller = caller;
        }

        /// <summary>
        /// Handles the event of an elapsed timer, making the method call
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void TimerElapsedHandler(object sender, ElapsedEventArgs e)
        {
            itsMethodToCall(itsCaller);
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
            if (itsTimer != null)
            {
                itsTimer.Stop();
                itsTimer.Elapsed -= new ElapsedEventHandler(TimerElapsedHandler);
            }
            itsMethodToCall = methodToCall;
            itsTimer = new Timer(itsDelayInMilliseconds);
            itsTimer.AutoReset = false;
            itsTimer.Elapsed += new ElapsedEventHandler(TimerElapsedHandler);
            itsTimer.Start();
        }
    }
}