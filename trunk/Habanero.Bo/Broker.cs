using System;
using Habanero.Bo.ClassDefinition;
using Habanero.Generic;

namespace Habanero.Bo
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
            if (!classDef.PrimaryKeyDef.IsObjectID)
            {
                throw new HabaneroApplicationException(
                    "GetBusinessObjectWithGuid can only be used for objects that use Guids as primary keys.");
            }
            else
            {
                string primaryKeyField = classDef.PrimaryKeyDef.KeyName;
                BusinessObjectCollection col = new BusinessObjectCollection(classDef);
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