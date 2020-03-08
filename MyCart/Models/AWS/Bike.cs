using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class Bike
    {
        public FrameNumber FrameNumber { get; set; } = new FrameNumber();
        public FrameNumberStatus FrameNumberStatus { get; set; } = new FrameNumberStatus();
        public List<FrameNumberExtra> FrameNumberExtras { get; set; } = new List<FrameNumberExtra>();
        public List<FrameNumberDocument> FrameNumberDocuments { get; set; } = new List<FrameNumberDocument>();                
    }
}
