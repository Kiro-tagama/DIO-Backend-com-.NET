using MinimalApi.Infrastructure.Db;
using MinimalApi.Domain.ModelViews;

namespace MinimalApi.Route;
public class HomeRoute{
  public static void MapHomeRoute(IEndpointRouteBuilder route){
    route.MapGet("/", async (DbConnect dbConnect) => {
      try{
        var canConnect = await dbConnect.Database.CanConnectAsync();
        return canConnect 
          ? Results.Ok(new { 
            message = "Servidor on", 
            detail = "Conexão com MySQL bem-sucedida.", 
            doc = new Home()
          }) 
          : Results.Json(new { message = "Falha ao conectar ao MySQL." }, statusCode: 500);
      }
      catch (Exception ex){
        return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
      }
    })
    .AllowAnonymous()
    .WithTags("*Home");
  }
}