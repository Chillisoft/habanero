using System;

namespace CoreBiz.Generic
{
	/// <summary>
	/// Summary description for UIPropertyCollectionName.
	/// </summary>
	public class UIPropertyCollectionName
	{
		private Type itsClassType;
		private string itsDescriptiveName;

		public UIPropertyCollectionName(Type classType, string descriptiveName) {
			itsClassType = classType;
			itsDescriptiveName = descriptiveName;
		}

		public override bool Equals(object obj)
		{
			if (obj is UIPropertyCollectionName) {
				UIPropertyCollectionName name = (UIPropertyCollectionName)obj;
				return (itsClassType == name.itsClassType && itsDescriptiveName == name.itsDescriptiveName );
			} else {
				return false;
			}
		}

		public override int GetHashCode()
		{
			return (this.itsClassType + this.itsDescriptiveName ).GetHashCode() ;
		}

		public override string ToString()
		{
			return this.itsClassType + "_" + this.itsDescriptiveName ;
		}



	}
}
