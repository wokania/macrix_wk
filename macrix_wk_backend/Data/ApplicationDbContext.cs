using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using macrix_wk_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace macrix_wk_backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<PersonModel> Person { get; set; }
    public DbSet<AddressModel> AddresModels { get; set; }
}