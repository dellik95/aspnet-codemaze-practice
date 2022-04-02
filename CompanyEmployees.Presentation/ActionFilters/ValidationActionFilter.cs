using System.Linq;
using CompanyEmployees.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyEmployees.Presentation.ActionFilters
{
	public class ValidationActionFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			var controller = context.RouteData.GetController();
			var action = context.RouteData.GetAction();
			var args = context.ActionArguments.SingleOrDefault(
				x => x.Value.GetType().Name.Contains("Dto")
			).Value;
			if (args is null)
			{
				context.Result =
					new BadRequestObjectResult($"Object is null. Controller: {controller}; Action: {action}");
				return;
			}

			if (!context.ModelState.IsValid) context.Result = new UnprocessableEntityObjectResult(context.ModelState);
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
		}
	}
}