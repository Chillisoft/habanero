using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Db
{
    /// <summary>
    /// Provides database utilities such as methods to reformat objects
    /// into a format appropriate for sql
    /// </summary>
    public class DatabaseUtil
    {
        /// <summary>
        /// Returns a DateTime object as a string in the format of:<br/>
        /// "yyyy-MM-dd hh:mm:ss"
        /// </summary>
        /// <param name="dte">The DateTime object</param>
        /// <returns>Returns a string</returns>
        public static string FormatDatabaseDateTime(DateTime dte)
        {
            return dte.ToString("yyyy-MM-dd hh:mm:ss");
            //return dte.ToString("dd MMM yyyy HH:mm:ss");
        }

        /// <summary>
        /// Prepares the value to be converted to a format appropriate for sql
        /// </summary>
        /// <param name="objValue">The value to prepare</param>
        /// <returns>Returns the reformatted object</returns>
        /// TODO ERIC - prepare value for what?
        /// - should this be public? is it useful beyond ConvertValueToStandardSql 
        public static object PrepareValue(object objValue)
        {
            if (objValue is Guid)
            {
                if (Guid.Empty.Equals(objValue))
                {
                    return DBNull.Value;
                }
                else
                {
                    return ((Guid) objValue).ToString("B").ToUpper(CultureInfo.InvariantCulture);
                }
            }
            else if (objValue is bool)
            {
                return (bool) objValue ? 1 : 0;
            }
            else if (objValue is Image)
            {
                return SerialisationUtilities.ObjectToByteArray(objValue);
            }
            else if (objValue is CustomProperty)
            {
                return ((CustomProperty) objValue).GetPersistValue();
            }
            else if (objValue is TimeSpan)
            {
                TimeSpan time = (TimeSpan) objValue;
                return new DateTime(1900, 1, 1, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
            }
            else
            {
                return objValue;
            }
        }

        /// <summary>
        /// Converts the value to a string format appropriate for sql
        /// </summary>
        /// <param name="objValue">The value to convert</param>
        /// <returns>Returns a string</returns>
        public static string ConvertValueToStandardSql(object objValue)
        {
            object preparedValue = PrepareValue(objValue);
            string stringValue = "";
            if (preparedValue == null || DBNull.Value.Equals(preparedValue))
            {
                return "null";
            }
            else if (preparedValue is DateTime)
            {
                stringValue = FormatDatabaseDateTime((DateTime) preparedValue);
            }
            else if (preparedValue is String)
            {
                try
                {
                    decimal decimalValue = Decimal.Parse((String) preparedValue);
                    stringValue = (String) preparedValue;
                }
                catch
                {
                    stringValue = "'" + (String) preparedValue + "'";
                }
            }
            else
            {
                stringValue = preparedValue.ToString();
            }
            return stringValue;
        }

        /// <summary>
        /// Returns the primary key of the table specified
        /// </summary>
        /// <param name="conn">The database connection</param>
        /// <param name="tableName">The table name</param>
        /// <param name="adapter">The adapter</param>
        /// <returns>Returns the primary key as a string</returns>
        public static String GetPrimaryKeyOfTable(IDbConnection conn, String tableName, IDbDataAdapter adapter)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            try
            {
                IDbCommand command = conn.CreateCommand();
                command.CommandText = "select * from " + tableName;
                DataSet dst = new DataSet();
                adapter.SelectCommand = command;
                bool complete = false;
                int count = 0;
                while (!complete)
                {
                    try
                    {
                        adapter.FillSchema(dst, SchemaType.Source);
                        complete = true;
                    }
                    catch (DataException ex)
                    {
                        throw ex;
                    }
                }
                return dst.Tables["Table"].PrimaryKey[0].ColumnName;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}