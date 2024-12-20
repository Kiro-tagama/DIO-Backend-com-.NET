namespace MinimalApi.Domain.ModelViews;

public record AdministradorModelView
{
    public int Id { get;set; } = default!;
    public string Email { get;set; } = default!;
    public string Profile { get;set; } = default!;
}