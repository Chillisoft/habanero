using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.BO
{
    class ContactPerson: BusinessObject
    {
        public ContactPerson() : base() { }

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
					<property  name=""Surname"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>


			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return itsClassDef;
        }


    }
}
