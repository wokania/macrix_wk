using macrix_wk_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace macrix_wk_backend.Data;

public class ApplicationDbContext : DbContext
{

    public virtual DbSet<PersonModel> Persons { get; set; }
    public virtual DbSet<AddressModel> Addresses { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}