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
        private XmlNode _node;
        private XmlWrapper _xmlWrapper;

        internal NumericColumnSettings(XmlNode node, XmlWrapper wrapper)
        {
            this._node = node;
            _xmlWrapper = wrapper;
        }

        //the number of decimal places to show
        public int DecimalPlaces
        {
            get { return int.Parse(_xmlWrapper.ReadXmlValue(_node, "DecimalPlaces")); }
            set { _xmlWrapper.WriteXmlValue(_node, "DecimalPlaces", value.ToString()); }
        }

        //whether or not show decimal places regardless of actual value
        //eg showing 0 vs 0.0
        public bool ForceDecimalPlaces
        {
            get { return bool.Parse(_xmlWrapper.ReadXmlValue(_node, "ForceDecimalPlaces")); }
            set { _xmlWrapper.WriteXmlValue(_node, "ForceDecimalPlaces", value.ToString()); }
        }

        //whether or not to show a total for this column
        public bool ShowTotal
        {
            get { return bool.Parse(_xmlWrapper.ReadXmlValue(_node, "ShowTotal")); }
            set { _xmlWrapper.WriteXmlValue(_node, "ShowTotal", value.ToString()); }
        }
    }
}