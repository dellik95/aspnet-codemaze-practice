using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees.MappingProfiles
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Company, CompanyDto>().ForMember(c => c.FullAddress,
				opt =>
					opt.MapFrom(x => string.Join(" ", x.Country, x.Address)));

			CreateMap<Company, CompanyForCreateDto>().ReverseMap();
			CreateMap<Company, CompanyForUpdateDto>();

			CreateMap<Employee, EmployeeDto>().ReverseMap();
			CreateMap<Employee, EmployeeForCreationDto>().ReverseMap();
			CreateMap<Employee, EmployeeForUpdateDto>().ReverseMap();
			CreateMap<UserForRegistrationDto, User>().ReverseMap();
		}
	}
}