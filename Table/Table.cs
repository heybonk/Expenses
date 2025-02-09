using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Quic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;


namespace Expenses
{
    public class Table:ObservableObject
    {
        private string _tableName;

        public  List<PropertyInfo> _pkList = new List<PropertyInfo>();

        public  List<PropertyInfo> _columnList = new List<PropertyInfo>();

        public Table()
        {
            _tableName = this.GetType().Name;
            this.SetColumnList();
            this.SetPkList();
        }

        #region 初期設定
        private void SetPkList()
        {
            foreach(var column in _columnList)
            {
                var columnAttributes = (TableAttribute)column.GetCustomAttributes(typeof(TableAttribute), false).First();
                if (columnAttributes.ColumnType == ColumnType.PrimaryKey) _pkList.Add(column);
            }
        }

        private void SetColumnList()
        {
            Type type = this.GetType();
            _columnList = type.GetProperties().Where(x => x.GetCustomAttributes(typeof(TableAttribute), false).Any()).ToList();  
        }
        #endregion

        internal void CreateTable()
        {
            //列属性を持つプロパティの取得
            Type type = this.GetType();

            var query = new StringBuilder();
            query.AppendLine(" CREATE TABLE " + _tableName);
            query.AppendLine(" ( ");
            for (int i = 0; i < _columnList.Count; i++)
            {
                var column = _columnList[i];
                var columnAttributes = (TableAttribute)column.GetCustomAttributes(typeof(TableAttribute), false).First();
                var columnType = columnAttributes.Category.ToString();
                var nullAble = columnAttributes.Nullable ? " NULL " : " NOT NULL ";
                if (i != 0) query.Append(" , ");
                query.AppendLine(string.Join(" ", column.Name, columnType, nullAble));
            }
            if (_pkList != null && _pkList.Count > 0)
            {
                query.AppendLine(string.Join(" ", ",PRIMARY KEY", "(", string.Join(",", _pkList.Select(x=>x.Name)), ")"));
            }
            query.AppendLine(" ) ");

            using (var connection = new SqliteConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }
        internal void SetByPrimaryKey()
        {
            //クエリ文の作成
            var terms = " WHERE ";
            for (int i = 0; i < _pkList.Count; i++)
            {
                if (i > 0) terms += " AND ";
                var pkName = _pkList[i].Name;
                var pkValue = _pkList.Find(x => x.Name == pkName).GetValue(this);
                string term = pkValue.ToSideForQuery();
                terms += string.Concat(pkName, "=", term);
            }
            var query = @"SELECT * FROM " + _tableName + terms;

            //結果の取得
            using (var connection = new SqliteConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var property = _columnList.Find(x => x.Name == columnName);
                            var value = Convert.ChangeType(reader.GetValue(i), property.PropertyType);
                            property.SetValue(this, value);
                        }
                    }
                }
            }
        }
        internal void Update()
        {
            if(this.IsExistData())
            {
                this.UpdateData();
            }
            else
            {
                this.InsertData();
            }
        }
        internal void InsertData()
        {
            var valueList = new List<string>();
            foreach(var column in _columnList)
            {
                var value = column.GetValue(this);
                if (value == null)
                {
                    valueList.Add("null");
                }
                else
                {
                    valueList.Add(value.ToSideForQuery());
                }
            }
            using (var connection = new SqliteConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    var query = new StringBuilder();
                    query.AppendLine(" INSERT INTO " + _tableName);
                    query.AppendLine(" ( ");
                    query.AppendLine(String.Join(",", _columnList.Select(x => x.Name)));
                    query.AppendLine(" ) ");
                    query.AppendLine(" VALUES ");
                    query.AppendLine(" ( ");
                    query.AppendLine(String.Join(",", valueList));
                    query.AppendLine(" ) ");

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }

        internal void UpdateData()
        {
            var query = new StringBuilder();

            query.AppendLine(" UPDATE " + _tableName + " SET ");
            for(int i = 0; i < _columnList.Count;i++)
            {
                var q = string.Empty;
                if (i != 0) q += ",";
                var column = _columnList[i];
                var value = column.GetValue(this).ToSideForQuery();
                query.AppendLine(string.Join(" ",q,column.Name,"=",value));
            }

            query.AppendLine(" WHERE ");
            for (int i = 0; i < _pkList.Count; i++)
            {
                if (i > 0) query.Append(" AND ");
                var pkName = _pkList[i].Name;
                var pkValue = _pkList.Find(x => x.Name == pkName).GetValue(this);
                string term = pkValue.ToSideForQuery();
                query.AppendLine(string.Concat(pkName, "=", term));
            }

            
            using (var connection = new SqliteConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }

        internal void DeleteData()
        {
            //クエリ文の作成
            var terms = " WHERE ";
            for (int i = 0; i < _pkList.Count; i++)
            {
                if (i > 0) terms += " AND ";
                var pkName = _pkList[i].Name;
                var pkValue = _pkList.Find(x => x.Name == pkName).GetValue(this);
                string term = pkValue.ToSideForQuery();
                terms += string.Concat(pkName, "=", term);
            }
            var query = @" DELETE FROM " + _tableName + terms;

            //結果の取得
            using (var connection = new SqliteConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }

        internal bool IsExistData()
        {
            //クエリ文の作成
            var terms = " WHERE ";
            
            for (int i = 0; i < _pkList.Count; i++)
            {
                if (i > 0) terms += " AND ";
                var pkName = _pkList[i].Name;
                var pkValue = _pkList.Find(x => x.Name == pkName).GetValue(this);
                string term = pkValue.ToSideForQuery();
                terms += string.Concat(pkName, "=", term);
            }
            var query = @"SELECT COUNT(*) FROM " + _tableName + terms;

            //結果の取得
            long count = 0;
            using (var connection = new SqliteConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count = reader.GetInt64(0);
                    }
                }
            }
            return count > 0;
        }
        internal IEnumerable<Table> SelectAll()
        {
            //クエリ文の作成
            var query = @"SELECT * FROM " + this._tableName;

            //結果の取得
            using (var connection = new SqliteConnection(DataBaseHelper.ConnectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return this.ToRecord(reader);
                    }
                }
            }
        }
        private Table ToRecord(SqliteDataReader reader)
        {
            var r = this;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var property = _columnList.Find(x => x.Name == columnName);
                var value = Convert.ChangeType(reader.GetValue(i), property.PropertyType);
                property.SetValue(r, value);
            }
            return r;
        }
    }
}
