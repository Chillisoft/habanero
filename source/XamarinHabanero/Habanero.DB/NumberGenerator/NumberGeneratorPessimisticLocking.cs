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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB.ConcurrencyControl;

namespace Habanero.DB
{
	/// <summary>
	/// This is a slightly more complex number generator class. This class implements a pessimistic locking strategy.
	/// I.e. If two users try to retrieve a number of the same type concurrently then the first user will 
	/// be allowed to retrieve the number and the second user will be given a locking error untill
	/// the first user either persists their changes, cancells their changes or times out.
	/// <br/>
	/// It is critical when using this strategy that the developer calls the NextNumber as close to the persisting
	/// of the objects as possible so as to ensure that the lock is held for as short a time as possible.
	/// This is the number generator to use when you cannot have missing numbers in the sequence i.e. you 
	/// cannot have invoice1 and then invoice3 with no invoice2.
	/// If you need a simpler number generator that implements no locking strategy then use
	/// NumberGenerator <see cref="NumberGenerator"/>
	/// </summary>
	public class NumberGeneratorPessimisticLocking : INumberGenerator
	{
	    private int LockDurationInMinutes { get; set; }
	    private readonly BOSequenceNumberLocking _boSequenceNumber;

		///<summary>
		/// Creates a number generator with Pesssimistic locking.
		///</summary>
		///<param name="numberType"></param>
		public NumberGeneratorPessimisticLocking(string numberType): this(numberType, 15)
		{
		}

	    ///<summary>
	    /// Creates a number generator with Pesssimistic locking.
	    ///</summary>
	    ///<param name="numberType"></param>
	    ///<param name="lockDurationInMinutes"></param>
	    public NumberGeneratorPessimisticLocking(string numberType, int lockDurationInMinutes)
		{
	        LockDurationInMinutes = lockDurationInMinutes;
	        if (!ClassDef.ClassDefs.Contains(typeof(BOSequenceNumberLocking)))
			{
				BOSequenceNumberLocking.LoadNumberGenClassDef();
			}
			_boSequenceNumber = LoadSequenceNumber(numberType);
		}

		internal BOSequenceNumberLocking BoSequenceNumber
		{
			get { return _boSequenceNumber; }
		}

		/// <summary>
		/// Returns the next numbers in the sequence
		/// </summary>
		public long NextNumber()
		{
			BoSequenceNumber.SequenceNumber++;
			return BoSequenceNumber.SequenceNumber.GetValueOrDefault();
		}

		private BOSequenceNumberLocking LoadSequenceNumber(string numberType)
		{
			var searchCriteria = string.Format("NumberType = '{0}'", numberType);
			var criteria = CriteriaParser.CreateCriteria(searchCriteria);
			var sequenceBOSequenceNumber =
				BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<BOSequenceNumberLocking>(criteria);

			if (sequenceBOSequenceNumber == null)
			{
                sequenceBOSequenceNumber = CreateSequenceForType(numberType);
			}
			return sequenceBOSequenceNumber;
		}

		private BOSequenceNumberLocking CreateSequenceForType(string numberType)
		{
            var sequenceBOSequenceNumber = new BOSequenceNumberLocking(LockDurationInMinutes) { NumberType = numberType, SequenceNumber = 0};
			sequenceBOSequenceNumber.Save();
			return sequenceBOSequenceNumber;
		}

		/// <summary>
		/// Proactively sets the current sequence number and persists it
		/// </summary>
		public void SetSequenceNumber(long newSequenceNumber)
		{
			BoSequenceNumber.SequenceNumber = newSequenceNumber;
			BoSequenceNumber.Save();
		}

		private BusinessObject GetBOSequenceNumberLocking()
		{
			return BoSequenceNumber;
		}

		/// <summary>
		/// Adds the sequence number change to the persistence transaction
		/// </summary>
		/// <param name="transactionCommitter">The transaction committer suitable
		/// for the persistence environment</param>
		public void AddToTransaction(ITransactionCommitter transactionCommitter)
		{
			var busObject = this.GetBOSequenceNumberLocking();
			transactionCommitter.AddBusinessObject(busObject);
		}
		/// <summary>
		/// This method is only used for testing purposes.
		/// </summary>
		protected void ReleaseLocks()
		{
			var concurrencyControl = this._boSequenceNumber.ConcurrencyControl();
			concurrencyControl.ReleaseWriteLocks();
		}
	    ///<summary>
	    /// Is this pessimistic number generator currently locked.
	    ///</summary>
	    public bool IsLocked
	    {
            get { return this.BoSequenceNumber.IsLocked; }
	    }
	}

    /// <summary>
	/// Manages locking for sequence number control. This is used by <see cref="NumberGeneratorPessimisticLocking"/> to implement the locking
	/// strategy.
	/// </summary>
	internal class BOSequenceNumberLocking : BusinessObject
	{
        private readonly PessimisticLockingDB _pessimisticLockingDB;

        public BOSequenceNumberLocking():this(15)
        {

        }
        public BOSequenceNumberLocking(int lockDurationInMinutes)
        {
            _pessimisticLockingDB = new PessimisticLockingDB(
                this, lockDurationInMinutes,
                this.Props["DateTimeLocked"],
                this.Props["UserLocked"],
                this.Props["MachineLocked"],
                this.Props["OperatingSystemUserLocked"],
                this.Props["Locked"]);
            SetConcurrencyControl(_pessimisticLockingDB);
        }

        internal static void LoadNumberGenClassDef()
		{
			var itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
			var itsClassDef =
				itsLoader.LoadClass(
					@"
			   <class name=""BOSequenceNumberLocking"" assembly=""Habanero.DB"" table=""NumberGenerator"">
					<property  name=""SequenceNumber"" type=""Int64"" />
					<property  name=""NumberType""/>
					<property  name=""DateTimeLocked"" type=""DateTime"" />
					<property  name=""UserLocked"" />
					<property  name=""Locked"" type=""Boolean""/>
					<property  name=""MachineLocked"" />
					<property  name=""OperatingSystemUserLocked"" />
					<primaryKey isObjectID=""false"">
						<prop name=""NumberType"" />
					</primaryKey>
				</class>
			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return;
		}

		/// <summary>
		/// Indicates the type of number
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
			get
			{
			    return ((long?) (base.GetPropertyValue("SequenceNumber")));
			}
			set { base.SetPropertyValue("SequenceNumber", value); }
		}
        public bool IsLocked{get { return _pessimisticLockingDB.IsLocked; }}


        internal IConcurrencyControl ConcurrencyControl()
		{
			return _concurrencyControl;
		}

        protected override void UpdateAsTransactionRolledBack()
        {
            base.UpdateAsTransactionRolledBack();
            this.CancelEdits();
        }
	}
}