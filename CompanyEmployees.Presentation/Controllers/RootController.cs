using System.Collections.Generic;
using CompanyEmployees.Presentation.Constants;
using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CompanyEmployees.Presentation.Controllers
{
	[Route("api")]
	[ApiController]
	public class RootController : ControllerBase
	{
		private readonly LinkGenerator _linkGenerator;

		public RootController(LinkGenerator linkGenerator)
		{
			_linkGenerator = linkGenerator;
		}

		[HttpGet(Name = "GetRoot")]
		public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
		{
			if (mediaType.Contains(HttpConstants.RootMediaTypeHeader))
			{
				var links = new List<Link>
				{
					new Link()
					{
						Href = _linkGenerator.GetUriByAction(HttpContext, controller: "Root", action: "GetRoot"),
						Method = "GET",
						Rel = "self"
					},
					new Link()
					{
						Href = _linkGenerator.GetUriByAction(HttpContext, controller: "Companies", action: "GetCompany"),
						Rel = "companies",
						Method = "GET"
					},
					new Link()
					{
						Href = _linkGenerator.GetUriByAction(HttpContext, controller: "Companies", action: "CreateCompany"),
						Rel = "create_company",
						Method = "POST"
					}
				};


				return Ok(links);
			}

			return NoContent();
		}
	}
}