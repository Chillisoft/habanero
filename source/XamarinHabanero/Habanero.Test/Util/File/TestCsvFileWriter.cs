#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Data;
using System.IO;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util.File
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
