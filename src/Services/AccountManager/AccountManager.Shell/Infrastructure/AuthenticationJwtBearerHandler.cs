using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace AccountManager.Shell.Infrastructure
{
    public class AuthenticationJwtBearerHandler: JwtBearerHandler
    {
        private readonly ITokenRepository _tokenRepository;

        public AuthenticationJwtBearerHandler(IOptionsMonitor<JwtBearerOptions> options, 
                                              ILoggerFactory logger, 
                                              UrlEncoder encoder, 
                                              ISystemClock clock,
                                              ITokenRepository tokenRepository) 
            : base(options, logger, encoder, clock)
        {
            _tokenRepository = tokenRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var result = await base.HandleAuthenticateAsync();
            if (result.Succeeded)
            {
                string authorization = Request.Headers[HeaderNames.Authorization];

                var subStrings = authorization.Split(" ");
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(subStrings[1]);

                var name = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier); 
                if (string.IsNullOrWhiteSpace(name) || !Guid.TryParse(name, out var accountInfoId))
                {
                    return AuthenticateResult.Fail("Unauthorized. Account id not exist");
                }

                var tokens = await _tokenRepository.GetAllAccessTokensAsync(accountInfoId, CancellationToken.None);
                var savedToken = tokens.SingleOrDefault(x => x.TokenHash == token.RawSignature);
                if (savedToken == null)
                {
                    return AuthenticateResult.Fail("Unauthorized. Token id not exist");
                }
            }

            return result;
        }
    }
}
