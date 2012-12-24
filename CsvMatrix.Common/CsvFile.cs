using System;
using System.Data;
using System.IO;

namespace CsvMatrix.Common
{
    public class CsvFile : IDisposable
    {
        private DataTable _data;

        public CsvFile(string filename)
        {
            using(var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                LoadCsvFile(fs);
            }
        }

        public CsvFile(MemoryStream stream)
        {
            LoadCsvFile(stream);
        }

        private void LoadCsvFile(Stream stream)
        {
            using(var sr = new StreamReader(stream))
            {
                // Try to read the first line to ascertain the schema

                string str = sr.ReadLine();

                ProcessHeaderLine(str);

                while((str = sr.ReadLine()) != null)
                {
                    if(str.Length > 0)
                    {
                        ProcessDataLine(str);
                    }
                }
            }
        }

        private void ProcessHeaderLine(string headerString)
        {
            if(headerString == null)
            {
                throw new InvalidCsvException();
            }

            // Split the line using tab character as a delimiter. Currently this does this blindly.
            // Later on it'll handle different delimiters and quotes.

            var cells = headerString.Split('\t');

            _data = new DataTable();

            foreach(var cell in cells)
            {
                var value = cell;

                // Trim quotes
                if(value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                _data.Columns.Add(value);
            }
        }

        private void ProcessDataLine(string dataString)
        {
            // Split the line using tab character as a delimiter. Currently this does this blindly.
            // Later on it'll handle different delimiters and quotes.

            var cells = dataString.Split('\t');

            if(cells.Length != _data.Columns.Count)
            {
                throw new InvalidCsvException();
            }

            var row = _data.NewRow();

            for(var cellIndex = 0; cellIndex < cells.Length; cellIndex++)
            {
                var value = cells[cellIndex];

                // Trim quotes
                if(value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                row[cellIndex] = value;
            }

            _data.Rows.Add(row);
        }

        public void Dispose()
        {
        }

        public DataTable DataSource
        {
            get { return _data; }
        }
    }
}
