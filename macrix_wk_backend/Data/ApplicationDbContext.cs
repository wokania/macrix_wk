using macrix_wk_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace macrix_wk_backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public required DbSet<PersonModel> Persons { get; set; }
    public required DbSet<AddressModel> Addresses { get; set; }
}