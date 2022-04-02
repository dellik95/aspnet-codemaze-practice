using System.Threading.Tasks;
using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers.Authentication
{
	[Route("api/authentication")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;

		public AuthenticationController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}


		[HttpPost]
		[ServiceFilter(typeof(ValidationActionFilter))]
		public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
		{
			var result = await _serviceManager.AuthenticationService.RegisterUser(userForRegistration);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.TryAddModelError(error.Code, error.Description);
				}

				return BadRequest(ModelState);
			}

			return StatusCode(201);
		}


		[HttpPost("login")]
		public async Task<IActionResult> Authenticate(UserForAuthenticationDto userForAuthentication)
		{
			var result = await _serviceManager.AuthenticationService.ValidateUser(userForAuthentication);
			if (!result) return Unauthorized();

			var tokenDto = await _serviceManager.AuthenticationService.CreateToken(true);
			return Ok(tokenDto);
		}
	}
}