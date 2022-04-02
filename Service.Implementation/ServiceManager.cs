using System;
using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;

namespace Service.Implementations
{
	public sealed class ServiceManager : IServiceManager
	{
		private Lazy<ICompanyService> _companyService { get; }
		private Lazy<IEmployeeService> _employeeService { get; }
		private Lazy<IAuthenticationService> _authenticationService { get; }


		public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper,
			IEmployeeLinks links, UserManager<User> userManager, IOptions<JwtOptions> jwtOptions)
		{
			_companyService =
				new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, loggerManager, mapper));
			_employeeService =
				new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, loggerManager, mapper, links));


			_authenticationService = new Lazy<IAuthenticationService>(() =>
				new AuthenticationService(loggerManager, mapper, userManager, jwtOptions));
		}

		public ICompanyService CompanyService => _companyService.Value;
		public IEmployeeService EmployeeService => _employeeService.Value;
		public IAuthenticationService AuthenticationService => _authenticationService.Value;
	}
}