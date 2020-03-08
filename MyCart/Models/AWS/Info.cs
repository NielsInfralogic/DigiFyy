using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class UIDInfoRequest
    {
        public string UID { get; set; } = "";
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";

    }

    public class UIDInfo
    {
        public string UID { get; set; } = "";
        public Owner Owner { get; set; } = null;
        public FrameNumber FrameNumber { get; set; } = null;
        public FrameNumberStatus FrameNumberStatus { get; set; } =  null;
        public List<FrameNumberExtra> FrameNumberExtras { get; set; } =  null;
        public List<FrameNumberDocument> FrameNumberDocuments { get; set; } = null;
    }

    public class UIDInfoResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public  UIDInfo UIDInfo { get; set;} = null;
    }
}
