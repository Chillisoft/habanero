using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.Test.BO
{
    class ContactPerson: BusinessObject
    {
        public ContactPerson() : base() { }

        internal ContactPerson(BOPrimaryKey id)
            : base(id)
        {
        }

        public Guid ContactPersonID
        {
            get { return (Guid) this.GetPropertyValue("ContactPersonID"); }
        }

        public override string ToString()
        {
            return (string)this.GetPropertyValue("Surname");
        }
        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPerson"" assembly=""Habanero.Test.BO"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>


			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return itsClassDef;
        }

        public string Surname
        {
            get { return GetPropertyValueString("Surname"); }
            set { SetPropertyValue("Surname", value); }
        }

        /// <summary>
        /// returns the ContactPerson identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object primary Key</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Habanero.BO.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static ContactPerson GetContactPerson(BOPrimaryKey id)
        {
            ContactPerson myContactPerson = (ContactPerson)BOLoader.Instance.GetLoadedBusinessObject(id);
            if (myContactPerson == null)
            {
                myContactPerson = new ContactPerson(id);
            }
            return myContactPerson;
        }
        internal static void DeleteAllContactPeople()
        {
            string sql = "DELETE FROM ContactPerson";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }


    }
}
