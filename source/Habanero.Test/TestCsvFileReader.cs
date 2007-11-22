using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Habanero.Util.File;
using NUnit.Framework;

namespace Habanero.Test
{
    [TestFixture]
    public class TestCsvFileReader
    {
        [Test]
        public void TestCsvReaderEmpty()
        {
            CompareToDataTable(Environment.NewLine, new DataTable());
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

        [Test, Ignore("This needs to be fixed at some stage")]
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
