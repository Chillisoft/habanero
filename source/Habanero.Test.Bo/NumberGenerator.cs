using System;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.Test.BO
{
    /// <summary>
    /// This is a simple number generator class. This class implements no locking strategy.
    /// I.e. If two users try to retrieve a number of the same type concurrently then they
    /// will retrieve the same number. The object that the number is being used for e.g. member number
    /// will raise an error when that object is being saved if the member number is an alternate key.
    /// There is also no guarantee that the number will not have missing numbers in the sequence.
    /// E.g. member 1 asks for a number and gets 0001, member2 gets 0002 and member3 gets 0003.
    /// member3 is saved and member2 and member1 are not then there will be missing numbers in the sequence.
    /// </summary>
    public class NumberGenerator : INumberGenerator
    {
        private readonly BOSequenceNumber _boSequenceNumber;

        public NumberGenerator(string numberType)
        {
            _boSequenceNumber = LoadSequenceNumber(numberType);
        }

        public int NextNumber()
        {
            _boSequenceNumber.SequenceNumber++;
            return _boSequenceNumber.SequenceNumber.Value;
        }

        private static BOSequenceNumber LoadSequenceNumber(string numberType)
        {
            BOSequenceNumber sequenceBOSequenceNumber =
                BOLoader.Instance.GetBusinessObject<BOSequenceNumber>(string.Format("NumberType = '{0}'",numberType));
            if (sequenceBOSequenceNumber == null)
            {
                sequenceBOSequenceNumber = CreateSequenceForType(numberType);
            }
            return sequenceBOSequenceNumber;
        }

        private static BOSequenceNumber CreateSequenceForType(string numberType)
        {
            BOSequenceNumber sequenceBOSequenceNumber = new BOSequenceNumber();
            sequenceBOSequenceNumber.NumberType = numberType;
            sequenceBOSequenceNumber.SequenceNumber = 0;
            sequenceBOSequenceNumber.Save();
            return sequenceBOSequenceNumber;
        }


        public void Save()
        {
            _boSequenceNumber.Save();
        }

        public void SetSequenceNumber(int newSequenceNumber)
        {
            _boSequenceNumber.SequenceNumber = newSequenceNumber;
            _boSequenceNumber.Save();
        }

        private BusinessObject GetTransactionalBO()
        {
            return _boSequenceNumber;
        }

        public void AddToTransaction(TransactionCommitter transactionCommitter)
        {
            BusinessObject busObject = this.GetTransactionalBO();
            busObject.UpdateObjectBeforePersisting(transactionCommitter);
            transactionCommitter.AddBusinessObject(busObject);
        }
    }
    public class BOSequenceNumber : BusinessObject
    {
        internal static void LoadNumberGenClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
               <class name=""BOSequenceNumber"" assembly=""Habanero.Test.BO"" table=""NumberGenerator"">
					<property  name=""SequenceNumber"" type=""Int32"" />
                    <property  name=""NumberType""/>
                    <primaryKey isObjectID=""false"">
                        <prop name=""NumberType"" />
                    </primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return;
        }


        public virtual String NumberType
        {
            get { return ((String)(base.GetPropertyValue("NumberType"))); }
            set { base.SetPropertyValue("NumberType", value); }
        }

        public virtual Int32? SequenceNumber
        {
            get { return ((Int32?)(base.GetPropertyValue("SequenceNumber"))); }
            set { base.SetPropertyValue("SequenceNumber", value); }
        }

        public static void DeleteAllNumbers()
        {
            DatabaseConnection.CurrentConnection.ExecuteRawSql("Delete From numbergenerator");
        }
    }
}