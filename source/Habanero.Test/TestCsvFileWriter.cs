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
    public class TestCsvFileWriter
    {
        [Test]
        public void TestCsvWriterEmpty()
        {
            StringWriter stringWriter = new StringWriter();
            CSVFileWriter csvFileWriter = new CSVFileWriter(stringWriter);
            DataTable dataTable = new DataTable();
            csvFileWriter.WriteFromDataTable(dataTable);
            csvFileWriter.Close();
            Assert.AreEqual(Environment.NewLine, stringWriter.ToString());
        }

        [Test]
        public void TestCsvWriterSimple()
        {
            StringWriter stringWriter = new StringWriter();
            CSVFileWriter csvFileWriter = new CSVFileWriter(stringWriter);
            DataTable dataTable = CsvTestsSamples.SimpleDataTable();
            csvFileWriter.WriteFromDataTable(dataTable);
            csvFileWriter.Close();
            string expected = CsvTestsSamples.SimpleCsvContents();
            Assert.AreEqual(expected, stringWriter.ToString());
        }

        [Test]
        public void TestCsvWriterComplex()
        {
            StringWriter stringWriter = new StringWriter();
            CSVFileWriter csvFileWriter = new CSVFileWriter(stringWriter);
            DataTable dataTable = CsvTestsSamples.ComplexDataTable();
            csvFileWriter.WriteFromDataTable(dataTable);
            csvFileWriter.Close();
            string expected = CsvTestsSamples.ComplexCsvContents();
            Assert.AreEqual(expected, stringWriter.ToString());
        }

        [Test]
        public void TestCsvWriterCommasInData()
        {
            StringWriter stringWriter = new StringWriter();
            CSVFileWriter csvFileWriter = new CSVFileWriter(stringWriter);
            DataTable dataTable = CsvTestsSamples.CommasInDataDataTable();
            csvFileWriter.WriteFromDataTable(dataTable);
            csvFileWriter.Close();
            string expected = CsvTestsSamples.CommasInDataCsvContents();
            Assert.AreEqual(expected, stringWriter.ToString());
        }

        [Test]
        public void TestCsvWriterCommasInHeader()
        {
            StringWriter stringWriter = new StringWriter();
            CSVFileWriter csvFileWriter = new CSVFileWriter(stringWriter);
            DataTable dataTable = CsvTestsSamples.CommasInHeaderDataTable();
            csvFileWriter.WriteFromDataTable(dataTable);
            csvFileWriter.Close();
            string expected = CsvTestsSamples.CommasInHeaderCsvContents();
            Assert.AreEqual(expected, stringWriter.ToString());
        }

        [Test]
        public void TestCsvWriterAdvanced()
        {
            StringWriter stringWriter = new StringWriter();
            CSVFileWriter csvFileWriter = new CSVFileWriter(stringWriter);
            DataTable dataTable = CsvTestsSamples.AdvancedDataTable();
            csvFileWriter.WriteFromDataTable(dataTable);
            csvFileWriter.Close();
            string expected = CsvTestsSamples.AdvancedCsvContents();
            Assert.AreEqual(expected, stringWriter.ToString());
        }
    }
}
