using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using MusicCollection.Application.Common.Interfaces;

namespace MusicCollection.Application.Services;

public class AuthDIYService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataProtectionProvider _dataProtectionProvider;
    
    public AuthDIYService(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _dataProtectionProvider = dataProtectionProvider;
    }

    public void SignUp()
    {
        IDataProtector dataProtector = _dataProtectionProvider.CreateProtector("auth-cookie");
        string cookie = dataProtector.Protect("name:tony");
        _httpContextAccessor.HttpContext!.Request.Headers.SetCookie = $"auth=${cookie}";
    }

    public void Authenticate()
    {
        var cookie = _httpContextAccessor.HttpContext!.Request.Headers.Cookie.FirstOrDefault(cookie => cookie is not null && cookie.StartsWith("auth="));

        if (cookie is null)
        {
            return;
        }

        IDataProtector dataProtector = _dataProtectionProvider.CreateProtector("auth-cookie");
        
        var protectedPayload = cookie.Split('=').Last();

        var payload = dataProtector.Unprotect(protectedPayload); 

        var keyValue = payload.Split(':');

        var key = keyValue[0];

        var value = keyValue[1];

        IList<Claim> claims = new List<Claim>();
        
        claims.Add(new Claim(key, value));

        ClaimsIdentity identity = new ClaimsIdentity(claims, "AuthScheme");

        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        _httpContextAccessor.HttpContext.User = principal;
    }

    public bool IsAuthenticated()
    {
        var cookie = _httpContextAccessor.HttpContext!.Request.Headers.Cookie.FirstOrDefault(cookie => cookie is not null && cookie.StartsWith("auth="));

        return string.IsNullOrEmpty(cookie);
    }

    public bool IsAuthorize()
    {
        Authenticate();

        if (_httpContextAccessor.HttpContext!.User.Identities.Any(identity =>
                identity.AuthenticationType == "AuthScheme"))
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return false;
        }

        if (_httpContextAccessor.HttpContext.User.HasClaim("name", "tony") == false)
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return false;
        }

        // Authorized
        return true;
    }
}

public class DebugAttribute : Attribute
{
    public bool DebugMode { get; set; }
}

[Debug(DebugMode = true)]
public class A
{
    [Debug(DebugMode = false)]
    public void B()
    {
        var classAttributes = typeof(A).GetCustomAttributes(false);

        var method = typeof(A).GetMethod("B");
        
        var methodAttributes = method!.GetCustomAttributes(false);
    }
}