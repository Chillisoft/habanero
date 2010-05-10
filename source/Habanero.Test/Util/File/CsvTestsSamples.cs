// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Data;

namespace Habanero.Test.Util.File
{
    internal class CsvTestsSamples
    {
        public static string SimpleCsvContents()
        {
            return "First,2nd,3" + Environment.NewLine + "a,b,c" + Environment.NewLine;
        }

        public static DataTable SimpleDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("First");
            dataTable.Columns.Add("2nd");
            dataTable.Columns.Add("3");
            dataTable.LoadDataRow(new object[] { "a", "b", "c" }, false);
            return dataTable;
        }

        public static string ComplexCsvContents()
        {
            return "First Entry,2nd 'Entry',\"3 \"\"Entry\"\"\"" + Environment.NewLine +
                   "a'a a,\"b\"\"b\"\"b\",c'c'c" + Environment.NewLine;
        }

        public static DataTable ComplexDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("First Entry");
            dataTable.Columns.Add("2nd 'Entry'");
            dataTable.Columns.Add("3 \"Entry\"");
            dataTable.LoadDataRow(new object[] { "a'a a", "b\"b\"b", "c'c'c" }, false);
            return dataTable;
        }

        public static string CommasInDataCsvContents()
        {
            return "First,Second,Third" + Environment.NewLine +
                   "\"a,a\",\"b,b\",\"c,c\"" + Environment.NewLine +
                   "\"a1,a1\",\"b1,b1\",\"c1,c1\"" + Environment.NewLine;
        }

        public static DataTable CommasInDataDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("First");
            dataTable.Columns.Add("Second");
            dataTable.Columns.Add("Third");
            dataTable.LoadDataRow(new object[] { "a,a", "b,b", "c,c" }, false);
            dataTable.LoadDataRow(new object[] { "a1,a1", "b1,b1", "c1,c1" }, false);
            return dataTable;
        }

        public static string CommasInHeaderCsvContents()
        {
            return "\"First,One\",\"Second,Two\",\"Third,Three\"" + Environment.NewLine +
                   "a,b,c" + Environment.NewLine;
        }

        public static DataTable CommasInHeaderDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("First,One");
            dataTable.Columns.Add("Second,Two");
            dataTable.Columns.Add("Third,Three");
            dataTable.LoadDataRow(new object[] { "a", "b", "c" }, false);
            return dataTable;
        }

        public static string AdvancedCsvContents()
        {
            return "\"First,One\"\"Singular\",\"Second\tTwo,''Double'\",\"Third\"\"\"\",Three'\t'\",Forth\tFour'\t'" + Environment.NewLine +
                   "\"a,a'\",\"\"\",bb\",\"c'\"\"c,\",d\t'd" + Environment.NewLine;
        }

        public static DataTable AdvancedDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("First,One\"Singular");
            dataTable.Columns.Add("Second\tTwo,''Double'");
            dataTable.Columns.Add("Third\"\",Three'\t'");
            dataTable.Columns.Add("Forth\tFour'\t'");
            dataTable.LoadDataRow(new object[] { "a,a'", "\",bb", "c'\"c,", "d\t'd" }, false);
            return dataTable;
        }

    }
}
