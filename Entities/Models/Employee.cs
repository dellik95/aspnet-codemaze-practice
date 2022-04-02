using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
	public class Employee
	{
		[Column("CompanyId")] public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[MaxLength(50, ErrorMessage = "The Name must be less than 50 characters.")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Age is required")]
		public int Age { get; set; }

		[Required(ErrorMessage = "Position is required")]
		[MaxLength(20, ErrorMessage = "Max Length for the Position is 20 characters")]
		public string? Position { get; set; }

		[ForeignKey(nameof(Company))] public int CompanyId { get; set; }
		public Company? Company { get; set; }
	}
}