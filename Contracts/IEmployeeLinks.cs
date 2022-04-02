﻿using System.Collections.Generic;
using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DataTransferObjects;

namespace Contracts
{
	public interface IEmployeeLinks
	{
		LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employees, string fields, int companyId,
			HttpContext context);
	}
}