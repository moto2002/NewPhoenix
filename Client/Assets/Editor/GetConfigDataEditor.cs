using UnityEngine;
using UnityEditor;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;


public sealed class RefreshConfigEditor : Editor
{
    
    private static MySqlConnection m_Connection;


    [MenuItem("jlx/GetConfigData")]
    private static void GetConfigData()
    {
    }

    
    private static void CreateConnect()
    {
        try
        {
            if (m_Connection == null || m_Connection.State != ConnectionState.Open)
            {
                Debug.Log("Start Connecting...");
                MySqlConnectionStringBuilder mysqlBuilder = new MySqlConnectionStringBuilder();
                mysqlBuilder.Server =  ConfigConst.Server;
                mysqlBuilder.Database = ConfigConst.Database;
                mysqlBuilder.UserID = ConfigConst.UserID;
                mysqlBuilder.Password = ConfigConst.Password;
                string connectionStr = mysqlBuilder.ConnectionString;
                Console.WriteLine("ConnectStr : " + connectionStr);
                m_Connection = new MySqlConnection(connectionStr);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e.Message);
        }
    }

    private List<string> GetTableNames()
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
            Console.WriteLine("Error : " + e.Message);
            throw;
        }
    }

    private MySqlDataAdapter GetTableData(string tableName)
    {
        Console.WriteLine("query : " + tableName);
        string query = string.Format(ConfigConst.TableQuery, tableName);
        return new MySqlDataAdapter(query, m_Connection);
    }

    public DataSet GetDataSet()
    {
        try
        {
            DataSet dataSet = new DataSet();
            List<string> tableNames = this.GetTableNames();
            tableNames.ForEach(a =>
            {
                this.GetTableData(a).Fill(dataSet, a);
            });
            dataSet.RemotingFormat = SerializationFormat.Binary;
            return dataSet;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error : " + e.Message);
            return null;
        }
    }
    
}
