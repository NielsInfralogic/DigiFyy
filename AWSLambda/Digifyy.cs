using System;
using System.Collections.Generic;
using System.Text;

namespace AWSLambda
{
    public class FrameNumbers
    {
        public string UID { get; set; } = "";
        public string Manufacturer { get; set; } = "";

        public string Model { get; set; } = "";

        public string Frame { get; set; } = "";

        public DateTime LastUpdateTime { get; set; } = DateTime.MinValue;

        public string LastUpdateClientID { get; set; } = "";

        public string Owner { get; set; } = "";

    }

    public class Owner
    {
        public int OwnerID { get; set; } = 0;
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string Password { get; set; } = "";
        public string Token { get; set; } = "";
    }
}