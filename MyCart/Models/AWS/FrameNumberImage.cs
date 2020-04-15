using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public enum ImageTypes { Generic = 0, ProfileImage = 1, Invoice = 2 };

    public class FrameNumberImage
    {
        public string ImageTitle { get; set; } = "";
        public string ImageUrl { get; set; } = "";

        public int ImageType { get; set; } = (int)ImageTypes.Generic;
    }

    public class FrameNumberImageRequest
    {
        public string UID { get; set; } = "";
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";

        public bool DeleteImage { get; set; } = false;
        public FrameNumberImage FrameNumberImage { get; set; } = null;

    }

    public class FrameNumberImageResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public FrameNumberImage FrameNumberImage { get; set; } = null;
    }
}
