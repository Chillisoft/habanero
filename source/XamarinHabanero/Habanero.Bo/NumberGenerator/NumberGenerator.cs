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
using Habanero.BO.Loaders;

namespace Habanero.BO
{
    /// <summary>
    /// This is a simple number generator class. This class implements no locking strategy.
    /// I.e. If two users try to retrieve a number of the same type concurrently then they
    /// will retrieve the same number. The object that the number is being used for e.g. member number
    /// will raise an error when that object is being saved if the member number is an alternate key.
    /// There is also no guarantee that the number will not have missing numbers in the sequence.
    /// E.g. member 1 asks for a number and gets 0001, member2 gets 0002 and member3 gets 0003.
    /// member3 is saved and member2 and member1 are not then there will be missing numbers in the sequence.
    /// If you require a number generator with a locking strategy then look at
    ///  Habanero.DB.NumberGeneratorPessimisticLocking. You can also implement your own number generator that 
    /// implements INumberGenerator.
    /// </summary>
    public class NumberGenerator : INumberGenerator
    {
        private readonly BOSequenceNumber _boSequenceNumber;
        private readonly string _tableName;

        ///<summary>
        /// Creates a number generator of the specified type. If no record is currently in the database for that type,
        /// then this will create an entry in the table with the seed number of zero.
        ///</summary>
        ///<param name="numberType">Type of number</param>
        public NumberGenerator(string numberType)
        {
            _tableName = "";
            _boSequenceNumber = LoadSequenceNumber(numberType);
        }

        ///<summary>
        /// Creates a number generator of the specified type. If no record is currently in the database for that type.
        /// Then this will create an entry in the table with the seed number of zero.
        ///</summary>
        ///<param name="numberType">Type of number</param>
        ///<param name="tableName">the table that the sequence number is being stored in.</param>
        public NumberGenerator(string numberType, string tableName)
        {
            _tableName = tableName;
            _boSequenceNumber = LoadSequenceNumber(numberType);
        }

        #region INumberGenerator Members

        /// <summary>
        /// Returns the next available unique number. One possible means
        /// of providing unique numbers is simply to increment the last one
        /// dispensed.
        /// </summary>
        /// <returns>Returns an integer</returns>
        public long NextNumber()
        {
            _boSequenceNumber.SequenceNumber++;
            return _boSequenceNumber.SequenceNumber.GetValueOrDefault();
        }

        /// <summary>
        /// Allows the developer to set the new Sequence number this can be used when initialy creating the numbers e.g. when 
        /// you want to ensure that the numbers are generated starting at 10000.
        /// </summary>
        /// <param name="newSequenceNumber"></param>
        public void SetSequenceNumber(long newSequenceNumber)
        {
            _boSequenceNumber.SequenceNumber = newSequenceNumber;
            _boSequenceNumber.Save();
        }

        /// <summary>
        /// Interface to add the number generator to a transaction via the transaction committer.
        /// </summary>
        /// <param name="transactionCommitter"></param>
        public void AddToTransaction(ITransactionCommitter transactionCommitter)
        {
            BusinessObject busObject = GetTransactionalBO();
            busObject.UpdateObjectBeforePersisting(transactionCommitter);
            transactionCommitter.AddBusinessObject(busObject);
        }

        #endregion

        private BOSequenceNumber LoadSequenceNumber(string numberType)
        {
            BOSequenceNumber.LoadNumberGenClassDef(_tableName);
            var criteria = new Criteria("NumberType", Criteria.ComparisonOp.Equals, numberType);
            var sequenceBOSequenceNumber =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<BOSequenceNumber>(criteria);

            if (sequenceBOSequenceNumber != null) return sequenceBOSequenceNumber;
            sequenceBOSequenceNumber = CreateSequenceForType(numberType);
            return sequenceBOSequenceNumber;
        }

        private static BOSequenceNumber CreateSequenceForType(string numberType)
        {
            var sequenceBOSequenceNumber = new BOSequenceNumber {NumberType = numberType, SequenceNumber = 0};
            sequenceBOSequenceNumber.Save();
            return sequenceBOSequenceNumber;
        }


        ///<summary>
        /// Updates the number generator table.
        ///</summary>
        public void Save()
        {
            _boSequenceNumber.Save();
        }

        private BusinessObject GetTransactionalBO()
        {
            return _boSequenceNumber;
        }
    }

    ///<summary>
    /// A simple sequential number generator business object
    ///</summary>
    public class BOSequenceNumber : BusinessObject
    {
        private static string _tableName;

//        internal static void LoadNumberGenClassDef()
//        {
//            XmlClassLoader itsLoader = new XmlClassLoader();
//            ClassDef itsClassDef =
//                itsLoader.LoadClass(
//                    @"
//               <class name=""BOSequenceNumber"" assembly=""Habanero.BO"" table=""NumberGenerator"">
//					<property  name=""SequenceNumber"" type=""Int32"" />
//                    <property  name=""NumberType""/>
//                    <primaryKey isObjectID=""false"">
//                        <prop name=""NumberType"" />
//                    </primaryKey>
//			    </class>
//			");
//            ClassDef.ClassDefs.Add(itsClassDef);
//            return;
//        }

        /// <summary>
        /// Gets or sets the type of number
        /// </summary>
        public virtual String NumberType
        {
            get { return ((String) (base.GetPropertyValue("NumberType"))); }
            set { base.SetPropertyValue("NumberType", value); }
        }

        /// <summary>
        /// Gets or sets the sequence number
        /// </summary>
        public virtual long? SequenceNumber
        {
            get { return ((long?)(base.GetPropertyValue("SequenceNumber"))); }
            set { base.SetPropertyValue("SequenceNumber", value); }
        }

        private static readonly object _loadClassDefLock = new object();
        internal static void LoadNumberGenClassDef()
        {
            LoadNumberGenClassDef(null);
        }

        internal static void LoadNumberGenClassDef(string tableName)
        {
            lock (_loadClassDefLock)
            {
                if (ClassDef.ClassDefs.Contains(typeof (BOSequenceNumber))) return;
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = "NumberGenerator";
                }
                _tableName = tableName;
                var itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
                string classDef = "<class name=\"BOSequenceNumber\" assembly=\"Habanero.BO\" table=\"" + _tableName +
                                  "\">" +
                                  "<property  name=\"SequenceNumber\" type=\"Int64\" />" +
                                  "<property  name=\"NumberType\"/>" +
                                  "<primaryKey isObjectID=\"false\">" +
                                  "<prop name=\"NumberType\" />" +
                                  "</primaryKey>" +
                                  "</class>";
                var itsClassDef = itsLoader.LoadClass(classDef);
                ClassDef.ClassDefs.Add(itsClassDef);
                return;
            }
        }
    }
}