using Microsoft.AspNetCore.Http;
using MusicCollection.Application.Common.Interfaces;

namespace MusicCollection.Application.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SignIn()
    {
        _httpContextAccessor.HttpContext!.Request.Headers.SetCookie = "auth=usr:tony";
    }

    public void Get()
    {
        string? cookie = _httpContextAccessor.HttpContext!.Request.Headers.Cookie.FirstOrDefault(x => x!.StartsWith("auth="));
    }
}
