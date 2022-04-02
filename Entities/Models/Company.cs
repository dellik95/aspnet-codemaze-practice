using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
	public class Company
	{
		[Column("EmployeeId")] public int Id { get; set; }

		[Required(ErrorMessage = "The Name is required")]
		[MaxLength(50, ErrorMessage = "The Name must be less than 50 characters.")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "The Company Address is required")]
		[MaxLength(50, ErrorMessage = "The Address must be less than 50 characters.")]
		public string? Address { get; set; }

		public string? Country { get; set; }

		public ICollection<Employee>? Employees { get; set; }
	}
}