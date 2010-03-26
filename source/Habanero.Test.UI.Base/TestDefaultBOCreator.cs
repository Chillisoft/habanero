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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestDefaultBOCreator : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            base.SetupDBConnection();
        }
        [Test]
        public void TestCreateBusinessObjectFromCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            new Address();
            new Engine();
            new Car(); new ContactPerson();
            BusinessObjectCollection<ContactPerson> col = new BusinessObjectCollection<ContactPerson>();

            IBusinessObjectCreator boCreator = new DefaultBOCreator(col);

            //---------------Execute Test ----------------------
            ContactPerson cp = (ContactPerson) boCreator.CreateBusinessObject();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(col.Contains(cp));
            //---------------Tear Down -------------------------
        }
    }
}
