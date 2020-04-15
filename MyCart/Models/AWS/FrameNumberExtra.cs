using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class FrameNumberExtra
    {
        public string ExtraType { get; set; } = "";
        public string ExtraBrand { get; set; } = "";
        public string ExtraModel { get; set; } = "";

    }

    public class FrameNumberExtraRequest
    {
        public string UID { get; set; } = "";
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";
        public bool DeleteExtra { get; set; } = false;

        public FrameNumberExtra FrameNumberExtra { get; set; } = null;

    }

    public class FrameNumberExtraResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public FrameNumberExtra FrameNumberExtra { get; set; } = null;
    }

}
