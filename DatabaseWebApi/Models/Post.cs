using DatabaseLibrary.Attributes;
using System;

namespace DatabaseWebApi.Models
{
    [MapAs("Bijdrage")]
    public class Post
    {
        [PrimaryKey]
        public int ID { get; set; }

        [ForgeinKey]
        public Account Account { get; set; }

        public DateTime Datum { get; set; }

        public string Soort { get; set; }
    }
}