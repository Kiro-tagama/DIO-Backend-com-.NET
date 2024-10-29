namespace MinimalApi.Domain.ModelViews;

public class AdmLoggedIn{
    public string Email { get;set; } = default!;
    public string Profile { get;set; } = default!;
    public string Token { get;set; } = default!;
}