/**This file contains various enums used by the report definition classes. 
  *These same enums have been defined in the Report Definition XML schema.
  */

namespace Chillisoft.Reporting.v2
{
    public enum Orientation : byte
    {
        Portrait,
        Landscape
    }

//	[Flags]
//	public enum ConnectionStringAttributes: byte
//	{
//		Provider = 1,
//		Server = 2,
//		InitialCatalog = 4,
//		DataSource = 8,
//		UserID = 16,
//		Password = 32,
//		IntegratedSecurity = 64
//	}

    public enum ColumnType : byte
    {
        Text,
        Numeric,
        Boolean
    }

    public enum ColumnSourceType : byte
    {
        Database,
        Calculation,
        Text
    }
}