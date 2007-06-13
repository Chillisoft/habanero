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
        private XmlNode _node;
        private XmlWrapper _xmlWrapper;
        private NumericColumnSettings _numSet;

        internal Column(XmlNode node, XmlWrapper wrapper)
        {
            this._node = node;
            _xmlWrapper = wrapper;

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
                    _numSet = new NumericColumnSettings(numSetNode, _xmlWrapper);
                }
            }
        }

        public string Name
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Name"); }
            set { _xmlWrapper.WriteXmlValue(_node, "Name", value); }
        }

        public string Caption
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Caption"); }
            set { _xmlWrapper.WriteXmlValue(_node, "Caption", value); }
        }

        public int Width
        {
            get { return int.Parse(_xmlWrapper.ReadXmlValue(_node, "Width")); }
            set { _xmlWrapper.WriteXmlValue(_node, "Width", value.ToString()); }
        }


        public ColumnType Type
        {
            get
            {
                return ((ColumnType) Enum.Parse(typeof (ColumnType),
                                                _xmlWrapper.ReadXmlValue(_node, "Type")));
            }
            set { _xmlWrapper.WriteXmlValue(_node, "Type", value.ToString()); }
        }

        public ColumnSourceType SourceType
        {
            get
            {
                return ((ColumnSourceType) Enum.Parse(typeof (ColumnSourceType),
                                                      _xmlWrapper.ReadXmlValue(_node, "SourceType")));
            }
            set { _xmlWrapper.WriteXmlValue(_node, "SourceType", value.ToString()); }
        }

        public string Source
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Source"); }
            set { _xmlWrapper.WriteXmlValue(_node, "Source", value); }
        }

        public bool SortBy
        {
            get { return bool.Parse(_xmlWrapper.ReadXmlValue(_node, "SortBy")); }
            set { _xmlWrapper.WriteXmlValue(_node, "SortBy", value.ToString()); }
        }

        public bool GroupBy
        {
            get { return bool.Parse(_xmlWrapper.ReadXmlValue(_node, "GroupBy")); }
            set { _xmlWrapper.WriteXmlValue(_node, "GroupBy", value.ToString()); }
        }

        public NumericColumnSettings NumericColumnSettings
        {
            get { return _numSet; }
        }

        public bool ShowTotal()
        {
            return Type == ColumnType.Numeric && NumericColumnSettings.ShowTotal;
        }
    }
}