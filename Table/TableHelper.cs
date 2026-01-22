using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Expenses
{
    static class TableHelper
    {
        public static void Init()
        {
            // if (!Path.Exists(DataBaseHelper.DBFilePath))
            // {
            //     GetTableClass().ToList().ForEach(x => x.CreateTable());
            // }
                GetTableClass().ToList().ForEach(x => x.CreateTable());

        }
        private static Table[] GetTableClass()
        {
            return new Table[] { new DMain(), new MainTag(), new TagCategory(), new DTotal(),new MVersion()};
        }
    }
}
