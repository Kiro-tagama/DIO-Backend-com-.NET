using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Administrator;

namespace MinimalApi.Infrastructure.Db;

public class DbConnect : DbContext
{
  private readonly IConfiguration _configurationApp;

  public DbConnect(IConfiguration configurationApp){
     _configurationApp = configurationApp;
  }

  public DbSet<Administrator> Administradores { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var stringConexao = _configurationApp.GetConnectionString("mysqlConnection")?.ToString();
    Console.WriteLine("code conection", stringConexao);

    if(!optionsBuilder.IsConfigured)
    {
      if(!string.IsNullOrEmpty(stringConexao))
      {
        optionsBuilder.UseMySql(
          stringConexao,
          ServerVersion.AutoDetect(stringConexao)
        );
      }
    }
  }
}