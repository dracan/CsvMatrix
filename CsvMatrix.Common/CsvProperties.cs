using System;

namespace CsvMatrix.Common
{
    public class CsvProperties : ICloneable
    {
        public const string DefaultDelimiter = ",";

        public string Delimiter { get; set; }
        public bool HasHeaderRow { get; set; }
        public int MaxRowCount { get; set; }
        public int NumColumns { get; set; }
        public int NumRows { get; set; }

        public CsvProperties()
        {
            Delimiter = DefaultDelimiter;
            HasHeaderRow = true;
            MaxRowCount = -1;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}