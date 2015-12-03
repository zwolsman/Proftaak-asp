using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;
using DatabaseLibrary.Extensions;
using Oracle.ManagedDataAccess.Client;

namespace DatabaseLibrary
{
    public static class DatabaseManager
    {
        private const string CONNECTION_STRING = "Data Source={0};User Id={1};Password={2};";
        private static OracleConnection oracleConnection;

        public static bool Initialize(string user, string password, string database)
        {
            oracleConnection = new OracleConnection(string.Format(CONNECTION_STRING, database, user, password));
            return Open();
        }

        private static bool Open()
        {
            try
            {
                oracleConnection.Open();
                return true;
            }
            catch (OracleException)
            {
                Debug.WriteLine("Could not open connection to database");
                return false;
            }
        }

        public static IEnumerable<T> GetItems<T>()
        {
            string sql = $"SELECT * FROM {typeof(T).RealName()}";
            Debug.WriteLine("SQL: " + sql);

            lock (oracleConnection)
            {
                using (OracleCommand cmd = new OracleCommand(sql, oracleConnection))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        foreach (DataRow row in table.Rows)
                        {
                            T instance = ToInstance<T>(row);
                            yield return instance;
                        }
                    }
                }
            }
        }

        public static IEnumerable<T> GetItems<T>(SearchCriteria searchCriteria)
        {
            string sql = $"SELECT * FROM {typeof (T).RealName()} WHERE";

            if (searchCriteria.Count() == 1)
            {
                sql += $" {searchCriteria[0].Key} = {Helper.GetValue(searchCriteria[0].Value)}";
            }
            else
            {
                for (int i = 0; i < searchCriteria.Count(); i++)
                {

                    sql += $" \"{searchCriteria[i].Key}\" = {Helper.GetValue(searchCriteria[i].Value)}";
                    if (i + 1 < searchCriteria.Count())
                    {
                        sql += " AND";
                    }
                }
            }
            Debug.WriteLine("SQL: " + sql);

            lock (oracleConnection)
            {
                using (OracleCommand cmd = new OracleCommand(sql, oracleConnection))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        foreach (DataRow row in table.Rows)
                        {
                            T instance = ToInstance<T>(row);
                            yield return instance;
                        }
                    }
                }
            }
        }

        public static T GetItem<T>()
        {
            return GetItems<T>().FirstOrDefault();
        }

        public static T GetItem<T>(SearchCriteria searchCriteria)
        {
            return GetItems<T>(searchCriteria).FirstOrDefault();
        }

        public static T InsertItem<T>(T item)
        {
            string sql = "INSERT INTO {0} ({1}) VALUES ({2}) RETURNING \"{3}\" INTO :returnPrimary";
            string clm = "";
            string values = "";
            string tableName = item.GetType().RealName();
            foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
            {
                if (propertyInfo == null)
                    continue;
                if (!propertyInfo.CanRead)
                    continue;

                if (propertyInfo.RealName() == "ID")
                    clm += propertyInfo.RealName() + ", ";
                else
                    clm += "\"" + propertyInfo.RealName().ToLower() + "\", ";

                values += Helper.GetValue(propertyInfo.GetValue(item)) + ", ";
            }

            
            clm = clm.Trim(',', ' ');
            values = values.Trim(',', ' ');
            sql = string.Format(sql, tableName, clm, values, item.PrimaryKey());
            try
            {
                Debug.WriteLine("SQL: " + sql);
                using (OracleCommand cmd = new OracleCommand(sql, oracleConnection))
                {
                    cmd.Parameters.Add(new OracleParameter("returnPrimary", OracleDbType.Decimal,
                        ParameterDirection.ReturnValue));

                    if (cmd.ExecuteNonQuery() != -1)
                    {
                        int ID = int.Parse(cmd.Parameters["returnPrimary"].Value.ToString());

                        item.GetType().GetProperty(item.PrimaryKey())?.SetValue(item, ID);
                        return item;
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Could not execute SQL!");
                Debug.WriteLine("");
                return default(T);
            }
            return default(T);
        }

        public static bool DeleteItem<T>(T item)
        {
            try
            {
                string primaryKey = item.PrimaryKey();
                object value = item.GetType().GetProperty(primaryKey)?.GetValue(item);
                if (value == null)
                {
                    Debug.WriteLine($"Can't delete item {item.GetType().Name} - Primary key is NULL");
                    return false;
                }
                if (primaryKey != "ID")
                {
                    primaryKey = "\"" + primaryKey.ToLower() + "\"";
                }

                string sql = $"DELETE FROM {item.GetType().RealName()} WHERE {primaryKey} = {Helper.GetValue(value)}";
                Debug.WriteLine("SQL: " + sql);
                using (OracleCommand cmd = new OracleCommand(sql, oracleConnection))
                {
                    return cmd.ExecuteNonQuery() != -1;
                }
            }
            catch
            {
                return false;
            }
        }

        public static T ToInstance<T>(DataRow row)
        {
            T instance = Activator.CreateInstance<T>();
            foreach (var propertyInfo in typeof (T).GetProperties())
            {
                if (propertyInfo == null)
                    continue;
                if (!propertyInfo.CanWrite)
                    continue;
                string name = propertyInfo.RealName();

                if (propertyInfo.CustomAttributes.Any(attribute => attribute.AttributeType == typeof (ForgeinKey)))
                {

                    var value = row.Field<object>(name.ToLower() + "_id");
                    var primaryKey = "id";

                    if (propertyInfo.PropertyType == typeof (T))
                    {
                        primaryKey = "\"" + instance.PrimaryKey() + "\"";
                    }
                    if (value == null)
                        continue;

                    propertyInfo.SetValue(instance,
                        typeof (DatabaseManager).GetMethod("GetItem", new [] {typeof(SearchCriteria)})
                            .MakeGenericMethod(propertyInfo.PropertyType)
                            .Invoke(null, new object[]
                            {
                                new SearchCriteria
                                {
                                    {
                                        primaryKey, value
                                    }
                                }
                            }));
                    continue;
                }

                if (!row.Table.Columns.Contains(name.ToLower()))
                    continue;

                if (propertyInfo.PropertyType == typeof (string))
                {
                    propertyInfo.SetValue(instance, row.Field<object>(name.ToLower())?.ToString() ?? string.Empty);
                    continue;
                }
                if (propertyInfo.PropertyType == typeof (int) || propertyInfo.PropertyType == typeof (int?))
                {
                    if (!string.IsNullOrEmpty(row.Field<object>(name.ToLower()).ToString()))
                        propertyInfo.SetValue(instance, int.Parse(row.Field<object>(name.ToLower()).ToString()));
                    continue;
                }
                if (propertyInfo.PropertyType == typeof (DateTime))
                {
                    propertyInfo.SetValue(instance, DateTime.Parse(row.Field<object>(name.ToLower()).ToString()));
                    continue;
                }


                Debug.WriteLine($"Did not resolve property {propertyInfo.Name} - {propertyInfo.PropertyType.Name}");
            }
            return instance;
        }
    }
}