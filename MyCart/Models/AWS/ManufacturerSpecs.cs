using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class ManufacturerSpec
    {
        public string SpecPartType { get; set; } = "";
        public string SpecPartName { get; set; } = "";
        public string SpecPartDetails { get; set; } = "";
    }

    public class ManufacturerSpecResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public List<ManufacturerSpec> Specs { get; set; } = null;
    }
}
