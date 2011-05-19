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
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test
{
    public class TestUtil
    {
        private static readonly Random rnd = new Random();

        public static string GetRandomString()
        {
            return GetRandomString(10);
        }

        public static string GetRandomString(int length)
        {
            var str = "Rnd" + GetRandomInt();
            while (str.Length < length)
            {
                str += GetRandomInt();
            }
            return str.Substring(0, length);
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
                FieldInfo[] fields = targetType.GetFields
                    (BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (FieldInfo field in fields)
                {
                    if (field.Name == name || field.Name == eventName)
                    {
                        EventHandlerList eventHandlers =
                            ((EventHandlerList)
                             (eventSource.GetType().GetProperty
                                 ("Events",
                                  (BindingFlags.FlattenHierarchy | (BindingFlags.NonPublic | BindingFlags.Instance))).
                                 GetValue(eventSource, null)));

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
                FieldInfo[] fields = targetType.GetFields
                    (BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (FieldInfo field in fields)
                {
                    if (field.Name == name || field.Name == eventName)
                    {
                        EventHandlerList eventHandlers =
                            ((EventHandlerList)
                             (eventSource.GetType().GetProperty
                                 ("Events",
                                  (BindingFlags.FlattenHierarchy | (BindingFlags.NonPublic | BindingFlags.Instance))).
                                 GetValue(eventSource, null)));

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

        public static int GetRandomInt()
        {
            return rnd.Next();
        }

        public static int GetRandomInt(int max)
        {
            return rnd.Next(0, max);
        }

        public static int GetRandomInt(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public static decimal GetRandomDecimal()
        {
            return (decimal) rnd.NextDouble();
        }

        public static TEnum GetRandomEnum<TEnum>()
            where TEnum : struct
        {
            return GetRandomEnum<TEnum>(null);
        }

        public static TEnum GetRandomEnum<TEnum>(TEnum? excluded)
            where TEnum : struct
        {
            Array values = Enum.GetValues(typeof(TEnum));
            int randomIndex = GetRandomInt(0, values.Length);
            TEnum value = (TEnum)values.GetValue(randomIndex);
            if (excluded.HasValue && excluded.Value.Equals(value))
            {
                return GetRandomEnum(excluded);
            }
            return value;
        }

        public static DatabaseConfig GetDatabaseConfig() {
            return new DatabaseConfig("MySql", "localhost", "habanero_test_trunk", "root", "root", "3306");
        }

        public static bool GetRandomBoolean()
        {
            return (GetRandomInt(100000) > 50000);
        }

        
        public static void AssertStringEmpty(string stringValue, string stringName)
        {
            AssertStringEmpty(stringValue, stringName, stringName + " Should not be empty");
        }

        public static void AssertStringNotEmpty(string stringValue, string stringName)
        {
            Assert.IsFalse(String.IsNullOrEmpty(stringValue), stringName + " Should not be null");
        }

        public static void AssertStringEmpty(string stringValue, string stringName, string errorMessage)
        {
            Assert.IsTrue(String.IsNullOrEmpty(stringValue), stringName + " : " + errorMessage);
        }

        public static void Wait(int timeout)
        {
            Thread.Sleep(timeout);
        }

        public static T AssertIsInstanceOf<T>(object obj)
        {
            Assert.IsInstanceOf(typeof(T), obj);
            return (T)obj;
        }

    	public static TimeSpan GetRandomTimeSpan()
    	{
    		return GetRandomTimeSpan(GetRandomInt());
    	}

    	public static TimeSpan GetRandomTimeSpan(int days)
    	{
    		return new TimeSpan(days, GetRandomInt(0, 23), GetRandomInt(0, 59), GetRandomInt(0, 59), GetRandomInt(0, 999));
    	}
    }
}