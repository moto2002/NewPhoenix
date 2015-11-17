using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Data;
using MySql.Data.MySqlClient;
using ICSharpCode.SharpZipLib.Zip;

public sealed class ConfigCtrller
{
    public static ConfigCtrller Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new ConfigCtrller();
            }
            return m_Instance;
        }
    }
    private static ConfigCtrller m_Instance;
    public ActorConfig Actor { get; private set; }
    public SkillConfig Skill { get; private set; }
    //private DataSet m_DataSet;

    private MySqlConnection m_Connection;

    public ConfigCtrller()
    {
        DataSet dataSet = this.GetDataSet();
        if (dataSet != null)
        {
            Debug.Log("文件解压成功");
            DataTable config_actor = dataSet.Tables["config_actor"];
            DataRow row = config_actor.Rows[0];
            Debug.Log("ID " + row["ID"] + " Name " + row["Name"] + " Model " + row["Model"]);
        }
        this.Actor = new ActorConfig(dataSet.Tables["config_actor"]);
        this.Skill = new SkillConfig(
            actorSkillTable: dataSet.Tables["config_actorskill"],
            activeSkillTable: dataSet.Tables["config_activeskill"], 
            passiveSkillTable: dataSet.Tables["config_passiveskill"],
            triggerSkillTable: dataSet.Tables["config_triggerskill"],
            buffTable: dataSet.Tables["config_buff"]);
    }

    private DataSet GetDataSet()
    {
        string configFilePath = Path.Combine(Application.persistentDataPath, ConfigConst.ConfigData);
        if (!File.Exists(configFilePath))
        {
            Debug.LogError("配置文件不存在 " + configFilePath);
            return null;
        }
        //取文件
        using (Stream stream = new FileStream(configFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            if (stream.Length == 0)
            {
                Debug.LogError("配置文件大小为0");
                //fs.Close();
                return null;
            }
            Debug.Log("文件大小 " + stream.Length);
            stream.Position = 0;
            stream.Seek(0, SeekOrigin.Begin);
            using (ZipInputStream zipInputStream = new ZipInputStream(stream))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ZipEntry zipEntery;
                    while ((zipEntery = zipInputStream.GetNextEntry()) != null)
                    {
                        int count = (int)zipEntery.Size;
                        byte[] data = new byte[count];
                        zipInputStream.Read(data, 0, count);
                        memoryStream.Write(data, 0, count);
                    }
                    memoryStream.Position = 0;
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    IFormatter formatter = new BinaryFormatter();
                    return (DataSet)formatter.Deserialize(memoryStream);
                }
            }
        }
    }
}
