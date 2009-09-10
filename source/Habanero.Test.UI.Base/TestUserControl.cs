// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the UserControl class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_UserControl : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return new UserControlWin();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the UserControl class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_UserControl : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return new UserControlVWG();
        }
    }

    /// <summary>
    /// This test class tests the UserControl class.
    /// </summary>
    [TestFixture]
    public class TestUserControl
    {
        
    }
}
