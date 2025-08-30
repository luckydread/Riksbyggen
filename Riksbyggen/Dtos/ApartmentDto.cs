using Riksbyggen.Models;

namespace Riksbyggen.Dtos
{
    public class ApartmentDto
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public int NumberOfRooms { get; set; }
        public DateTime LeaseExpiryDate { get; set; }
        public string Status { get; set; } = null!;
        public int CompanyId { get; set; }
    }
}
