using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspNetCoreRateLimit;
using CompanyEmployees.Formatters;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.Constants;
using CompanyEmployees.Utility;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using LoggerService;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Service.Contracts;
using Service.Implementations;
using Service.Implementations.DataShaping;

namespace CompanyEmployees
{
	public static class ServiceExtensions
	{
		public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(s =>
			{
				s.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = "Code maze API", Version = "v1", Contact = new OpenApiContact()
					{
						Email = "test@test.com",
						Name = "Test User",
					},
					License = new OpenApiLicense()
					{
						Name = "Open API License"
					}
				});
				s.SwaggerDoc("v2", new OpenApiInfo() {Title = "Code maze API", Version = "v2"});
				s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					In = ParameterLocation.Header,
					Description = "Place to add JWT auth Token",
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				s.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme()
						{
							Reference = new OpenApiReference()
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Name = "Bearer"
						},
						new List<string>()
					}
				});
			});
			return services;
		}

		public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
		{
			services.AddIdentity<User, IdentityRole>(o =>
				{
					o.Password.RequireDigit = false;
					o.Password.RequireLowercase = false;
					o.Password.RequireUppercase = false;
					o.Password.RequireNonAlphanumeric = false;
					o.Password.RequiredLength = 8;
					o.User.RequireUniqueEmail = true;
				})
				.AddEntityFrameworkStores<RepositoryContext>()
				.AddDefaultTokenProviders();

			return services;
		}

		public static IServiceCollection ConfigureOptions(this IServiceCollection services,
			IConfiguration configuration) =>
			services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Section));

		public static IServiceCollection ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtOptions = new JwtOptions();
			configuration.Bind(JwtOptions.Section, jwtOptions);

			services.AddAuthentication(o =>
			{
				o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o =>
			{
				o.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtOptions.ValidIssuer,
					ValidAudience = jwtOptions.ValidAudience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
				};
			});

			return services;
		}

		public static IServiceCollection ConfigureLogging(this IServiceCollection services)
		{
			return services.AddSingleton<ILoggerManager, LoggerManager>();
		}

		public static IServiceCollection ConfigureServices(this IServiceCollection services) =>
			services.AddScoped(typeof(IDataShaper<>), typeof(DataShaper<>))
				.AddScoped<IEmployeeLinks, EmployeeLinks>();


		public static IServiceCollection ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("default", options =>
					options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("X-Pagination"));
			});
			return services;
		}

		public static IServiceCollection ConfigureRepositoryManager(this IServiceCollection serviceCollection)
		{
			return serviceCollection.AddScoped<IRepositoryManager, RepositoryManager>();
		}


		public static IServiceCollection ConfigureServiceManager(this IServiceCollection serviceCollection)
		{
			return serviceCollection.AddScoped<IServiceManager, ServiceManager>();
		}


		public static IServiceCollection ConfigureSqlContext(this IServiceCollection serviceCollection,
			IConfiguration configuration)
		{
			return serviceCollection.AddDbContext<RepositoryContext>(
				o => o.UseSqlServer(configuration.GetConnectionString("Default")));
		}


		public static IMvcBuilder AddOutputCsvFormatter(this IMvcBuilder builder)
		{
			return builder.AddMvcOptions(opt => { opt.OutputFormatters.Add(new OutputCsvFormatter()); });
		}

		public static IServiceCollection ConfigureActionFilters(this IServiceCollection services)
		{
			services.AddScoped<ValidationActionFilter>();
			services.AddScoped<ValidateMediaTypeFilter>();
			return services;
		}

		public static IServiceCollection ConfigureRateLimits(this IServiceCollection services)
		{
			var rateLimits = new List<RateLimitRule>()
			{
				new RateLimitRule()
				{
					Endpoint = "*",
					Limit = 30,
					Period = "5m"
				}
			};


			services.Configure<IpRateLimitOptions>(opt => { opt.GeneralRules = rateLimits; });

			services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
			services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
			services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
			services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
			return services;
		}

		public static IServiceCollection ConfigureCacheStore(this IServiceCollection services) =>
			services.AddResponseCaching().AddHttpCacheHeaders((validationOptions) =>
			{
				validationOptions.MaxAge = 35;
				validationOptions.CacheLocation = CacheLocation.Private;
			}, (expirationOptions) => { expirationOptions.MustRevalidate = true; }).AddMemoryCache();

		public static IServiceCollection ConfigureVersioning(this IServiceCollection services) =>
			services.AddApiVersioning(
				options =>
				{
					options.ReportApiVersions = true;
					options.AssumeDefaultVersionWhenUnspecified = true;
					options.DefaultApiVersion = new ApiVersion(1, 0);
					options.ApiVersionReader = new HeaderApiVersionReader("api-version");
					/*options.Conventions.Controller<CompaniesController>().HasApiVersion(new ApiVersion(1, 0));
					options.Conventions.Controller<CompaniesV2Controller>().HasApiVersion(new ApiVersion(2, 0));*/
				});


		public static IServiceCollection AddCustomMediaTypes(this IServiceCollection services)
		{
			services.Configure<MvcOptions>(options =>
			{
				var jsonFormatter = options.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();
				if (jsonFormatter != null)
				{
					jsonFormatter.SupportedMediaTypes.Add(HttpConstants.HateoasJsonMediaTypeHeader);
					jsonFormatter.SupportedMediaTypes.Add(HttpConstants.RootMediaTypeHeader);
				}

				var xmlFormatter = options.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()
					.FirstOrDefault();
				if (xmlFormatter != null)
				{
					xmlFormatter.SupportedMediaTypes.Add(HttpConstants.HateoasXmlMediaTypeHeader);
					xmlFormatter.SupportedMediaTypes.Add(HttpConstants.RootMediaTypeHeader);
				}
			});
			return services;
		}
	}
}