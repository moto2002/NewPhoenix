using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Data;
using MySql.Data.MySqlClient;

public sealed class ConfigController
{
    public static ConfigController Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new ConfigController();
            }
            return m_Instance;
        }
    }
    private static ConfigController m_Instance;
    public ActorConfig Actor { get; private set; }
    public SkillConfig Skill { get; private set; }
    //private DataSet m_DataSet;

    private  MySqlConnection m_Connection;

    public ConfigController()
    {
        DataSet dataSet = this.GetDataSet();
        if(dataSet!=null)
        {
            Debug.Log("文件解压成功");
        }
        this.Actor = new ActorConfig();
        this.Skill = new SkillConfig();
       
    }



    private DataSet GetDataSet()
    {
        string configFilePath = Path.Combine(Application.persistentDataPath, AssetNameConst.Loading);
        if (!File.Exists(configFilePath))
        {
            Debug.LogError("配置文件不存在 " + configFilePath);
            return null;
        }
        DataSet dataSet = null;
        //取文件
        using (FileStream fs = new FileStream(configFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            if (fs.Length == 0)
            {
                Debug.LogError("配置文件大小为0");
                //fs.Close();
                return null;
            }
            Debug.Log("文件大小 " + fs.Length);
            fs.Position = 0;
            //反序列化
            IFormatter fm = new BinaryFormatter();
            object o = fm.Deserialize(fs);
            //可以尝试直接发序列化到 MemoryStream 里
            using (MemoryStream compressedMS = new MemoryStream())
            {
                //序列化到内存流
                fm.Serialize(compressedMS, o);
                //解压
                using (MemoryStream decompressedMS = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(compressedMS, CompressionMode.Decompress))
                    {
                        int bufferLength = 2000;
                        byte[] tmpBuffer = new byte[bufferLength];
                        int length = 0;
                        while((length = gzip.Read(tmpBuffer,0, bufferLength))>0)
                        {
                            decompressedMS.Write(tmpBuffer, 0, length);
                        }
                        fm.Serialize(decompressedMS, dataSet);
                    }
                }
            }
        }
        return dataSet;
    } 


}
