using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts
{
	public interface IEmployeeService
	{
		Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(int companyId,
			LinkParameters parameters, bool trackChanges);

		Task<EmployeeDto> GetEmployeeAsync(int companyId, int id, bool trackChanges);

		Task<EmployeeDto> CreateEmployeeForCompanyAsync(int companyId, EmployeeForCreationDto employee,
			bool trackChanges);

		Task DeleteEmployeeAsync(int companyId, int id, bool trackChanges);

		Task UpdateEmployeeAsync(int companyId, int id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges,
			bool empTrackChanges);

		Task<(EmployeeForUpdateDto employeeForUpdate, Employee employee)> GetEmployeeForPatchAsync(int companyId,
			int id,
			bool compTrackChanges, bool empTrackChanges);

		Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employee);
	}
}