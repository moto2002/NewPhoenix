using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetConfigConsole
{
    class MyConst
    {
        public const string Server = @"localhost";
        public const string Database = @"phoenixconfig";
        public const string UserID = @"root";
        public const string Password = @"jlxmysql";
        public const string MysqlCmd = @"select table_name from information_schema.tables where table_type = 'base table'";
        public const string TableNameKeyWord = @"config_";
        public const string TableQuery = @"select * from {0}";
        public const string ConfigPath = @"C:/Users/jiang/AppData/LocalLow/jlx/Phoenix/ConfigData.zip";
    }
}
