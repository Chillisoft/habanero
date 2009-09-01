using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Habanero.Testing.Base
{
    /// <summary>
    /// This class is used when you specifically require a single test method to run in MTA or STA thread.
    /// Unfortunately not all test runners implement the [RequireSTA] or other attributes such as config files.
    /// This may result in tests passing in NUnit GUI failing on the build server or failing on another developer's machine
    /// e.g. someone using Resharper. By using this Component you can ensure that the Individual test runs as required.
    /// The common requirement for STA threading is a visual component under test.
    /// <example>
    /// <code>
    ///        [Test]<br/>
    ///        public void Test_SetSolution_WithNull_ShouldRaiseError()<br/>
    ///        {<br/>
    ///            CrossThreadTestRunner runner = new CrossThreadTestRunner();<br/>
    ///            runner.RunInSTA(<br/>
    ///                delegate<br/>
    ///                    {<br/>
    ///                        //---------------Set up test pack-------------------<br/>
    ///                        ReportSourceDefCreatorControl creatorControl = new ReportSourceDefCreatorControl();<br/>
    ///                        //---------------Assert Precondition----------------<br/>
    ///                        //---------------Execute Test ----------------------<br/>
    ///                        try<br/>
    ///                        {<br/>
    ///                            creatorControl.SetSolution(null);<br/>
    ///                            Assert.Fail("expected ArgumentNullException");<br/>
    ///                        }<br/>
    ///                            //---------------Test Result -----------------------<br/>
    ///                        catch (ArgumentNullException ex)<br/>
    ///                        {<br/>
    ///                            StringAssert.Contains("Value cannot be null", ex.Message);<br/>
    ///                            StringAssert.Contains("solution", ex.ParamName);<br/>
    ///                        }<br/>
    ///                    });<br/>
    ///        }<br/>
    /// </code></example> 
    /// </summary>
    public class CrossThreadTestRunner
    {
        private Exception _lastException;

        public void RunInMTA(ThreadStart userDelegate)
        {
            Run(userDelegate, ApartmentState.MTA);
        }

        public void RunInSTA(ThreadStart userDelegate)
        {
            Run(userDelegate, ApartmentState.STA);
        }

        private void Run(ThreadStart userDelegate, ApartmentState apartmentState)
        {
            _lastException = null;

            Thread thread = new Thread(() => MultiThreadedWorker(userDelegate));
            thread.SetApartmentState(apartmentState);

            thread.Start();
            thread.Join();

            if (ExceptionWasThrown())
                ThrowExceptionPreservingStack(_lastException);
        }

        private void MultiThreadedWorker(ThreadStart userDelegate)
        {
            try
            {
                userDelegate.Invoke();
            }
            catch (Exception e)
            {
                _lastException = e;
            }
        }

        private bool ExceptionWasThrown()
        {
            return _lastException != null;
        }

        [ReflectionPermission(SecurityAction.Demand)]
        private static void ThrowExceptionPreservingStack(Exception exception)
        {
            FieldInfo remoteStackTraceString = typeof(Exception).GetField(
                "_remoteStackTraceString",
                BindingFlags.Instance | BindingFlags.NonPublic);
            remoteStackTraceString.SetValue(exception, exception.StackTrace + Environment.NewLine);
            throw exception;
        }
    }
}
