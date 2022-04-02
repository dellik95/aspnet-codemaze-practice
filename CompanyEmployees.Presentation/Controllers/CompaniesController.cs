using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
	[ApiVersion("1.0")]
	[ApiExplorerSettings(GroupName = "v1")]
	[ResponseCache(CacheProfileName = "120SecondsDuration")]
	[Authorize]
	[Route("api/companies")]
	[ApiController]
	public class CompaniesController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;

		public CompaniesController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}


		[HttpOptions]
		//[HttpCacheExpiration]
		//[HttpCacheValidation]
		public IActionResult GetCompaniesOptions()
		{
			Response.Headers.Add("Allow", "GET, POST, OPTIONS");
			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetCompanies()
		{
			var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(false);
			return Ok(companies);
		}


		[HttpGet("{id:int}", Name = "CompanyById")]
		public async Task<IActionResult> GetCompany(int id)
		{
			var company = await _serviceManager.CompanyService.GetCompanyAsync(id, false);
			return Ok(company);
		}

		[HttpPost]
		[ServiceFilter(typeof(ValidationActionFilter))]
		public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreateDto company)
		{
			var createdCompany = await _serviceManager.CompanyService.CreateCompanyAsync(company);
			return CreatedAtRoute("CompanyById", new {Id = createdCompany.Id}, createdCompany);
		}


		[HttpGet("/collection/({ids})", Name = "CompanyCollection")]
		public async Task<IActionResult> GetCompanyCollection(
			[ModelBinder(typeof(ArrayModelBinder))]
			IEnumerable<int> ids)
		{
			var companies = await _serviceManager.CompanyService.GetByIdsAsync(ids, false);
			return Ok(companies);
		}

		[HttpPost("collection")]
		[ServiceFilter(typeof(ValidationActionFilter))]
		public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreateDto> companies)
		{
			var result = await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companies);
			return CreatedAtRoute("CompanyCollection", new {ids = result.ids}, result.companies);
		}


		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteCompany(int id)
		{
			await _serviceManager.CompanyService.DeleteCompanyAsync(id, false);
			return NoContent();
		}


		[HttpPut("{id:int}")]
		[ServiceFilter(typeof(ValidationActionFilter))]
		public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyForUpdateDto company)
		{
			await _serviceManager.CompanyService.UpdateCompanyAsync(id, company, true);
			return NoContent();
		}
	}
}