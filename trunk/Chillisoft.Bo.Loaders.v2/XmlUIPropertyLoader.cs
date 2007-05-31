using System;
using System.Xml;
using CoreBiz.Generic;

namespace CoreBiz.Bo.Loaders
{
	/// <summary>
	/// Summary description for XmlUIPropertyLoader.
	/// </summary>
	public class XmlUIPropertyLoader : XmlLoader
	{
		private string itsLabel;
		private string itsPropertyName;
		private string itsControlTypeName;
		private string itsMapperTypeName;

		public XmlUIPropertyLoader()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public UIProperty LoadUIProperty(string xmlUIProp) 
		{
			return this.LoadUIProperty(this.CreateXmlElement(xmlUIProp)) ;			
		}

		public UIProperty LoadUIProperty(XmlElement uiPropElement) 
		{
			return (UIProperty) Load(uiPropElement);
		}

		protected override object Create() {
			return new UIProperty(itsLabel, itsPropertyName, itsControlTypeName, itsMapperTypeName);
		}

		protected override void LoadFromReader() {
			itsReader.Read();
			LoadLabel();
			LoadPropertyName();
			LoadControlTypeName();
			LoadMapperTypeName();
		}

		private void LoadMapperTypeName() {
			itsMapperTypeName = itsReader.GetAttribute("mapperTypeName"); 
		}

		private void LoadControlTypeName() {
			itsControlTypeName = itsReader.GetAttribute("controlTypeName"); 
		}

		private void LoadPropertyName() {
			itsPropertyName = itsReader.GetAttribute("propertyName"); 
		}

		private void LoadLabel() {
			itsLabel = itsReader.GetAttribute("label"); 
		}


	}
}
