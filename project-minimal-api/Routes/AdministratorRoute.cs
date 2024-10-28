using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Enuns;
using MinimalApi.Domain.Interface;
using MinimalApi.Domain.ModelViews;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;

namespace MinimalApi.Route;
public class AdministratorRoute{
  public static void MapAdminRoute(IEndpointRouteBuilder route, string keyJwt){

    string GenerationJwtToken(Administrator administrator){
      // if(string.IsNullOrEmpty(keyJwt)) return string.Empty;

      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyJwt));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new List<Claim>()
      {
        new Claim("Email", administrator.Email),
        new Claim("Profile", administrator.Profile),
        new Claim(ClaimTypes.Role, administrator.Profile),
      };
      
      var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    route.MapPost("/administrator/login", 
    ( 
      [FromBody] 
      LoginDTO loginDTO,
      IAdministratorServices administratorServices
    ) =>{
      try{
        var adm = administratorServices.Login(loginDTO);

        if(adm == null) return Results.Unauthorized();

        string token = GenerationJwtToken(adm);

        return Results.Ok(new AdmLoggedIn{
          Email = adm.Email,
          Profile = adm.Profile,
          Token = token,
        });
      }
      catch (System.Exception ex)
      {
        return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
      }
    })
    .AllowAnonymous()
    .WithTags("Administrator");

    route.MapPost("/administrator", 
    ( 
      [FromBody] 
      AdministratorDTO administratorDTO,
      IAdministratorServices administratorServices
    ) => {
      try{
        var validation = new ValidationErr{
          Messages = new List<string>()
        };

        if(string.IsNullOrEmpty(administratorDTO.Email)) validation.Messages.Add("O email não pode ser vazio");
        if(string.IsNullOrEmpty(administratorDTO.Password)) validation.Messages.Add("A senha não pode ser vazia");
        if(administratorDTO.Profile == null) validation.Messages.Add("O profile não pode ser vazio");

        if(validation.Messages.Count > 0) return Results.BadRequest(validation);

        var adm = new Administrator{
          Email = administratorDTO.Email,
          Password = administratorDTO.Password,
          Profile = administratorDTO.Profile.ToString() ?? Profile.Editor.ToString()
        };

        administratorServices.AddAdministrator(adm);

        return Results.Created($"/admin/{adm.Id}", new AdministradorModelView {
            Id = adm.Id,
            Email = adm.Email,
            Profile = adm.Profile
          });
      }
      catch (System.Exception ex)
      {
        return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
      }
    })
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("Administrator");

    route.MapGet("/administrator", 
    (
      [FromQuery]
      int? page,
      IAdministratorServices administratorServices
    ) => {
      try{
        var adms = new List<AdministradorModelView>();
        var listAdms = administratorServices.GetAllAdministrators(page);

        foreach (var adm in listAdms){
          adms.Add(new AdministradorModelView{
            Id = adm.Id,
            Email = adm.Email,
            Profile = adm.Profile
          });
        }

        return adms.Count() > 0 
          ? Results.Ok(adms) 
          : Results.NotFound();
      }
      catch (System.Exception ex)
      {
        return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
      }
    })
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("Administrator");

    route.MapGet("/administrator/{id}", 
    (
      [FromQuery]
      int id,
      IAdministratorServices administratorServices
    ) => {
      try{
        var adm = administratorServices.GetAdministratorById(id);

        // formated data
        return adm != null 
          ? Results.Ok(new AdministradorModelView {
            Id = adm.Id,
            Email = adm.Email,
            Profile = adm.Profile
          }) 
          : Results.NotFound();
      }
      catch (System.Exception ex)
      {
        return Results.Json(new { message = "Erro", detail = ex.Message }, statusCode: 500);
      }
    })
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("Administrator");
  }
}