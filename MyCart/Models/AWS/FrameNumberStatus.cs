﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{

    public enum FrameNumberStatusValue { Unknown= -1, Unregistered=0, Registered=1, Stolen=2, Found=3 };

    public class FrameNumberStatus
    {
        public int Status { get; set; } = (int)FrameNumberStatusValue.Unknown;
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;
        public DateTime LastUpdateTime { get; set; } = DateTime.MinValue;
        public string LastUpdateClientID { get; set; } = "";
    }

    public class FrameNumberStatusRequest
    {
        public string UID { get; set; } = "";
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";
        public FrameNumberStatus FrameNumberStatus { get; set; }

    }

    public class FrameNumberStatusResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public FrameNumberStatus FrameNumberStatus { get; set; } = null;
    }

}
