using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.NFC
{
    public class TagRead
    {

        public string SerialNumber { get; set; } = "";
        public string Model { get; set; } = "";
        public string Data { get; set; } = "";

        public string ErrorMessage { get; set; } = "";
    }
}
