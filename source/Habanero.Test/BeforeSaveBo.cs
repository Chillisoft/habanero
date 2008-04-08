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
