using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookManagement.Domain.Entities;
using System.Diagnostics;
using BookManagement.Contracts.Requests;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BookManagement.Api.Controllers
{
    /// <summary>
    /// API controller for managing author identity, including registration, login, token refresh, and logout.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class IdentityController(
        UserManager<Author> userManager,
        IUserStore<Author> userStore,
        SignInManager<Author> signInManager,
        IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
        TimeProvider timeProvider)
        : ControllerBase
    {
        /// <summary>
        /// Registers a new author.
        /// </summary>
        /// <param name="registration">The registration details for the author.</param>
        /// <returns>Returns 200 OK on success, or validation errors on failure.</returns>
        /// <response code="200">Author registered successfully.</response>
        /// <response code="400">Validation failed.</response>
        [HttpPost]
        [Route("/RegisterAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok, ValidationProblem>> RegisterAuthor(
            [FromBody] RegisterAuthorRequest registration)
        {
            #region Setup
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"{nameof(RegisterAuthor)} requires a user store with email support.");
            }

            var emailStore = (IUserEmailStore<Author>)userStore;
            var username = registration.Username;
            var email = registration.Email;
            var password = registration.Password;
            var firstname = registration.FirstName;
            var lastname = registration.LastName;
            #endregion

            #region Author Creation
            var user = new Author();
            await userStore.SetUserNameAsync(user, username, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);

            // Custom properties
            user.FirstName = firstname;
            user.LastName = lastname;

            var result = await userManager.CreateAsync(user, password);
            #endregion

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        }

        /// <summary>
        /// Logs in an author using their username and password.
        /// </summary>
        /// <param name="login">The login request containing username and password.</param>
        /// <returns>Returns an access token or a 401 error on failure.</returns>
        /// <response code="200">Login successful, access token returned.</response>
        /// <response code="401">Unauthorized, invalid credentials.</response>
        [HttpPost]
        [Route("/LoginAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> LoginAuthor(
            [FromBody] LoginAuthorRequest login)
        {
            signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

            var result = await signInManager.PasswordSignInAsync(login.Username, login.Password, false, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
            }

            // SignInManager produces the needed response (e.g., cookie or bearer token).
            return TypedResults.Empty;
        }

        /// <summary>
        /// Refreshes an access token using a valid refresh token.
        /// </summary>
        /// <param name="refreshRequest">The request containing the refresh token.</param>
        /// <returns>Returns a new access token or a 401/403 error on failure.</returns>
        /// <response code="200">Access token refreshed successfully.</response>
        /// <response code="401">Unauthorized, refresh token is invalid or expired.</response>
        /// <response code="403">Forbidden, token could not be refreshed.</response>
        [HttpPost]
        [Route("/refreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>>
            RefreshToken([FromBody] RefreshRequest refreshRequest)
        {
            var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
            var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

            // Reject the refresh attempt with a 401 if the token is expired or security stamp validation fails.
            if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                timeProvider.GetUtcNow() >= expiresUtc ||
                await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } author)
            {
                return TypedResults.Challenge();
            }

            var newPrincipal = await signInManager.CreateUserPrincipalAsync(author);
            return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
        }

        /// <summary>
        /// Creates a validation problem response from an IdentityResult failure.
        /// </summary>
        /// <param name="result">The failed IdentityResult.</param>
        /// <returns>ValidationProblem result.</returns>
        private static ValidationProblem CreateValidationProblem(IdentityResult result)
        {
            Debug.Assert(!result.Succeeded);
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    var newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                    errorDictionary[error.Code] = newDescriptions;
                }
                else
                {
                    errorDictionary[error.Code] = new[] { error.Description };
                }
            }

            return TypedResults.ValidationProblem(errorDictionary);
        }
    }
}
