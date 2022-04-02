using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
	//[ApiVersion("2.0", Deprecated = true)]
	[ApiController]
	[ApiExplorerSettings(GroupName = "v2")]
	[Route("api/companies")]
	public class CompaniesV2Controller : ControllerBase
	{
		private readonly IServiceManager _serviceManager;

		public CompaniesV2Controller(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		[HttpGet]
		public async Task<IActionResult> GetCompanies()
		{
			var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(false);

			var result = companies.Select(x => x.Name);
			return Ok(result);
		}
	}
}