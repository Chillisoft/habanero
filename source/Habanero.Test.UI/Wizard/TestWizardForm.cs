//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Habanero.UI.Wizard;

namespace Habanero.Test.UI.Wizard
{
    [TestFixture]
    public class TestWizardForm
    {
        [Test]
        public void TestFinish()
        {
            TestWizardControl.MyWizardController _wizardController = new TestWizardControl.MyWizardController();
            WizardForm wizardForm = new WizardForm(_wizardController);
            wizardForm.Show();
            Assert.IsTrue(_wizardController.IsFirstStep());
            wizardForm.WizardControl.Next();
            Assert.IsTrue(_wizardController.IsLastStep());
            wizardForm.WizardControl.Finish();
            Assert.IsFalse(wizardForm.Visible);
        }
    }
}
