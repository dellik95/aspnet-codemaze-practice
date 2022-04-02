using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
	public record CompanyForManipulationDto
	{
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; init; }

		[Required(ErrorMessage = "Address is required")]
		[StringLength(50, ErrorMessage = "Mex length of address is 50 chanrecters")]
		public string Address { get; init; }

		[Required(ErrorMessage = "Country is required")]
		[StringLength(50, ErrorMessage = "Mex length of Country is 50 chanrecters")]
		public string Country { get; init; }

		public IEnumerable<EmployeeForUpdateDto> Employees { get; init; }
	};
}