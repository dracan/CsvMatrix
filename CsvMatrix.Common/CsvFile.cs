using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CsvMatrix.Common
{
    public class CsvFile : IDisposable
    {
        private DataTable _data;

        public CsvProperties Properties { get; set; }

        public CsvFile(CsvProperties properties = null)
        {
            Properties = properties ?? new CsvProperties();
        }

        public CsvFile(string filename, CsvProperties properties = null)
        {
            Properties = properties ?? new CsvProperties();

            using(var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                LoadCsvFile(fs);
            }
        }

        public CsvFile(MemoryStream stream, CsvProperties properties = null)
        {
            Properties = properties ?? new CsvProperties();

            Properties = new CsvProperties();

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
                        ProcessDataLine(str, sr.ReadLine);
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

            var cells = SplitLine(headerString, null);

            _data = new DataTable();

            foreach(var cell in cells)
            {
                var value = cell;

                _data.Columns.Add(value);
            }
        }

        private void ProcessDataLine(string dataString, Func<string> getNextLine)
        {
            var cells = SplitLine(dataString, getNextLine);

            if(cells.Count != _data.Columns.Count)
            {
                throw new InvalidCsvException();
            }

            var row = _data.NewRow();

            for(var cellIndex = 0; cellIndex < cells.Count; cellIndex++)
            {
                var value = cells[cellIndex];

                row[cellIndex] = value;
            }

            _data.Rows.Add(row);
        }

        private string ProcessLookaheadLines(Func<string> getNextLine, string cellString, List<string> cells)
        {
            if(getNextLine != null)
            {
                var nextLine = getNextLine();

                if(nextLine != null)
                {
                    // Trim any leading and trailing whitespace characters
                    nextLine = nextLine.Trim();

                    var nextLinesCells = nextLine.Split(new[] { Properties.Delimiter }, StringSplitOptions.None);

                    var n = 0;

                    while(n < nextLinesCells.Length && !nextLinesCells[n].EndsWith("\""))
                    {
                        cellString += nextLinesCells[n] + ((n == nextLinesCells.Length - 1) ? "\n" : Properties.Delimiter.ToString(CultureInfo.InvariantCulture));
                        n++;
                    }

                    if(n < nextLinesCells.Length)
                    {
                        Debug.Assert(nextLinesCells[n].EndsWith("\""));

                        cellString += nextLinesCells[n];

                        n++;

                        for(var j = n; j < nextLinesCells.Length; j++)
                        {
                            var cell = nextLinesCells[j];

                            // Trim quotes
                            if(cell.StartsWith("\"") && cell.EndsWith("\""))
                            {
                                cell = cell.Substring(1, cell.Length - 2);
                            }

                            cells.Add(cell);
                        }
                    }
                    else
                    {
                        cellString = ProcessLookaheadLines(getNextLine, cellString, cells);
                    }
                }
            }

            return cellString;
        }

        private IList<string> SplitLine(string lineString, Func<string> getNextLine)
        {
            // Trim any leading and trailing whitespace characters
            lineString = lineString.Trim();

            var cells = new List<string>();
            
            cells.AddRange(lineString.Split(new[] { Properties.Delimiter }, StringSplitOptions.None));

            var outCells = new List<string>();

            for(var n = 0; n < cells.Count; n++)
            {
                string cell = cells[n];
                string buffer = "";
                var lookaheadCells = new List<string>();

                // If cell begins with a quote, but doesn't end with one, then we need to merge these cells.
                // This also handles merging cells with line breaks by getting "lookahead" lines from the CSV
                // file (via an event handler passed in as a parameter). The ProcessLookaheadLines internally
                // recursively calls itself if there is more than one linebreak within the same CSV cell.

                if(cell.StartsWith("\"") && !cell.EndsWith("\""))
                {
                    while(n < cells.Count && !cells[n].EndsWith("\""))
                    {
                        buffer += cells[n] + ((n == cells.Count - 1) ? "\n" : Properties.Delimiter.ToString(CultureInfo.InvariantCulture));
                        n++;
                    }

                    if(n < cells.Count)
                    {
                        Debug.Assert(cells[n].EndsWith("\""));

                        buffer += cells[n];
                    }
                    else
                    {
                        buffer = ProcessLookaheadLines(getNextLine, buffer, lookaheadCells);
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
                outCells.AddRange(lookaheadCells);
            }

            return outCells;
        }

        /// <summary>
        /// Saves the CSV file
        /// </summary>
        /// <param name="filename">Destination CSV filename</param>
        /// <param name="columns">List of columns indices to save. This is passed is because the user may have changed the sort order, or they may just saving a subset of columns</param>
        public void Save(string filename, IList<int> columns = null)
        {
            if(columns == null)
            {
                // No column order / subset has been specified, so use all the columns in the order in the main datatable
                columns = new List<int>();

                for(var c = 0; c < _data.Columns.Count; c++)
                {
                    columns.Add(c);
                }
            }
            using(var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using(var sw = new StreamWriter(fs))
                {
                    // Write the header row

                    var firstColumn = true;

                    foreach(var columnIndex in columns)
                    {
                        if(firstColumn)
                        {
                            firstColumn = false;
                        }
                        else
                        {
                            sw.Write(Properties.Delimiter);
                        }

                        sw.Write(_data.Columns[columnIndex].ColumnName);
                    }

                    sw.WriteLine();

                    // Write the data rows

                    foreach(DataRow row in _data.Rows)
                    {
                        var firstCell = true;

                        foreach(var columnIndex in columns)
                        {
                            if(firstCell)
                            {
                                firstCell = false;
                            }
                            else
                            {
                                sw.Write(Properties.Delimiter);
                            }

                            var value = row[columnIndex];

                            if(value.ToString().Contains(Properties.Delimiter.ToString(CultureInfo.InvariantCulture)))
                            {
                                sw.Write("\"" + row[columnIndex] + "\"");
                            }
                            else
                            {
                                sw.Write(row[columnIndex]);
                            }
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
