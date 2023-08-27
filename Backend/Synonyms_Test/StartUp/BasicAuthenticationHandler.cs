using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Synonyms_Test.StartUp;

public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header is missing"));
        }

        try
        {
            //find the authorization header
            var authenticationHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

            //check if it has parameters
            if (authenticationHeader.Parameter is null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Unable to retrieve authentication parameters!"));
            }

            //Get the cridentials
            var credentialBytes = Convert.FromBase64String(authenticationHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            var username = credentials[0];
            var password = credentials[1];
            

            //Check cridentials
            if (IsUserValid(username, password))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header"));
        }
    }

    private static bool IsUserValid(string username, string password)
    {
        //This is only a basic authorization and user validation, ideally it would be more inteligent.
        return username == "user" && password == "password";
    }
}