using System;

namespace CsvMatrix.Common
{
    public class CsvProperties : ICloneable
    {
        public string Delimiter { get; set; }
        public bool HasHeaderRow { get; set; }

        public CsvProperties()
        {
            Delimiter = "\t";
            HasHeaderRow = true;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}