namespace Riksbyggen.Dtos
{
    public class ApartmentCreateDto
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public int NumberOfRooms { get; set; }
        public string Status { get; set; } = null!;
        public int CompanyId { get; set; }
        public DateTime LeaseExpiryDate { get; set; }

    }
}