using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class MapAs : Attribute
    {
        public string Mapping { get; set; }
        public MapAs(string map)
        {
            Mapping = map;
        }
    }
}
