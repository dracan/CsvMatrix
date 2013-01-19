using System.ComponentModel;

namespace CsvMatrix
{
    public class ColumnInfo
    {
        [DisplayName("Column Name")]
        public string ColumnName { get; set; }

        [DisplayName("Delete")]
        public bool Deleted { get; set; }
    }
}