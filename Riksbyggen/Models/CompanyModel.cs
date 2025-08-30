using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Riksbyggen.Models
{
    public class Company
    {
        public int Id { get; set; } = 0;
        [Required]
        public required string Name { get; set; }
        [Required]
        public required Address Address { get; set; }
        public ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    }

    [Owned]
    public class Address
    {
        [Required]
        public required string Street { get; set; }
        [Required]
        public required string City { get; set; }
        [Required]
        public required string ZipCode { get; set; }
    }

    public class Apartment
    {
        public required int Id { get; set; } = 0;
        [Required]
        public required Address Adress { get; set; }
        public required int NumberOfRooms { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public required string Status { get; set; } = "Available";

        [Required]
        public required DateTime LeaseExpiryDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

    }

}


