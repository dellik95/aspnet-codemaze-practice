using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
	public interface ICompanyRepository
	{
		Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
		Task<Company> GetCompanyAsync(int id, bool trackChanges);

		void CreateCompany(Company company);
		Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);

		void DeleteCompany(Company company);
	}
}