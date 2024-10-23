using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Infrastructure.Db;

public class DbConnect : DbContext
{
  private readonly IConfiguration _configurationApp;

  public DbConnect(IConfiguration configurationApp){
     _configurationApp = configurationApp;
  }

  public DbSet<Administrator> Administrators { get; set; } = default!;

  // seed: inputa direto no db
  protected override void OnModelCreating(ModelBuilder modelBuilder){
    modelBuilder.Entity<Administrator>().HasData(
      new Administrator { 
        Id = 1, 
        Email = "adm@test.com",
        Password = "123456",
        Profile = "admin"  // admin, user, guest
      }
    );
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var stringConnection = _configurationApp.GetConnectionString("mysqlConnection")?.ToString();

    if(!optionsBuilder.IsConfigured)
    {
      if(!string.IsNullOrEmpty(stringConnection))
      {
        optionsBuilder.UseMySql(
          stringConnection,
          ServerVersion.AutoDetect(stringConnection)
        );
      }
    }
  }
}