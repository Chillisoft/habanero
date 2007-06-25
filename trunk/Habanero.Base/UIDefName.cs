//using System;
//
//namespace Habanero.Base
//{
//	/// <summary>
//	/// Summary description for UIDefName.
//	/// </summary>
//	public class UIDefName
//	{
//		private Type itsClassType;
//		private string itsDescriptiveName;
//
//		public UIDefName(Type classType, string descriptiveName) 
//		{
//			itsClassType = classType;
//			itsDescriptiveName = descriptiveName;
//		}
//
//		public override bool Equals(object obj)
//		{
//			if (obj is UIDefName) 
//			{
//				UIDefName name = (UIDefName)obj;
//				return (itsClassType == name.itsClassType && itsDescriptiveName == name.itsDescriptiveName );
//			} 
//			else 
//			{
//				return false;
//			}
//		}
//
//		public override int GetHashCode()
//		{
//			return (this.itsClassType + this.itsDescriptiveName ).GetHashCode() ;
//		}
//
//		public override string ToString()
//		{
//			return this.itsClassType + "_" + this.itsDescriptiveName ;
//		}
//
//	}
//}
