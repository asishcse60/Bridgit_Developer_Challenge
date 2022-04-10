using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScreechrDemo.Security.@interface;

namespace ScreechrDemo.Security.Auth
{
    public class BasicAuthenticationHandler: AuthenticationHandler<BasicAuthSchemeOptions>
    {
        const string AuthHeaderKey = "Authorization";
        private readonly IScreechUserAuthenticationManager _screechUserAuthentication;
        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IScreechUserAuthenticationManager screechUserAuthentication) : base(options, logger, encoder, clock)
        {
            _screechUserAuthentication = screechUserAuthentication;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
           
            if (!Request.Headers.ContainsKey(AuthHeaderKey))
            {
                return AuthenticateResult.NoResult();
            }
            string userToken = Request.Headers[AuthHeaderKey];
            var result = await _screechUserAuthentication.ValidateToken(userToken?.Replace("Bearer ", string.Empty));
            if (result.IsAuthenticated)
            {
                var authVerifiedTicket = GetAuthVerifiedTicket(result);
                return AuthenticateResult.Success(authVerifiedTicket);
            }

            return AuthenticateResult.Fail("Unauthorized");
        }

        #region auth ticket

        private AuthenticationTicket GetAuthVerifiedTicket(AccessResult user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{user.UserName}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationTicket(principal, Scheme.Name);

        }

        #endregion

    }
}
