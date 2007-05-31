//using System;
//using System.Collections;
//using System.Data;
//using System.Xml;
//using Chillisoft.Xml.v2;
//using Chillisoft.Db.v2;
//using Chillisoft.Generic.v2;
//
//namespace Chillisoft.Reporting.v2
//{
//	public class SqlDataSource : IReportDataSource
//	{
////		private XmlNode node;
////		private XmlWrapper xmlWrapper;
//
//		private IDatabaseConnection itsConnection;
//		private readonly SqlStatement itsQuery;
//		private readonly ReportDef itsReportDef;
//
//		//		internal SqlDataSource(XmlNode node, XmlWrapper wrapper) 
////		{
////			this.node = node;
////			xmlWrapper = wrapper;
////			itsConnection = new XmlDatabaseConfig(node.SelectSingleNode("ConnectionString"), xmlWrapper).GetDatabaseConnection() ;
////			string queryString = xmlWrapper.ReadXmlValue(node, "Query");
////			if (queryString == string.Empty) 
////				queryString = "select * from " + xmlWrapper.ReadXmlValue(node, "TableName");
////			itsQuery = new SqlStatement(itsConnection.GetConnection(), queryString);
////		}
//
//		public SqlDataSource(IDatabaseConnection connection, SqlStatement query, ReportDef reportDef) {
//			itsConnection = connection;
//			itsQuery = query;
//			itsReportDef = reportDef;
//
//		}
//
//		public IDatabaseConnection Connection 
//		{
//			get { return this.itsConnection; }
//		}
//
//		private DataTable GetDataTable() {
//			DataTable myDataTable = DatabaseConnection.CurrentConnection.LoadDataTable(itsQuery, "", ""); 
//			return myDataTable;
//		}
//
//		public IList ReportGroups {
//			get {
//				
//				IList rows = new ArrayList() ;
//				foreach (DataRow dataRow in this.GetDataTable().Rows) 
//				{
//					ReportRow reportRow = new ReportRow();
//					for (int i = 0; i < dataRow.ItemArray.Length; i++) 
//					{
//						reportRow.Add(dataRow[i]);
//					}
//					rows.Add(reportRow);
//				}
//				ReportGroup group = new ReportGroup();
//				IList groups = new ArrayList();
//				groups.Add(group);
//				return groups;
//			}
//		}
//	}
//}
