using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
	public interface ICompanyService
	{
		Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
		Task<CompanyDto> GetCompanyAsync(int id, bool trackChanges);
		Task<CompanyDto> CreateCompanyAsync(CompanyForCreateDto company);

		Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);

		Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(
			IEnumerable<CompanyForCreateDto> companies);

		Task DeleteCompanyAsync(int id, bool trackChanges);

		Task UpdateCompanyAsync(int companyId, CompanyForUpdateDto company, bool trackChanges);
	}
}