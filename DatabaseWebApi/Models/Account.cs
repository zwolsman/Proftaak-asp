using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;
using Newtonsoft.Json;

namespace DatabaseWebApi.Models
{
    public class Account
    {
        [PrimaryKey]
        public int? ID { get; set; }

        public string Gebruikersnaam { get; set; }

        [JsonIgnore]
        public string Email { get; set; }

        [MapAs("activatiehash")]
        [JsonIgnore]
        public string ActivateHash { get; set; }

        [MapAs("geactiveerd")]
        [JsonIgnore]
        public int Activated { get; set; }

    }
}
