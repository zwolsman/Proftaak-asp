using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;

namespace DatabaseLibrary.Extensions
{
    public static class TypeExtensions
    {
        public static string RealName(this Type t)
        {
            return t.GetCustomAttribute<MapAs>()?.Mapping ?? t.Name;
        }
    }
}
