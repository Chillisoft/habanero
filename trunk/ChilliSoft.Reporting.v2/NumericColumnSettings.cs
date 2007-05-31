using System.Xml;
using Chillisoft.Xml.v2;

namespace Chillisoft.Reporting.v2
{
    /**This class stores the numeric settings for a column. This data
		 * must be included in the report definition for any numeric column
		 * This component will throw an exception if this is not the case
		 */

    public class NumericColumnSettings
    {
        private XmlNode node;
        private XmlWrapper xmlWrapper;

        internal NumericColumnSettings(XmlNode node, XmlWrapper wrapper)
        {
            this.node = node;
            xmlWrapper = wrapper;
        }

        //the number of decimal places to show
        public int DecimalPlaces
        {
            get { return int.Parse(xmlWrapper.ReadXmlValue(node, "DecimalPlaces")); }
            set { xmlWrapper.WriteXmlValue(node, "DecimalPlaces", value.ToString()); }
        }

        //whether or not show decimal places regardless of actual value
        //eg showing 0 vs 0.0
        public bool ForceDecimalPlaces
        {
            get { return bool.Parse(xmlWrapper.ReadXmlValue(node, "ForceDecimalPlaces")); }
            set { xmlWrapper.WriteXmlValue(node, "ForceDecimalPlaces", value.ToString()); }
        }

        //whether or not to show a total for this column
        public bool ShowTotal
        {
            get { return bool.Parse(xmlWrapper.ReadXmlValue(node, "ShowTotal")); }
            set { xmlWrapper.WriteXmlValue(node, "ShowTotal", value.ToString()); }
        }
    }
}