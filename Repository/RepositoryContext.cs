using System;
using System.Linq;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configurations;

namespace Repository
{
	public class RepositoryContext : IdentityDbContext<User>
	{
		public RepositoryContext(DbContextOptions options) : base(options)
		{
			var pendingMigrations = Database.GetPendingMigrations();

			if (Database.EnsureCreated() || pendingMigrations.Any())
			{
				Console.WriteLine($"You have {pendingMigrations.Count()} pending migrations to apply.");
				Console.WriteLine("Applying pending migrations now");
				Database.MigrateAsync().Wait();
			}
		}

		public DbSet<Company>? Companies { get; set; }
		public DbSet<Employee>? Employees { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfiguration(new CompanyConfiguration());
			modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
			modelBuilder.ApplyConfiguration(new RoleConfiguration());
		}
	}
}