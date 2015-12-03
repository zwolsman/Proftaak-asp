using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;

namespace DatabaseLibraryTests.Classes
{
    class Categorie
    {

        [PrimaryKey]
        [ForgeinKey]
        public Bijdrage Bijdrage { get; set; }

        [ForgeinKey]
        public Categorie categorie { get; set; }

        public string Naam { get; set; }
    }
}
