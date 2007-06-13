using System;
using System.Collections;
using System.Data;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    /// Summary description for Report.
    /// </summary>
    public class Report
    {
        private readonly ReportDef _reportDef;
        private readonly IReportDataSource _dataSource;

        public Report(ReportDef reportDef, IReportDataSource dataSource)
        {
            _reportDef = reportDef;
            _dataSource = dataSource;
        }

        public DataTable DataTable
        {
            get { return _dataSource.GetDataTable(); }
        }


        public IList ReportGroups
        {
            get
            {
                IList groups = new ArrayList();

                object groupByValue = null;
                ReportGroup group = null;
                DataTable reportData = this._dataSource.GetDataTable();
                string groupByColumn = _reportDef.GroupByColumn;
                for (int i = 0; i < reportData.Rows.Count; i++)
                {
                    DataRow dataRow = reportData.Rows[i];
                    if (i == 0)
                    {
                        group = new ReportGroup();
                    }
                    else if (_reportDef.GroupByColumn != "")
                    {
                        if ((groupByValue == null && dataRow[groupByColumn] != null) ||
                            (groupByValue != null && dataRow[groupByColumn] == null) ||
                            (!groupByValue.Equals(dataRow[groupByColumn])))
                        {
                            group.Header = Convert.ToString(groupByValue);
                            groups.Add(group);
                            group = new ReportGroup();
                        }
                    }

                    int startCol = 0;
                    if (groupByColumn != "")
                    {
                        groupByValue = reportData.Rows[i][groupByColumn];
                        startCol = 1;
                    }

                    ReportRow reportRow = new ReportRow();
                    for (int col = startCol; col < dataRow.ItemArray.Length; col++)
                    {
                        reportRow.Add(dataRow[col]);
                    }
                    group.Add(reportRow);
                }
                if (group == null)
                {
                    group = new ReportGroup();
                }
                group.Header = Convert.ToString(groupByValue);
                groups.Add(group);

                return groups;
            }
        }

        public ReportDef ReportDef
        {
            get { return _reportDef; }
        }
    }
}