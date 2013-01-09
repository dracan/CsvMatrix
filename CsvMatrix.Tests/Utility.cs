using System;
using System.IO;
using System.Reflection;
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

        public static object RunStaticMethod(Type classType, string methodName, object[] methodParams)
        {
            var m = classType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            if(m == null)
            {
                throw new ArgumentException(String.Format("Static method for testing not found: {0}->{1}", classType, methodName));
            }

            return m.Invoke(null, methodParams);
        }

        public static object RunMethod(object instance, string methodName, object[] methodParams)
        {
            var t = instance.GetType();
            var m = t.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if(m == null)
            {
                throw new ArgumentException(String.Format("Method for testing not found: {0}->{1}", t, methodName));
            }

            return m.Invoke(instance, methodParams);
        }
    }
}
