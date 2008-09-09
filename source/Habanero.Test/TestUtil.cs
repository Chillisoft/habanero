using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Habanero.Test
{
    public class TestUtil
    {
        public static string CreateRandomString()
        {
            return Guid.NewGuid().ToString("N");

        }

        public static void WaitForGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Returns the number of handlers subscribed to a specified event.
        /// </summary>
        /// <param name="eventSource">The object that has the event</param>
        /// <param name="eventName">The name of the event</param>
        /// <returns>Returns the number of handlers subscribed</returns>
        /// <remarks>
        /// This code was sourced from:
        /// http://www.experts-exchange.com/Microsoft/Development/.NET/Visual_CSharp/Q_22597142.html
        /// </remarks>
        public static int CountEventSubscribers(object eventSource, string eventName)
        {
            //            string name = "EVENT_" + eventName.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            string name = "Event" + eventName;

            int n = 0;

            Type targetType = eventSource.GetType();

            do
            {
                FieldInfo[] fields = targetType.GetFields(
                     BindingFlags.Static |
                     BindingFlags.Instance |
                     BindingFlags.NonPublic);

                foreach (FieldInfo field in fields)
                {
                    if (field.Name == name || field.Name == eventName)
                    {
                        EventHandlerList eventHandlers = ((EventHandlerList)(eventSource.GetType().GetProperty("Events",
                            (BindingFlags.FlattenHierarchy |
                            (BindingFlags.NonPublic | BindingFlags.Instance))).GetValue(eventSource, null)));

                        Delegate d = eventHandlers[field.GetValue(eventSource)];

                        if ((!(d == null)))
                        {
                            Delegate[] subscribers = d.GetInvocationList();

                            foreach (Delegate d1 in subscribers)
                            {
                                n++;
                            }

                            return n;
                        }
                    }
                }

                targetType = targetType.BaseType;

            } while (targetType != null);

            return n;
        }

        /// <summary>
        /// Indicates whether a given method has been subscribed to an event
        /// on a specified object
        /// </summary>
        /// <param name="eventSource">The object that has the event</param>
        /// <param name="eventName">The name of the event</param>
        /// <param name="subscriberMethodName">The method name assigned</param>
        /// <returns>Returns true if assigned, false if not</returns>
        public static bool EventHasSubscriber(object eventSource, string eventName, string subscriberMethodName)
        {
            //            string name = "EVENT_" + eventName.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            string name = "Event" + eventName;

            Type targetType = eventSource.GetType();

            do
            {
                FieldInfo[] fields = targetType.GetFields(
                     BindingFlags.Static |
                     BindingFlags.Instance |
                     BindingFlags.NonPublic);

                foreach (FieldInfo field in fields)
                {
                    if (field.Name == name || field.Name == eventName)
                    {
                        EventHandlerList eventHandlers = ((EventHandlerList)(eventSource.GetType().GetProperty("Events",
                            (BindingFlags.FlattenHierarchy |
                            (BindingFlags.NonPublic | BindingFlags.Instance))).GetValue(eventSource, null)));

                        Delegate d = eventHandlers[field.GetValue(eventSource)];

                        if ((!(d == null)))
                        {
                            Delegate[] subscribers = d.GetInvocationList();

                            foreach (Delegate d1 in subscribers)
                            {
                                if (d1.Method.Name == subscriberMethodName) return true;
                            }

                            return false;
                        }
                    }
                }

                targetType = targetType.BaseType;

            } while (targetType != null);

            return false;
        }
    }
}
