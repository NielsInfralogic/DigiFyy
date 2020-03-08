
namespace DigiFyy.Models.AWS
{
    public enum GenderType { Female = 0, Male = 1 };

    public class Owner
    {
        
        public int OwnerID { get; set; } = 0;
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string Address1 { get; set; } = "";
        public string Address2 { get; set; } = "";
        public string City { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string CountryISO { get; set; } = "";
        public string Phone { get; set; } = "";

        public int Age { get; set; } = 0;
        public int Gender { get; set; } = (int)GenderType.Male;

    }
}
