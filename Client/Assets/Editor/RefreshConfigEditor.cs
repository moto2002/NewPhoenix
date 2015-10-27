using UnityEngine;
using UnityEditor;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using System.Runtime.Serialization;

public sealed class RefreshConfigEditor : Editor
{
    private static MySqlConnection m_Connection;

    [MenuItem("jlx/RefreshConfigData")]
    private static void RefreshConfigData()
    {
        CreateConnect();
        DataSet dataSet = GetDataSet();
        //MemoryStream ms = Compression(dataSet);
        //SaveToFile(ms);

        byte[] bytes = Compression(dataSet);
        SaveToFile(bytes);
    }

    [MenuItem("jlx/GetConfigData")]
    private static void GetConfigData()
    {
        DataSet dataSet = new DataSet();
        Stream stream = GetFromFile();
        Debug.Log("GetFromFile " + stream.Length);
        MemoryStream memoryStream = new MemoryStream();
        ZipInputStream zipInputStream = new ZipInputStream(stream);
        ZipEntry zipEntry;
        while ((zipEntry = zipInputStream.GetNextEntry()) != null)
        {
            byte[] data = new byte[zipEntry.Size];
            zipInputStream.Read(data, 0, data.Length);
            memoryStream.Write(data, 0, data.Length);
        }
       
        Debug.Log("ms " + memoryStream.Length);
        memoryStream.Position = 0;
        memoryStream.Seek(0, SeekOrigin.Begin);
        IFormatter formatter = new BinaryFormatter();
        dataSet = (DataSet)formatter.Deserialize(memoryStream);
        memoryStream.Close();
        zipInputStream.Close();
        stream.Close();
        DataTable config_actor = dataSet.Tables["config_actor"];
        foreach (DataRow item in config_actor.Rows)
        {
            Debug.Log(item["id"] + "" + item["name"]);
        }
    }

    private static void CreateConnect()
    {
        try
        {
            if (m_Connection == null || m_Connection.State != ConnectionState.Open)
            {
                Debug.Log("Start Connecting...");
                MySqlConnectionStringBuilder mysqlBuilder = new MySqlConnectionStringBuilder();
                mysqlBuilder.Server = ConfigConst.Server;
                mysqlBuilder.Database = ConfigConst.Database;
                mysqlBuilder.UserID = ConfigConst.UserID;
                mysqlBuilder.Password = ConfigConst.Password;
                string connectionStr = mysqlBuilder.ConnectionString;
                Debug.Log("ConnectStr : " + connectionStr);
                m_Connection = new MySqlConnection(connectionStr);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e.Message);
        }
    }

    private static List<string> GetTableNames()
    {
        try
        {
            m_Connection.Open();
            MySqlDataReader mysqlDR = new MySqlCommand(ConfigConst.MysqlCmd, m_Connection).ExecuteReader();
            List<string> tables = new List<string>();
            while (mysqlDR.Read())
            {
                string table = mysqlDR.GetString(0);
                if (!tables.Contains(table) && table.Contains(ConfigConst.TableNameKeyWord))
                {
                    tables.Add(table);
                }
            }
            m_Connection.Close();
            return tables;
        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e.Message);
            throw;
        }
    }

    private static MySqlDataAdapter GetTableData(string tableName)
    {
        //Debug.Log("query : " + tableName);
        string query = string.Format(ConfigConst.TableQuery, tableName);
        return new MySqlDataAdapter(query, m_Connection);
    }

    private static DataSet GetDataSet()
    {
        try
        {
            DataSet dataSet = new DataSet();
            List<string> tableNames = GetTableNames();
            tableNames.ForEach(a =>
            {
                GetTableData(a).Fill(dataSet, a);
            });
            dataSet.RemotingFormat = SerializationFormat.Binary;
            return dataSet;
        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e.Message);
            return null;
        }
    }

    private static void SaveToFile(MemoryStream ms)
    {
        using (FileStream fs = new FileStream(Path.Combine(Application.persistentDataPath, ConfigConst.ConfigName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            fs.Seek(0, SeekOrigin.Begin);
            ms.WriteTo(fs);
        }
        Debug.Log("SaveToFile Complete");
    }

    private static void SaveToFile(byte[] bytes)
    {
        using (FileStream fs = new FileStream(Path.Combine(Application.persistentDataPath, ConfigConst.ConfigName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            fs.Seek(0, SeekOrigin.Begin);
            fs.Write(bytes,0, bytes.Length);
        }
        Debug.Log("SaveToFile Complete");
    }

    private static Stream GetFromFile()
    {
        return File.OpenRead(Path.Combine(Application.persistentDataPath, ConfigConst.ConfigName));
    }

    private static byte[]/*MemoryStream*/ Compression(DataSet dataSet)
    {
        MemoryStream ms = new MemoryStream();
        ms.Position = 0;
        ms.Seek(0, SeekOrigin.Begin);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(ms, dataSet);
        Debug.Log("Compression Before Length : " + ms.Length);
        Debug.Log("Start Compressing...");
        //MemoryStream compressedMS = Compression(ms.GetBuffer());
        byte[] compressedBytes = Compression(ms.ToArray());
        Debug.Log("Compression After Length : " + compressedBytes.Length);
        //return new MemoryStream(compressedBytes);
        return compressedBytes;
    }

    private static byte[] Compression(byte[] bytes)
    {
        MemoryStream ms = new MemoryStream();
        ZipOutputStream zos = new ZipOutputStream(ms);
        ZipEntry ze = new ZipEntry(ConfigConst.ZipEntryName);
        zos.PutNextEntry(ze);
        zos.SetLevel(9);
        zos.Write(bytes, 0, bytes.Length);//写入压缩文件 
        zos.Close();
        return ms.ToArray();
    }
}
