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
using System.Collections.Generic;
using System.Data;
using System.IO;
using Habanero.Base.Util;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util.File
{
    [TestFixture]
    public class TestCsvFileReader
    {
        [Test]
        public void TestCsvReaderEmpty()
        {
            CompareToDataTable(EnvironmentCF.NewLine, new DataTable());
        }
                
        [Test]
        public void TestCsvReaderSimple()
        {
            CompareToDataTable(CsvTestsSamples.SimpleCsvContents(), CsvTestsSamples.SimpleDataTable());
        }

        [Test]
        public void TestCsvReaderComplex()
        {
            CompareToDataTable(CsvTestsSamples.ComplexCsvContents(), CsvTestsSamples.ComplexDataTable());
        }

        [Test]
        public void TestCsvReaderCommasInData()
        {
            CompareToDataTable(CsvTestsSamples.CommasInDataCsvContents(), CsvTestsSamples.CommasInDataDataTable());
        }

        [Test]
        public void TestCsvReaderCommasInHeader()
        {
            CompareToDataTable(CsvTestsSamples.CommasInHeaderCsvContents(), CsvTestsSamples.CommasInHeaderDataTable());
        }

        [Test]
        public void TestCsvReaderAdvanced()
        {
            CompareToDataTable(CsvTestsSamples.AdvancedCsvContents(), CsvTestsSamples.AdvancedDataTable());
        }

        private void CompareToDataTable(string csvContents, DataTable dataTable)
        {
            StringReader stringReader = new StringReader(csvContents);
            CSVFileReader csvFileReader = new CSVFileReader(stringReader);
            CompareToDataTable(csvFileReader, dataTable);
            csvFileReader.Close();
        }

        private void CompareToDataTable(CSVFileReader csvFileReader, DataTable dataTable)
        {
            if (!csvFileReader.MoveToNextLine())
            {
                Assert.AreEqual(0, dataTable.Columns.Count, "Data table should have no columns");
                Assert.AreEqual(0, dataTable.Rows.Count, "Data table should have no rows");
                return;
            }
            List<string> values = csvFileReader.GetValues();
            Assert.AreEqual(dataTable.Columns.Count, values.Count, "The number of columns must be equal");
            foreach (DataColumn column in dataTable.Columns)
            {
                string columnName = column.ColumnName;
                Assert.IsTrue(values.Contains(columnName),
                              String.Format("The column '{0}' does not exist in the csv output.", columnName));
            }
            int counter = 0;
            while (csvFileReader.MoveToNextLine())
            {
                values = csvFileReader.GetValues();
                DataRow dataRow = dataTable.Rows[counter];
                int counter2 = 0;
                foreach (string value in values)
                {
                    string dataValue = dataRow[counter2].ToString();
                    Assert.AreEqual(dataValue, value,
                                    String.Format("Csv data value was not as expected at [{0},{1}](base 0).", counter, counter2));
                    counter2++;
                };
                counter++;
            }
        }

    }
}
