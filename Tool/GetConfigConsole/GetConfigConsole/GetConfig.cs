using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetConfigConsole
{
    class GetConfig
    {
        public void Execute()
        {
            DataSet dataSet = new MysqlHandler().GetDataSet();
        }
    }
}
