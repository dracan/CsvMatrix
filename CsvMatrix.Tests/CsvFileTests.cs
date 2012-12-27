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

            var csv = Utility.CreateCsvObject(testData);

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

            var csv = Utility.CreateCsvObject(testData);

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

            var csv = Utility.CreateCsvObject(testData);

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

            var csv = Utility.CreateCsvObject(testData);

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

        [Test, ExpectedException(typeof(InvalidCsvException))]
        public void TestExceptionIsThrownIfInvalidNumberOfColumnsInDataRow()
        {
            var testData = new StringBuilder();

            testData.AppendLine("\"FirstName\"\t\"Surname\"\tAge\t\"Gender\"");
            testData.AppendLine("\"Dan\"\t\"Clarke\"\t\"Male\"");

            var csv = Utility.CreateCsvObject(testData);
        }

        [Test]
        public void TestSaveCsv()
        {
            var testData = new StringBuilder();

            testData.AppendLine("FirstName\tSurname\tAge\tGender");
            testData.AppendLine("Dan\tClarke\t34\tMale");

            var csv = Utility.CreateCsvObject(testData);

            var tempFilename = Path.GetTempFileName();

            csv.Save(tempFilename);

            var fileData = File.ReadAllText(tempFilename);

            Assert.That(fileData, Is.EqualTo(testData.ToString()));
        }
    }
}
