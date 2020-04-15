using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class FrameNumber
    {
        public string UID { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Model { get; set; } = "";
        public string Frame { get; set; } = "";

        public DateTime ProductionDate { get; set; } = DateTime.MinValue;
        public string ManufacturerSKU { get; set; } = "";
        public DateTime LastUpdateTime { get; set; } = DateTime.MinValue;

        public string Owner { get; set; } = "";

    }

    public class FrameNumberResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public FrameNumber FrameNumber { get; set; } = null;
    }

}
