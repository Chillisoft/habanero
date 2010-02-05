//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.UI.Base;

using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestDataGridViewNumericUpDownColumn
    {
        [SetUp]
        public void SetupTest()
        {
            //ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();



        //TODO: look at creating a NumericUpDown column for VWG
        //[TestFixture]
        //public class TestDataGridViewNumericUpDownColumnVWG : TestDataGridViewNumericUpDownColumn
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryVWG();
        //    }
        //}
    }
}
