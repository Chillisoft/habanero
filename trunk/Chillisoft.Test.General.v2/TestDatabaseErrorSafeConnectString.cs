//using Habanero.Db;
//using NUnit.Framework;
//
//namespace Chillisoft.Test.General.v2 {
//	/// <summary>
//	/// Summary description for TestDatabaseErrorSafeConnectString.
//	/// </summary>
//	[TestFixture]
//	public class TestDatabaseErrorSafeConnectString {
// TODO:Get this class going again.  Currently it's interfering with other test by changing the database connect string.
//		private string mPassWord = "myPrivatePassword";
//
//		public TestDatabaseErrorSafeConnectString() {}
//
//		[Test]
//		public void TestSafeConnectStringNoColon() {
//			DatabaseConnection.CurrentConnection.ConnectionString =
//				@"data source=Core;database=WorkShopManagement;uid=sa;pwd=" + mPassWord;
//			Assert.IsTrue(DatabaseConnection.CurrentConnection.ConnectionString.IndexOf(mPassWord, 0) > 0);
//			Assert.IsFalse(DatabaseConnection.CurrentConnection.ErrorSafeConnectString().IndexOf(mPassWord, 0) > 0);
//		}
//
//		[Test]
//		public void TestSafeConnectStringColon() {
//			DatabaseConnection.CurrentConnection.ConnectionString =
//				@"data source=Core;database=WorkShopManagement;uid=sa;pwd=" + mPassWord + ";";
//			Assert.IsTrue(DatabaseConnection.CurrentConnection.ConnectionString.IndexOf(mPassWord, 0) > 0);
//			Assert.IsFalse(DatabaseConnection.CurrentConnection.ErrorSafeConnectString().IndexOf(mPassWord, 0) > 0);
//		}
//
//		[Test]
//		public void TestSafeConnectStringWith_password() {
//			DatabaseConnection.CurrentConnection.ConnectionString =
//				@"data source=Core;database=WorkShopManagement;uid=sa;password=" + mPassWord + ";";
//			Assert.IsTrue(DatabaseConnection.CurrentConnection.ConnectionString.IndexOf(mPassWord, 0) > 0);
//			Assert.IsFalse(DatabaseConnection.CurrentConnection.ErrorSafeConnectString().IndexOf(mPassWord, 0) > 0);
//		}
//
//		[Test]
//		public void TestSafeConnectStringWithPassword() {
//			DatabaseConnection.CurrentConnection.ConnectionString =
//				@"data source=Core;database=WorkShopManagement;uid=sa;Password=" + mPassWord + ";";
//			Assert.IsTrue(DatabaseConnection.CurrentConnection.ConnectionString.IndexOf(mPassWord, 0) > 0);
//			Assert.IsFalse(DatabaseConnection.CurrentConnection.ErrorSafeConnectString().IndexOf(mPassWord, 0) > 0);
//		}
//
//	}
//}
