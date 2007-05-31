using System.Drawing;
using System.Xml;
using Chillisoft.Xml.v2;

namespace Chillisoft.Reporting.v2
{
    //Class to store layout information, used for layout for all section types
    public class Layout
    {
        private XmlNode node;
        private XmlWrapper xmlWrapper;
        private XmlNode defaultNode;

        internal Layout(XmlNode node, XmlWrapper wrapper)
        {
            defaultNode = wrapper.XmlDocument.SelectSingleNode("Report/DefaultLayout");

            //if the node passed in is null then it isn't included in the XML document
            //if this is the case use the default node for all properties
            if (node != null)
            {
                this.node = node;
            }
            else
            {
                this.node = defaultNode;
            }

            xmlWrapper = wrapper;
        }

        public int Height
        {
            get { return int.Parse(GetXmlValue("Height")); }
            set { xmlWrapper.WriteXmlValue(node, "Height", value.ToString()); }
        }

        public string FontName
        {
            get { return GetXmlValue("FontName"); }
            set { xmlWrapper.WriteXmlValue(node, "FontName", value); }
        }

        public int FontSize
        {
            get { return int.Parse(GetXmlValue("FontSize")); }
            set { xmlWrapper.WriteXmlValue(node, "FontSize", value.ToString()); }
        }

        public bool Border
        {
            get { return bool.Parse(GetXmlValue("Border")); }
            set { xmlWrapper.WriteXmlValue(node, "Border", value.ToString()); }
        }

        public bool FontBold
        {
            get { return bool.Parse(GetXmlValue("FontBold")); }
            set { xmlWrapper.WriteXmlValue(node, "FontBold", value.ToString()); }
        }

        public bool FontItalic
        {
            get { return bool.Parse(GetXmlValue("FontItalic")); }
            set { xmlWrapper.WriteXmlValue(node, "FontItalic", value.ToString()); }
        }

        public Color BackColor
        {
            get { return Color.FromName(GetXmlValue("BackColor")); }
            set { xmlWrapper.WriteXmlValue(node, "BackColor", value.Name); }
        }

        public Color ForeColor
        {
            get { return Color.FromName(GetXmlValue("ForeColor")); }
            set { xmlWrapper.WriteXmlValue(node, "ForeColor", value.Name); }
        }

        public int MarginLeft
        {
            get { return int.Parse(GetXmlValue("MarginLeft")); }
            set { xmlWrapper.WriteXmlValue(node, "MarginLeft", value.ToString()); }
        }

        public int MarginRight
        {
            get { return int.Parse(GetXmlValue("MarginRight")); }
            set { xmlWrapper.WriteXmlValue(node, "MarginRight", value.ToString()); }
        }

        public int MarginTop
        {
            get { return int.Parse(GetXmlValue("MarginTop")); }
            set { xmlWrapper.WriteXmlValue(node, "MarginTop", value.ToString()); }
        }

        public int MarginBottom
        {
            get { return int.Parse(GetXmlValue("MarginBottom")); }
            set { xmlWrapper.WriteXmlValue(node, "MarginBottom", value.ToString()); }
        }

        //first tries to the get the xml from the specific layout node of this class
        //if this returns an empty string the this method gets the default layout
        private string GetXmlValue(string elementName)
        {
            string xmlValue = xmlWrapper.ReadXmlValue(node, elementName);
            if (xmlValue != string.Empty)
            {
                return xmlValue;
            }
            else
            {
                return xmlWrapper.ReadXmlValue(defaultNode, elementName);
            }
        }
    }
}