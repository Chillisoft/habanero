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
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    public static class BOTestUtils
    {
        private static readonly Random rndm = new Random();

        public static int RandomInt
        {
            get { return rndm.Next(0, 100000); }
        }

        public static string RandomString
        {
            get { return "Rnd" + RandomInt; }
        }

        public static void WaitForGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void AssertBOStateIsValidAfterDelete(IBusinessObject bo)
        {
            Assert.IsTrue(bo.Status.IsNew);
            Assert.IsTrue(bo.Status.IsDeleted);
        }

        public static void DropNewContactPersonAndAddressTables()
        {
            if (ClassDef.ClassDefs.Count > 0 && (ClassDef.ClassDefs.Contains("Habanero.Test.BO", "AddressTestBO")))
            {
                var classDef = ClassDef.Get<AddressTestBO>();
                string defaultCpAddressTableName = "contact_person_address";
                if (classDef.TableName.ToLower() != defaultCpAddressTableName)
                {
                    AddressTestBO.DropCpAddressTable(classDef.TableName);
                }
            }

            if (ClassDef.ClassDefs.Count > 0 && (ClassDef.ClassDefs.Contains("Habanero.Test.BO", "ContactPersonTestBO")))
            {
                var classDef = ClassDef.Get<ContactPersonTestBO>();
                string defaultContactPersonTableName = "contact_person";
                if (classDef.TableName.ToLower() != defaultContactPersonTableName)
                {
                    ContactPersonTestBO.DropContactPersonTable(classDef.TableName);
                }
            }
        }
    }
}