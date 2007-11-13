using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test
{
    public class BeforeSaveBo: BusinessObject
    {
        //This class is not intended to be persisted by the tests

        public Guid? BeforeSaveBoId
        {
            get
            {
                return (Guid?)GetPropertyValue("BeforeSaveBoId");
            }
        }

        public string FirstPart
        {
            get { return GetPropertyValueString("FirstPart"); }
            set { SetPropertyValue("FirstPart", value); }
        }

        public string SecondPart
        {
            get { return GetPropertyValueString("SecondPart"); }
            set { SetPropertyValue("SecondPart", value); }
        }

        public string CombinedParts
        {
            get { return GetPropertyValueString("CombinedParts"); }
            set { SetPropertyValue("CombinedParts", value); }
        }

        protected internal override void BeforeSave(ITransactionCommitter transactionCommiter)
        {
            CombinedParts = FirstPart + SecondPart;
        }

        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""BeforeSaveBo"" assembly=""Habanero.Test"" >
					<property name=""BeforeSaveBoId"" type=""Guid"" />
					<property name=""FirstPart"" />
					<property name=""SecondPart"" />
					<property name=""CombinedParts"" />
					<primaryKey>
						<prop name=""BeforeSaveBoId"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
    }
}
