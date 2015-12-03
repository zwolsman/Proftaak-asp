using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary
{
    internal class Helper
    {
        public static string GetValues(Hashtable table, string tableName = "")
        {
            string values = "";
            foreach (var val in table.Values)
            {
                if (val is int && int.Parse(val.ToString()) == -1)
                {
                    //TODO fix primary key shit werkt niet goed als je items gaat verwijderen en weer toevoegen
                    values += $"ISNULL((SELECT MAX(ID) FROM {tableName}), -1) + 1, ";
                }
                else
                {
                    values += GetValue(val) + ", ";
                }
            }
            return values.Trim(',', ' ');
        }

        public static string GetValue(object val)
        {

            if (val == null)
                return "NULL";

            if (val is string)
            {

                return $"'{val.ToString().Replace("'", "''")}'";
            }
            if (val is int || val is long)
            {
                if (int.Parse(val.ToString()) == -1)
                {
                    //TODO fix primary key shit
                    //values += $"(SELECT COUNT(*) FROM {tableName}) + 1, ";
                }
                else
                {
                    return $"{val}";
                }
            }
            if (val is DateTime)
            {
                DateTime v = (DateTime)val;
                return $"'{v.Year}-{v.Month}-{v.Day}'";
            }

            Debug.WriteLine($"NULL value for {val.GetType()}");
            return "NULL";
        }

        public static string MD5(string input) => new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input)).Aggregate("", (current, b) => current + b.ToString("x2"));
    }
}
