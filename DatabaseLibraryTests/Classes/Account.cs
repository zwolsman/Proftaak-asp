using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;

namespace DatabaseLibraryTests
{
    public class Account
    {
        [PrimaryKey]
        public int? ID { get; set; }

        public string Gebruikersnaam { get; set; }
        public string Email { get; set; }

        [MapAs("activatiehash")]
        public string ActivateHash { get; set; }

        [MapAs("geactiveerd")]
        public int Activated { get; set; }

    }
}
