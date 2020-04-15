using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class FrameNumberDocument
    {
        public string DocumentTitle { get; set; } = "";
        public string DocumentUrl{ get; set; } = "";
        public int DocumentType { get; set; } = (int)ImageTypes.Generic;
    }

    public class FrameNumberDocumentRequest
    {
        public string UID { get; set; } = "";
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";
        public bool DeleteDocument { get; set; } = false;
        public FrameNumberDocument FrameNumberDocument { get; set; } = null;

    }

    public class FrameNumberDocumentResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public FrameNumberDocument FrameNumberDocument { get; set; } = null;
    }
}
