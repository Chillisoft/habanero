using System;

namespace Habanero.Base.Logging
{
    ///<summary>
    ///</summary>
    public interface IHabaneroLoggerFactory
    {
        ///<summary>
        /// Creates the Appropriate <see cref="IHabaneroLogger"/>
        /// based on the LoggerFactory you have implemented.
        /// By Default Habanero uses the <see cref="Log4NetLoggerFactory"/>.
        /// You can change the logger factor by changing your the BootStrapper code when
        /// Your Application starts up or by creating your own <see cref="HabaneroApp"/>
        /// and overriding the <see cref="HabaneroApp.SetupLogging"/> or by calling
        /// the following code in your app startup.
        /// <code>
        /// GlobalRegistry.LoggerFactory = new Log4NetLoggerFactory();
        /// </code>
        ///</summary>
        ///<param name="contextName"></param>
        ///<returns></returns>
        IHabaneroLogger GetLogger(string contextName);

        ///<summary>
        /// Creates the Appropriate <see cref="IHabaneroLogger"/>
        /// based on the LoggerFactory you have implemented.
        /// See <see cref="GetLogger(string)"/> form more details
        ///</summary>
        ///<param name="type">The Type of BO this log is for</param>
        ///<returns></returns>
        IHabaneroLogger GetLogger(Type type);
    }
}