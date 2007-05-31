using System;
using System.Xml;
using Chillisoft.Xml.v2;

namespace Chillisoft.Reporting.v2
{
    /// <summary>
    /// class to store definition of a column
    /// </summary>
    public class Column
    {
        private XmlNode node;
        private XmlWrapper xmlWrapper;
        private NumericColumnSettings numSet;

        internal Column(XmlNode node, XmlWrapper wrapper)
        {
            this.node = node;
            xmlWrapper = wrapper;

            //only initalise the numeric setting if this column has a number
            //it is an error if this column is a number and does not have 
            //numeric settings
            if (Type == ColumnType.Numeric)
            {
                XmlNode numSetNode = node.SelectSingleNode("NumericColumnSettings");
                if (numSetNode == null)
                {
                    throw new Exception(
                        "Error in XML document: Numeric Column settings are compulsory when the column type is text");
                }
                else
                {
                    numSet = new NumericColumnSettings(numSetNode, xmlWrapper);
                }
            }
        }

        public string Name
        {
            get { return xmlWrapper.ReadXmlValue(node, "Name"); }
            set { xmlWrapper.WriteXmlValue(node, "Name", value); }
        }

        public string Caption
        {
            get { return xmlWrapper.ReadXmlValue(node, "Caption"); }
            set { xmlWrapper.WriteXmlValue(node, "Caption", value); }
        }

        public int Width
        {
            get { return int.Parse(xmlWrapper.ReadXmlValue(node, "Width")); }
            set { xmlWrapper.WriteXmlValue(node, "Width", value.ToString()); }
        }


        public ColumnType Type
        {
            get
            {
                return ((ColumnType) Enum.Parse(typeof (ColumnType),
                                                xmlWrapper.ReadXmlValue(node, "Type")));
            }
            set { xmlWrapper.WriteXmlValue(node, "Type", value.ToString()); }
        }

        public ColumnSourceType SourceType
        {
            get
            {
                return ((ColumnSourceType) Enum.Parse(typeof (ColumnSourceType),
                                                      xmlWrapper.ReadXmlValue(node, "SourceType")));
            }
            set { xmlWrapper.WriteXmlValue(node, "SourceType", value.ToString()); }
        }

        public string Source
        {
            get { return xmlWrapper.ReadXmlValue(node, "Source"); }
            set { xmlWrapper.WriteXmlValue(node, "Source", value); }
        }

        public bool SortBy
        {
            get { return bool.Parse(xmlWrapper.ReadXmlValue(node, "SortBy")); }
            set { xmlWrapper.WriteXmlValue(node, "SortBy", value.ToString()); }
        }

        public bool GroupBy
        {
            get { return bool.Parse(xmlWrapper.ReadXmlValue(node, "GroupBy")); }
            set { xmlWrapper.WriteXmlValue(node, "GroupBy", value.ToString()); }
        }

        public NumericColumnSettings NumericColumnSettings
        {
            get { return numSet; }
        }

        public bool ShowTotal()
        {
            return Type == ColumnType.Numeric && NumericColumnSettings.ShowTotal;
        }
    }
}