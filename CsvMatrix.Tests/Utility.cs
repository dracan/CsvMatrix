using System.IO;
using System.Text;
using CsvMatrix.Common;

namespace CsvMatrix.Tests
{
    class Utility
    {
        public static CsvFile CreateCsvObject(StringBuilder sourceData)
        {
            using(var ms = new MemoryStream())
            {
                using(var sw = new StreamWriter(ms, Encoding.UTF8))
                {
                    sw.Write(sourceData.ToString());
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    return new CsvFile(ms);
                }
            }
        }
    }
}
