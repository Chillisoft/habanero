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
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Serves as a broker between the application and the database, by
    /// loading a specified business object by its ID
    /// </summary>
    public class Broker
    {
        /// <summary>
        /// Constructor to initialise a new instance of the broker
        /// </summary>
        public Broker()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Returns a business object with the ID specified as a Guid
        /// </summary>
        /// <param name="id">The ID as a Guid</param>
        /// <param name="classDef">The class definition</param>
        /// <returns>Returns a business object or null if not found</returns>
        public static BusinessObject GetBusinessObjectWithGuid(Guid id, ClassDef classDef)
        {
            PrimaryKeyDef primaryKeyDef = classDef.GetPrimaryKeyDef();
            if (!primaryKeyDef.IsObjectID)
            {
                throw new HabaneroApplicationException(
                    "GetBusinessObjectWithGuid can only be used for objects that use Guids as primary keys.");
            }
            else
            {
                string primaryKeyField = primaryKeyDef.KeyName;
                BusinessObjectCollection<BusinessObject> col = new BusinessObjectCollection<BusinessObject>(classDef);
                col.Load(primaryKeyField + " = '" + id.ToString("B").ToUpper() + "'", "");
                if (col.Count == 1)
                {
                    return col[0];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}