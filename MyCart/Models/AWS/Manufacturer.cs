using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class Manufacturer
    {
        public int ManufacturerID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Address1 { get; set; } = "";
        public string Address2 { get; set; } = "";
        public string City { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string CountryISO { get; set; } = "";
        public string CompanyNo { get; set; } = "";
        public string ContactPhone { get; set; } = "";
        public string ContactEmail { get; set; } = "";
	public string WWW { get; set; } = "";

    }

    public class ManufacturerResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public List<Manufacturer> Manufacturers { get; set; } = null;
    }

}
