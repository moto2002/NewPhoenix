using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetConfigConsole
{
    class MysqlHandler
    {
        private MySqlConnection m_Connection; 

        public MysqlHandler()
        {
            this.CreateConnect();
        }

        #region private methods

        private void CreateConnect()
        {
            try
            {
                if (this.m_Connection == null || this.m_Connection.State != ConnectionState.Open)
                {
                    Console.WriteLine("Start Connecting...");
                    MySqlConnectionStringBuilder mysqlBuilder = new MySqlConnectionStringBuilder();
                    mysqlBuilder.Server = MyConst.Server;
                    mysqlBuilder.Database = MyConst.Database;
                    mysqlBuilder.UserID = MyConst.UserID;
                    mysqlBuilder.Password = MyConst.Password;
                    string connectionStr = mysqlBuilder.ConnectionString;
                    Console.WriteLine("ConnectStr : "+ connectionStr);
                    this.m_Connection = new MySqlConnection(connectionStr);
                }
            }
            catch (Exception e )
            {
                Console.WriteLine("Error : " + e.Message);
            }
        }

        private List<string> GetTableNames()
        {
            try
            {
                this.m_Connection.Open();
                MySqlDataReader mysqlDR = new MySqlCommand(MyConst.MysqlCmd, this.m_Connection).ExecuteReader();
                List<string> tables = new List<string>();
                while (mysqlDR.Read())
                {
                    string table = mysqlDR.GetString(0);
                    if (!tables.Contains(table) && table.Contains(MyConst.TableNameKeyWord))
                    {
                        tables.Add(table);
                    }
                }
                this.m_Connection.Close();
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
            string query = string.Format(MyConst.TableQuery, tableName);
            return  new MySqlDataAdapter(query, this.m_Connection);
        }

        #endregion

        #region public methods

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

        #endregion
    }
}
