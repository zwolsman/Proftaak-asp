using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;

namespace DatabaseLibrary.Extensions
{
    public static class ObjectExtensions
    {
        public static string PrimaryKey(this object instance)
        {
            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttribute<PrimaryKey>() != null)
                {

                    if (propertyInfo.GetCustomAttribute<ForgeinKey>() == null)
                    {
                        if (propertyInfo.PropertyType == typeof (int) || propertyInfo.PropertyType == typeof (int?))
                            return propertyInfo.RealName();
                    }
                    else
                    {
                        object newInstance = Activator.CreateInstance(propertyInfo.PropertyType);
                        return newInstance.GetType().RealName().ToLower() + "_" + newInstance.PrimaryKey().ToLower();
                    }
                    
                }
            }
            return string.Empty;
        }

        public static int? PrimaryKeyValue(this object instance)
        {
            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttribute<PrimaryKey>() != null)
                {

                    if (propertyInfo.GetCustomAttribute<ForgeinKey>() == null)
                    {
                        if (propertyInfo.PropertyType == typeof (int) || propertyInfo.PropertyType == typeof (int?))
                        {
                            string val = propertyInfo.GetValue(instance).ToString();
                            if (string.IsNullOrEmpty(val))
                                return null;
                            return int.Parse(val);
                        }
                    }
                    else
                    {
                        object newInstance = propertyInfo.GetValue(instance);
                        return newInstance.PrimaryKeyValue();
                    }

                }
            }
            return null;
        }
    }
}
