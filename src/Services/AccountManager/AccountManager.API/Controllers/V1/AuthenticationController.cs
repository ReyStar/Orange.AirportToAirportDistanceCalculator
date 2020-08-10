using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.API.Attributes;
using AccountManager.API.Controllers.V1.DTO;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.API.Controllers.V1
{
    /// <summary>Authentication controller</summary>
    [Authorize]
    [RequireHttps]
    [ApiController]
    [VersionRoute("auth")]
    [NullModelValidation]
    [ValidateModel]
    [WebApiVersion(ApiVersions.V1)]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountAuthenticationService _accountAuthenticationService;
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="AccountManagerController" /> class.</summary>
        /// <param name="accountAuthenticationService">The account authentication service.</param>
        /// <param name="mapper">The mapper.</param>
        public AuthenticationController(IAccountAuthenticationService accountAuthenticationService, 
                                        IMapper mapper)
        {
            _accountAuthenticationService = accountAuthenticationService;
            _mapper = mapper;
        }

        /// <summary>Authenticate</summary>
        /// <param name="loginRequest">The account request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginByPasswordRequest loginRequest, CancellationToken cancellationToken)
        {
            var authenticateModel = await _accountAuthenticationService.LoginAsync(loginRequest.Username, loginRequest.Password, cancellationToken);
            //if (authenticateModel == null)
            //{
            //    return BadRequest("invalid user credentials user account not found");
            //}
            
            var authenticateResponse = _mapper.Map<LoginResponse>(authenticateModel);

            return Ok(authenticateResponse);
        }

        [HttpPost("{account-id}/logout")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogoutAsync([FromRoute(Name = "account-id"),
                                                      Required]
                                                     Guid id, CancellationToken cancellationToken)
        {
            if (!VerifyUserRoles(id))
            {
                return BadRequest(new {message = "Can't logout unknown user"});
            }

            await _accountAuthenticationService.LogoutAsync(id, cancellationToken);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("{account-id}/refresh-token")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RefreshTokenAsync([FromRoute(Name = "account-id"),
                                                            Required] Guid id,
                                                           [FromBody] LoginByRefreshTokenRequest model,
                                                            CancellationToken cancellationToken)
        {
            var authenticateModel = await _accountAuthenticationService.RefreshTokenAsync(id, model.RefreshToken, cancellationToken);

            var authenticateResponse = _mapper.Map<LoginResponse>(authenticateModel);

            return Ok(authenticateResponse);

            //if (authenticateModel == null)
            //    return Unauthorized(new { message = "Invalid token" });

            //return Ok(authenticateModel);
        }

        /// <summary>
        /// User can manage his account or admin can manage other accounts
        /// TODO Add Roles to access token
        /// </summary>
        private bool VerifyUserRoles(Guid userId)
        {
            if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var authUserId))
            {
                return userId == authUserId;
            }

            return false;
        }

        private Guid? GetUserId()
        {
            if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var authUserId))
            {
                return authUserId;
            }

            return null;
        }
    }
}
