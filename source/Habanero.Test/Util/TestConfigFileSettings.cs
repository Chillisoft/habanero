using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestConfigFileSettings
    {
        [Test]
        public void TestConstructor_ConfigurationSpecified()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestConstructor_FailsWithNullConfiguration()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                ConfigFileSettings configFileSettings = new ConfigFileSettings(null);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Error Expected");
            Assert.IsInstanceOfType(typeof(ArgumentNullException), exception);
            ArgumentNullException argumentNullException = (ArgumentNullException) exception;
            Assert.AreEqual("configuration", argumentNullException.ParamName);
        }

        //---------------- Testing Strings -----------------------------------
        
        [Test]
        public void TestGetString_SettingDNE()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
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
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exception);
            Assert.AreEqual(string.Format("The key '{0}' does not exist in the appSettings configuration section.", settingName), exception.Message);
        }

        [Test]
        public void TestGetString()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            string settingValue = TestUtil.CreateRandomString();
            configuration.AppSettings.Settings.Add(settingName, settingValue);
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            string returnedSettingValue = configFileSettings.GetString(settingName);
            //---------------Test Result -----------------------
            Assert.AreEqual(settingValue, returnedSettingValue);
        }

        [Test]
        public void TestGetString_WithDate()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                configFileSettings.GetString(TestUtil.CreateRandomString(), DateTime.Now);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Error Expected");
            Assert.IsInstanceOfType(typeof(NotSupportedException), exception);
            Assert.AreEqual("ConfigFileSettings does not support date ranging settings.", exception.Message);
        }

        [Test]
        public void TestSetString_AddsSetting()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            string settingValue = TestUtil.CreateRandomString();
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            configFileSettings.SetString(settingName, settingValue);
            //---------------Test Result -----------------------
            KeyValueConfigurationElement configurationElement = configuration.AppSettings.Settings[settingName];
            Assert.IsNotNull(configurationElement, "Setting should have been created");
            Assert.AreEqual(settingValue, configurationElement.Value);
        }

        [Test]
        public void TestSetString_UpdatesSetting()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            string settingValue = TestUtil.CreateRandomString();
            configuration.AppSettings.Settings.Add(settingName, "");
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            configFileSettings.SetString(settingName, settingValue);
            //---------------Test Result -----------------------
            KeyValueConfigurationElement configurationElement = configuration.AppSettings.Settings[settingName];
            Assert.IsNotNull(configurationElement, "Setting should have been created");
            Assert.AreEqual(settingValue, configurationElement.Value);
        }

        //---------------- Testing Decimals -----------------------------------

        [Test]
        public void TestGetDecimal_SettingDNE()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            //---------------Assert Preconditions --------------

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
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exception);
            Assert.AreEqual(string.Format("The key '{0}' does not exist in the appSettings configuration section.", settingName), exception.Message);
        }

        [Test]
        public void TestGetDecimal()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            decimal settingValue = GetRandomDecimal();
            configuration.AppSettings.Settings.Add(settingName, Convert.ToString(settingValue, CultureInfo.InvariantCulture.NumberFormat));
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            decimal returnedSettingValue = configFileSettings.GetDecimal(settingName);
            //---------------Test Result -----------------------
            Assert.AreEqual(settingValue, returnedSettingValue);
        }

        private static decimal GetRandomDecimal()
        {
            Random random = new Random();
            return (decimal) random.NextDouble() * random.Next(100);
        }

        [Test]
        public void TestGetDecimal_WithDate()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                configFileSettings.GetDecimal(TestUtil.CreateRandomString(), DateTime.Now);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNotNull(exception, "Error Expected");
            Assert.IsInstanceOfType(typeof(NotSupportedException), exception);
            Assert.AreEqual("ConfigFileSettings does not support date ranging settings.", exception.Message);
        }

        [Test]
        public void TestSetDecimal_AddsSetting()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            decimal settingValue = GetRandomDecimal();
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            configFileSettings.SetDecimal(settingName, settingValue);
            //---------------Test Result -----------------------
            KeyValueConfigurationElement configurationElement = configuration.AppSettings.Settings[settingName];
            Assert.IsNotNull(configurationElement, "Setting should have been created");
            Assert.AreEqual(settingValue, Convert.ToDecimal(configurationElement.Value, CultureInfo.InvariantCulture.NumberFormat));
        }

        [Test]
        public void TestSetDecimal_UpdatesSetting()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            decimal settingValue = GetRandomDecimal();
            configuration.AppSettings.Settings.Add(settingName, Convert.ToString(0, CultureInfo.InvariantCulture.NumberFormat));
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            configFileSettings.SetDecimal(settingName, settingValue);
            //---------------Test Result -----------------------
            KeyValueConfigurationElement configurationElement = configuration.AppSettings.Settings[settingName];
            Assert.IsNotNull(configurationElement, "Setting should have been created");
            Assert.AreEqual(settingValue, Convert.ToDecimal(configurationElement.Value, CultureInfo.InvariantCulture.NumberFormat));
        }

        //---------------- Testing Booleans -----------------------------------

        [Test]
        public void TestGetBoolean_SettingDNE()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            //---------------Assert Preconditions --------------

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
            Assert.IsInstanceOfType(typeof(InvalidOperationException), exception);
            Assert.AreEqual(string.Format("The key '{0}' does not exist in the appSettings configuration section.", settingName), exception.Message);
        }

        [Test]
        public void TestGetBoolean()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            bool settingValue = GetRandomBoolean();
            configuration.AppSettings.Settings.Add(settingName, Convert.ToString(settingValue, CultureInfo.InvariantCulture.NumberFormat));
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            bool returnedSettingValue = configFileSettings.GetBoolean(settingName);
            //---------------Test Result -----------------------
            Assert.AreEqual(settingValue, returnedSettingValue);
        }

        private static bool GetRandomBoolean()
        {
            return Convert.ToBoolean(new Random().Next(0,1));
        }

        [Test]
        public void TestSetBoolean_AddsSetting()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            bool settingValue = GetRandomBoolean();
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            configFileSettings.SetBoolean(settingName, settingValue);
            //---------------Test Result -----------------------
            KeyValueConfigurationElement configurationElement = configuration.AppSettings.Settings[settingName];
            Assert.IsNotNull(configurationElement, "Setting should have been created");
            Assert.AreEqual(settingValue, Convert.ToBoolean(configurationElement.Value));
        }

        [Test]
        public void TestSetBoolean_UpdatesSetting()
        {
            //---------------Set up test pack-------------------
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSettings configFileSettings = new ConfigFileSettings(configuration);
            string settingName = TestUtil.CreateRandomString();
            bool settingValue = GetRandomBoolean();
            configuration.AppSettings.Settings.Add(settingName, Convert.ToString(true));
            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            configFileSettings.SetBoolean(settingName, settingValue);
            //---------------Test Result -----------------------
            KeyValueConfigurationElement configurationElement = configuration.AppSettings.Settings[settingName];
            Assert.IsNotNull(configurationElement, "Setting should have been created");
            Assert.AreEqual(settingValue, Convert.ToBoolean(configurationElement.Value));
        }
    }
}
