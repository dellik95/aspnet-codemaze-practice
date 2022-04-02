using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository
{
	public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(RepositoryContext context) : base(context)
		{
		}

		public async Task<PagedList<Employee>> GetEmployeesAsync(int companyId, EmployeeParameters parameters,
			bool trackChanges)
		{
			var items = await FindByCondition(
					e => e.CompanyId == companyId,
					trackChanges)
				.FilterEmployee(parameters.MinAge, parameters.MaxAge)
				.Search(parameters.SearchTerm)
				.ApplyOrder(parameters.OrderBy)
				.Skip((parameters.PageNumber - 1) * parameters.PageSize) /////
				.Take(parameters.PageSize)
				.ToListAsync();
			var count = await FindByCondition(e => e.CompanyId == companyId, trackChanges).CountAsync();
			return new PagedList<Employee>(items, count, parameters.PageNumber, parameters.PageSize);
		}

		public async Task<Employee> GetEmployeeAsync(int companyId, int id, bool trackChanges) =>
			await FindByCondition(e => e.CompanyId == companyId && e.Id == id, trackChanges).SingleOrDefaultAsync();

		public void CreateEmployeeForCompany(int companyId, Employee employee)
		{
			employee.CompanyId = companyId;
			Create(employee);
		}

		public void DeleteEmployee(Employee employee) => Delete(employee);
	}
}