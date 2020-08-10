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
    /// <summary>Account manager</summary>
    [Authorize]
    [RequireHttps]
    [ApiController]
    [VersionRoute("accounts")]
    [NullModelValidation]
    [ValidateModel]
    [WebApiVersion(ApiVersions.V1)]
    public class AccountManagerController : ControllerBase
    {
        private readonly IAccountManagerService _accountManagerService;
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="AccountManagerController" /> class.</summary>
        /// <param name="accountManagerService">The account manager service.</param>
        /// <param name="mapper">The mapper.</param>
        public AccountManagerController(IAccountManagerService accountManagerService, 
                                        IMapper mapper)
        {
            _accountManagerService = accountManagerService;
            _mapper = mapper;
        }

        /// <summary>Registers user account</summary>
        /// <param name="registerAccountRequest">The register account request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [AllowAnonymous]
        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterAccountRequest registerAccountRequest, CancellationToken cancellationToken)
        {
            var registerAccountModel = _mapper.Map<RegisterAccountModel>(registerAccountRequest);

            var account = await _accountManagerService.CreateAsync(registerAccountModel, cancellationToken);
            var authenticateResponse = _mapper.Map<AccountResponse>(account);

            return Ok(authenticateResponse);
        }

        /// <summary>Gets user account information</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [HttpGet("{account-id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateAccountRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute(Name = "account-id"),
                                                   Required] Guid id,
                                                  CancellationToken cancellationToken)
        {
            if (!VerifyUserRoles(id))
            {
                return BadRequest("can't get information about a another user account");
            }

            var account = await _accountManagerService.GetByIdAsync(id, cancellationToken);

            var response = _mapper.Map<AccountResponse>(account);

            return Ok(response);
        }

        /// <summary>Updates user account information</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="updateAccountRequest">The update account request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut("{account-id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateAccountRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromRoute(Name = "account-id"),
                                                      Required] Guid id,
                                                     [FromBody] UpdateAccountRequest updateAccountRequest,
                                                      CancellationToken cancellationToken)
        {
            if (!VerifyUserRoles(id))
            {
                return BadRequest("can't update a different user account");
            }

            var updateAccountModel = _mapper.Map<UpdateAccountModel>(updateAccountRequest);
            updateAccountModel.Id = id;

            var account = await _accountManagerService.UpdateAsync(updateAccountModel, cancellationToken);
            var response = _mapper.Map<AccountResponse>(account);

            return Ok(response);
        }

        /// <summary>Delete user account</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpDelete("{account-id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute(Name="account-id"), 
                                                 Required] Guid id,
                                                CancellationToken cancellationToken = default)
        {
            if (!VerifyUserRoles(id))
            {
                return BadRequest("can't delete a different user account");
            }

            await _accountManagerService.DeleteAsync(id, cancellationToken);
            
            return NoContent();
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
    }
}
