#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion

using System;
using System.Globalization;
using Habanero.Base.Util;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestConfigFileSettings
    {
        [Test]
        public void TestGetString_SettingDNE()
        {
            //---------------Set up test pack-------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings();
            string settingName = TestUtil.GetRandomString();
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                configFileSettings.GetString(settingName);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Error Expected");
            Assert.IsInstanceOf(typeof(InvalidOperationException), exception);
            Assert.AreEqual(string.Format("The key '{0}' does not exist in the appSettings configuration section.", settingName), exception.Message);
        }

        [Test]
        public void TestGetString()
        {
            //---------------Set up test pack-------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings();
            string settingName = TestUtil.GetRandomString();
            string settingValue = TestUtil.GetRandomString();
            ConfigurationManager.AppSettings[settingName] = settingValue;
            //---------------Execute Test ----------------------
            string returnedSettingValue = configFileSettings.GetString(settingName);
            //---------------Test Result -----------------------
            Assert.AreEqual(settingValue, returnedSettingValue);
        }

        [Test]
        public void TestGetString_WithDate()
        {
            //---------------Set up test pack-------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings();
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                configFileSettings.GetString(TestUtil.GetRandomString(), DateTime.Now);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Error Expected");
            Assert.IsInstanceOf(typeof(NotSupportedException), exception);
            Assert.AreEqual("ConfigFileSettings does not support date ranging settings.", exception.Message);
        }

        [Test]
        public void TestSetString_AddsSetting()
        {
            //---------------Set up test pack-------------------
            var configFileSettings = new ConfigFileSettings();
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.Throws<NotImplementedException>(() => configFileSettings.SetString("foo", "bar"));
        }

        [Test]
        public void TestGetDecimal_SettingDNE()
        {
            //---------------Set up test pack-------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings();
            string settingName = TestUtil.GetRandomString();
            
            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                configFileSettings.GetDecimal(settingName);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Error Expected");
            Assert.IsInstanceOf(typeof(InvalidOperationException), exception);
            Assert.AreEqual(string.Format("The key '{0}' does not exist in the appSettings configuration section.", settingName), exception.Message);
        }

        [Test]
        public void TestGetDecimal()
        {
            //---------------Set up test pack-------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings();
            string settingName = TestUtil.GetRandomString();
            decimal settingValue = GetRandomDecimal();
            ConfigurationManager.AppSettings[settingName] = settingValue.ToString(CultureInfo.InvariantCulture);
            decimal returnedSettingValue = configFileSettings.GetDecimal(settingName);
            //---------------Test Result -----------------------
            Assert.AreEqual(settingValue, returnedSettingValue);
        }

        private static decimal GetRandomDecimal()
        {
            Random random = new Random();
            return (decimal)random.NextDouble() * random.Next(100);
        }

        [Test]
        public void TestGetDecimal_WithDate()
        {
            //---------------Set up test pack-------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings();
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                configFileSettings.GetDecimal(TestUtil.GetRandomString(), DateTime.Now);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Error Expected");
            Assert.IsInstanceOf(typeof(NotSupportedException), exception);
            Assert.AreEqual("ConfigFileSettings does not support date ranging settings.", exception.Message);
        }

        [Test]
        public void TestSetDecimal_AddsSetting()
        {
            //---------------Set up test pack-------------------
            var configFileSettings = new ConfigFileSettings();
            var settingName = TestUtil.GetRandomString();
            var settingValue = GetRandomDecimal();
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.Throws<NotImplementedException>(() => configFileSettings.SetDecimal(settingName, settingValue));
        }

        [Test]
        public void TestGetBoolean_SettingDNE()
        {
            //---------------Set up test pack-------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings();
            string settingName = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                configFileSettings.GetBoolean(settingName);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Error Expected");
            Assert.IsInstanceOf(typeof(InvalidOperationException), exception);
            Assert.AreEqual(string.Format("The key '{0}' does not exist in the appSettings configuration section.", settingName), exception.Message);
        }

        [Test]
        public void TestGetBoolean()
        {
            //---------------Set up test pack-------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings();
            string settingName = TestUtil.GetRandomString();
            bool settingValue = GetRandomBoolean();
            ConfigurationManager.AppSettings[settingName] = settingValue.ToString(CultureInfo.InvariantCulture);
            //---------------Execute Test ----------------------
            bool returnedSettingValue = configFileSettings.GetBoolean(settingName);
            //---------------Test Result -----------------------
            Assert.AreEqual(settingValue, returnedSettingValue);
        }

        private static bool GetRandomBoolean()
        {
            return Convert.ToBoolean(new Random().Next(0, 1));
        }

        [Test]
        public void TestSetBoolean_AddsSetting()
        {
            //---------------Set up test pack-------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings();
            string settingName = TestUtil.GetRandomString();
            bool settingValue = GetRandomBoolean();
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.Throws<NotImplementedException>(() => configFileSettings.SetBoolean(settingName, settingValue));
        }
    }
}
