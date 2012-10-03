using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Habanero.Base
{
    /// <summary>
    /// Helper class to facilitate asynchronous workers on pre-dotnet-4.5 machines,
    /// using the native system threading model and an abstraction for synchronisation
    /// with the UI thread for user feedback on completion and exception
    /// </summary>
    public class HabaneroBackgroundWorker
    {
        /// <summary>
        /// The delegate method type to be called to do the long-running background work
        /// </summary>
        /// <param name="data">ConcurrentDictionary containing data which will be passed through 
        ///                     all phases of the worker (background, success, abort, exception)</param>
        /// <returns></returns>
        public delegate bool BackgroundWorkerMethodDelegate(ConcurrentDictionary<string, object> data);
        /// <summary>
        /// The delegate method type to be called to do UI feedback when asynchronous code completes or aborts.
        /// </summary>
        /// <param name="data">ConcurrentDictionary containing data which will be passed through 
        ///                     all phases of the worker (background, success, abort, exception)</param>
        public delegate void UIWorkerMethodDelegate(ConcurrentDictionary<string, object> data);
        /// <summary>
        /// The delegate method type to be called when the background thread hits an exception
        /// </summary>
        /// <param name="ex">Exception hit in the background thread</param>
        public delegate void BackgroundWorkerExceptionHandlerDelegate(Exception ex);

        public IActionDispatcher ActionDispatcher { get; protected set; }
        public BackgroundWorkerMethodDelegate BackgroundWorker { get; set; }
        public UIWorkerMethodDelegate OnSuccess { get; set; }
        public UIWorkerMethodDelegate OnCancelled { get; set; }
        public BackgroundWorkerExceptionHandlerDelegate OnException { get; set; }
        public ConcurrentDictionary<string, object> Data { get; set; }

        private Thread _thread;

        public void Run()
        {
            this._thread = new Thread(this.RunBackgroundWorker);
            this._thread.IsBackground = true;
            this._thread.Start();
        }

        public void WaitForBackgroundWorkerToComplete()
        {
            if (this._thread == null)
                return;
            if (this._thread.IsAlive)
                this._thread.Join();
        }

        protected void RunBackgroundWorker()
        {
            var success = true;
            if (this.BackgroundWorker != null)
            {
                foreach (var del in this.BackgroundWorker.GetInvocationList())
                {
                    object result;
                    try
                    {
                        result = del.DynamicInvoke(this.Data);
                    }
                    catch (Exception ex)
                    {
                        this.RunExceptionDelegate(ex);
                        success = false;
                        break;
                    }
                    try
                    {
                        success = (bool)result;
                        if (!success)
                            break;
                    }
                    catch (Exception)
                    {
                        this.RunExceptionDelegate(new Exception("Delegate method returns a result which cannot be coalesced into a boolean value"));
                        success = false;
                        break;
                    }
                }
            }
            this.RunUIWorkerDelegate(success);
        }

        protected void RunExceptionDelegate(Exception ex)
        {
            if (this.OnException != null)
            {
                this.ActionDispatcher.Dispatch(() => this.OnException(ex));
            }
        }

        protected void RunUIWorkerDelegate(bool success)
        {
            if (success && (this.OnSuccess != null))
            {
                this.ActionDispatcher.Dispatch(() => this.OnSuccess(this.Data));
            }
            else if (this.OnCancelled != null)
            {
                this.ActionDispatcher.Dispatch(() => this.OnCancelled(this.Data));
            }
        }

        public static HabaneroBackgroundWorker Run(IActionDispatcher dispatcher, ConcurrentDictionary<string, object> data, BackgroundWorkerMethodDelegate backgroundWorker, UIWorkerMethodDelegate onSuccess, UIWorkerMethodDelegate onCancel, BackgroundWorkerExceptionHandlerDelegate onException)
        {
            var runner = new HabaneroBackgroundWorker()
                {
                    ActionDispatcher = dispatcher,
                    BackgroundWorker = backgroundWorker,
                    OnSuccess = onSuccess,
                    OnCancelled = onCancel,
                    OnException = onException,
                    Data = data
                };
            runner.Run();
            return runner;
        }
    }
}