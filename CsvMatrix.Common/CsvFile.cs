using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace CsvMatrix.Common
{
    public class CsvFile : IDisposable
    {
        private DataTable _data;
        private const char _delimiter = '\t'; //(todo) Support different delimiters

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

            _data.AcceptChanges();
        }

        private void ProcessHeaderLine(string headerString)
        {
            if(headerString == null)
            {
                throw new InvalidCsvException();
            }

            var cells = SplitLine(headerString);

            _data = new DataTable();

            foreach(var cell in cells)
            {
                var value = cell;

                _data.Columns.Add(value);
            }
        }

        private string[] SplitLine(string lineString)
        {
            var cells = lineString.Split(_delimiter);
            var outCells = new List<string>();

            for(var n = 0; n < cells.Length; n++)
            {
                string cell = cells[n];
                string buffer = "";

                // If cell begins with a quote, but doesn't end with one, then we need to merge these cells
                if(cell.StartsWith("\"") && !cell.EndsWith("\""))
                {
                    while(!cells[n].EndsWith("\""))
                    {
                        buffer += cells[n++] + _delimiter;
                    }

                    if(n < cells.Length)
                    {
                        Debug.Assert(cells[n].EndsWith("\""));

                        buffer += cells[n];
                    }
                }
                else
                {
                    buffer = cells[n];
                }

                // Trim quotes
                if(buffer.StartsWith("\"") && buffer.EndsWith("\""))
                {
                    buffer = buffer.Substring(1, buffer.Length - 2);
                }

                outCells.Add(buffer);
            }

            return outCells.ToArray();
        }

        private void ProcessDataLine(string dataString)
        {
            var cells = SplitLine(dataString);

            if(cells.Length != _data.Columns.Count)
            {
                throw new InvalidCsvException();
            }

            var row = _data.NewRow();

            for(var cellIndex = 0; cellIndex < cells.Length; cellIndex++)
            {
                var value = cells[cellIndex];

                row[cellIndex] = value;
            }

            _data.Rows.Add(row);
        }

        public void Save(string filename)
        {
            using(var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using(var sw = new StreamWriter(fs))
                {
                    // Write the header row

                    var firstColumn = true;

                    foreach(DataColumn column in _data.Columns)
                    {
                        if(firstColumn)
                        {
                            firstColumn = false;
                        }
                        else
                        {
                            sw.Write(_delimiter);
                        }

                        sw.Write(column.ColumnName);
                    }

                    sw.WriteLine();

                    // Write the data rows

                    foreach(DataRow row in _data.Rows)
                    {
                        var firstCell = true;

                        foreach(var cell in row.ItemArray)
                        {
                            if(firstCell)
                            {
                                firstCell = false;
                            }
                            else
                            {
                                sw.Write(_delimiter);
                            }

                            sw.Write(cell);
                        }

                        sw.WriteLine();
                    }
                }
            }

            _data.AcceptChanges();
        }

        public void Dispose()
        {
        }

        public DataTable DataSource
        {
            get { return _data; }
        }

        public bool HasChanges
        {
            get { return _data.GetChanges() != null; }
        }
    }
}
