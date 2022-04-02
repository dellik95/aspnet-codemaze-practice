using System;
using System.Linq;
using AspNetCoreRateLimit;
using CompanyEmployees.Extentions;
using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Repository;

namespace CompanyEmployees
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Configuration.AddEnvironmentVariables();

			builder.Services.AddControllers(opt =>
				{
					opt.RespectBrowserAcceptHeader = true;
					opt.ReturnHttpNotAcceptable = true;
					opt.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
					opt.CacheProfiles.Add("120SecondsDuration", new CacheProfile()
					{
						Duration = 120
					});
				})
				.AddXmlDataContractSerializerFormatters()
				.AddOutputCsvFormatter()
				.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
			builder.Services.ConfigureIdentity()
				.AddEndpointsApiExplorer()
				.ConfigureOptions(builder.Configuration)
				.ConfigureJWT(builder.Configuration)
				.ConfigureVersioning()
				.ConfigureSwagger()
				.AddCustomMediaTypes()
				.ConfigureRateLimits()
				.AddHttpContextAccessor()
				.ConfigureCacheStore()
				.ConfigureCors()
				.ConfigureLogging()
				.ConfigureSqlContext(builder.Configuration)
				.ConfigureRepositoryManager()
				.ConfigureServiceManager()
				.ConfigureServices()
				.ConfigureActionFilters()
				.AddAutoMapper(typeof(Program))
				.Configure<ApiBehaviorOptions>(o => { o.SuppressModelStateInvalidFilter = true; });

			var app = builder.Build();
			var logger = app.Services.GetRequiredService<ILoggerManager>();
			app.ConfigureExceptionHandler(logger);

			if (app.Environment.IsDevelopment())
			{
				app.UseHsts();
				app.UseSwagger();
				app.UseSwaggerUI(s =>
				{
					s.SwaggerEndpoint("/swagger/v1/swagger.json", "Code maze API v1");
					s.SwaggerEndpoint("/swagger/v2/swagger.json", "Code maze API v2");
				});
			}

			app.UseIpRateLimiting();
			app.UseCors("default");
			app.UseResponseCaching();
			app.UseHttpCacheHeaders();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}

		static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() => new ServiceCollection().AddLogging()
			.AddMvc()
			.AddNewtonsoftJson().Services.BuildServiceProvider().GetRequiredService<IOptions<MvcOptions>>().Value
			.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();
	}
}