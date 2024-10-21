var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginDTO loginDTO) =>{
  var userAdmin = loginDTO.Email == "adm@test.com" && loginDTO.Password == "123456";

  return userAdmin? Results.Ok("Login admin") : Results.Unauthorized(); 
});

app.Run();