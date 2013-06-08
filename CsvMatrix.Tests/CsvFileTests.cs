using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvMatrix.Common;
using NUnit.Framework;

namespace CsvMatrix.Tests
{
    [TestFixture]
    public class CsvFileTests
    {
        [Test]
        public void TestVerySimpleValidCsvData()
        {
            var testData = new StringBuilder();

            testData.AppendLine("FirstName\tSurname\tAge\tGender");
            testData.AppendLine("Dan\tClarke\t34\tMale");
            testData.AppendLine("Anna\tClarke\t30\tFemale");
            testData.AppendLine("Jo\tBloggs\t50\tMale");

            bool loadRetVal;

            var csv = Utility.CreateCsvObject(testData, "\t", out loadRetVal);

            Assert.That(loadRetVal, Is.True);
            Assert.That(csv.DataSource.Columns.Count, Is.EqualTo(4));
            Assert.That(csv.DataSource.Rows.Count, Is.EqualTo(3));
            Assert.That(csv.DataSource.Columns[0].ColumnName, Is.EqualTo("FirstName"));
            Assert.That(csv.DataSource.Columns[1].ColumnName, Is.EqualTo("Surname"));
            Assert.That(csv.DataSource.Columns[2].ColumnName, Is.EqualTo("Age"));
            Assert.That(csv.DataSource.Columns[3].ColumnName, Is.EqualTo("Gender"));
            Assert.That(csv.DataSource.Rows[0][0], Is.EqualTo("Dan"));
            Assert.That(csv.DataSource.Rows[0][1], Is.EqualTo("Clarke"));
            Assert.That(csv.DataSource.Rows[0][2], Is.EqualTo("34"));
            Assert.That(csv.DataSource.Rows[0][3], Is.EqualTo("Male"));
        }

        [Test]
        public void TestCsvDataWithSurroundingQuotes()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"\t\"Surname\"\tAge\t\"Gender\"");
            testData.AppendLine("\"Dan\"\t\"Clarke\"\t34\t\"Male\"");

            bool loadRetVal;

            var csv = Utility.CreateCsvObject(testData, "\t", out loadRetVal);

            Assert.That(loadRetVal, Is.True);
            Assert.That(csv.DataSource.Columns.Count, Is.EqualTo(4));
            Assert.That(csv.DataSource.Rows.Count, Is.EqualTo(1));
            Assert.That(csv.DataSource.Columns[0].ColumnName, Is.EqualTo("FirstName"));
            Assert.That(csv.DataSource.Columns[1].ColumnName, Is.EqualTo("Surname"));
            Assert.That(csv.DataSource.Columns[2].ColumnName, Is.EqualTo("Age"));
            Assert.That(csv.DataSource.Columns[3].ColumnName, Is.EqualTo("Gender"));
            Assert.That(csv.DataSource.Rows[0][0], Is.EqualTo("Dan"));
            Assert.That(csv.DataSource.Rows[0][1], Is.EqualTo("Clarke"));
            Assert.That(csv.DataSource.Rows[0][2], Is.EqualTo("34"));
            Assert.That(csv.DataSource.Rows[0][3], Is.EqualTo("Male"));
        }

        [Test]
        public void TestCsvDataWithQuotesInContent()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"\t\"Surname\"\tAge\t\"Gender\"");
            testData.AppendLine("\"Dan\"\t\"Cl\"arke\"\t34\t\"Male\"");

            bool loadRetValue;

            var csv = Utility.CreateCsvObject(testData, "\t", out loadRetValue);

            Assert.That(loadRetValue, Is.True);
            Assert.That(csv.DataSource.Columns.Count, Is.EqualTo(4));
            Assert.That(csv.DataSource.Rows.Count, Is.EqualTo(1));
            Assert.That(csv.DataSource.Columns[0].ColumnName, Is.EqualTo("FirstName"));
            Assert.That(csv.DataSource.Columns[1].ColumnName, Is.EqualTo("Surname"));
            Assert.That(csv.DataSource.Columns[2].ColumnName, Is.EqualTo("Age"));
            Assert.That(csv.DataSource.Columns[3].ColumnName, Is.EqualTo("Gender"));
            Assert.That(csv.DataSource.Rows[0][0], Is.EqualTo("Dan"));
            Assert.That(csv.DataSource.Rows[0][1], Is.EqualTo("Cl\"arke"));
            Assert.That(csv.DataSource.Rows[0][2], Is.EqualTo("34"));
            Assert.That(csv.DataSource.Rows[0][3], Is.EqualTo("Male"));
        }

        [Test]
        public void TestCsvDataWithDelimiterInContent()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"\t\"Surname\"\tAge\t\"Gender\"");
            testData.AppendLine("\"Dan\"\t\"Cl\tarke\"\t34\t\"Male\"");

            bool loadRetValue;

            var csv = Utility.CreateCsvObject(testData, "\t", out loadRetValue);

            Assert.That(loadRetValue, Is.True);
            Assert.That(csv.DataSource.Columns.Count, Is.EqualTo(4));
            Assert.That(csv.DataSource.Rows.Count, Is.EqualTo(1));
            Assert.That(csv.DataSource.Columns[0].ColumnName, Is.EqualTo("FirstName"));
            Assert.That(csv.DataSource.Columns[1].ColumnName, Is.EqualTo("Surname"));
            Assert.That(csv.DataSource.Columns[2].ColumnName, Is.EqualTo("Age"));
            Assert.That(csv.DataSource.Columns[3].ColumnName, Is.EqualTo("Gender"));
            Assert.That(csv.DataSource.Rows[0][0], Is.EqualTo("Dan"));
            Assert.That(csv.DataSource.Rows[0][1], Is.EqualTo("Cl\tarke"));
            Assert.That(csv.DataSource.Rows[0][2], Is.EqualTo("34"));
            Assert.That(csv.DataSource.Rows[0][3], Is.EqualTo("Male"));
        }

        [Test]
        public void TestInvalidNumberOfColumnsInDataRowReturnsTrueButWithErrorMessage()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"\t\"Surname\"\tAge\t\"Gender\"");
            testData.AppendLine("\"Dan\"\t\"Clarke\"\t\"Male\"");

            bool loadRetValue;

            var csv = Utility.CreateCsvObject(testData, "\t", out loadRetValue);

            Assert.That(loadRetValue, Is.True);
            Assert.That(csv.LoadErrors.Count == 1);
        }

        [Test]
        public void TestLeadingAndTrailingWhiteSpaceIsRemovedFromLine()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\",\"Surname\",Age,\"Gender\"");
            testData.AppendLine("      \t\"Dan\",\"Clarke\",34,\"Male\"     \t    ");

            bool loadRetValue;

            var csv = Utility.CreateCsvObject(testData, ",", out loadRetValue);

            Assert.That(loadRetValue, Is.True);
            Assert.That(csv.DataSource.Columns.Count, Is.EqualTo(4));
            Assert.That(csv.DataSource.Rows.Count, Is.EqualTo(1));
            Assert.That(csv.DataSource.Columns[0].ColumnName, Is.EqualTo("FirstName"));
            Assert.That(csv.DataSource.Columns[1].ColumnName, Is.EqualTo("Surname"));
            Assert.That(csv.DataSource.Columns[2].ColumnName, Is.EqualTo("Age"));
            Assert.That(csv.DataSource.Columns[3].ColumnName, Is.EqualTo("Gender"));
            Assert.That(csv.DataSource.Rows[0][0], Is.EqualTo("Dan"));
            Assert.That(csv.DataSource.Rows[0][1], Is.EqualTo("Clarke"));
            Assert.That(csv.DataSource.Rows[0][2], Is.EqualTo("34"));
            Assert.That(csv.DataSource.Rows[0][3], Is.EqualTo("Male"));
        }

        [Test]
        public void TestSplitLine_Simple()
        {
            const string testLine = "\"Dan\",\"Clarke\",34,\"Male\"";
            var callback = new Func<string>(() => "");

            var result = (List<string>)Utility.RunMethod(new CsvFile(), "SplitLine", new object[] {testLine, callback});

            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0], Is.EqualTo("Dan"));
            Assert.That(result[1], Is.EqualTo("Clarke"));
            Assert.That(result[2], Is.EqualTo("34"));
            Assert.That(result[3], Is.EqualTo("Male"));
        }

        [Test]
        public void TestSplitLine_Multiline()
        {
            const string testLine = "\"Dan\",\"Clar";
            const string testLine2 = "ke\",34,\"Male\"";
            var callback = new Func<string>(() => testLine2);

            var result = (List<string>)Utility.RunMethod(new CsvFile(), "SplitLine", new object[] {testLine, callback});

            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0], Is.EqualTo("Dan"));
            Assert.That(result[1], Is.EqualTo("Clar\nke"));
            Assert.That(result[2], Is.EqualTo("34"));
            Assert.That(result[3], Is.EqualTo("Male"));
        }

        [Test]
        public void TestCellContainingWithNewLine()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"\t\"Surname\"\tAge\t\"Gender\"");
            testData.AppendLine("\"Dan\"\t\"Clar\nke\"\t34\t\"Male\"");

            bool loadRetValue;

            var csv = Utility.CreateCsvObject(testData, "\t", out loadRetValue);

            Assert.That(loadRetValue, Is.True);
            Assert.That(csv.DataSource.Columns.Count, Is.EqualTo(4));
            Assert.That(csv.DataSource.Rows.Count, Is.EqualTo(1));
            Assert.That(csv.DataSource.Columns[0].ColumnName, Is.EqualTo("FirstName"));
            Assert.That(csv.DataSource.Columns[1].ColumnName, Is.EqualTo("Surname"));
            Assert.That(csv.DataSource.Columns[2].ColumnName, Is.EqualTo("Age"));
            Assert.That(csv.DataSource.Columns[3].ColumnName, Is.EqualTo("Gender"));
            Assert.That(csv.DataSource.Rows[0][0], Is.EqualTo("Dan"));
            Assert.That(csv.DataSource.Rows[0][1], Is.EqualTo("Clar\nke"));
            Assert.That(csv.DataSource.Rows[0][2], Is.EqualTo("34"));
            Assert.That(csv.DataSource.Rows[0][3], Is.EqualTo("Male"));
        }

        [Test]
        public void TestCellContainingWithMultipleNewLines()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"\t\"Surname\"\tAge\t\"Gender\"");
            testData.AppendLine("\"Dan\"\t\"Cl\nar\nke\"\t34\t\"Male\"");

            bool loadRetValue;

            var csv = Utility.CreateCsvObject(testData, "\t", out loadRetValue);

            Assert.That(loadRetValue, Is.True);
            Assert.That(csv.DataSource.Columns.Count, Is.EqualTo(4));
            Assert.That(csv.DataSource.Rows.Count, Is.EqualTo(1));
            Assert.That(csv.DataSource.Columns[0].ColumnName, Is.EqualTo("FirstName"));
            Assert.That(csv.DataSource.Columns[1].ColumnName, Is.EqualTo("Surname"));
            Assert.That(csv.DataSource.Columns[2].ColumnName, Is.EqualTo("Age"));
            Assert.That(csv.DataSource.Columns[3].ColumnName, Is.EqualTo("Gender"));
            Assert.That(csv.DataSource.Rows[0][0], Is.EqualTo("Dan"));
            Assert.That(csv.DataSource.Rows[0][1], Is.EqualTo("Cl\nar\nke"));
            Assert.That(csv.DataSource.Rows[0][2], Is.EqualTo("34"));
            Assert.That(csv.DataSource.Rows[0][3], Is.EqualTo("Male"));
        }

        [Test]
        public void TestDetermineDelimiter_UsingTabs()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"\t\"Surname\"\tAge\t\"Gender\"");
            testData.AppendLine("\"Dan\"\t\"Cl\nar\nke\"\t34\t\"Male\"");

            var tempFilename = Path.GetTempFileName();
            File.WriteAllText(tempFilename, testData.ToString());

            int numColumns;

            var delimiter = CsvFile.DetermineDelimiter(tempFilename, out numColumns);

            Assert.That(delimiter, Is.EqualTo("\t"));
            Assert.That(numColumns, Is.EqualTo(4));
        }

        [Test]
        public void TestDetermineDelimiter_UsingComma()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\",\"Surname\",Age,\"Gender\"");
            testData.AppendLine("\"Dan\",\"Cl\nar\nke\",34,\"Male\"");

            var tempFilename = Path.GetTempFileName();
            File.WriteAllText(tempFilename, testData.ToString());

            int numColumns;

            var delimiter = CsvFile.DetermineDelimiter(tempFilename, out numColumns);

            Assert.That(delimiter, Is.EqualTo(","));
        }

        [Test]
        public void TestDetermineDelimiter_UsingSemicolon()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\";\"Surname\";Age;\"Gender\"");
            testData.AppendLine("\"Dan\";\"Cl\nar\nke\";34;\"Male\"");

            var tempFilename = Path.GetTempFileName();
            File.WriteAllText(tempFilename, testData.ToString());

            int numColumns;

            var delimiter = CsvFile.DetermineDelimiter(tempFilename, out numColumns);

            Assert.That(delimiter, Is.EqualTo(";"));
        }

        [Test]
        public void TestDetermineDelimiter_UsingPipe()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"|\"Surname\"|Age|\"Gender\"");
            testData.AppendLine("\"Dan\"|\"Cl\nar\nke\"|34|\"Male\"");

            var tempFilename = Path.GetTempFileName();
            File.WriteAllText(tempFilename, testData.ToString());

            int numColumns;

            var delimiter = CsvFile.DetermineDelimiter(tempFilename, out numColumns);

            Assert.That(delimiter, Is.EqualTo("|"));
        }

        [Test]
        public void TestEmptyStringWithoutQuotesAsLastColumn()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"\t\"Surname\"\tAge\t");
            testData.AppendLine("\"Dan\"\t\"Cl\nar\nke\"\t34\t");

            var tempFilename = Path.GetTempFileName();
            File.WriteAllText(tempFilename, testData.ToString());

            int numColumns;

            var delimiter = CsvFile.DetermineDelimiter(tempFilename, out numColumns);

            Assert.That(delimiter, Is.EqualTo("\t"));
            Assert.That(numColumns, Is.EqualTo(4));
        }

        [Test]
        public void TestSaveCsv()
        {
            var testData = new StringBuilder();

            testData.AppendLine("FirstName\tSurname\tAge\tGender");
            testData.AppendLine("Dan\tClarke\t34\tMale");

            bool loadRetValue;

            var csv = Utility.CreateCsvObject(testData, "\t", out loadRetValue);
            Assert.That(loadRetValue, Is.True);

            var tempFilename = Path.GetTempFileName();

            csv.Save(tempFilename);

            var fileData = File.ReadAllText(tempFilename);

            Assert.That(fileData, Is.EqualTo(testData.ToString()));
        }

        [Test]
        public void TestSaveCsvWithDelimiterInCellValue()
        {
            var testData = new StringBuilder();

            testData.AppendLine("FirstName\tSurname\tAge\tGender");
            testData.AppendLine("Dan\t\"Cla\trke\"\t34\tMale");

            bool loadRetValue;

            var csv = Utility.CreateCsvObject(testData, "\t", out loadRetValue);
            Assert.That(loadRetValue, Is.True);

            var tempFilename = Path.GetTempFileName();

            csv.Save(tempFilename);

            var fileData = File.ReadAllText(tempFilename);

            Assert.That(fileData, Is.EqualTo(testData.ToString()));
        }
    }
}
