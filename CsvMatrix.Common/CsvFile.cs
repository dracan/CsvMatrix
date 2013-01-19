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
        private CsvProperties _properties;

        public CsvProperties Properties
        {
            get { return _properties; }
            set
            {
                _properties = value; ProcessPropertyChange();
            }
        }

        private readonly List<string> _loadErrors = new List<string>();
        public List<string> LoadErrors
        {
            get { return _loadErrors; }
        }

        public CsvFile(CsvProperties properties = null)
        {
            Properties = properties ?? new CsvProperties();
        }

        public void CreateNew(int numColumns, int numRows)
        {
            _data = new DataTable();

            for(var x = 0; x < numColumns; x++)
            {
                _data.Columns.Add();
            }

            for(var y = 0; y < numRows; y++)
            {
                _data.Rows.Add(_data.NewRow());
            }
        }

        public bool Load(string filename)
        {
            using(var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return LoadCsvFile(fs);
            }
        }

        public bool Load(MemoryStream stream)
        {
            return LoadCsvFile(stream);
        }

        public static string DetermineDelimiter(string filename, out int numColumns)
        {
            var delimiters = new[] { "\t", ",", ";", "|" };

            foreach(var delimiter in delimiters)
            {
                using(var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    using(var csv = new CsvFile(new CsvProperties { Delimiter = delimiter, MaxRowCount = 5 }))
                    {
                        if(csv.LoadCsvFile(fs) && csv.DataSource.Columns.Count > 1)
                        {
                            numColumns = csv.DataSource.Columns.Count;
                            return delimiter;
                        }
                    }
                }
            }

            numColumns = 0;
            return CsvProperties.DefaultDelimiter;
        }

        private bool LoadCsvFile(Stream stream)
        {
            LoadErrors.Clear();

            using(var sr = new StreamReader(stream))
            {
                // Try to read the first line to ascertain the schema

                string str = sr.ReadLine();

                if(!ProcessHeaderLine(str))
                {
                    return false;
                }

                var rowCount = 0;

                while((str = sr.ReadLine()) != null)
                {
                    if(str.Length > 0)
                    {
                        if(!ProcessDataLine(str, sr.ReadLine))
                        {
                            rowCount++;
                            LoadErrors.Add(String.Format("Error in row {0} - invalid number of columns. This line will be ignored.", rowCount));
                            continue;
                        }

                        if((Properties.MaxRowCount != -1) && (rowCount > Properties.MaxRowCount))
                        {
                            break;
                        }

                        rowCount++;
                    }
                }
            }

            _data.AcceptChanges();

            return true;
        }

        private bool ProcessHeaderLine(string headerString)
        {
            if(headerString == null)
            {
                LoadErrors.Add("File is an empty file");
                return false;
            }

            var cells = SplitLine(headerString, null);

            _data = new DataTable();

            foreach(var cell in cells)
            {
                _data.Columns.Add(Properties.HasHeaderRow ? cell : "");
            }

            // If this CSV doesn't have a header row, then add a data row for this first row
            if(!Properties.HasHeaderRow)
            {
                var row = _data.NewRow();

                var cellIndex = 0;

                foreach(var cell in cells)
                {
                    row[cellIndex++] = cell;
                }

                _data.Rows.Add(row);
            }

            return true;
        }

        private bool ProcessDataLine(string dataString, Func<string> getNextLine)
        {
            var cells = SplitLine(dataString, getNextLine);

            if(cells.Count != _data.Columns.Count)
            {
                return false;
            }

            var row = _data.NewRow();

            for(var cellIndex = 0; cellIndex < cells.Count; cellIndex++)
            {
                var value = cells[cellIndex];

                row[cellIndex] = value;
            }

            _data.Rows.Add(row);

            return true;
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
            string originalFileCopyFilename = null;

            if(File.Exists(filename))
            {
                // Copy old file to temp file, so we can restore if there's an error when saving
                originalFileCopyFilename = Path.GetTempFileName();

                File.Copy(filename, originalFileCopyFilename, true);
            }

            _data.AcceptChanges();

            if(columns == null)
            {
                // No column order / subset has been specified, so use all the columns in the order in the main datatable
                columns = new List<int>();

                for(var c = 0; c < _data.Columns.Count; c++)
                {
                    columns.Add(c);
                }
            }

            try
            {
                using(var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    using(var sw = new StreamWriter(fs))
                    {
                        // Write the header row

                        var firstColumn = true;

                        if(Properties.HasHeaderRow)
                        {
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

                                var columnName = _data.Columns[columnIndex].ColumnName;

                                if(columnName.Contains(Properties.Delimiter) || columnName.Contains("\n"))
                                {
                                    sw.Write("\"" + columnName + "\"");
                                }
                                else
                                {
                                    sw.Write(columnName);
                                }
                            }

                            sw.WriteLine();
                        }

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

                                var value = row[columnIndex].ToString();

                                if(value.Contains(Properties.Delimiter) || value.Contains("\n"))
                                {
                                    sw.Write("\"" + value + "\"");
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

                // No exception occurred, so we can remove the temporary backup copy
                if(!string.IsNullOrEmpty(originalFileCopyFilename))
                {
                    File.Delete(originalFileCopyFilename);
                }
            }
            catch(Exception)
            {
                // An error has occurred, so restore the original file

                if(!string.IsNullOrEmpty(originalFileCopyFilename))
                {
                    File.Copy(originalFileCopyFilename, filename, true);
                }

                throw;
            }
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

        public void UpdateProperties()
        {
            if(_data != null)
            {
                Properties.NumColumns = _data.Columns.Count;
                Properties.NumRows = _data.Rows.Count;
            }
        }

        private void ProcessPropertyChange()
        {
            if(_data != null)
            {
                while(_data.Columns.Count > Properties.NumColumns)
                {
                    _data.Columns.RemoveAt(_data.Columns.Count - 1);
                }

                while(Properties.NumColumns > _data.Columns.Count)
                {
                    _data.Columns.Add();
                }

                while(_data.Rows.Count > Properties.NumRows)
                {
                    _data.Rows.RemoveAt(_data.Rows.Count - 1);
                }

                while(Properties.NumRows > _data.Rows.Count)
                {
                    _data.Rows.Add();
                }
            }
        }
    }
}
