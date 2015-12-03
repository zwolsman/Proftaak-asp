using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;

namespace DatabaseLibraryTests.Classes
{
    public class Bericht
    {
        [PrimaryKey]
        [ForgeinKey]
        public Bijdrage Bijdrage { get; set; }

        public string Titel { get; set; }

        public string Inhoud { get; set; }

        public List<Bericht> Comments = new List<Bericht>(); 
    }
}
