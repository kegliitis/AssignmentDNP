using System.Security.Claims;
using BlazorWASM.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWASM.Auth;

public class CustomAuthProvider : AuthenticationStateProvider
{
    private readonly IAuthService _authService;

    public CustomAuthProvider(IAuthService authService)
    {
        _authService = authService;
        _authService.OnAuthStateChanged += AuthStateChanged;
    }

    private void AuthStateChanged(ClaimsPrincipal principal)
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal principal = await _authService.GetAuthAsync();

        return new AuthenticationState(principal);
    }
}