using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace Expenses
{
    static class StringExtention
    {
        public static string ToSideForQuery(this object obj)
        {
            if (isDate(obj))
            {
                var dateobj = (DateTime)obj;
                return string.Concat("'", dateobj.Date, "'");
            }
            else if (!isNumeric(obj))
            {
                return string.Concat("'", obj, "'");
            }
            else
            {
                return obj.ToString();
            }
        }

        private static bool isNumeric(object obj)
        {
            return obj is int || obj is float || obj is double || obj is decimal;
        }
        private static bool isDate(object obj)
        {
            return obj is DateTime;
        }


        // public static T ToRecord<T>(this SqliteDataReader reader,List<PropertyInfo>columnList) where T : Table
        // {
        //     var r = new Table() as T;
        //     for (int i = 0; i < reader.FieldCount; i++)
        //     {
        //         var columnName = reader.GetName(i);
        //         var property = columnList.Find(x => x.Name == columnName);
        //         var value = Convert.ChangeType(reader.GetValue(i), property.PropertyType);
        //         property.SetValue(r, value);
        //     }
        //     return r;
    }
}
