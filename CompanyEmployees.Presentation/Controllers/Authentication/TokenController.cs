using System.Threading.Tasks;
using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers.Authentication
{
	[Route("api/token")]
	[ApiController]
	public class TokenController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;

		public TokenController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}


		[HttpPost("refresh")]
		[ServiceFilter(typeof(ValidationActionFilter))]
		public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
		{
			var tokens = await _serviceManager.AuthenticationService.RefreshToken(tokenDto);
			return Ok(tokens);
		}
	}
}