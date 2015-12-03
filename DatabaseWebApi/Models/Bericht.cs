using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Attributes;

namespace DatabaseWebApi.Models
{
    public class Bericht
    {
        [PrimaryKey]
        [ForgeinKey]
        [MapAs("Bijdrage")]
        public Post Post { get; set; }

        public string Titel { get; set; }

        public string Inhoud { get; set; }
    }
}
