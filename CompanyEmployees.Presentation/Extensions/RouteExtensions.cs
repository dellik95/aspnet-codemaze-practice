using Microsoft.AspNetCore.Routing;

namespace CompanyEmployees.Presentation.Extensions
{
	public static class RouteExtensions
	{
		private const string Controller = "controller";
		private const string Action = "action";
		public static object? GetAction(this RouteData routeData) => routeData.Values[Action];
		public static object? GetController(this RouteData routeData) => routeData.Values[Controller];
	}
}