using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations
{
	public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			builder.HasData(
				new Employee()
				{
					Id = 1,
					Age = 26,
					Name = "Alex",
					CompanyId = 1,
					Position = "Middle"
				},
				new Employee()
				{
					Id = 2,
					Age = 21,
					Name = "Bill",
					CompanyId = 1,
					Position = "Junior"
				},
				new Employee()
				{
					Id = 3,
					Age = 30,
					Name = "Ann",
					CompanyId = 2,
					Position = "QA"
				}
			);
		}
	}
}