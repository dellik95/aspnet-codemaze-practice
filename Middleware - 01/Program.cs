using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Middleware
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();


			app.UseHttpsRedirection();

			app.UseAuthorization();
/*
			app.Use(async (context, next) =>
			{
				Console.WriteLine("Before Hello from middleware");
				await next.Invoke(context);
				Console.WriteLine("After Hello from middleware");
			});
			
			app.Run(async (context) =>
			{
				await context.Response.WriteAsync("Hello from middleware");
			});*/

			app.Map("/test", applicationBuilder =>
			{
				applicationBuilder.Use(async (context, next) =>
				{
					Console.WriteLine("Before Hello from middleware");
					await next.Invoke(context);
					Console.WriteLine("After Hello from middleware");
				});

				applicationBuilder.Run(async (context) =>
				{
					await context.Response.WriteAsync("Hello from middleware");
				});
			});
			
			app.MapWhen(context => context.Request.Query.ContainsKey("test") , applicationBuilder =>
			{
				applicationBuilder.Use(async (context, next) =>
				{
					Console.WriteLine("Before Hello from middleware");
					await next.Invoke(context);
					Console.WriteLine("After Hello from middleware");
				});

				applicationBuilder.Run(async (context) =>
				{
					await context.Response.WriteAsync("Hello Query from middleware");
				});
			});

			app.Run();
		}
	}
}