using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Repository;

namespace CompanyEmployees
{
	public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
	{
		public RepositoryContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var builder = new DbContextOptionsBuilder<RepositoryContext>()
				.UseSqlServer(configuration.GetConnectionString("Default"),
					b => b.MigrationsAssembly("CompanyEmployees"));
			return new RepositoryContext(builder.Options);
		}
	}
}