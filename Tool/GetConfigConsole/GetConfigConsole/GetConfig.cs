using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;

namespace GetConfigConsole
{
    class GetConfig
    {
        public void Execute()
        {
            DataSet dataSet = new MysqlHandler().GetDataSet();
            MemoryStream ms = this.Compression(dataSet);
            this.SaveToFile(ms);
        }

        private MemoryStream Compression(DataSet dataSet)
        {
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            ms.Seek(0, SeekOrigin.Begin);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, dataSet);
            Console.WriteLine("Compression Before Length : " + ms.Length);
            Console.WriteLine("Start Compressing...");
            MemoryStream compressedMS = this.Compression(ms.GetBuffer());
            //byte[] compressedBytes = this.Compression(ms.ToArray());
            Console.WriteLine("Compression After Length : " + compressedMS.Length);
            return compressedMS;
        }

        private MemoryStream Compression(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream gzip = new GZipStream(ms, CompressionLevel.Optimal);
            gzip.Write(bytes, 0, bytes.Length);
            gzip.Flush();
            gzip.Close();
            return ms;
        }

        private void SaveToFile(MemoryStream ms)
        {
            using (FileStream fs = new FileStream(MyConst.ConfigPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs.Seek(0, SeekOrigin.Begin);
                ms.WriteTo(fs);
            }
        }
    }
}
