using System;

namespace Shared.RequestFeatures
{
	public class EmployeeParameters : RequestParameters
	{
		public EmployeeParameters() => OrderBy = "Name";
		public uint MaxAge { get; set; } = Int32.MaxValue;
		public uint MinAge { get; set; }

		public bool ValidAgeRange => MaxAge > MinAge;

		public string? SearchTerm { get; set; }

		public string? Fields { get; set; }
	}
}