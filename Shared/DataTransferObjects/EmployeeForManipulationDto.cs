using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
	public abstract record EmployeeForManipulationDto
	{
		[Required(ErrorMessage = "Name is required property")]
		public string Name { get; init; }

		[Required(ErrorMessage = "Age is required property")]
		[Range(0, 150, ErrorMessage = "Age can be in 0-150 range")]
		public int Age { get; init; }

		[Required(ErrorMessage = "Age is required property")]
		[MaxLength(150, ErrorMessage = "Position can`t be so long")]
		public string Position { get; init; }
	};
}