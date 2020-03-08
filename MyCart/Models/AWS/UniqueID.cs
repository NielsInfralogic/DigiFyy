using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class UniqueID
    {
        public string UID { get; set; } = "";       
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Model { get; set; } = "";
        public string Frame { get; set; } = "";
        public DateTime ProductionDate { get; set; } = DateTime.MinValue;
        public Owner Owner { get; set; } = new Owner();
        public FrameNumberStatus Status { get; set; } = new FrameNumberStatus();
        public List<FrameNumberExtra> Extras { get; set; } = new List<FrameNumberExtra>();
        public DateTime EventTime { get; set; } = DateTime.MinValue;
    }
}
