using System;
using System.Collections.Generic;
using System.Linq;
using CompanyEmployees.Presentation.Constants;
using Contracts;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Utility
{
	public class EmployeeLinks : IEmployeeLinks
	{
		private readonly LinkGenerator _linkGenerator;
		private readonly IDataShaper<EmployeeDto> _dataShaper;

		public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
		{
			_linkGenerator = linkGenerator;
			_dataShaper = dataShaper;
		}

		public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employees, string fields, int companyId,
			HttpContext context)
		{
			var shapedEmployees = ShapeData(employees, fields).ToList();

			if (ShouldGenerateLinks(context))
			{
				return ReturnLinkedEmployees(employees, fields, companyId, context, shapedEmployees);
			}

			return ReturnShapedEmployees(shapedEmployees);
		}

		private LinkResponse ReturnShapedEmployees(List<Entity> employees)
		{
			return new LinkResponse()
			{
				ShapedEntities = employees
			};
		}

		private LinkResponse ReturnLinkedEmployees(IEnumerable<EmployeeDto> employees, string fields, int companyId,
			HttpContext context, List<Entity> shapedEmployees)
		{
			var employeeList = employees.ToList();
			for (int i = 0; i < employees.Count(); i++)
			{
				var employeeLinks = CreateLinksForEmployee(context, companyId, employeeList[i].Id, fields);
				shapedEmployees[i].Add("Links", employeeLinks);
			}

			var employeeCollection = new LinkCollectionsWrapper<Entity>(shapedEmployees);
			var linkedEmployees = CreateLinksForEmployee(context, employeeCollection);

			return new LinkResponse() {HasLinks = true, LinkedEntities = linkedEmployees};
		}

		private LinkCollectionsWrapper<Entity> CreateLinksForEmployee(HttpContext context,
			LinkCollectionsWrapper<Entity> employeesWrapper)
		{
			employeesWrapper.Links.Add(
				new Link(
					_linkGenerator.GetUriByAction(context, "GetEmployeesForCompany", controller: "Employees",
						values: new { }), "self",
					"GET"));
			return employeesWrapper;
		}

		private List<Link> CreateLinksForEmployee(HttpContext context, int companyId, int id,
			string fields = "")
		{
			var links = new List<Link>
			{
				new Link(_linkGenerator.GetUriByAction(context, "GetEmployeeForCompany", controller: "Employees",
					values: new
					{
						companyId, id, fields
					}), "self", "GET"),
				new Link(_linkGenerator.GetUriByAction(context, "DeleteEmployeeForCompany", controller: "Employees",
					values: new
					{
						companyId, id
					}), "delete_employee", "DELETE"),
				new Link(_linkGenerator.GetUriByAction(context, "UpdateEmployeeForCompany", controller: "Employees",
					values: new
					{
						companyId, id
					}), "update_employee", "PUT"),
				new Link(_linkGenerator.GetUriByAction(context, "PartiallyEmployeeUpdateForCompany",
					controller: "Employees", values: new
					{
						companyId, id
					}), "partially_update_employee", "PATCH")
			};

			return links;
		}

		private bool ShouldGenerateLinks(HttpContext context)
		{
			var mediaTypes = (MediaTypeHeaderValue) context.Items[HttpConstants.AcceptMediaTypeHeaderName];
			return mediaTypes.SubTypeWithoutSuffix.EndsWith(HttpConstants.HateoasMediaTypeHeaderName,
				StringComparison.InvariantCultureIgnoreCase);
		}

		private IEnumerable<Entity> ShapeData(IEnumerable<EmployeeDto> employees, string fields)
		{
			return _dataShaper.ShapeData(employees, fields).Select(e => e.Entity).ToList();
		}
	}
}