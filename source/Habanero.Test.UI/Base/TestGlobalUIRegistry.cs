//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.UI;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestGlobalUIRegistry
    {
        private IUISettings _uiSettings = GlobalUIRegistry.UISettings;
        private DateDisplaySettings _dateDisplaySettings = GlobalUIRegistry.DateDisplaySettings;
        
        [SetUp]
        public void ResetRegistry()
        {
            GlobalUIRegistry.UISettings = _uiSettings;
            GlobalUIRegistry.DateDisplaySettings = _dateDisplaySettings;
        }

        [TearDown]
        public void RestoreRegistry()
        {
            GlobalUIRegistry.UISettings = _uiSettings;
            GlobalUIRegistry.DateDisplaySettings = _dateDisplaySettings;
        }

        [Test]
        public void TestGetsAndSetsOnUISettings()
        {
            GlobalUIRegistry.UISettings = new UISettings();
            Assert.IsNull(GlobalUIRegistry.UISettings.PermitComboBoxRightClick);

            GlobalUIRegistry.UISettings.PermitComboBoxRightClick += delegate { return false; };
            Assert.IsNotNull(GlobalUIRegistry.UISettings.PermitComboBoxRightClick);
            Assert.IsFalse(GlobalUIRegistry.UISettings.PermitComboBoxRightClick(typeof(String), null));

            GlobalUIRegistry.UISettings.PermitComboBoxRightClick += delegate { return true; };
            Assert.IsTrue(GlobalUIRegistry.UISettings.PermitComboBoxRightClick(typeof(String), null));
        }

        [Test]
        public void TestGetsAndSetsOnDateDisplaySettings()
        {
            GlobalUIRegistry.DateDisplaySettings = new DateDisplaySettings();
            Assert.IsNull(GlobalUIRegistry.DateDisplaySettings.GridDateFormat);

            GlobalUIRegistry.DateDisplaySettings.GridDateFormat = "ddMMyy";
            Assert.AreEqual("ddMMyy", GlobalUIRegistry.DateDisplaySettings.GridDateFormat);
        }
    }
}
