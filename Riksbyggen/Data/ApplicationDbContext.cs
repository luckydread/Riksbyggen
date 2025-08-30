using Microsoft.EntityFrameworkCore;
using Riksbyggen.Models;

namespace Riksbyggen.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Apartment> Apartments { get; set; }

    }
}