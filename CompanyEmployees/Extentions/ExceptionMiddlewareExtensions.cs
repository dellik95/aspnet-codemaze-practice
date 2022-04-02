using Contracts;
using Entities.ErrorModels;
using Entities.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace CompanyEmployees.Extentions
{
	public static class ExceptionMiddlewareExtensions
	{
		public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager loggerManager)
		{
			app.UseExceptionHandler(appError =>
			{
				appError.Run(async context =>
				{
					context.Response.ContentType = "application/json";

					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
					if (contextFeature != null)
					{
						context.Response.StatusCode = contextFeature.Error switch
						{
							NotFoundException => StatusCodes.Status404NotFound,
							BadRequestException => StatusCodes.Status400BadRequest,
							_ => StatusCodes.Status500InternalServerError
						};

						loggerManager.LogError($"Something went wrong: {contextFeature.Error}");
						await context.Response.WriteAsync(new ErrorDetails()
						{
							StatusCode = context.Response.StatusCode,
							Message = contextFeature.Error.Message
						}.ToString());
					}
				});
			});
		}
	}
}