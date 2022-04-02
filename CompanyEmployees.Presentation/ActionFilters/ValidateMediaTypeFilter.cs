using System.Linq;
using CompanyEmployees.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace CompanyEmployees.Presentation.ActionFilters
{
	public class ValidateMediaTypeFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			var acceptExists = context.HttpContext.Request.Headers.ContainsKey("Accept");
			if (!acceptExists)
			{
				context.Result = new BadRequestObjectResult("Accept header is missing");
				return;
			}

			var mediaType = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();
			if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? mediaTypeValue))
			{
				context.Result =
					new BadRequestObjectResult("Media type not presented. Pleas add Accept with required media type");
				return;
			}

			context.HttpContext.Items.Add(HttpConstants.AcceptMediaTypeHeaderName, mediaTypeValue);
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
		}
	}
}