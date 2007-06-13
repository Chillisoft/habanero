using System.Drawing;
using System.Xml;
using Chillisoft.Xml.v2;

namespace Chillisoft.Reporting.v2
{
    //Class to store layout information, used for layout for all section types
    public class Layout
    {
        private XmlNode _node;
        private XmlWrapper _xmlWrapper;
        private XmlNode _defaultNode;

        internal Layout(XmlNode node, XmlWrapper wrapper)
        {
            _defaultNode = wrapper.XmlDocument.SelectSingleNode("Report/DefaultLayout");

            //if the node passed in is null then it isn't included in the XML document
            //if this is the case use the default node for all properties
            if (node != null)
            {
                this._node = node;
            }
            else
            {
                this._node = _defaultNode;
            }

            _xmlWrapper = wrapper;
        }

        public int Height
        {
            get { return int.Parse(GetXmlValue("Height")); }
            set { _xmlWrapper.WriteXmlValue(_node, "Height", value.ToString()); }
        }

        public string FontName
        {
            get { return GetXmlValue("FontName"); }
            set { _xmlWrapper.WriteXmlValue(_node, "FontName", value); }
        }

        public int FontSize
        {
            get { return int.Parse(GetXmlValue("FontSize")); }
            set { _xmlWrapper.WriteXmlValue(_node, "FontSize", value.ToString()); }
        }

        public bool Border
        {
            get { return bool.Parse(GetXmlValue("Border")); }
            set { _xmlWrapper.WriteXmlValue(_node, "Border", value.ToString()); }
        }

        public bool FontBold
        {
            get { return bool.Parse(GetXmlValue("FontBold")); }
            set { _xmlWrapper.WriteXmlValue(_node, "FontBold", value.ToString()); }
        }

        public bool FontItalic
        {
            get { return bool.Parse(GetXmlValue("FontItalic")); }
            set { _xmlWrapper.WriteXmlValue(_node, "FontItalic", value.ToString()); }
        }

        public Color BackColor
        {
            get { return Color.FromName(GetXmlValue("BackColor")); }
            set { _xmlWrapper.WriteXmlValue(_node, "BackColor", value.Name); }
        }

        public Color ForeColor
        {
            get { return Color.FromName(GetXmlValue("ForeColor")); }
            set { _xmlWrapper.WriteXmlValue(_node, "ForeColor", value.Name); }
        }

        public int MarginLeft
        {
            get { return int.Parse(GetXmlValue("MarginLeft")); }
            set { _xmlWrapper.WriteXmlValue(_node, "MarginLeft", value.ToString()); }
        }

        public int MarginRight
        {
            get { return int.Parse(GetXmlValue("MarginRight")); }
            set { _xmlWrapper.WriteXmlValue(_node, "MarginRight", value.ToString()); }
        }

        public int MarginTop
        {
            get { return int.Parse(GetXmlValue("MarginTop")); }
            set { _xmlWrapper.WriteXmlValue(_node, "MarginTop", value.ToString()); }
        }

        public int MarginBottom
        {
            get { return int.Parse(GetXmlValue("MarginBottom")); }
            set { _xmlWrapper.WriteXmlValue(_node, "MarginBottom", value.ToString()); }
        }

        //first tries to the get the xml from the specific layout node of this class
        //if this returns an empty string the this method gets the default layout
        private string GetXmlValue(string elementName)
        {
            string xmlValue = _xmlWrapper.ReadXmlValue(_node, elementName);
            if (xmlValue != string.Empty)
            {
                return xmlValue;
            }
            else
            {
                return _xmlWrapper.ReadXmlValue(_defaultNode, elementName);
            }
        }
    }
}