using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;

namespace Habanero.Test.Bo
{
    class ContactPerson: BusinessObject
    {
        public ContactPerson() : base() { }

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
				<class name=""ContactPerson"" assembly=""Habanero.Test.Bo"">
					<property  name=""ContactPersonID"" />
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
