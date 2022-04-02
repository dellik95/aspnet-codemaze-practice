using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
	{
		public CompanyRepository(RepositoryContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
			await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();

		public async Task<Company> GetCompanyAsync(int id, bool trackChanges) =>
			await FindByCondition(c => c.Id == id, trackChanges).FirstOrDefaultAsync();

		public void CreateCompany(Company company)
		{
			Create(company);
		}

		public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
			await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

		public void DeleteCompany(Company company) => Delete(company);
	}
}