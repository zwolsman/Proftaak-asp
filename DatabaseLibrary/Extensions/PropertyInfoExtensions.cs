using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;

namespace DatabaseLibrary.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string RealName(this PropertyInfo info)
        {
            return info.GetCustomAttribute<MapAs>()?.Mapping ?? info.Name;
        }
    }
}
