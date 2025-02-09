using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;


namespace Expenses
{
    internal static class DataBaseHelper
    {
        private const string _appName = "Expenses";
        private const string _dbName = "DB.db";
        private static string _dbFilePath;
        private static string _connectionString;
        public static string DBFilePath { get { return _dbFilePath; } }
        public static string ConnectionString { get { return _connectionString; } }

        /// <summary>
        /// 初期設定(テーブルフォルダの作成とメンバ変数への代入)
        /// </summary>
        public static void Init()
        {
            //OSごとのデータフォルダ
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //DBフォルダパス
            var dbFolderPath = Path.Combine(appDataPath, _appName);
            //DBファイルパス
            _dbFilePath = Path.Combine(dbFolderPath, _dbName);
            _connectionString = @"Data Source=" + _dbFilePath + ";";

            if (!Directory.Exists(dbFolderPath))
            {
                Directory.CreateDirectory(dbFolderPath);
            }
        }
    }
}
