using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Formatters
{
	public class OutputCsvFormatter : TextOutputFormatter
	{
		public OutputCsvFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
			SupportedEncodings.Add(Encoding.UTF8);
			SupportedEncodings.Add(Encoding.Unicode);
		}


		protected override bool CanWriteType(Type? type)
		{
			if (typeof(CompanyDto).IsAssignableFrom(type) || typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type))
				return base.CanWriteType(type);

			return false;
		}

		public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
			Encoding selectedEncoding)
		{
			var response = context.HttpContext.Response;
			var buffer = new StringBuilder();
			if (context.Object is IEnumerable<CompanyDto>)
				foreach (var dto in (IEnumerable<CompanyDto>) context.Object)
					FormatCsv(buffer, dto);
			else if (context.Object is CompanyDto) FormatCsv(buffer, (CompanyDto) context.Object);

			await response.WriteAsync(buffer.ToString());
		}

		private void FormatCsv(StringBuilder buffer, CompanyDto dto)
		{
			buffer.Append($"{dto.Id}, \"{dto.Name}\", \"{dto.FullAddress}\"");
		}
	}
}