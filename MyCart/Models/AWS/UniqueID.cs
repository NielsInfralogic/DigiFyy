using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class UniqueID
    {
        public string UID { get; set; } = "";
        public string Username { get; set; } = "";
        public string Name { get; set; } = "";
        public string Password { get; set; } = "";
        public Owner Owner { get; set; } = new Owner();
        public DateTime EventTime { get; set; } = DateTime.MinValue;
    }
}
