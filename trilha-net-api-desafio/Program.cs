using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrganizadorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/api", async (OrganizadorContext dbContext) =>
{
    string dbStatus;

    try
    {
        await dbContext.Database.ExecuteSqlRawAsync("SELECT 1");
        dbStatus = "Connected";
    }
    catch (Exception ex)
    {
        dbStatus = $"Disconnected: {ex.Message}";
    }

    var status = new
    {
        ApiStatus = "Running",
        DbStatus = dbStatus
    };

    return Results.Ok(status);
})
.WithTags("*Status");

app.MapControllers();

app.Run();
