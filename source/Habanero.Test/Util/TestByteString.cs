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
using System.Collections;
using System.Data;
using System.Linq;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{ // ReSharper disable InconsistentNaming
	/// <summary>
	/// This Test Class tests the functionality of the ByteString custom property class.
	/// </summary>
	[TestFixture]
	public class TestByteString : TestUsingDatabase
	{
		private readonly IClassDef _classDef;

		//These unicode bytes spell 'test':   t - 116, e - 101, s - 115, t - 116
		private readonly byte[] itsByteArrSpelling_test = { 116, 0, 101, 0, 115, 0, 116, 0 };

		public TestByteString()
		{
			ClassDef.ClassDefs.Clear();
			var loader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
			_classDef =
				loader.LoadClass(
					@"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" type=""Habanero.Util.ByteString"" assembly=""Habanero.Base"" />
					<property  name=""ByteArrayProp"" type=""Byte[]"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
			ClassDef.ClassDefs.Add(_classDef);
			base.SetupDBConnection();
		}

		[Test]
		public void TestLoadingConstructor()
		{
			var byteString = new ByteString(itsByteArrSpelling_test, true);
			Assert.IsTrue(byteString.Value.Equals("test"));
		}

		[Test]
		public void TestNonLoadingConstructor()
		{
			var byteString = new ByteString("test", false);
			Assert.IsTrue(byteString.Value.Equals("test"));
		}

		[Test]
		public void TestStringConstructor()
		{
			var byteString = new ByteString("test");
			Assert.IsTrue(byteString.Value.Equals("test"));
		}

		[Test]
		public void TestOtherTypeConstructor()
		{
			var byteString = new ByteString(1, false);
			Assert.AreEqual("1", byteString.Value);
		}

		[Test]
		public void TestSettingValue()
		{
			var byteString = new ByteString("test");
			byteString.Value = "newtest";
			Assert.IsTrue(byteString.Value.Equals("newtest"));
		}

		[Test]
		public void TestEquals()
		{
			var byteString1 = new ByteString("test");
			var byteString2 = new ByteString("test");
			Assert.IsTrue(byteString1.Equals(byteString2));
		}

		[Test]
		public void TestEqualsWithByte()
		{
			ByteString byteString1 = new ByteString("test");
			ByteString byteString2 = new ByteString(itsByteArrSpelling_test, true);
			Assert.IsTrue(byteString1.Equals(byteString2));
		}

		[Test]
		public void TestEqualsBothByte()
		{
			var byteString1 = new ByteString(itsByteArrSpelling_test, false);
			var byteString2 = new ByteString(itsByteArrSpelling_test, true);
			Assert.IsTrue(byteString1.Equals(byteString2));
		}

		[Test]
		public void TestHashCode()
		{
			var byteString = new ByteString("test");
			if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "x86")
			{
				Assert.AreEqual(-354185609, byteString.GetHashCode());
			}
			else
			{
				Assert.AreEqual(-871206010, byteString.GetHashCode());
			}

		}

		[Test]
		public void TestToString()
		{
			var byteString = new ByteString("test");
			Assert.AreEqual("test", byteString.ToString());
		}

		[Test]
		public void TestToStringFromByte()
		{
			var byteString = new ByteString(itsByteArrSpelling_test, true);
			Assert.AreEqual("test", byteString.ToString());
		}

		[Test]
		public void TestPropertyType()
		{
			var propDef = (PropDef)_classDef.PropDefcol["TestProp"];
			Assert.AreEqual(propDef.PropertyType, typeof(ByteString));
		}

		[Test]
		public void TestPropertyValue()
		{
			var bo = _classDef.CreateNewBusinessObject();
			bo.SetPropertyValue("TestProp", new ByteString("test"));
			Assert.AreSame(typeof(ByteString), bo.GetPropertyValue("TestProp").GetType());
		}

		[Test]
		public void TestSetPropertyValueWithString()
		{
			var bo = _classDef.CreateNewBusinessObject();
			bo.SetPropertyValue("TestProp", "test");
			Assert.AreSame(typeof(ByteString), bo.GetPropertyValue("TestProp").GetType());
			Assert.AreEqual("test", bo.GetPropertyValue("TestProp").ToString());
		}

		[Test]
		public void TestPersistSqlParameterValue()
		{
			TestUsingDatabase.SetupDBOracleConnection();
			IBusinessObject bo = _classDef.CreateNewBusinessObject();
			bo.SetPropertyValue("TestProp", "test");
			var sqlCol = new TransactionalBusinessObjectDB(bo, DatabaseConnection.CurrentConnection).GetPersistSql();
			ISqlStatement sqlStatement = sqlCol.First();
			IList parameters = sqlStatement.Parameters;
			IDbDataParameter byteStringParam = (IDbDataParameter)parameters[2];
			Assert.IsTrue(byteStringParam.Value is byte[], "Should be a byte array");
			byte[] paramValue = (byte[])byteStringParam.Value;
			Assert.AreEqual(paramValue.Length, itsByteArrSpelling_test.Length);
			Assert.AreEqual(paramValue[0], itsByteArrSpelling_test[0]);
			Assert.AreEqual(paramValue[1], itsByteArrSpelling_test[1]);
			Assert.AreEqual(paramValue[2], itsByteArrSpelling_test[2]);
			Assert.AreEqual(paramValue[3], itsByteArrSpelling_test[3]);
			Assert.AreEqual(paramValue[4], itsByteArrSpelling_test[4]);
			Assert.AreEqual(paramValue[5], itsByteArrSpelling_test[5]);
			Assert.AreEqual(paramValue[6], itsByteArrSpelling_test[6]);
			Assert.AreEqual(paramValue[7], itsByteArrSpelling_test[7]);
		}

		[Test]
		public void PersistSQLparamaterValue_WhenByteArrayNull_WhenMySQL_ShouldNotExist_FIXBUG1741()
		{
			//---------------Set up test pack-------------------
			TestUsingDatabase.SetupDBDataAccessor();
			var bo = _classDef.CreateNewBusinessObject();
			bo.SetPropertyValue("ByteArrayProp", null);
			//---------------Assert Precondition----------------

			//---------------Execute Test ----------------------
			var sqlCol = new TransactionalBusinessObjectDB(bo, DatabaseConnection.CurrentConnection).GetPersistSql();
			var sqlStatement = sqlCol.First();
			//IList parameters = sqlStatement.Parameters;

			DatabaseConnection.CurrentConnection.ExecuteSql(sqlStatement);
			//---------------Test Result -----------------------
			Assert.Pass("If it got here then it is OK");
		}
	}
}