using System.Text.Json;
using System.Threading.Tasks;
using CompanyEmployees.Presentation.ActionFilters;
using Entities.LinkModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace CompanyEmployees.Presentation.Controllers
{
	[Route("api/companies/{companyId:int}/employees")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;

		public EmployeesController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		[HttpGet]
		[HttpHead]
		[ServiceFilter(typeof(ValidateMediaTypeFilter))]
		public async Task<IActionResult> GetEmployeesForCompany(int companyId,
			[FromQuery] EmployeeParameters parameters)
		{
			var linkParams = new LinkParameters(parameters, HttpContext);
			var result = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, linkParams, false);
			Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));
			return (result.linkResponse.HasLinks)
				? Ok(result.linkResponse.LinkedEntities)
				: Ok(result.linkResponse.ShapedEntities);
		}


		[HttpGet("{id:int}", Name = "GetEmployeeForCompany")]
		public async Task<IActionResult> GetEmployeeForCompany(int companyId, int id)
		{
			var employee = await _serviceManager.EmployeeService.GetEmployeeAsync(companyId, id, false);
			return Ok(employee);
		}

		[HttpPost]
		[ServiceFilter(typeof(ValidationActionFilter))]
		public async Task<IActionResult> CreateEmployee(int companyId, [FromBody] EmployeeForCreationDto? employee)
		{
			var employeeToReturn =
				await _serviceManager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, false);
			return CreatedAtRoute("GetEmployeeForCompany", new {companyId, id = employeeToReturn.Id}, employeeToReturn);
		}


		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteEmployeeForCompany(int companyId, int id)
		{
			await _serviceManager.EmployeeService.DeleteEmployeeAsync(companyId, id, false);
			return NoContent();
		}

		[HttpPut("{id:int}")]
		[ServiceFilter(typeof(ValidationActionFilter))]
		public async Task<IActionResult> UpdateEmployeeForCompany(int companyId, int id,
			[FromBody] EmployeeForUpdateDto? employee)
		{
			await _serviceManager.EmployeeService.UpdateEmployeeAsync(companyId, id, employee, false, true);
			return NoContent();
		}


		[HttpPatch("{id:int}")]
		public async Task<IActionResult> PartiallyEmployeeUpdateForCompany(int companyId, int id,
			[FromBody] JsonPatchDocument<EmployeeForUpdateDto>? patchDoc)
		{
			if (patchDoc is null) return BadRequest("Input data not valid");

			var (employeeForUpdate, employee) = await
				_serviceManager.EmployeeService.GetEmployeeForPatchAsync(companyId, id, false, true);
			patchDoc.ApplyTo(employeeForUpdate, ModelState);
			TryValidateModel(employeeForUpdate);
			if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
			await _serviceManager.EmployeeService.SaveChangesForPatchAsync(employeeForUpdate, employee);
			return NoContent();
		}
	}
}