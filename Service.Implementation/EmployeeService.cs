using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Implementations
{
	internal sealed class EmployeeService : IEmployeeService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly ILoggerManager _loggerManager;
		private readonly IEmployeeLinks _links;
		private readonly IMapper _mapper;

		public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper,
			IEmployeeLinks links)
		{
			_repositoryManager = repositoryManager;
			_loggerManager = loggerManager;
			_mapper = mapper;
			_links = links;
		}

		public async Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(int companyId,
			LinkParameters parameters, bool trackChanges)
		{
			if (!parameters.EmployeeParameters.ValidAgeRange) throw new MaxAgeRangeBadRequestException();
			await CheckIfCompanyExists(companyId, trackChanges);
			var employeesWithMetadata =
				await _repositoryManager.Employee.GetEmployeesAsync(companyId, parameters.EmployeeParameters,
					trackChanges);
			var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetadata);
			var linkResponse = _links.TryGenerateLinks(employeesDto, parameters.EmployeeParameters.Fields, companyId,
				parameters.context);


			return (linkResponse: linkResponse, metaData: employeesWithMetadata.MetaData);
		}

		public async Task<EmployeeDto> GetEmployeeAsync(int companyId, int id, bool trackChanges)
		{
			await CheckIfCompanyExists(companyId, trackChanges);
			var employee = await GetEmployeeForCompanyAndCheckIfExists(companyId, id, trackChanges);
			var employeeDto = _mapper.Map<EmployeeDto>(employee);
			return employeeDto;
		}

		public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(int companyId, EmployeeForCreationDto employee,
			bool trackChanges)
		{
			await CheckIfCompanyExists(companyId, trackChanges);

			var entity = _mapper.Map<Employee>(employee);
			_repositoryManager.Employee.CreateEmployeeForCompany(companyId, entity);
			await _repositoryManager.SaveAsync();
			var entityToReturn = _mapper.Map<EmployeeDto>(entity);
			return entityToReturn;
		}

		public async Task DeleteEmployeeAsync(int companyId, int id, bool trackChanges)
		{
			await CheckIfCompanyExists(companyId, trackChanges);
			var employee = await GetEmployeeForCompanyAndCheckIfExists(companyId, id, trackChanges);

			_repositoryManager.Employee.DeleteEmployee(employee);
			await _repositoryManager.SaveAsync();
		}

		public async Task UpdateEmployeeAsync(int companyId, int id, EmployeeForUpdateDto employeeForUpdate,
			bool compTrackChanges,
			bool empTrackChanges)
		{
			await CheckIfCompanyExists(companyId, compTrackChanges);
			var employee = await GetEmployeeForCompanyAndCheckIfExists(companyId, id, empTrackChanges);

			_mapper.Map(employeeForUpdate, employee);
			await _repositoryManager.SaveAsync();
		}

		public async Task<(EmployeeForUpdateDto employeeForUpdate, Employee employee)> GetEmployeeForPatchAsync(
			int companyId, int id,
			bool compTrackChanges, bool empTrackChanges)
		{
			await CheckIfCompanyExists(companyId, compTrackChanges);
			var employee = await GetEmployeeForCompanyAndCheckIfExists(companyId, id, empTrackChanges);

			var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employee);
			return (employeeToPatch, employee);
		}

		public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employee)
		{
			_mapper.Map(employeeToPatch, employee);
			await _repositoryManager.SaveAsync();
		}

		private async Task CheckIfCompanyExists(int companyId, bool trackChanges)
		{
			var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
			if (company == null) throw new CompanyNotFoundException(companyId);
		}


		private async Task<Employee> GetEmployeeForCompanyAndCheckIfExists(int companyId, int employeeId,
			bool trackChanges)
		{
			var employee = await _repositoryManager.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
			if (employee == null) throw new CompanyNotFoundException(employeeId);
			return employee;
		}
	}
}