using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;

namespace DatabaseLibraryTests
{
    public class Bijdrage
    {
        [PrimaryKey]
        public int ID { get; set; }

        [ForgeinKey]
        public Account Account { get; set; }

        public DateTime Datum { get; set; }

        public string Soort { get; set; }
    }
}
