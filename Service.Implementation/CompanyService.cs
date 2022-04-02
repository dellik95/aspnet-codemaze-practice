using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service.Implementations
{
	internal sealed class CompanyService : ICompanyService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly ILoggerManager _loggerManager;
		private readonly IMapper _mapper;

		public CompanyService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_loggerManager = loggerManager;
			_mapper = mapper;
		}


		public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
		{
			var companies = await _repositoryManager.Company.GetAllCompaniesAsync(trackChanges);
			var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
			return companiesDto;
		}

		public async Task<CompanyDto> GetCompanyAsync(int id, bool trackChanges)
		{
			var entity = await GetEntityAndCheckIfExists(id, trackChanges);
			var companyDto = _mapper.Map<CompanyDto>(entity);
			return companyDto;
		}

		public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreateDto company)
		{
			var entity = _mapper.Map<Company>(company);
			_repositoryManager.Company.CreateCompany(entity);
			await _repositoryManager.SaveAsync();
			var entityToReturn = _mapper.Map<CompanyDto>(entity);
			return entityToReturn;
		}

		public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
		{
			if (ids is null || !ids.Any()) throw new IdParametersBadRequestException();

			var entities = await _repositoryManager.Company.GetByIdsAsync(ids, trackChanges);
			if (entities.Count() != ids.Count()) throw new CollectionByIdsBadRequestException();

			var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(entities);
			return companiesToReturn;
		}

		public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(
			IEnumerable<CompanyForCreateDto> companies)
		{
			if (companies is null || !companies.Any()) throw new CompanyCollectionBadRequest();
			var entities = _mapper.Map<IEnumerable<Company>>(companies);
			foreach (var company in entities)
			{
				_repositoryManager.Company.CreateCompany(company);
			}

			await _repositoryManager.SaveAsync();

			var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(entities);
			var ids = string.Join(",", companiesToReturn.Select(x => x.Id));
			return (companies: companiesToReturn, ids: ids);
		}

		public async Task DeleteCompanyAsync(int id, bool trackChanges)
		{
			var entity = await GetEntityAndCheckIfExists(id, trackChanges);

			_repositoryManager.Company.DeleteCompany(entity);
			await _repositoryManager.SaveAsync();
		}

		public async Task UpdateCompanyAsync(int companyId, CompanyForUpdateDto company, bool trackChanges)
		{
			var entity = await GetEntityAndCheckIfExists(companyId, trackChanges);
			_mapper.Map(company, entity);
			await _repositoryManager.SaveAsync();
		}

		private async Task<Company> GetEntityAndCheckIfExists(int companyId, bool trackChanges)
		{
			var entity = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
			if (entity == null) throw new CompanyNotFoundException(companyId);
			return entity;
		}
	}
}