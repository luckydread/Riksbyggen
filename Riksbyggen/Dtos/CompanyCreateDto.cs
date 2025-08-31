namespace Riksbyggen.Dtos
{
    public class CompanyCreateDto
    {
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string ZipCode { get; set; }
    }
}
