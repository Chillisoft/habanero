using System;
using System.Xml;
using Chillisoft.Generic.v2;
using Chillisoft.Xml.v2;
using log4net;

namespace Chillisoft.Reporting.v2
{
    /**stores the definition of a report including all sub-definitions
	 * this is the top level class in the heirarchy and the only one that
	 * can be initialised outside this component
	 */

    public class ReportDef
    {
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Reporting.v2.ReportDef");

        private XmlNode _node;
        private XmlWrapper _xmlWrapper;

        //variables for storing sub-definition data
        //private IReportDataSource itsDataSource;
        private Layout itsHeaderLayout;
        private Layout itsGroupHeaderLayout;
        private Layout itsColumnHeaderLayout;
        private Layout itsDetailLayout;
        private Layout itsGroupFooterLayout;
        private Layout itsFooterLayout;
        private ColumnCollection itsColumns;
        private Layout itsDefaultLayout;


        //		public ReportDef( string filename )
        //		{
        //			LoadFromFile(filename);
        //			itsDataSource = new SqlDataSource(_node.SelectSingleNode("DataSource"), _xmlWrapper);
        //		}

        public ReportDef(string filename)
        {
            LoadFromFile(filename);
            //itsDataSource = dataSource;
        }

        private void LoadFromFile(string filename)
        {
            try
            {
                _xmlWrapper = new XmlWrapper(filename);
            }
            catch (Exception ex)
            {
                log.Error(ExceptionUtil.GetExceptionString(ex, 0));
                throw ex;
            }
            _node = _xmlWrapper.XmlDocument.DocumentElement;

            //initialise sub-definition variables

            itsDefaultLayout = new Layout(_node.SelectSingleNode("DefaultLayout"), _xmlWrapper);
            itsHeaderLayout = new Layout(_node.SelectSingleNode("HeaderLayout"), _xmlWrapper);
            itsGroupHeaderLayout = new Layout(_node.SelectSingleNode("GroupHeaderLayout"), _xmlWrapper);
            itsColumnHeaderLayout = new Layout(_node.SelectSingleNode("ColumnHeaderLayout"), _xmlWrapper);
            itsDetailLayout = new Layout(_node.SelectSingleNode("DetailLayout"), _xmlWrapper);
            itsGroupFooterLayout = new Layout(_node.SelectSingleNode("GroupFooterLayout"), _xmlWrapper);
            itsFooterLayout = new Layout(_node.SelectSingleNode("FooterLayout"), _xmlWrapper);

            itsColumns = new ColumnCollection();
            foreach (XmlNode columnNode in _node.SelectNodes("Column"))
            {
                itsColumns.Add(new Column(columnNode, _xmlWrapper));
            }
        }

//		public IReportDataSource DataSource {
//			get { return itsDataSource; }
//		}

        public Orientation Orientation
        {
            get
            {
                return ((Orientation) Enum.Parse(typeof (Orientation),
                                                 _xmlWrapper.ReadXmlValue(_node, "Orientation")));
            }
            set { _xmlWrapper.WriteXmlValue(_node, "Orientation", value.ToString()); }
        }

        public string Caption
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Caption"); }
            set { _xmlWrapper.WriteXmlValue(_node, "Caption", value); }
        }

        public Layout HeaderLayout
        {
            get { return itsHeaderLayout; }
        }

        public Layout GroupHeaderLayout
        {
            get { return itsGroupHeaderLayout; }
        }

        public Layout ColumnHeaderLayout
        {
            get { return itsColumnHeaderLayout; }
        }

        public Layout DetailLayout
        {
            get { return itsDetailLayout; }
        }

        public Layout GroupFooterLayout
        {
            get { return itsGroupFooterLayout; }
        }

        public Layout FooterLayout
        {
            get { return itsFooterLayout; }
        }

        public Layout DefaultLayout
        {
            get { return itsDefaultLayout; }
        }

        public ColumnCollection Columns
        {
            get { return itsColumns; }
        }

        //returns the sum of all the column widths 
        //exluding ones that are grouped by
        public int GetWidth()
        {
            int total = 0;
            foreach (Column col in Columns)
            {
                if (!col.GroupBy)
                {
                    total += col.Width;
                }
            }
            return total;
        }

        //returns true if there is at least one column that has a total
        //this can be used to determine whether footer sections should be 
        //included when rendering the report
        public bool ShowTotal()
        {
            foreach (Column col in Columns)
            {
                if (col.ShowTotal())
                {
                    return true;
                }
            }
            return false;
        }

        //If there is not enough room for a total label in the footer because
        //the first total column is too close to the left side of the report
        //then expand the first non-grouped column
        //the consumer of this component can choose whether or not use this
        //functionality
        public void ExpandFirstColumnForTotalLabel(int totalLabelWidth)
        {
            int width = 0;
            Column firstCol = itsColumns[0];

            foreach (Column col in Columns)
            {
                if (col.ShowTotal())
                {
                    break;
                }
                if (!col.GroupBy)
                {
                    width += col.Width;
                    firstCol = col;
                }
            }
            if (width < totalLabelWidth)
            {
                firstCol.Width += totalLabelWidth - width;
            }
        }

        public string GroupByColumn
        {
            get
            {
                foreach (Column column in itsColumns)
                {
                    if (column.GroupBy)
                    {
                        return column.Name;
                    }
                }
                return "";
            }
        }

        public string Name
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Name"); }
        }
    }
}