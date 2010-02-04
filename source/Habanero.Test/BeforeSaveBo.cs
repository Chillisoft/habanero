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
using System;
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

        //protected internal override void BeforeSave(ITransactionCommitter transactionCommiter)
        //{
        //    CombinedParts = FirstPart + SecondPart;
        //}

        ///<summary>
        /// Executes any custom code required by the business object before it is persisted to the database.
        /// This has the additionl capability of creating or updating other business objects and adding these
        /// to the transaction committer.
        /// <remarks> Recursive call to UpdateObjectBeforePersisting will not be done i.e. it is the bo developers responsibility to implement</remarks>
        ///</summary>
        ///<param name="transactionCommitter">the transaction committer that is executing the transaction</param>
        protected internal override void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
        {
            CombinedParts = FirstPart + SecondPart;
        }

        public static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
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
    public class AfterSaveBO : BusinessObject
    {
        //This class is not intended to be persisted by the tests

        public Guid? AfterSaveBoId
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

        /////<summary>
        ///// Executes any custom code required by the business object before it is persisted to the database.
        ///// This has the additionl capability of creating or updating other business objects and adding these
        ///// to the transaction committer.
        ///// <remarks> Recursive call to UpdateObjectBeforePersisting will not be done i.e. it is the bo developers responsibility to implement</remarks>
        /////</summary>
        /////<param name="transactionCommitter">the transaction committer that is executing the transaction</param>
        ////protected internal override void UpdateObjectBeforePersisting(TransactionCommitter transactionCommitter)
        ////{
        ////    CombinedParts = FirstPart + SecondPart;
        ////}
        protected internal override void AfterSave()
        {
            if (this.Status.IsDeleted)
            {
                CombinedParts = "deleted";
            }
            else
            {
                CombinedParts = FirstPart + SecondPart;
            }

        }
        public static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""AfterSaveBO"" assembly=""Habanero.Test"" >
					<property name=""AfterSaveBoId"" type=""Guid"" />
					<property name=""FirstPart"" />
					<property name=""SecondPart"" />
					<property name=""CombinedParts"" />
					<primaryKey>
						<prop name=""AfterSaveBoId"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
    }
}
