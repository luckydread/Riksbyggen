namespace Riksbyggen.Dtos
{
    public class CompanyWithApartmentsDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string ZipCode { get; set; }
        public required List<ApartmentDto> Apartments { get; set; } = new();
    }
}
