public struct ConfigConst
{
    public const string Server = @"localhost";
    public const string Database = @"phoenixconfig";
    public const string UserID = @"root";
    public const string Password = @"jlxmysql";
    public const string MysqlCmd = @"select table_name from information_schema.tables where table_type = 'base table'";
    public const string TableNameKeyWord = @"config_";
    public const string TableQuery = @"select * from {0}";
    public const string ConfigName = @"ConfigData.zip";
    public const string ZipEntryName = @"010101010111000";
}
