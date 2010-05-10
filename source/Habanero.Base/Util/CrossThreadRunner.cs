// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace Habanero.Util
{
    /// <summary>
    /// This class is used when you specifically require some code method to run in MTA or STA thread.
    /// </summary>
    /// <remarks>
    /// Unfortunately not all test runners implement the [RequireSTA] or other attributes such as config files.
    /// This may result in tests passing in NUnit GUI failing on the build server or failing on another developer's machine
    /// e.g. someone using Resharper. By using this Component you can ensure that the Individual test runs as required.
    /// The common requirement for STA threading is a visual component under test.
    /// </remarks>
    /// <example>
    /// <code>
    ///        [Test]<br/>
    ///        public void Test_SetSolution_WithNull_ShouldRaiseError()<br/>
    ///        {<br/>
    ///            CrossThreadRunner runner = new CrossThreadRunner();<br/>
    ///            runner.RunInSTA(delegate<br/>
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
    /// </code>
    /// </example> 
    public class CrossThreadRunner
    {
        private Exception _lastException;

        ///<summary>
        /// Run the specified delegate in the MTA Thread.
        ///</summary>
        ///<param name="userDelegate">The delegate to be run in the MTA Thread</param>
        public void RunInMTA(ThreadStart userDelegate)
        {
            Run(userDelegate, ApartmentState.MTA);
        }

        /// <summary>
        /// Run the specified funcion delegate in the MTA Thread and return the return value of the function.
        /// </summary>
        /// <param name="function">The function delegate to be run in the MTA Thread</param>
        /// <returns>The return value of the function</returns>
        public TReturn RunInMTA<TReturn>(Function<TReturn> function)
        {
            TReturn returnValue = default(TReturn);
            RunInMTA(delegate
            {
                returnValue = function();
            });
            return returnValue;
        }

        ///<summary>
        /// Run the specified delegate in the STA Thread.
        ///</summary>
        ///<param name="userDelegate">The delegate to be run in the STA Thread</param>
        public void RunInSTA(ThreadStart userDelegate)
        {
            Run(userDelegate, ApartmentState.STA);
        }

        /// <summary>
        /// Run the specified funcion delegate in the STA Thread and return the return value of the function.
        /// </summary>
        /// <param name="function">The function delegate to be run in the STA Thread</param>
        /// <returns>The return value of the function</returns>
        public TReturn RunInSTA<TReturn>(Function<TReturn> function)
        {
            TReturn returnValue = default(TReturn);
            RunInSTA(delegate
            {
                returnValue = function();
            });
            return returnValue;
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
