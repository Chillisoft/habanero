//using System;
//using System.Collections;
//using System.Data;
//using System.Drawing;
//using Root.Reports;
//
//namespace Chillisoft.Reporting.PDFRenderer.v2 {
//	/// <summary>
//	/// Summary description for PDFReport.
//	/// </summary>
//	internal class PDFReport : Root.Reports.Report {
//		private readonly Report itsReport;
//
//		private PageDimension itsPageDimension;
//		private FontDef fd;
//		private string itsGroupHeader;
//
//		public PDFReport(Report report) {
//			itsReport = report;
//			if (itsReport.ReportDef.Orientation == Orientation.Landscape ) {
//				itsPageDimension = new PageDimensionLandscape() ;
//			} else {
//				itsPageDimension = new PageDimensionPortrait();
//			}
//		}
//
//		protected override void Create() {
//			fd = new FontDef(this, "Arial");
//			FontProp fp = new FontPropMM(fd, 1.9); // standard font
//			FontProp fp_Header = new FontPropMM(fd, 1.9); // font of the table header
//			fp_Header.bBold = true;
//
//			foreach (ReportGroup reportGroup in itsReport.ReportGroups) {
//				itsGroupHeader = reportGroup.Header;
//				using (TableLayoutManager tlm = new TableLayoutManager(fp_Header)) 
//				{
//					tlm.rContainerHeightMM = itsPageDimension.Bottom - itsPageDimension.Top; // set height of table
//					tlm.headerCellDef.rAlignV = RepObj.rAlignCenter; // set vertical alignment of all header cells
//					tlm.cellDef.pp_LineBottom = new PenProp(this, 0.05, Color.LightGray); // set bottom line for all cells
//					tlm.eNewContainer += new TlmBase.NewContainerEventHandler(Tlm_NewContainer);
//
//					// define columns
//					TableLayoutManager.Column col;
//					foreach (Column column in itsReport.ReportDef.Columns) 
//					{
//						if (!column.GroupBy)
//							col = new TableLayoutManager.ColumnMM(tlm, column.Caption, column.Width);
//					}
//
//					BrushProp bpDarker = new BrushProp(this, Color.FromArgb(220, 220, 220));
//					int rowCount = 0;
//					foreach (ReportRow reportRow in reportGroup.Rows) 
//					{
//						tlm.NewRow();
//						rowCount++;
//						for (int i = 0; i < reportRow.Count; i++) 
//						{
//							tlm.Add(i, new RepString(fp, reportRow[i].ToString()));
//							if (rowCount % 2 == 0) 
//							{
//								tlm.tlmRow_Cur.aTlmCell[i].textMode = TextMode.
//								tlm.tlmRow_Cur.aTlmCell[i].bp_Back = bpDarker;
//							}
//						}
//					}
//					if (rowCount % 2 == 0) 
//					{
//						tlm.NewRow();
//					}
//				}
//			}
//
//			// print page number and current date/time
//			foreach (Page page in enum_Page) {
//				
//				Double rY = itsPageDimension.Bottom + 1.5;
//				page.AddLT_MM(itsPageDimension.Left , rY, new RepString(fp, DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString()));
//				page.AddRT_MM(itsPageDimension.Right , rY, new RepString(fp, page.iPageNo + " / " + iPageCount));
//			}
//		}
//
//		public void Tlm_NewContainer(Object oSender, TlmBase.NewContainerEventArgs ea) { // only "public" for NDoc, should be "private"
//			this.CreateNewPage(ea.container ) ;
//					}
//
//		private void CreateNewPage(Container tableContainer) {
//			new Page(this);
//			if (itsReport.ReportDef.Orientation == Orientation.Landscape)  
//			{
//				page_Cur.SetLandscape() ;
//			}
//
//			FontProp fp_Title = new FontPropMM(fd, 4);
//			fp_Title.bBold = true;
//			page_Cur.AddCT_MM(itsPageDimension.Left + (itsPageDimension.Right - itsPageDimension.Left)/2, itsPageDimension.Top, new RepString(fp_Title, itsReport.ReportDef.Caption));
//			tableContainer.rHeightMM -= fp_Title.rLineFeedMM; // reduce height of table container because of the report heading
//		
//			if (itsGroupHeader != "") {
//				FontProp fp_groupHeader = new FontPropMM(fd, 3);
//				fp_groupHeader.bItalic = true;
//				page_Cur.AddLT_MM(itsPageDimension.Left, itsPageDimension.Top + fp_Title.rLineFeedMM, new RepString(fp_groupHeader, itsGroupHeader));
//				tableContainer.rHeightMM -= fp_groupHeader.rLineFeedMM; // reduce height of table container because of the group by heading
//
//			}
//
//			// the new container must be added to the current page
//			page_Cur.AddMM(itsPageDimension.Left, itsPageDimension.Bottom - tableContainer.rHeightMM, tableContainer);
//
//		}
//	}
//}
